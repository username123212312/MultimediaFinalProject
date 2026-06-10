using System;

namespace MultimediaFinalProject
{
    public class AudioCompressor
    {
        public byte[] ApplyNonlinearQuantization(float[] samples, int mu = 255)
        {
            int bitsPerSample = (mu <= 256) ? 8 : 16;
            int bytesPerSample = bitsPerSample / 8;
            byte[] output = new byte[samples.Length * bytesPerSample];

            for (int i = 0; i < samples.Length; i++)
            {
                float compressed = (float)(Math.Sign(samples[i]) * (Math.Log(1 + mu * Math.Abs(samples[i])) / Math.Log(1 + mu)));

                if (bitsPerSample == 8)
                {
                    byte quantized = (byte)((compressed + 1) / 2 * 255);
                    output[i] = quantized;
                }
                else
                {
                    ushort quantized = (ushort)((compressed + 1) / 2 * 65535);
                    output[i * 2] = (byte)(quantized & 0xFF);
                    output[i * 2 + 1] = (byte)((quantized >> 8) & 0xFF);
                }
            }
            return output;
        }

        public float[] DecompressNonlinearQuantization(byte[] compressed, int mu = 255, int totalSamples = 0)
        {
            int bitsPerSample = (mu <= 256) ? 8 : 16;
            int bytesPerSample = bitsPerSample / 8;
            int sampleCount = (totalSamples > 0) ? totalSamples : compressed.Length / bytesPerSample;
            float[] output = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float normalized;
                if (bitsPerSample == 8)
                {
                    normalized = compressed[i] / 255.0f * 2 - 1;
                }
                else
                {
                    ushort value = (ushort)(compressed[i * 2] | (compressed[i * 2 + 1] << 8));
                    normalized = value / 65535.0f * 2 - 1;
                }

                float sign = Math.Sign(normalized);
                float absY = Math.Abs(normalized);
                output[i] = sign * (float)((Math.Pow(1 + mu, absY) - 1.0) / mu);
            }
            return output;
        }

        public byte[] ApplyDPCM(float[] samples, int bitsPerSample = 8)
        {
            int maxValue = (1 << bitsPerSample) - 1;
            byte[] output = new byte[samples.Length];
            float prev = 0;
            float minDiff = float.MaxValue;
            float maxDiff = float.MinValue;

            float[] diffs = new float[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                diffs[i] = samples[i] - prev;
                prev = samples[i];
                if (diffs[i] < minDiff) minDiff = diffs[i];
                if (diffs[i] > maxDiff) maxDiff = diffs[i];
            }

            float range = maxDiff - minDiff;
            for (int i = 0; i < samples.Length; i++)
            {
                int quantized = (int)(((diffs[i] - minDiff) / range) * maxValue);
                if (quantized < 0) quantized = 0;
                if (quantized > maxValue) quantized = maxValue;
                output[i] = (byte)quantized;
            }

            return output;
        }

        public float[] DecompressDPCM(byte[] compressed, int bitsPerSample = 8, float minDiff = -1f, float maxDiff = 1f)
        {
            int maxValue = (1 << bitsPerSample) - 1;
            float[] output = new float[compressed.Length];
            float range = maxDiff - minDiff;

            output[0] = compressed[0] / (float)maxValue * range + minDiff;
            for (int i = 1; i < compressed.Length; i++)
            {
                float diff = compressed[i] / (float)maxValue * range + minDiff;
                output[i] = output[i - 1] + diff;
            }
            return output;
        }

        public byte[] ApplyPredictiveCoding(float[] samples, float predictorGain = 0.9f, int bitsPerSample = 8)
        {
            int maxValue = (1 << bitsPerSample) - 1;
            byte[] output = new byte[samples.Length];
            float prev = samples[0];
            float minError = float.MaxValue;
            float maxError = float.MinValue;

            float[] errors = new float[samples.Length];
            errors[0] = samples[0];

            for (int i = 1; i < samples.Length; i++)
            {
                float predicted = prev * predictorGain;
                errors[i] = samples[i] - predicted;
                prev = samples[i];
                if (errors[i] < minError) minError = errors[i];
                if (errors[i] > maxError) maxError = errors[i];
            }

            float range = maxError - minError;
            for (int i = 0; i < samples.Length; i++)
            {
                int quantized = (int)(((errors[i] - minError) / range) * maxValue);
                if (quantized < 0) quantized = 0;
                if (quantized > maxValue) quantized = maxValue;
                output[i] = (byte)quantized;
            }

            return output;
        }

        public float[] DecompressPredictiveCoding(byte[] compressed, float predictorGain = 0.9f, int bitsPerSample = 8, float minError = -1f, float maxError = 1f)
        {
            int maxValue = (1 << bitsPerSample) - 1;
            float[] output = new float[compressed.Length];
            float range = maxError - minError;

            output[0] = compressed[0] / (float)maxValue * range + minError;

            for (int i = 1; i < compressed.Length; i++)
            {
                float error = compressed[i] / (float)maxValue * range + minError;
                float predicted = output[i - 1] * predictorGain;
                output[i] = predicted + error;
            }
            return output;
        }

        public byte[] ApplyDeltaModulation(float[] samples, float step = 0.05f)
        {
            int[] bits = ApplyDeltaModulationBits(samples, step);
            return PackBitsToBytes(bits);
        }

        public int[] ApplyDeltaModulationBits(float[] samples, float step = 0.05f)
        {
            int[] output = new int[samples.Length];
            float current = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                output[i] = (samples[i] > current) ? 1 : 0;
                current += (output[i] == 1) ? step : -step;
            }
            return output;
        }

        public float[] DecompressDeltaModulation(byte[] packed, int totalSamples, float step = 0.05f)
        {
            int[] bits = UnpackBytesToBits(packed, totalSamples);
            return DecompressDeltaModulationBits(bits, step);
        }

        public float[] DecompressDeltaModulationBits(int[] bits, float step = 0.05f)
        {
            float[] output = new float[bits.Length];
            float current = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                output[i] = current;
                current += (bits[i] == 1) ? step : -step;
            }
            return output;
        }

        public byte[] ApplyAdaptiveDeltaModulation(float[] samples)
        {
            int[] bits = ApplyAdaptiveDeltaModulationBits(samples);
            return PackBitsToBytes(bits);
        }

        public int[] ApplyAdaptiveDeltaModulationBits(float[] samples)
        {
            int[] output = new int[samples.Length];
            float current = 0;
            float step = 0.05f;
            for (int i = 0; i < samples.Length; i++)
            {
                output[i] = (samples[i] > current) ? 1 : 0;
                current += (output[i] == 1) ? step : -step;
                step = (samples[i] > current) ? step * 1.1f : step * 0.9f;
                if (step < 0.001f) step = 0.001f;
            }
            return output;
        }

        public float[] DecompressAdaptiveDeltaModulation(byte[] packed, int totalSamples, float initialStep = 0.05f)
        {
            int[] bits = UnpackBytesToBits(packed, totalSamples);
            return DecompressAdaptiveDeltaModulationBits(bits, initialStep);
        }

        public float[] DecompressAdaptiveDeltaModulationBits(int[] bits, float initialStep = 0.05f)
        {
            float[] output = new float[bits.Length];
            float current = 0;
            float step = initialStep;
            for (int i = 0; i < bits.Length; i++)
            {
                output[i] = current;
                current += (bits[i] == 1) ? step : -step;
                step = (bits[i] == 1) ? step * 1.1f : step * 0.9f;
                if (step < 0.001f) step = 0.001f;
            }
            return output;
        }


        private byte[] PackBitsToBytes(int[] bits)
        {
            int byteCount = (bits.Length + 7) / 8;
            byte[] packed = new byte[byteCount];
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == 1)
                {
                    packed[i / 8] |= (byte)(1 << (i % 8));
                }
            }
            return packed;
        }
        private int[] UnpackBytesToBits(byte[] packed, int totalBits)
        {
            int[] bits = new int[totalBits];
            for (int i = 0; i < totalBits; i++)
            {
                bits[i] = (packed[i / 8] >> (i % 8)) & 1;
            }
            return bits;
        }
    }
}
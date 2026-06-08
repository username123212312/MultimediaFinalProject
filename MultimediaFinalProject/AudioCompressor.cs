using System;

namespace MultimediaFinalProject
{
    public class AudioCompressor
    {
        // 1. Nonlinear Quantization 
        public float[] ApplyNonlinearQuantization(float[] samples, int mu = 255)
        {
            float[] output = new float[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                output[i] = (float)(Math.Sign(samples[i]) * (Math.Log(1 + mu * Math.Abs(samples[i])) / Math.Log(1 + mu)));
            }
            return output;
        }

        // Inverse of Nonlinear Quantization (mu-law expansion)
        public float[] DecompressNonlinearQuantization(float[] compressed, int mu = 255)
        {
            float[] output = new float[compressed.Length];
            for (int i = 0; i < compressed.Length; i++)
            {
                float y = compressed[i];
                float sign = Math.Sign(y);
                float absY = Math.Abs(y);
                // inverse mu-law: x = sign * ( (1+mu)^{|y|} - 1 ) / mu
                output[i] = sign * (float)((Math.Pow(1 + mu, absY) - 1.0) / mu);
            }
            return output;
        }

        // 2. DPCM
        public float[] ApplyDPCM(float[] samples)
        {
            float[] output = new float[samples.Length];
            float prev = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                output[i] = samples[i] - prev;
                prev = samples[i];
            }
            return output;
        }

        // Inverse DPCM (cumulative sum)
        public float[] DecompressDPCM(float[] diffs)
        {
            float[] output = new float[diffs.Length];
            if (diffs.Length == 0) return output;
            output[0] = diffs[0];
            for (int i = 1; i < diffs.Length; i++)
            {
                output[i] = diffs[i] + output[i - 1];
            }
            return output;
        }

        // 3. Predictive Differential Coding
        public float[] ApplyPredictiveCoding(float[] samples)
        {
            float[] output = new float[samples.Length];
            for (int i = 1; i < samples.Length; i++)
            {
                output[i] = samples[i] - (samples[i - 1] * 0.9f); 
            }
            return output;
        }

        // Inverse Predictive Coding
        // Assumes compressed[0] contains the original first sample (if not available, reconstruction will be imperfect).
        public float[] DecompressPredictiveCoding(float[] compressed, float predictorGain = 0.9f)
        {
            float[] output = new float[compressed.Length];
            if (compressed.Length == 0) return output;
            output[0] = compressed[0];
            for (int i = 1; i < compressed.Length; i++)
            {
                output[i] = compressed[i] + (output[i - 1] * predictorGain);
            }
            return output;
        }

        // 4. Delta Modulation 
        public int[] ApplyDeltaModulation(float[] samples, float step = 0.05f)
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

        // Inverse Delta Modulation (reconstruct from bits)
        public float[] DecompressDeltaModulation(int[] bits, float step = 0.05f)
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

        // 5. Adaptive Delta Modulation 
        public int[] ApplyAdaptiveDeltaModulation(float[] samples)
        {
            int[] output = new int[samples.Length];
            float current = 0;
            float step = 0.05f;
            for (int i = 0; i < samples.Length; i++)
            {
                output[i] = (samples[i] > current) ? 1 : 0;
                current += (output[i] == 1) ? step : -step;
                step = (samples[i] > current) ? step * 1.1f : step * 0.9f;
            }
            return output;
        }

        // Inverse Adaptive Delta Modulation (reconstruct with adaptive step)
        public float[] DecompressAdaptiveDeltaModulation(int[] bits, float initialStep = 0.05f)
        {
            float[] output = new float[bits.Length];
            float current = 0;
            float step = initialStep;
            for (int i = 0; i < bits.Length; i++)
            {
                output[i] = current;
                current += (bits[i] == 1) ? step : -step;
                step = (bits[i] == 1) ? step * 1.1f : step * 0.9f;
            }
            return output;
        }
    }
}
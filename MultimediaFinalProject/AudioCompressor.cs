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

    }
}
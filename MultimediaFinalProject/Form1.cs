using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;
using NAudio.Wave;
using ScottPlot;



namespace MultimediaFinalProject
{
    public partial class Form1 : Form
    {
        private float[] audioSamples;
        private int audioSampleRate; // preserve original sample rate

        // playback/plot members
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;

        // compression control
        private CancellationTokenSource? compressionCts;

        private string? currentAudioFilePath;

        private double originalFileSizeMB;
        private object compressedData;
        private bool isCompressedAsBytes = false;

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;

            // configure ScottPlot if present
            try
            {
                formsPlot1.Plot.FigureBackground.Color = ScottPlot.Colors.White;
                formsPlot1.Plot.DataBackground.Color = ScottPlot.Colors.WhiteSmoke;
                formsPlot1.Plot.Grid.MajorLineColor = ScottPlot.Colors.LightGray;
                formsPlot1.Plot.Grid.MinorLineColor = ScottPlot.Colors.Gainsboro;

                formsPlotCompression.Plot.Title("Compression Rate");
                formsPlotCompression.Plot.XLabel("Time (s)");
                formsPlotCompression.Plot.YLabel("Ratio");

                formsPlotSpeed.Plot.Title("Processing Speed");
                formsPlotSpeed.Plot.XLabel("Time (s)");
                formsPlotSpeed.Plot.YLabel("Samples/sec");
            }
            catch
            {
                // ignore if designer doesn't contain plots
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.aac;*.flac;*.m4a",
                Title = "Select an audio file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PlayAudio(openFileDialog.FileName);
            }
        }

        // replace Process.Start preview with in-app load + optional play
        private void PlayAudio(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            currentAudioFilePath = filePath;

            FileInfo fileInfo = new FileInfo(filePath);
            originalFileSizeMB = fileInfo.Length / (1024.0 * 1024.0);

            try
            {
                using var reader = new AudioFileReader(filePath);

                int targetRate = (int)nudSampleRate.Value;
                // fall back to source sample rate if UI value is invalid
                if (targetRate <= 0)
                    targetRate = reader.WaveFormat.SampleRate;

                var resampler = new NAudio.Wave.SampleProviders.WdlResamplingSampleProvider(reader, targetRate);

                // Read until provider returns 0 (don't rely on TotalTime)
                var sampleList = new List<float>();
                float[] buffer = new float[8192];
                int read;
                while ((read = resampler.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (read == buffer.Length)
                        sampleList.AddRange(buffer);
                    else
                    {
                        for (int i = 0; i < read; i++)
                            sampleList.Add(buffer[i]);
                    }
                }

                audioSamples = sampleList.ToArray();
                audioSampleRate = resampler.WaveFormat.SampleRate;

                btnCompress.Enabled = (audioSamples != null && audioSamples.Length > 0);

                // initialize playback & plot
                LoadAndPlay(filePath, autoPlay: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading audio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DisplayAudioInfo(filePath);
        }

        private void LoadAndPlay(string path, bool autoPlay = false)
        {
            try
            {
                // stop & dispose previous
                outputDevice?.Stop();
                outputDevice?.Dispose();
                audioFile?.Dispose();

                outputDevice = new WaveOutEvent();
                audioFile = new AudioFileReader(path);

                outputDevice.Init(audioFile);

                DrawWaveformPlot(path);
                DisplayAudioInfo(path);

                if (autoPlay)
                    outputDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing playback:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawWaveformPlot(string filePath)
        {
            try
            {
                using var reader = new AudioFileReader(filePath);

                int sampleRate = reader.WaveFormat.SampleRate;
                int channels = reader.WaveFormat.Channels;

                List<double> samples = new();

                float[] buffer = new float[4096];
                int read;
                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < read; i += channels)
                        samples.Add(buffer[i]);
                }

                const int maxPoints = 5000;
                int step = Math.Max(1, samples.Count / maxPoints);

                var ys = new List<double>();
                var xs = new List<double>();

                for (int i = 0; i < samples.Count; i += step)
                {
                    ys.Add(samples[i]);
                    xs.Add((double)i / sampleRate);
                }

                formsPlot1.Plot.Clear();
                var signal = formsPlot1.Plot.Add.Scatter(xs.ToArray(), ys.ToArray());
                signal.LineWidth = 1;
                signal.Color = ScottPlot.Colors.DodgerBlue;

                formsPlot1.Plot.Title($"Waveform - {Path.GetFileName(filePath)}");
                formsPlot1.Plot.XLabel("Time (seconds)");
                formsPlot1.Plot.YLabel("Amplitude");
                formsPlot1.Plot.Axes.AutoScale();
                formsPlot1.Refresh();
            }
            catch (Exception ex)
            {
                // non-fatal for plotting
                Debug.WriteLine("Plot error: " + ex.Message);
            }
        }

        private void DisplayAudioInfo(string filePath)
        {
            try
            {
                using var file = TagLib.File.Create(filePath);
                FileInfo info = new FileInfo(filePath);

                string duration =
                    file.Properties.Duration.Hours.ToString("00") + ":" +
                    file.Properties.Duration.Minutes.ToString("00") + ":" +
                    file.Properties.Duration.Seconds.ToString("00");

                txtInfo.Text =
                    "File Name: " + info.Name + Environment.NewLine +
                    "Size: " + (info.Length / 1024.0 / 1024.0).ToString("0.00") + " MB" + Environment.NewLine +
                    "Duration: " + duration + Environment.NewLine +
                    "Sample Rate: " + file.Properties.AudioSampleRate + " Hz" + Environment.NewLine +
                    "Channels: " + file.Properties.AudioChannels + Environment.NewLine +
                    "Bitrate: " + file.Properties.AudioBitrate + " kbps" + Environment.NewLine +
                    "Encoding Type: " + info.Extension.ToUpper().Replace(".", "") + Environment.NewLine;
            }
            catch (Exception ex)
            {
                txtInfo.Text = "Error reading metadata:" + Environment.NewLine + ex.Message;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
                PlayAudio(files[0]);
        }

        // playback controls from sample
        private void playAudio_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
                outputDevice.Play();
        }

        private void stopAudio_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                if (audioFile != null)
                    audioFile.Position = 0;
            }
        }

        // ----- restored compression / decompression UI handlers & I/O helpers -----

        private async void btnCompress_Click(object sender, EventArgs e)
        {
            if (audioSamples == null || audioSamples.Length == 0)
            {
                MessageBox.Show("No audio loaded.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbAlgorithm.SelectedItem == null)
            {
                MessageBox.Show("Please select a compression algorithm.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnCompress.Enabled = false;
            btnDecompress.Enabled = false;
            btnCancel.Enabled = true;
            compressionCts = new CancellationTokenSource();
            var token = compressionCts.Token;

            string algo = cmbAlgorithm.SelectedItem.ToString() ?? string.Empty;
            int quantLevels = (int)nudQuant.Value;
            int lastProcessed = 0;

            // Setup data for live graphs
            var timePoints = new List<double>();
            var compRates = new List<double>();
            var speedPoints = new List<double>();
            var swTotal = Stopwatch.StartNew();
            var swBlock = new Stopwatch();

            pbProgress.Value = 0;
            formsPlotCompression.Plot.Clear();
            formsPlotSpeed.Plot.Clear();

            try
            {
                // run compression with progress
                object result = await Task.Run(() =>
                    ProcessCompressionWithProgress(algo, quantLevels, progressCallback: (processed, total, bytesCompressed, elapsedMs) =>
                    {
                        // executed on thread pool — marshal to UI
                        this.BeginInvoke(() =>
                        {
                            // overall percent
                            double percent = total > 0 ? (processed * 100.0 / total) : 0;
                            pbProgress.Value = (int)Math.Min(100, Math.Max(0, percent));

                            double t = swTotal.Elapsed.TotalSeconds;
                            timePoints.Add(t);

                            // compute compression percentage: 100 * (1 - compressedBytes / originalBytesProcessed)
                            double originalBytes = processed * 4.0;
                            double compressionPercent = 0.0;
                            if (originalBytes > 0)
                                compressionPercent = 100.0 * (1.0 - (bytesCompressed / originalBytes));
                            compRates.Add(compressionPercent);

                            // instantaneous processing speed (samples/sec) for last block
                            int blockSamples = processed - lastProcessed;
                            double blockSeconds = Math.Max(1e-3, elapsedMs / 1000.0); // elapsedMs is block time in ms
                            double speed = blockSamples / blockSeconds;
                            speedPoints.Add(speed);

                            // remember for next update
                            lastProcessed = processed;

                            // update compression plot
                            formsPlotCompression.Plot.Clear();
                            var cpX = DataToArray(timePoints);
                            var cpY = DataToArray(compRates);
                            var cpScatter = formsPlotCompression.Plot.Add.Scatter(cpX, cpY);
                            cpScatter.MarkerSize = 0;
                            cpScatter.LineWidth = 2;
                            formsPlotCompression.Plot.Title("Compression % (higher = more reduction)");
                            formsPlotCompression.Plot.XLabel("Time (s)");
                            formsPlotCompression.Plot.YLabel("% reduction");
                            // immediately autoscale axes so new points are visible
                            formsPlotCompression.Plot.Axes.AutoScale();
                            formsPlotCompression.Refresh();

                            // update speed plot
                            formsPlotSpeed.Plot.Clear();
                            var spX = DataToArray(timePoints);
                            var spY = DataToArray(speedPoints);
                            var spScatter = formsPlotSpeed.Plot.Add.Scatter(spX, spY);
                            spScatter.MarkerSize = 0;
                            spScatter.LineWidth = 2;
                            formsPlotSpeed.Plot.Title("Processing Speed (samples/sec)");
                            // immediately autoscale axes so speed changes show
                            formsPlotSpeed.Plot.Axes.AutoScale();
                            formsPlotSpeed.Refresh();
                        });
                    }, token), token);

                // if completed without cancellation, prompt Save As
                if (!token.IsCancellationRequested)
                {
                    compressedData = result;

                    double originalSizeMB = originalFileSizeMB;
                    double compressedSizeMB;

                    if (result is byte[] byteResult)
                    {
                        compressedSizeMB = byteResult.Length / (1024.0 * 1024.0);
                        compressedData = byteResult;
                        isCompressedAsBytes = true;
                    }
                    else if (result is float[] floatResult)
                    {
                        compressedSizeMB = (floatResult.Length * 4.0) / (1024.0 * 1024.0);
                        compressedData = floatResult;
                        isCompressedAsBytes = false;
                    }
                    else
                    {
                        compressedSizeMB = 0;
                        compressedData = null;
                        isCompressedAsBytes = false;
                    }
                    double savingsPercent = 100.0 * (1.0 - (compressedSizeMB / originalSizeMB));
                    if (savingsPercent < 0) savingsPercent = 0;
                    double timeSeconds = swTotal.Elapsed.TotalSeconds;

                    int sampleBitRate = GetBitRateForAlgorithm(algo);
                    int samplingRate = audioSampleRate;

                    // Add compression info to the info text box
                    string reportText = "";
                    reportText += "-----------------------------------------" + Environment.NewLine;
                    reportText += "        COMPRESSION REPORT" + Environment.NewLine;
                    reportText += "-----------------------------------------" + Environment.NewLine;
                    reportText += Environment.NewLine;
                    reportText += $"Original File Size:     {originalSizeMB:F2} MB" + Environment.NewLine;
                    reportText += $"Compressed File Size:   {compressedSizeMB:F2} MB" + Environment.NewLine;
                    reportText += $"Compression Savings:    {savingsPercent:F2}%" + Environment.NewLine;
                    reportText += $"Processing Time:        {timeSeconds:F2} seconds" + Environment.NewLine;
                    reportText += Environment.NewLine;
                    reportText += "-----------------------------------------" + Environment.NewLine;
                    reportText += "        ALGORITHM SETTINGS" + Environment.NewLine;
                    reportText += "-----------------------------------------" + Environment.NewLine;
                    reportText += $"Algorithm Used:         {algo}" + Environment.NewLine;
                    reportText += $"Sample Bit Rate:        {sampleBitRate} bit/sample" + Environment.NewLine;
                    reportText += $"Sampling Rate:          {samplingRate} Hz" + Environment.NewLine;

                    this.BeginInvoke(new Action(() =>
                    {
                        txtInfo.Text += Environment.NewLine + reportText;
                    }));
                }
                else
                {
                    MessageBox.Show("Compression cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Compression cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Compression failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                compressionCts?.Dispose();
                compressionCts = null;
                pbProgress.Value = 0;
                btnCancel.Enabled = false;
                btnCompress.Enabled = true;
                btnDecompress.Enabled = true;
            }
        }

        // Cancel handler
        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            compressionCts?.Cancel();
        }

        // Core processing with chunked operation, progress callback, and cancellation support.
        // progressCallback parameters: processedSamplesSoFar, totalSamples, bytesCompressedSoFar, elapsedMsSinceStart
        private object ProcessCompressionWithProgress(string algorithm, int quantLevels, Action<int, int, double, double> progressCallback, CancellationToken token)
        {
            const int blockSize = 4096; // smaller block for more frequent updates
            int total = audioSamples.Length;
            var output = new List<float>(total);
            var sw = Stopwatch.StartNew();
            var compressor = new AudioCompressor();

            // stateful variables for algorithms that need continuity across blocks
            const float predGain = 0.9f;
            float dmStep = 0.05f;

            List<byte> allBytes = new List<byte>();
            bool isPacked = false;

            double compressedBytesSoFar = 0.0;
            double originalBytesSoFar = 0.0;

            for (int offset = 0; offset < total; offset += blockSize)
            {
                token.ThrowIfCancellationRequested();

                int len = Math.Min(blockSize, total - offset);
                var block = new float[len];
                Array.Copy(audioSamples, offset, block, 0, len);

                var swBlock = Stopwatch.StartNew();

                switch (algorithm)
                {
                    case "Nonlinear Quantization":
                        {
                            int mu = Math.Max(1, quantLevels);
                            byte[] packed = compressor.ApplyNonlinearQuantization(block, mu);
                            allBytes.AddRange(packed);
                            isPacked = true;
                        }
                        break;

                    case "DPCM":
                        {
                            byte[] packed = compressor.ApplyDPCM(block, 8);
                            allBytes.AddRange(packed);
                            isPacked = true;
                        }
                        break;

                    case "Predictive Differential Coding":
                        {
                            byte[] packed = compressor.ApplyPredictiveCoding(block, predGain, 8);
                            allBytes.AddRange(packed);
                            isPacked = true;
                        }
                        break;

                    case "Delta Modulation":
                        {
                            byte[] packed = compressor.ApplyDeltaModulation(block, dmStep);
                            allBytes.AddRange(packed);
                            isPacked = true;
                        }
                        break;

                    case "Adaptive Delta Modulation":
                        {
                            byte[] packed = compressor.ApplyAdaptiveDeltaModulation(block);
                            allBytes.AddRange(packed);
                            isPacked = true;
                        }
                        break;

                    default:
                        throw new InvalidOperationException("Unknown algorithm: " + algorithm);
                }

                swBlock.Stop();
                compressedBytesSoFar = allBytes.Count;
                double blockElapsedMs = Math.Max(1.0, swBlock.Elapsed.TotalMilliseconds);
                progressCallback?.Invoke(offset + len, total, compressedBytesSoFar, blockElapsedMs);
            }

            sw.Stop();

            if (isPacked)
            {
                return allBytes.ToArray();
            }

            return output.ToArray();
        }

        // helper to convert List<double> to arrays (for plotting)
        private static double[] DataToArray(List<double> list)
        {
            if (list == null || list.Count == 0) return Array.Empty<double>();
            return list.ToArray();
        }

        // SaveCompressedFile and SaveDecompressedAs16Bit remain unchanged (existing implementations)
        private void SaveCompressedFile(float[] data, string fileName)
        {
            int sampleRate = audioSampleRate > 0 ? audioSampleRate : 44100;
            var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1);
            using (var writer = new WaveFileWriter(fileName, waveFormat))
            {
                writer.WriteSamples(data, 0, data.Length);
            }
        }

        private void SaveDecompressedAs16Bit(float[] data, int sampleRate, string outPath)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("No audio data to save.", nameof(data));

            float max = 0f;
            for (int i = 0; i < data.Length; i++)
            {
                float v = data[i];
                if (float.IsNaN(v) || float.IsInfinity(v)) v = 0f;
                float abs = Math.Abs(v);
                if (abs > max) max = abs;
            }

            float scale = 1f;
            if (max > 0f)
            {
                scale = 0.98f / max;
            }

            var shorts = new short[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                float v = data[i] * scale;
                if (v > 1f) v = 1f;
                if (v < -1f) v = -1f;
                shorts[i] = (short)Math.Round(v * short.MaxValue);
            }

            byte[] bytes = new byte[shorts.Length * sizeof(short)];
            Buffer.BlockCopy(shorts, 0, bytes, 0, bytes.Length);

            var waveFormat = new WaveFormat(sampleRate, 16, 1);
            using var writer = new WaveFileWriter(outPath, waveFormat);
            writer.Write(bytes, 0, bytes.Length);
        }

        private int GetBitRateForAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "Delta Modulation":
                case "Adaptive Delta Modulation":
                    return 1;  // 1 bit per sample

                case "Nonlinear Quantization":
                    int mu = (int)nudQuant.Value;
                    if (mu <= 256) return 8;   // 8 bits per sample
                    return 16;  // 16 bits per sample

                case "DPCM":
                case "Predictive Differential Coding":
                    return 16;  // 16 bits per sample (float)

                default:
                    return 16;
            }
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Compressed Files|*.bin",
                Title = "Open compressed WAV"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string filePath = ofd.FileName;
            int sampleRate = 44100;

            byte[] compressedBytes = System.IO.File.ReadAllBytes(filePath);

            if (compressedBytes == null || compressedBytes.Length == 0)
            {
                MessageBox.Show("No samples in selected file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbAlgorithm.SelectedItem == null)
            {
                MessageBox.Show("Select the algorithm used to compress the file.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var compressor = new AudioCompressor();
            string algo = cmbAlgorithm.SelectedItem.ToString() ?? string.Empty;
            float[] decompressed = null!;
            int totalSamples = audioSamples?.Length ?? 0;

            try
            {

                switch (algo)
                {
                    case "Nonlinear Quantization":
                        decompressed = compressor.DecompressNonlinearQuantization(compressedBytes, (int)nudQuant.Value, totalSamples);
                        break;

                    case "DPCM":
                        decompressed = compressor.DecompressDPCM(compressedBytes, 8, -1f, 1f);
                        break;

                    case "Predictive Differential Coding":
                        decompressed = compressor.DecompressPredictiveCoding(compressedBytes, 0.9f, 8, -1f, 1f);
                        break;

                    case "Delta Modulation":
                        decompressed = compressor.DecompressDeltaModulation(compressedBytes, totalSamples, 0.05f);
                        break;

                    case "Adaptive Delta Modulation":
                        decompressed = compressor.DecompressAdaptiveDeltaModulation(compressedBytes, totalSamples, 0.05f);
                        break;

                    default:
                        MessageBox.Show("Decompression not implemented for selected algorithm.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Decompression failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (decompressed == null || decompressed.Length == 0)
            {
                MessageBox.Show("Decompression produced no data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "WAV Files|*.wav",
                Title = "Save decompressed audio (16-bit PCM)",
                FileName = "DecompressedAudio.wav",
                DefaultExt = "wav"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                SaveDecompressedAs16Bit(decompressed, sampleRate, sfd.FileName);
                MessageBox.Show("Decompressed file saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save decompressed file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveCompressedBytes(byte[] data, string fileName)
        {
            System.IO.File.WriteAllBytes(fileName, data);
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            if (cmbAlgorithm.Items.Count > 0)
            {
                cmbAlgorithm.SelectedIndex = 0;
            }
            nudQuant.Value = 256;
            pbProgress.Value = 0;

            compressedData = null;

            outputDevice?.Stop();

            if (audioSamples != null && audioSamples.Length > 0 && !string.IsNullOrEmpty(currentAudioFilePath))
            {
                try
                {
                    LoadAndPlay(currentAudioFilePath, autoPlay: false);

                    using var readerForSamples = new AudioFileReader(currentAudioFilePath);
                    audioSamples = new float[readerForSamples.Length / 4];
                    readerForSamples.Read(audioSamples, 0, audioSamples.Length);
                    audioSampleRate = readerForSamples.WaveFormat.SampleRate;

                    if (audioFile != null)
                        audioFile.Position = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error resetting audio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Settings have been reset to default values.\n(No audio file was loaded)",
                                "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (compressedData == null)
            {
                MessageBox.Show("No compressed data to save. Please compress first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Compressed Files|*.bin",
                Title = "Save compressed audio as",
                FileName = "CompressedAudio.bin",
                DefaultExt = "bin"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (isCompressedAsBytes && compressedData is byte[] byteData)
                    {
                        SaveCompressedBytes(byteData, sfd.FileName);
                        MessageBox.Show("Compressed file saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (compressedData is float[] floatData)
                    {
                        SaveCompressedFile(floatData, sfd.FileName);
                        MessageBox.Show("Compressed file saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
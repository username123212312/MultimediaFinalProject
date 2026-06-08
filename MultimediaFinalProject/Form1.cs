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

            try
            {
                // read samples for compressor usage (unchanged)
                using var readerForSamples = new AudioFileReader(filePath);
                audioSamples = new float[readerForSamples.Length / 4];
                readerForSamples.Read(audioSamples, 0, audioSamples.Length);
                audioSampleRate = readerForSamples.WaveFormat.SampleRate;

                btnCompress.Enabled = true;

                // initialize playback & plot
                LoadAndPlay(filePath, autoPlay: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading audio: " + ex.Message);
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
                float[] result = await Task.Run(() =>
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
                    using var sfd = new SaveFileDialog
                    {
                        Filter = "WAV Files|*.wav",
                        Title = "Save compressed audio as",
                        FileName = "CompressedAudio.wav",
                        DefaultExt = "wav"
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        SaveCompressedFile(result, sfd.FileName);
                        MessageBox.Show("Compressed file saved: " + sfd.FileName, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
        private float[] ProcessCompressionWithProgress(string algorithm, int quantLevels, Action<int, int, double, double> progressCallback, CancellationToken token)
        {
            const int blockSize = 4096; // smaller block for more frequent updates
            int total = audioSamples.Length;
            var output = new List<float>(total);
            var sw = Stopwatch.StartNew();

            // stateful variables for algorithms that need continuity across blocks
            float dpcmPrev = 0f;
            float predPrev = 0f;
            const float predGain = 0.9f;
            float dmCurrent = 0f;
            float admCurrent = 0f;
            float admStep = 0.05f;
            float dmStep = 0.05f;

            double compressedBytesSoFar = 0.0;
            double originalBytesSoFar = 0.0;

            for (int offset = 0; offset < total; offset += blockSize)
            {
                token.ThrowIfCancellationRequested();

                int len = Math.Min(blockSize, total - offset);
                var block = new float[len];
                Array.Copy(audioSamples, offset, block, 0, len);

                var swBlock = Stopwatch.StartNew();

                float[] compressedBlock;

                switch (algorithm)
                {
                    case "Nonlinear Quantization":
                        compressedBlock = new float[len];
                        int mu = Math.Max(1, quantLevels);
                        for (int i = 0; i < len; i++)
                        {
                            if ((i & 63) == 0) token.ThrowIfCancellationRequested();

                            float x = block[i];
                            compressedBlock[i] = Math.Sign(x) * (float)(Math.Log(1 + mu * Math.Abs(x)) / Math.Log(1 + mu));
                        }
                        break;

                    case "DPCM":
                        compressedBlock = new float[len];
                        for (int i = 0; i < len; i++)
                        {
                            if ((i & 63) == 0) token.ThrowIfCancellationRequested();

                            compressedBlock[i] = block[i] - dpcmPrev;
                            dpcmPrev = block[i];
                        }
                        break;

                    case "Predictive Differential Coding":
                        compressedBlock = new float[len];
                        for (int i = 0; i < len; i++)
                        {
                            if ((i & 63) == 0) token.ThrowIfCancellationRequested();

                            compressedBlock[i] = block[i] - (predPrev * predGain);
                            predPrev = block[i];
                        }
                        break;

                    case "Delta Modulation":
                        compressedBlock = new float[len];
                        for (int i = 0; i < len; i++)
                        {
                            if ((i & 63) == 0) token.ThrowIfCancellationRequested();

                            int bit = (block[i] > dmCurrent) ? 1 : 0;
                            compressedBlock[i] = bit == 1 ? 1f : 0f;
                            dmCurrent += (bit == 1) ? dmStep : -dmStep;
                        }
                        break;

                    case "Adaptive Delta Modulation":
                        compressedBlock = new float[len];
                        for (int i = 0; i < len; i++)
                        {
                            if ((i & 63) == 0) token.ThrowIfCancellationRequested();

                            int bit = (block[i] > admCurrent) ? 1 : 0;
                            compressedBlock[i] = bit == 1 ? 1f : 0f;
                            admCurrent += (bit == 1) ? admStep : -admStep;
                            admStep = (block[i] > admCurrent) ? admStep * 1.1f : admStep * 0.9f;
                            if (admStep < 1e-6f) admStep = 1e-6f;
                        }
                        break;

                    default:
                        throw new InvalidOperationException("Unknown algorithm: " + algorithm);
                }

                // append compressedBlock to output
                output.AddRange(compressedBlock);

                // account bytes (we store as floats currently)
                compressedBytesSoFar = output.Count * 4.0;
                originalBytesSoFar += len * 4.0;

                swBlock.Stop();
                double elapsedMs = sw.Elapsed.TotalMilliseconds;
                double blockElapsedMs = Math.Max(1.0, swBlock.Elapsed.TotalMilliseconds); // avoid div by zero

                // report processed samples, total samples, compressed bytes and blockElapsedMs to compute speed externally
                progressCallback?.Invoke(offset + len, total, compressedBytesSoFar, blockElapsedMs);
            }

            sw.Stop();
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

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "WAV Files|*.wav",
                Title = "Open compressed WAV"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string filePath = ofd.FileName;
            float[] compressedSamples;
            int sampleRate = 44100;

            try
            {
                using var reader = new WaveFileReader(filePath);
                sampleRate = reader.WaveFormat.SampleRate;
                int channels = reader.WaveFormat.Channels;
                var sampleProvider = reader.ToSampleProvider();

                var samplesList = new List<float>();
                float[] buffer = new float[4096 * Math.Max(1, channels)];
                int read;
                while ((read = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (channels == 1)
                    {
                        for (int i = 0; i < read; i++) samplesList.Add(buffer[i]);
                    }
                    else
                    {
                        int frames = read / channels;
                        for (int f = 0; f < frames; f++)
                        {
                            float sum = 0;
                            for (int c = 0; c < channels; c++) sum += buffer[f * channels + c];
                            samplesList.Add(sum / channels);
                        }

                        int trailing = read % channels;
                        if (trailing != 0)
                        {
                            float sum = 0;
                            for (int t = read - trailing; t < read; t++) sum += buffer[t];
                            samplesList.Add(sum / trailing);
                        }
                    }
                }

                compressedSamples = samplesList.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to read compressed WAV: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (compressedSamples == null || compressedSamples.Length == 0)
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

            try
            {
                switch (algo)
                {
                    case "Nonlinear Quantization":
                        decompressed = compressor.DecompressNonlinearQuantization(compressedSamples, (int)nudQuant.Value);
                        break;
                    case "DPCM":
                        decompressed = compressor.DecompressDPCM(compressedSamples);
                        break;
                    case "Predictive Differential Coding":
                        decompressed = compressor.DecompressPredictiveCoding(compressedSamples);
                        break;
                    case "Delta Modulation":
                        {
                            var bits = Array.ConvertAll(compressedSamples, s => s >= 0.5f ? 1 : 0);
                            decompressed = compressor.DecompressDeltaModulation(bits);
                        }
                        break;
                    case "Adaptive Delta Modulation":
                        {
                            var bits = Array.ConvertAll(compressedSamples, s => s >= 0.5f ? 1 : 0);
                            decompressed = compressor.DecompressAdaptiveDeltaModulation(bits);
                        }
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

            using var sfd = new SaveFileDialog
            {
                Filter = "WAV Files|*.wav",
                Title = "Save decompressed audio (16-bit PCM)",
                FileName = Path.GetFileNameWithoutExtension(filePath) + "_decompressed.wav",
                DefaultExt = "wav"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                SaveDecompressedAs16Bit(decompressed, sampleRate, sfd.FileName);
                MessageBox.Show("Decompressed file saved: " + sfd.FileName, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save decompressed file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
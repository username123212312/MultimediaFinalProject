using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TagLib;

namespace MultimediaFinalProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.aac",
                Title = "Select an audio file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PlayAudio(openFileDialog.FileName);
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop) ?? Array.Empty<string>();
                if (files.Length > 0)
                {
                    PlayAudio(files[0]);
                }
            }
        }

        private void PlayAudio(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                MessageBox.Show("File not found: " + filePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var psi = new ProcessStartInfo(filePath)
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to start playback: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DisplayAudioInfo(filePath);
        }

        private void DisplayAudioInfo(string filePath)
        {
            try
            {
                using var tagFile = TagLib.File.Create(filePath);
                var props = tagFile.Properties;
                var info = new FileInfo(filePath);

                TimeSpan dur = props?.Duration ?? TimeSpan.Zero;
                string duration = dur.ToString(@"hh\:mm\:ss");

                txtInfo.Text =
                    "File Name: " + info.Name + Environment.NewLine +
                    "Size: " + (info.Length / 1024.0 / 1024.0).ToString("0.00") + " MB" + Environment.NewLine +
                    "Duration: " + duration + Environment.NewLine +
                    "Sampling Rate: " + (props?.AudioSampleRate ?? 0) + " Hz" + Environment.NewLine +
                    "Channels: " + (props?.AudioChannels ?? 0) + Environment.NewLine +
                    "Bit Rate: " + (props?.AudioBitrate ?? 0) + " kbps" + Environment.NewLine +
                    "Encoding Type: " + info.Extension.ToUpper().Replace(".", "") + Environment.NewLine;
            }
            catch (Exception ex)
            {
                txtInfo.Text = "Error reading metadata:" + Environment.NewLine + ex.Message;
            }
        }
    }
}
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ScottPlot.WinForms;

namespace MultimediaFinalProject
{
    partial class Form1
    {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new Container();
            button1 = new Button();
            playAudio = new Button();
            stopAudio = new Button();
            cmbAlgorithm = new ComboBox();
            nudQuant = new NumericUpDown();
            btnCompress = new Button();
            btnDecompress = new Button();
            txtInfo = new TextBox();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            pbProgress = new ProgressBar();
            btnCancel = new Button();
            formsPlotCompression = new ScottPlot.WinForms.FormsPlot();
            formsPlotSpeed = new ScottPlot.WinForms.FormsPlot();

            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(125, 36);
            button1.TabIndex = 0;
            button1.Text = "Select Audio";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // playAudio
            // 
            playAudio.Location = new Point(150, 12);
            playAudio.Name = "playAudio";
            playAudio.Size = new Size(110, 36);
            playAudio.TabIndex = 1;
            playAudio.Text = "Play";
            playAudio.UseVisualStyleBackColor = true;
            playAudio.Click += playAudio_Click;
            // 
            // stopAudio
            // 
            stopAudio.Location = new Point(270, 12);
            stopAudio.Name = "stopAudio";
            stopAudio.Size = new Size(110, 36);
            stopAudio.TabIndex = 2;
            stopAudio.Text = "Stop";
            stopAudio.UseVisualStyleBackColor = true;
            stopAudio.Click += stopAudio_Click;
            // 
            // cmbAlgorithm
            // 
            cmbAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAlgorithm.Location = new Point(12, 70);
            cmbAlgorithm.Name = "cmbAlgorithm";
            cmbAlgorithm.Size = new Size(250, 23);
            cmbAlgorithm.TabIndex = 3;
            cmbAlgorithm.Items.AddRange(new object[] {
                "Nonlinear Quantization",
                "DPCM",
                "Predictive Differential Coding",
                "Delta Modulation",
                "Adaptive Delta Modulation"
            });
            cmbAlgorithm.SelectedIndex = 0;
            // 
            // nudQuant
            // 
            nudQuant.Location = new Point(12, 110);
            nudQuant.Minimum = 2;
            nudQuant.Maximum = 65536;
            nudQuant.Value = 256;
            nudQuant.Name = "nudQuant";
            nudQuant.Size = new Size(120, 23);
            nudQuant.TabIndex = 4;
            // 
            // btnCompress
            // 
            btnCompress.Location = new Point(12, 150);
            btnCompress.Name = "btnCompress";
            btnCompress.Size = new Size(120, 30);
            btnCompress.TabIndex = 5;
            btnCompress.Text = "Compress";
            btnCompress.UseVisualStyleBackColor = true;
            btnCompress.Enabled = false;
            btnCompress.Click += btnCompress_Click;
            // 
            // btnDecompress
            // 
            btnDecompress.Location = new Point(150, 150);
            btnDecompress.Name = "btnDecompress";
            btnDecompress.Size = new Size(112, 30);
            btnDecompress.TabIndex = 6;
            btnDecompress.Text = "Decompress";
            btnDecompress.UseVisualStyleBackColor = true;
            btnDecompress.Click += btnDecompress_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(270, 150);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(92, 30);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Enabled = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(12, 192);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(350, 20);
            pbProgress.TabIndex = 8;
            // 
            // txtInfo
            // 
            txtInfo.Location = new Point(560, 12);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Size = new Size(220, 260);
            txtInfo.TabIndex = 7;
            // 
            // formsPlot1 (waveform)
            // 
            formsPlot1.Location = new Point(12, 220);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(380, 210);
            formsPlot1.TabIndex = 10;
            // 
            // formsPlotCompression (rate)
            // 
            formsPlotCompression.Location = new Point(405, 220);
            formsPlotCompression.Name = "formsPlotCompression";
            formsPlotCompression.Size = new Size(190, 100);
            formsPlotCompression.TabIndex = 11;
            // 
            // formsPlotSpeed (processing speed)
            // 
            formsPlotSpeed.Location = new Point(405, 330);
            formsPlotSpeed.Name = "formsPlotSpeed";
            formsPlotSpeed.Size = new Size(380, 100);
            formsPlotSpeed.TabIndex = 12;

            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);

            Controls.Add(formsPlotSpeed);
            Controls.Add(formsPlotCompression);
            Controls.Add(formsPlot1);
            Controls.Add(pbProgress);
            Controls.Add(btnCancel);
            Controls.Add(txtInfo);
            Controls.Add(btnDecompress);
            Controls.Add(btnCompress);
            Controls.Add(nudQuant);
            Controls.Add(cmbAlgorithm);
            Controls.Add(stopAudio);
            Controls.Add(playAudio);
            Controls.Add(button1);

            Name = "Form1";
            Text = "MultimediaFinalProject";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button playAudio;
        private Button stopAudio;
        private ComboBox cmbAlgorithm;
        private NumericUpDown nudQuant;
        private Button btnCompress;
        private Button btnDecompress;
        private TextBox txtInfo;
        private FormsPlot formsPlot1;
        private ProgressBar pbProgress;
        private Button btnCancel;
        private FormsPlot formsPlotCompression;
        private FormsPlot formsPlotSpeed;
    }
}

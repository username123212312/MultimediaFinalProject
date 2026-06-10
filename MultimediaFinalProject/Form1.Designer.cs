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
            flowTop = new FlowLayoutPanel();
            button1 = new Button();
            playAudio = new Button();
            stopAudio = new Button();
            cmbAlgorithm = new ComboBox();
            nudQuant = new NumericUpDown();
            btnCompress = new Button();
            btnDecompress = new Button();
            btnCancel = new Button();
            btnSave = new Button();
            btnReset = new Button();
            lblRate = new Label();
            nudSampleRate = new NumericUpDown();
            pbProgress = new ProgressBar();
            tableMain = new TableLayoutPanel();
            formsPlot1 = new FormsPlot();
            rightColumn = new TableLayoutPanel();
            txtInfo = new TextBox();
            formsPlotCompression = new FormsPlot();
            formsPlotSpeed = new FormsPlot();
            flowTop.SuspendLayout();
            ((ISupportInitialize)nudQuant).BeginInit();
            ((ISupportInitialize)nudSampleRate).BeginInit();
            tableMain.SuspendLayout();
            rightColumn.SuspendLayout();
            SuspendLayout();
            // 
            // flowTop
            // 
            flowTop.AutoSize = true;
            flowTop.Controls.Add(button1);
            flowTop.Controls.Add(playAudio);
            flowTop.Controls.Add(stopAudio);
            flowTop.Controls.Add(cmbAlgorithm);
            flowTop.Controls.Add(nudQuant);
            flowTop.Controls.Add(btnCompress);
            flowTop.Controls.Add(btnDecompress);
            flowTop.Controls.Add(btnCancel);
            flowTop.Controls.Add(btnSave);
            flowTop.Controls.Add(btnReset);
            flowTop.Controls.Add(lblRate);
            flowTop.Controls.Add(nudSampleRate);
            flowTop.Dock = DockStyle.Top;
            flowTop.Location = new Point(0, 0);
            flowTop.Margin = new Padding(3, 4, 3, 4);
            flowTop.Name = "flowTop";
            flowTop.Padding = new Padding(9, 11, 9, 11);
            flowTop.Size = new Size(1257, 109);
            flowTop.TabIndex = 2;
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.Location = new Point(12, 15);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(118, 40);
            button1.TabIndex = 0;
            button1.Text = "Select Audio";
            button1.Click += button1_Click;
            // 
            // playAudio
            // 
            playAudio.AutoSize = true;
            playAudio.Location = new Point(136, 15);
            playAudio.Margin = new Padding(3, 4, 3, 4);
            playAudio.Name = "playAudio";
            playAudio.Size = new Size(86, 40);
            playAudio.TabIndex = 1;
            playAudio.Text = "Play";
            playAudio.Click += playAudio_Click;
            // 
            // stopAudio
            // 
            stopAudio.AutoSize = true;
            stopAudio.Location = new Point(228, 15);
            stopAudio.Margin = new Padding(3, 4, 3, 4);
            stopAudio.Name = "stopAudio";
            stopAudio.Size = new Size(86, 40);
            stopAudio.TabIndex = 2;
            stopAudio.Text = "Stop";
            stopAudio.Click += stopAudio_Click;
            // 
            // cmbAlgorithm
            // 
            cmbAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAlgorithm.Items.AddRange(new object[] { "Nonlinear Quantization", "DPCM", "Predictive Differential Coding", "Delta Modulation", "Adaptive Delta Modulation" });
            cmbAlgorithm.Location = new Point(320, 15);
            cmbAlgorithm.Margin = new Padding(3, 4, 3, 4);
            cmbAlgorithm.Name = "cmbAlgorithm";
            cmbAlgorithm.Size = new Size(365, 28);
            cmbAlgorithm.TabIndex = 4;
            // 
            // nudQuant
            // 
            nudQuant.Location = new Point(691, 15);
            nudQuant.Margin = new Padding(3, 4, 3, 4);
            nudQuant.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            nudQuant.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            nudQuant.Name = "nudQuant";
            nudQuant.Size = new Size(114, 27);
            nudQuant.TabIndex = 6;
            nudQuant.Value = new decimal(new int[] { 256, 0, 0, 0 });
            // 
            // btnCompress
            // 
            btnCompress.AutoSize = true;
            btnCompress.Enabled = false;
            btnCompress.Location = new Point(811, 15);
            btnCompress.Margin = new Padding(3, 4, 3, 4);
            btnCompress.Name = "btnCompress";
            btnCompress.Size = new Size(96, 40);
            btnCompress.TabIndex = 7;
            btnCompress.Text = "Compress";
            btnCompress.Click += btnCompress_Click;
            // 
            // btnDecompress
            // 
            btnDecompress.AutoSize = true;
            btnDecompress.Location = new Point(913, 15);
            btnDecompress.Margin = new Padding(3, 4, 3, 4);
            btnDecompress.Name = "btnDecompress";
            btnDecompress.Size = new Size(115, 40);
            btnDecompress.TabIndex = 8;
            btnDecompress.Text = "Decompress";
            btnDecompress.Click += btnDecompress_Click;
            // 
            // btnCancel
            // 
            btnCancel.AutoSize = true;
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(1034, 15);
            btnCancel.Margin = new Padding(3, 4, 3, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(86, 40);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(1126, 15);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(86, 40);
            btnSave.TabIndex = 10;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(12, 63);
            btnReset.Margin = new Padding(3, 4, 3, 4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(86, 31);
            btnReset.TabIndex = 11;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // lblRate
            // 
            lblRate.Location = new Point(104, 59);
            lblRate.Name = "lblRate";
            lblRate.Size = new Size(100, 23);
            lblRate.TabIndex = 0;
            // 
            // nudSampleRate
            // 
            nudSampleRate.Location = new Point(12,300);
            nudSampleRate.Name = "nudSampleRate";
            nudSampleRate.Size = new Size(114, 27);
            nudSampleRate.TabIndex = 1;
            // 
            // pbProgress
            // 
            pbProgress.Dock = DockStyle.Top;
            pbProgress.Location = new Point(0, 109);
            pbProgress.Margin = new Padding(9, 11, 9, 11);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(1257, 32);
            pbProgress.TabIndex = 1;
            // 
            // tableMain
            // 
            tableMain.ColumnCount = 2;
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableMain.Controls.Add(formsPlot1, 0, 0);
            tableMain.Controls.Add(rightColumn, 1, 0);
            tableMain.Dock = DockStyle.Fill;
            tableMain.Location = new Point(0, 141);
            tableMain.Margin = new Padding(3, 4, 3, 4);
            tableMain.Name = "tableMain";
            tableMain.Padding = new Padding(9, 11, 9, 11);
            tableMain.RowCount = 1;
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.Size = new Size(1257, 819);
            tableMain.TabIndex = 0;
            // 
            // formsPlot1
            // 
            formsPlot1.Dock = DockStyle.Fill;
            formsPlot1.Location = new Point(16, 19);
            formsPlot1.Margin = new Padding(7, 8, 7, 8);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(853, 781);
            formsPlot1.TabIndex = 0;
            // 
            // rightColumn
            // 
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 352F));
            rightColumn.Controls.Add(txtInfo, 0, 0);
            rightColumn.Controls.Add(formsPlotCompression, 0, 1);
            rightColumn.Controls.Add(formsPlotSpeed, 0, 2);
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(879, 15);
            rightColumn.Margin = new Padding(3, 4, 3, 4);
            rightColumn.Name = "rightColumn";
            rightColumn.Padding = new Padding(7, 8, 7, 8);
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            rightColumn.Size = new Size(366, 789);
            rightColumn.TabIndex = 1;
            // 
            // txtInfo
            // 
            txtInfo.Dock = DockStyle.Fill;
            txtInfo.Location = new Point(14, 16);
            txtInfo.Margin = new Padding(7, 8, 7, 8);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Size = new Size(338, 293);
            txtInfo.TabIndex = 0;
            // 
            // formsPlotCompression
            // 
            formsPlotCompression.Dock = DockStyle.Fill;
            formsPlotCompression.Location = new Point(14, 325);
            formsPlotCompression.Margin = new Padding(7, 8, 7, 8);
            formsPlotCompression.Name = "formsPlotCompression";
            formsPlotCompression.Size = new Size(338, 215);
            formsPlotCompression.TabIndex = 1;
            // 
            // formsPlotSpeed
            // 
            formsPlotSpeed.Dock = DockStyle.Fill;
            formsPlotSpeed.Location = new Point(14, 556);
            formsPlotSpeed.Margin = new Padding(7, 8, 7, 8);
            formsPlotSpeed.Name = "formsPlotSpeed";
            formsPlotSpeed.Size = new Size(338, 217);
            formsPlotSpeed.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1257, 960);
            Controls.Add(tableMain);
            Controls.Add(pbProgress);
            Controls.Add(flowTop);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "MultimediaFinalProject";
            flowTop.ResumeLayout(false);
            flowTop.PerformLayout();
            ((ISupportInitialize)nudQuant).EndInit();
            ((ISupportInitialize)nudSampleRate).EndInit();
            tableMain.ResumeLayout(false);
            rightColumn.ResumeLayout(false);
            rightColumn.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private NumericUpDown nudSampleRate;
        private FlowLayoutPanel flowTop;
        private TableLayoutPanel tableMain;
        private TableLayoutPanel rightColumn;

        private Button button1;
        private Button playAudio;
        private Button stopAudio;
        private ComboBox cmbAlgorithm;
        private NumericUpDown nudQuant;
        private Button btnCompress;
        private Button btnDecompress;
        private Button btnCancel;
        private Button btnReset;
        private Button btnSave;
        private TextBox txtInfo;
        private FormsPlot formsPlot1;
        private ProgressBar pbProgress;
        private FormsPlot formsPlotCompression;
        private FormsPlot formsPlotSpeed;
        private Label lblRate;
    }
}

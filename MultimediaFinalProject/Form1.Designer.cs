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
            pbProgress = new ProgressBar();
            tableMain = new TableLayoutPanel();
            formsPlot1 = new FormsPlot();
            rightColumn = new TableLayoutPanel();
            txtInfo = new TextBox();
            formsPlotCompression = new FormsPlot();
            formsPlotSpeed = new FormsPlot();
            btnSave = new Button();
            btnReset = new Button();
            flowTop.SuspendLayout();
            ((ISupportInitialize)nudQuant).BeginInit();
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
            flowTop.Dock = DockStyle.Top;
            flowTop.Location = new Point(0, 0);
            flowTop.Name = "flowTop";
            flowTop.Padding = new Padding(8);
            flowTop.Size = new Size(1100, 78);
            flowTop.TabIndex = 2;
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.Location = new Point(11, 11);
            button1.Name = "button1";
            button1.Size = new Size(83, 25);
            button1.TabIndex = 0;
            button1.Text = "Select Audio";
            button1.Click += button1_Click;
            // 
            // playAudio
            // 
            playAudio.AutoSize = true;
            playAudio.Location = new Point(100, 11);
            playAudio.Name = "playAudio";
            playAudio.Size = new Size(75, 25);
            playAudio.TabIndex = 1;
            playAudio.Text = "Play";
            playAudio.Click += playAudio_Click;
            // 
            // stopAudio
            // 
            stopAudio.AutoSize = true;
            stopAudio.Location = new Point(181, 11);
            stopAudio.Name = "stopAudio";
            stopAudio.Size = new Size(75, 25);
            stopAudio.TabIndex = 2;
            stopAudio.Text = "Stop";
            stopAudio.Click += stopAudio_Click;
            // 
            // cmbAlgorithm
            // 
            cmbAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAlgorithm.Items.AddRange(new object[] { "Nonlinear Quantization", "DPCM", "Predictive Differential Coding", "Delta Modulation", "Adaptive Delta Modulation" });
            cmbAlgorithm.Location = new Point(368, 11);
            cmbAlgorithm.Name = "cmbAlgorithm";
            cmbAlgorithm.Size = new Size(320, 23);
            cmbAlgorithm.TabIndex = 4;
            // 
            // nudQuant
            // 
            nudQuant.Location = new Point(800, 11);
            nudQuant.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            nudQuant.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            nudQuant.Name = "nudQuant";
            nudQuant.Size = new Size(100, 23);
            nudQuant.TabIndex = 6;
            nudQuant.Value = new decimal(new int[] { 256, 0, 0, 0 });
            // 
            // btnCompress
            // 
            btnCompress.AutoSize = true;
            btnCompress.Enabled = false;
            btnCompress.Location = new Point(906, 11);
            btnCompress.Name = "btnCompress";
            btnCompress.Size = new Size(75, 25);
            btnCompress.TabIndex = 7;
            btnCompress.Text = "Compress";
            btnCompress.Click += btnCompress_Click;
            // 
            // btnDecompress
            // 
            btnDecompress.AutoSize = true;
            btnDecompress.Location = new Point(987, 11);
            btnDecompress.Name = "btnDecompress";
            btnDecompress.Size = new Size(82, 25);
            btnDecompress.TabIndex = 8;
            btnDecompress.Text = "Decompress";
            btnDecompress.Click += btnDecompress_Click;
            // 
            // btnCancel
            // 
            btnCancel.AutoSize = true;
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(11, 42);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 25);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // pbProgress
            // 
            pbProgress.Dock = DockStyle.Top;
            pbProgress.Location = new Point(0, 78);
            pbProgress.Margin = new Padding(8);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(1100, 24);
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
            tableMain.Location = new Point(0, 102);
            tableMain.Name = "tableMain";
            tableMain.Padding = new Padding(8);
            tableMain.RowCount = 1;
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.Size = new Size(1100, 618);
            tableMain.TabIndex = 0;
            // 
            // formsPlot1
            // 
            formsPlot1.Dock = DockStyle.Fill;
            formsPlot1.Location = new Point(14, 14);
            formsPlot1.Margin = new Padding(6);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(746, 590);
            formsPlot1.TabIndex = 0;
            // 
            // rightColumn
            // 
            rightColumn.ColumnCount = 1;
            rightColumn.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            rightColumn.Controls.Add(txtInfo, 0, 0);
            rightColumn.Controls.Add(formsPlotCompression, 0, 1);
            rightColumn.Controls.Add(formsPlotSpeed, 0, 2);
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.Location = new Point(769, 11);
            rightColumn.Name = "rightColumn";
            rightColumn.Padding = new Padding(6);
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            rightColumn.Size = new Size(320, 596);
            rightColumn.TabIndex = 1;
            // 
            // txtInfo
            // 
            txtInfo.Dock = DockStyle.Fill;
            txtInfo.Location = new Point(12, 12);
            txtInfo.Margin = new Padding(6);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Size = new Size(296, 221);
            txtInfo.TabIndex = 0;
            // 
            // formsPlotCompression
            // 
            formsPlotCompression.Dock = DockStyle.Fill;
            formsPlotCompression.Location = new Point(12, 245);
            formsPlotCompression.Margin = new Padding(6);
            formsPlotCompression.Name = "formsPlotCompression";
            formsPlotCompression.Size = new Size(296, 163);
            formsPlotCompression.TabIndex = 1;
            // 
            // formsPlotSpeed
            // 
            formsPlotSpeed.Dock = DockStyle.Fill;
            formsPlotSpeed.Location = new Point(12, 420);
            formsPlotSpeed.Margin = new Padding(6);
            formsPlotSpeed.Name = "formsPlotSpeed";
            formsPlotSpeed.Size = new Size(296, 164);
            formsPlotSpeed.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(92, 42);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 10;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(173, 42);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 23);
            btnReset.TabIndex = 11;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 720);
            Controls.Add(tableMain);
            Controls.Add(pbProgress);
            Controls.Add(flowTop);
            Name = "Form1";
            Text = "MultimediaFinalProject";
            flowTop.ResumeLayout(false);
            flowTop.PerformLayout();
            ((ISupportInitialize)nudQuant).EndInit();
            tableMain.ResumeLayout(false);
            rightColumn.ResumeLayout(false);
            rightColumn.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
    }
}

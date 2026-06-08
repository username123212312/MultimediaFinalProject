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

            // Controls
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

            // flowTop (top controls)
            flowTop.Dock = DockStyle.Top;
            flowTop.AutoSize = true;
            flowTop.Padding = new Padding(8);
            flowTop.FlowDirection = FlowDirection.LeftToRight;
            flowTop.WrapContents = true;

            // button1
            button1.Text = "Select Audio";
            button1.AutoSize = true;
            button1.Click += button1_Click;

            // playAudio
            playAudio.Text = "Play";
            playAudio.AutoSize = true;
            playAudio.Click += playAudio_Click;

            // stopAudio
            stopAudio.Text = "Stop";
            stopAudio.AutoSize = true;
            stopAudio.Click += stopAudio_Click;

            // cmbAlgorithm
            cmbAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAlgorithm.Items.AddRange(new object[] {
                "Nonlinear Quantization",
                "DPCM",
                "Predictive Differential Coding",
                "Delta Modulation",
                "Adaptive Delta Modulation"
            });
            cmbAlgorithm.SelectedIndex = 0;
            cmbAlgorithm.Width = 320;

            // nudQuant
            nudQuant.Minimum = 2;
            nudQuant.Maximum = 65536;
            nudQuant.Value = 256;
            nudQuant.Width = 100;

            // btnCompress
            btnCompress.Text = "Compress";
            btnCompress.AutoSize = true;
            btnCompress.Enabled = false;
            btnCompress.Click += btnCompress_Click;

            // btnDecompress
            btnDecompress.Text = "Decompress";
            btnDecompress.AutoSize = true;
            btnDecompress.Click += btnDecompress_Click;

            // btnCancel
            btnCancel.Text = "Cancel";
            btnCancel.AutoSize = true;
            btnCancel.Enabled = false;
            btnCancel.Click += btnCancel_Click;

            // add controls to flowTop in desired order
            flowTop.Controls.Add(button1);
            flowTop.Controls.Add(playAudio);
            flowTop.Controls.Add(stopAudio);
            flowTop.Controls.Add(new Label() { Text = "Algorithm:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(12, 8, 0, 0) });
            flowTop.Controls.Add(cmbAlgorithm);
            flowTop.Controls.Add(new Label() { Text = "Quant:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(12, 8, 0, 0) });
            flowTop.Controls.Add(nudQuant);
            flowTop.Controls.Add(btnCompress);
            flowTop.Controls.Add(btnDecompress);
            flowTop.Controls.Add(btnCancel);

            // progress bar
            pbProgress.Dock = DockStyle.Top;
            pbProgress.Height = 24;
            pbProgress.Margin = new Padding(8);

            // tableMain: 2 columns (left big, right narrow)
            tableMain.Dock = DockStyle.Fill;
            tableMain.ColumnCount = 2;
            tableMain.RowCount = 1;
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F)); // waveform
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // right column
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.Padding = new Padding(8);

            // formsPlot1 (waveform) - left column
            formsPlot1.Dock = DockStyle.Fill;
            formsPlot1.Margin = new Padding(6);

            // rightColumn: 3 rows - txtInfo, compression plot, speed plot
            rightColumn.Dock = DockStyle.Fill;
            rightColumn.ColumnCount = 1;
            rightColumn.RowCount = 3;
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 40F)); // txtInfo
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 30F)); // compression
            rightColumn.RowStyles.Add(new RowStyle(SizeType.Percent, 30F)); // speed
            rightColumn.Padding = new Padding(6);

            // txtInfo
            txtInfo.Dock = DockStyle.Fill;
            txtInfo.Multiline = true;
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Margin = new Padding(6);

            // formsPlotCompression
            formsPlotCompression.Dock = DockStyle.Fill;
            formsPlotCompression.Margin = new Padding(6);

            // formsPlotSpeed
            formsPlotSpeed.Dock = DockStyle.Fill;
            formsPlotSpeed.Margin = new Padding(6);

            // assemble right column
            rightColumn.Controls.Add(txtInfo, 0, 0);
            rightColumn.Controls.Add(formsPlotCompression, 0, 1);
            rightColumn.Controls.Add(formsPlotSpeed, 0, 2);

            // add left and right to main table
            tableMain.Controls.Add(formsPlot1, 0, 0);
            tableMain.Controls.Add(rightColumn, 1, 0);

            // Form properties
            this.Text = "MultimediaFinalProject";
            this.Name = "Form1";
            this.ClientSize = new Size(1100, 720);
            this.AutoScaleMode = AutoScaleMode.Font;

            // add top-level controls to the Form
            this.Controls.Add(tableMain);
            this.Controls.Add(pbProgress);
            this.Controls.Add(flowTop);

            // finalize
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
        private TextBox txtInfo;
        private FormsPlot formsPlot1;
        private ProgressBar pbProgress;
        private FormsPlot formsPlotCompression;
        private FormsPlot formsPlotSpeed;
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MultimediaFinalProject
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            txtInfo = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(125, 36);
            button1.TabIndex = 0;
            button1.Text = "SelectAudio";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // txtInfo
            // 
            txtInfo.Location = new Point(556, 24);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Size = new Size(188, 313);
            txtInfo.TabIndex = 2;


            Label lblAlgo = new Label() { Text = "Algorithm:", Location = new Point(12, 60), AutoSize = true };
            cmbAlgorithm = new ComboBox();
            cmbAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAlgorithm.Items.AddRange(new object[] {
                   "Nonlinear Quantization",
                   "DPCM",
                   "Predictive Differential Coding",
                   "Delta Modulation",
                    "Adaptive Delta Modulation"
             });

            cmbAlgorithm.Location = new Point(150, 58);
            cmbAlgorithm.Size = new Size(200, 23); 

            Label lblRate = new Label() { Text = "Sample Rate (Hz):", Location = new Point(12, 95), AutoSize = true };
            NumericUpDown nudSampleRate = new NumericUpDown() { Location = new Point(150, 93), Size = new Size(200, 23), Maximum = 96000, Value = 44100 };

            Label lblQuant = new Label() { Text = "Quantization Levels:", Location = new Point(12, 130), AutoSize = true };
            nudQuant = new NumericUpDown() { Location = new Point(150, 128), Size = new Size(200, 23), Value = 8 };

            btnCompress = new Button()
            {
                Text = "Start Compression",
                Location = new Point(150, 165),
                Size = new Size(150, 30),
                Enabled = false 
            };
            btnCompress.Click += btnCompress_Click;

            Controls.Add(lblAlgo); Controls.Add(cmbAlgorithm);
            Controls.Add(lblRate); Controls.Add(nudSampleRate);
            Controls.Add(lblQuant); Controls.Add(nudQuant);
            Controls.Add(btnCompress);


            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtInfo);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
          
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox txtInfo;
        private ComboBox cmbAlgorithm;
        private NumericUpDown nudQuant;
        private Button btnCompress;
    }
}

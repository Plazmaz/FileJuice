namespace File_through_image_based_distribution_system_FIDS
{
    using System;
    using System.Windows.Forms;
    partial class main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileselector = new System.Windows.Forms.OpenFileDialog();
            this.btnCompress = new System.Windows.Forms.Button();
            this.filechooser = new System.Windows.Forms.Button();
            this.pBarConversion = new System.Windows.Forms.ProgressBar();
            this.pBoxEncoded = new System.Windows.Forms.PictureBox();
            this.cbDecompressing = new System.Windows.Forms.CheckBox();
            this.saveData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxEncoded)).BeginInit();
            this.SuspendLayout();
            // 
            // fileselector
            // 
            this.fileselector.Filter = "Executable files (*.exe)|*.exe";
            this.fileselector.RestoreDirectory = true;
            // 
            // btnCompress
            // 
            this.btnCompress.Location = new System.Drawing.Point(486, 361);
            this.btnCompress.Name = "btnCompress";
            this.btnCompress.Size = new System.Drawing.Size(110, 62);
            this.btnCompress.TabIndex = 0;
            this.btnCompress.Text = "Compress/Decompress";
            this.btnCompress.UseVisualStyleBackColor = true;
            this.btnCompress.Click += new System.EventHandler(this.compress_Click);
            // 
            // filechooser
            // 
            this.filechooser.Location = new System.Drawing.Point(382, 361);
            this.filechooser.Name = "filechooser";
            this.filechooser.Size = new System.Drawing.Size(97, 61);
            this.filechooser.TabIndex = 1;
            this.filechooser.Text = "Choose a File";
            this.filechooser.UseVisualStyleBackColor = true;
            this.filechooser.Click += new System.EventHandler(this.fileChooser_Click);
            // 
            // pBarConversion
            // 
            this.pBarConversion.Location = new System.Drawing.Point(12, 361);
            this.pBarConversion.Name = "pBarConversion";
            this.pBarConversion.Size = new System.Drawing.Size(363, 61);
            this.pBarConversion.Step = 1;
            this.pBarConversion.TabIndex = 2;
            // 
            // pBoxEncoded
            // 
            this.pBoxEncoded.Location = new System.Drawing.Point(12, 12);
            this.pBoxEncoded.Name = "pBoxEncoded";
            this.pBoxEncoded.Size = new System.Drawing.Size(692, 320);
            this.pBoxEncoded.TabIndex = 3;
            this.pBoxEncoded.TabStop = false;
            // 
            // cbDecompressing
            // 
            this.cbDecompressing.AutoSize = true;
            this.cbDecompressing.Location = new System.Drawing.Point(491, 338);
            this.cbDecompressing.Name = "cbDecompressing";
            this.cbDecompressing.Size = new System.Drawing.Size(105, 17);
            this.cbDecompressing.TabIndex = 4;
            this.cbDecompressing.Text = "Decompressing?";
            this.cbDecompressing.UseVisualStyleBackColor = true;
            this.cbDecompressing.CheckedChanged += new System.EventHandler(this.cbDecompressing_CheckedChanged);
            // 
            // saveData
            // 
            this.saveData.Location = new System.Drawing.Point(602, 361);
            this.saveData.Name = "saveData";
            this.saveData.Size = new System.Drawing.Size(102, 62);
            this.saveData.TabIndex = 5;
            this.saveData.Text = "Save Processed Data";
            this.saveData.UseVisualStyleBackColor = true;
            this.saveData.Click += new System.EventHandler(this.saveData_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 435);
            this.Controls.Add(this.saveData);
            this.Controls.Add(this.cbDecompressing);
            this.Controls.Add(this.pBoxEncoded);
            this.Controls.Add(this.pBarConversion);
            this.Controls.Add(this.filechooser);
            this.Controls.Add(this.btnCompress);
            this.Name = "main";
            this.Text = "FIDS";
            ((System.ComponentModel.ISupportInitialize)(this.pBoxEncoded)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion 

        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.Button filechooser;
        private System.Windows.Forms.OpenFileDialog fileselector;
        private ProgressBar pBarConversion;
        private PictureBox pBoxEncoded;
        private CheckBox cbDecompressing;
        private Button saveData;

    }
}


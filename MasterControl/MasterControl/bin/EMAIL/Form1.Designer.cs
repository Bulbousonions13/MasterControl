namespace MasterControl
{
    partial class Form1
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
            if (disposing && (components != null)) {
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
            this.StartRecordingButton = new System.Windows.Forms.Button();
            this.StopRecordingButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartRecordingButton
            // 
            this.StartRecordingButton.Location = new System.Drawing.Point(12, 12);
            this.StartRecordingButton.Name = "StartRecordingButton";
            this.StartRecordingButton.Size = new System.Drawing.Size(100, 25);
            this.StartRecordingButton.TabIndex = 0;
            this.StartRecordingButton.Text = "Start Recording";
            this.StartRecordingButton.UseVisualStyleBackColor = true;
            this.StartRecordingButton.Click += new System.EventHandler(this.StartRecordingButton_Click);
            // 
            // StopRecordingButton
            // 
            this.StopRecordingButton.Location = new System.Drawing.Point(10, 67);
            this.StopRecordingButton.Name = "StopRecordingButton";
            this.StopRecordingButton.Size = new System.Drawing.Size(100, 25);
            this.StopRecordingButton.TabIndex = 1;
            this.StopRecordingButton.Text = "Stop Recording";
            this.StopRecordingButton.UseVisualStyleBackColor = true;
            this.StopRecordingButton.Click += new System.EventHandler(this.StopRecordingButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(122, 160);
            this.Controls.Add(this.StopRecordingButton);
            this.Controls.Add(this.StartRecordingButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartRecordingButton;
        private System.Windows.Forms.Button StopRecordingButton;
    }
}


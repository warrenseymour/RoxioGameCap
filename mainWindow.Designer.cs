namespace RoxioGameCap {
    partial class mainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.previewPanel = new System.Windows.Forms.Panel();
            this.buttonToggleRecording = new System.Windows.Forms.Button();
            this.buttonToggleStreaming = new System.Windows.Forms.Button();
            this.labelRecordingStatus = new System.Windows.Forms.Label();
            this.labelRecordingFile = new System.Windows.Forms.Label();
            this.labelRecordingLength = new System.Windows.Forms.Label();
            this.labelRecordingSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // previewPanel
            // 
            this.previewPanel.Location = new System.Drawing.Point(12, 12);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(268, 151);
            this.previewPanel.TabIndex = 0;
            // 
            // buttonToggleRecording
            // 
            this.buttonToggleRecording.Location = new System.Drawing.Point(12, 169);
            this.buttonToggleRecording.Name = "buttonToggleRecording";
            this.buttonToggleRecording.Size = new System.Drawing.Size(131, 23);
            this.buttonToggleRecording.TabIndex = 1;
            this.buttonToggleRecording.Text = "Start Recording";
            this.buttonToggleRecording.UseVisualStyleBackColor = true;
            this.buttonToggleRecording.Click += new System.EventHandler(this.buttonToggleRecording_Click);
            // 
            // buttonToggleStreaming
            // 
            this.buttonToggleStreaming.Enabled = false;
            this.buttonToggleStreaming.Location = new System.Drawing.Point(149, 169);
            this.buttonToggleStreaming.Name = "buttonToggleStreaming";
            this.buttonToggleStreaming.Size = new System.Drawing.Size(131, 23);
            this.buttonToggleStreaming.TabIndex = 2;
            this.buttonToggleStreaming.Text = "Start Livestreaming";
            this.buttonToggleStreaming.UseVisualStyleBackColor = true;
            // 
            // labelRecordingStatus
            // 
            this.labelRecordingStatus.AutoSize = true;
            this.labelRecordingStatus.Location = new System.Drawing.Point(12, 199);
            this.labelRecordingStatus.Name = "labelRecordingStatus";
            this.labelRecordingStatus.Size = new System.Drawing.Size(112, 13);
            this.labelRecordingStatus.TabIndex = 3;
            this.labelRecordingStatus.Text = "Status: Not Recording";
            // 
            // labelRecordingFile
            // 
            this.labelRecordingFile.AutoSize = true;
            this.labelRecordingFile.Location = new System.Drawing.Point(12, 212);
            this.labelRecordingFile.Name = "labelRecordingFile";
            this.labelRecordingFile.Size = new System.Drawing.Size(26, 13);
            this.labelRecordingFile.TabIndex = 4;
            this.labelRecordingFile.Text = "File:";
            // 
            // labelRecordingLength
            // 
            this.labelRecordingLength.AutoSize = true;
            this.labelRecordingLength.Location = new System.Drawing.Point(12, 225);
            this.labelRecordingLength.Name = "labelRecordingLength";
            this.labelRecordingLength.Size = new System.Drawing.Size(95, 13);
            this.labelRecordingLength.TabIndex = 5;
            this.labelRecordingLength.Text = "Recording Length:";
            // 
            // labelRecordingSize
            // 
            this.labelRecordingSize.AutoSize = true;
            this.labelRecordingSize.Location = new System.Drawing.Point(12, 238);
            this.labelRecordingSize.Name = "labelRecordingSize";
            this.labelRecordingSize.Size = new System.Drawing.Size(82, 13);
            this.labelRecordingSize.TabIndex = 6;
            this.labelRecordingSize.Text = "Recording Size:";
            // 
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 262);
            this.Controls.Add(this.labelRecordingSize);
            this.Controls.Add(this.labelRecordingLength);
            this.Controls.Add(this.labelRecordingFile);
            this.Controls.Add(this.labelRecordingStatus);
            this.Controls.Add(this.buttonToggleStreaming);
            this.Controls.Add(this.buttonToggleRecording);
            this.Controls.Add(this.previewPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "mainWindow";
            this.Text = "RoxioGameCap";
            this.Load += new System.EventHandler(this.mainWindow_Load);
            this.Shown += new System.EventHandler(this.mainWindow_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel previewPanel;
        private System.Windows.Forms.Button buttonToggleRecording;
        private System.Windows.Forms.Button buttonToggleStreaming;
        private System.Windows.Forms.Label labelRecordingStatus;
        private System.Windows.Forms.Label labelRecordingFile;
        private System.Windows.Forms.Label labelRecordingLength;
        private System.Windows.Forms.Label labelRecordingSize;
    }
}


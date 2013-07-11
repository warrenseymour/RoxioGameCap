using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RoxioGameCap {
    public partial class mainWindow : Form {
        private GraphManager captureGraph;
        private bool isRecording = false;

        public mainWindow() {
            InitializeComponent();
        }

        private void mainWindow_Load(object sender, EventArgs e) {

        }

        private void mainWindow_Shown(object sender, EventArgs e) {
            this.captureGraph = new GraphManager();
            this.captureGraph.buildFromFile("capture.grf");
            this.captureGraph.constrainOutputToPanel(previewPanel);
            this.captureGraph.run();
        }

        private void buttonToggleRecording_Click(object sender, EventArgs e) {
            this.toggleRecording();
        }

        private void toggleRecording() {
            if (this.isRecording) {
                this.stopRecording();
            } else {
                this.startRecording();
            }
        }

        private void stopRecording() {
            this.isRecording = false;
            this.labelRecordingStatus.Text = "Status: Not Recording";
            this.labelRecordingFile.Text = "-";
            buttonToggleRecording.Text = "Start Recording";

            this.captureGraph.stopFileWriter();
        }

        private void startRecording() {
            string recordingDirectory = Properties.Settings.Default.RecordingDirectory;
            string recordingFilename = DateTime.Now.ToString("yyyy-MM-dd-HH-ss") + ".m2ts";

            string filename = recordingDirectory + recordingFilename;

            this.isRecording = true;
            this.labelRecordingStatus.Text = "Status: Recording";
            this.labelRecordingFile.Text = filename;
            buttonToggleRecording.Text = "Stop Recording";

            captureGraph.startFileWriter(filename);
        }
    }
}

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

        private GraphManager previewGraph;

        public mainWindow() {
            InitializeComponent();
        }

        private void mainWindow_Load(object sender, EventArgs e) {
            this.previewGraph = new GraphManager();
            this.previewGraph.buildFromFile("preview.grf");
            this.previewGraph.constrainOutputToPanel(previewPanel);
        }

        private void mainWindow_Shown(object sender, EventArgs e) {
            this.previewGraph.run();
        }
    }
}

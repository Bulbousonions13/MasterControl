using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterControl
{
    public partial class Form1 : Form
    {
        private MQTTServerManager _manager = new MQTTServerManager();
        public Form1( MQTTServerManager manager )
        {
            _manager = manager;
            InitializeComponent();
        }

        private void StartRecordingButton_Click(object sender, EventArgs e)
        {
           _manager.publish("Start Recording","master/start");
        }

        private void StopRecordingButton_Click(object sender, EventArgs e)
        {
             _manager.publish("Stop Recording","master/stop");
        }
    }
}

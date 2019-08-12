using System;
using System.Globalization;
using System.Windows.Forms;

namespace MasterControl
{
    public partial class Form1 : Form
    {
        private MQTTServerManager _manager ;
        private string MasterIP;
        private int MasterPort;
        public Form1( MQTTServerManager manager, string masterIP, int masterPort )
        {
            _manager = manager;
            MasterIP = masterIP;
            MasterPort = masterPort;
            InitializeComponent();
        }

        private void StartRecordingButton_Click(object sender, EventArgs e)
        {
           
           _manager.publish("\nStart Signal sent from "+MasterIP+":"+MasterPort,"master/start");
        }

        private void StopRecordingButton_Click(object sender, EventArgs e)
        {
            
           _manager.publish("\nStop Signal sent from "+MasterIP+":"+MasterPort,"master/stop");
        }
    }
}

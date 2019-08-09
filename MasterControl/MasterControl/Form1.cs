using System;
using System.Globalization;
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
           
           _manager.publish("\nStart Signal sent @ "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture),"master/start");
        }

        private void StopRecordingButton_Click(object sender, EventArgs e)
        {
            
           _manager.publish("\nStop Signal sent @ "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture),"master/stop");
        }
    }
}

using NAudio.Wave;
using System;
using System.IO;

namespace ClientRecorder
{
    class Recorder
    {
        static NAudio.Wave.WaveInEvent WaveSource = null;
        static NAudio.Wave.WaveFileWriter WaveFile = null;
        static string Filename = null;
        private int count;
        private int ActiveDevice = 0;

        public Recorder( int activeDevice){ 
            ActiveDevice = activeDevice;
        }

        public void record(){           
            
            Console.WriteLine();
            Console.WriteLine("Recording on Device  # 0 ");
                      
            WaveSource = new WaveInEvent();
            WaveSource.DeviceNumber = ActiveDevice;
            WaveSource.WaveFormat = new WaveFormat(44100, 1);

            WaveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            WaveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            long milliseconds = (long) Math.Round(DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds);
            Filename = Path.Combine(Directory.GetCurrentDirectory(), $"AudioSample_{milliseconds}.wav");
            WaveFile = new WaveFileWriter(Filename, WaveSource.WaveFormat);

            WaveSource.StartRecording();      
            
        }
        public void stop(){  
            
            Console.WriteLine($"Stopping recording. ({Path.GetFileName(Filename)})");
            //Console.ReadLine();        
            WaveSource.StopRecording();
            count++; 
        }
        
        public void showDevices(){ 
            
            count = GetStartingCount();

            for (int n = -1; n < WaveIn.DeviceCount; n++)
            {
                var caps = WaveIn.GetCapabilities(n);
                Console.WriteLine($"\t{n}: {caps.ProductName}");
            }
        }

        private static int GetStartingCount()
        {
            int count = 1;
            string filename = Path.Combine(Directory.GetCurrentDirectory(), $"Test{count:00000}.wav");
            while(File.Exists(filename))
            {
                count++;
                filename = Path.Combine(Directory.GetCurrentDirectory(), $"Test{count:00000}.wav");
            }

            return count;
        }

        static void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (WaveFile != null)
            {
                WaveFile.Write(e.Buffer, 0, e.BytesRecorded);
                WaveFile.Flush();
            }
        }

        static void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (WaveSource != null)
            {
                WaveSource.Dispose();
                WaveSource = null;
            }

            if (WaveFile != null)
            {
                WaveFile.Dispose();
                WaveFile = null;
            }
        }
    }
}

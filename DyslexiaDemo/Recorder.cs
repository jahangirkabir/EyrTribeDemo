using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyslexiaDemo
{
    public class Recorder
    {
        NAudio.Wave.WaveIn sourceStream = null;
        NAudio.Wave.DirectSoundOut waveOut = null;
        NAudio.Wave.WaveFileWriter waveWriter = null;
        private int sourceDevice = 0;
        public Recorder()
        {

        }
        public void Initialize()
        {

        }

        public void Deinitialize()
        {
            StopRecording();
        }

        public List<NAudio.Wave.WaveInCapabilities> GetSources()
        {
            List<NAudio.Wave.WaveInCapabilities> sources = new List<NAudio.Wave.WaveInCapabilities>();

            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                sources.Add(NAudio.Wave.WaveIn.GetCapabilities(i));
            }

            return sources;
        }
        public Array GetSourcesArray()
        {
            NAudio.CoreAudioApi.MMDeviceEnumerator enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();

            var devices = enumerator.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.Capture, DeviceState.Active);

            return devices.ToArray();
        }
        public List<string> GetSourcesList()
        {
            Array array = GetSourcesArray();

            List<string> sources = new List<string>();
            foreach (var a in array)
            {
                sources.Add(a.ToString());
            }

            return sources;
        }

        public void SelectSourceDevice(int no)
        {
            sourceDevice = no;
        }

        public int GetSelectedSourceDevice()
        {
            return sourceDevice;
        }

        public void StartRecording(string audioFile)
        {
            try
            {
                sourceStream = new NAudio.Wave.WaveIn();
                sourceStream.DeviceNumber = sourceDevice;
                sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(44100, NAudio.Wave.WaveIn.GetCapabilities(sourceDevice).Channels);

                sourceStream.BufferMilliseconds = 250;

                sourceStream.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(sourceStream_DataAvailable);
                waveWriter = new NAudio.Wave.WaveFileWriter(audioFile, sourceStream.WaveFormat);

                sourceStream.StartRecording();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void sourceStream_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            waveWriter.Write(e.Buffer, 0, e.BytesRecorded); // LAST BYTES ARE NOT RECORDED.
            waveWriter.Flush();
        }
        public void StopRecording()
        {
            try
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    waveOut = null;
                }
                try
                {
                    if (sourceStream != null)
                    {
                        //th.Abort();
                        sourceStream.StopRecording();
                        sourceStream.Dispose();
                        sourceStream = null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                try
                {
                    if (waveWriter != null)
                    {
                        waveWriter.Dispose();
                        waveWriter = null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.Message);
            }
        }
    }
}

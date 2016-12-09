using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using TETCSharpClient;
using TETCSharpClient.Data;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Timers;
using AForge.Video.FFMPEG;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using NAudio.Wave;

namespace DyslexiaDemo
{
    /// <summary>
    /// Interaction logic for ReadingTest.xaml
    /// </summary>
    public partial class ReadingTest : Page
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetDrawCallback(string text);

        // This thread is used to demonstrate both thread-safe and
        // unsafe ways to call a Windows Forms control.
        private Thread demoThread = null;

        // This BackgroundWorker is used to demonstrate the 
        // preferred way of performing asynchronous operations.
        private BackgroundWorker backgroundWorker1;
        //[STAThread]
        ReadingTest readingTest;
        int counter = 0;
        System.Timers.Timer timer;
        int width = 1230;
        int height = 640;
        //int width = 2650;
        //int height = 1124;
        String folderPath = @"d:\Dyslexia_Reading_Test\";
        String finalImage = "";
        private static Bitmap result = null;
        public bool isRecording = false;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        string response = "";
        TestWindow testWindow;
        List<EyeAxis> eyeAxisList;
        WaveIn waveIn;
        WaveFileWriter writer;
        AudioRecorder audioRecorder;

        public ReadingTest(TestWindow testWindow)
        {
            InitializeComponent();
            readingTest = this;
            this.testWindow = testWindow;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            //Thread t = new Thread(ThreadProc);
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
            //GazePoint gazePoint = new GazePoint();
            // Connect client
            //GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);

            // Register this class for events
            //GazeManager.Instance.AddGazeListener(this);

            //Thread.Sleep(3000); // simulate app lifespan (e.g. OnClose/Exit event)
            //myCanvas_Draw(100, 200);
            // Disconnect client
            //GazeManager.Instance.Deactivate();
            this.Loaded += new RoutedEventHandler(ReadingTest_Loaded);
            //GrantAccess(@"d:\");
        }

        private bool GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
            return true;
        }

        void ReadingTest_Loaded(object sender, RoutedEventArgs e)
        {
            EyeTribeListener mListener = new EyeTribeListener((ReadingTest)sender);
            Directory.CreateDirectory(@"d:\Dyslexia_Reading_Test");
            //GazeManager.Instance.AddGazeListener(this);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // ... Get RadioButton reference.
            var button = sender as RadioButton;

            // ... Display button content as title.
            this.Title = button.Content.ToString();
            if (button.Content.ToString().Contains("English"))
            {
                englishImage.Visibility = Visibility.Visible;
                arabicImage.Visibility = Visibility.Hidden;

                BackenglishImage.Visibility = Visibility.Visible;
                BackarabicImage.Visibility = Visibility.Hidden;
            }
            else
            {
                arabicImage.Visibility = Visibility.Visible;
                englishImage.Visibility = Visibility.Hidden;

                BackarabicImage.Visibility = Visibility.Visible;
                BackenglishImage.Visibility = Visibility.Hidden;
            }
        }

        private void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            //CreateBitmapFromVisual(readingTest, @"d:\Dyslexia_Reading_Test\test.png");
            startbutton.Visibility = Visibility.Hidden;
            submitbutton.Visibility = Visibility.Visible;

            eyeAxisList = new List<EyeAxis>();

            startImageSaving();

            audioRecorder = new AudioRecorder();
            audioRecorder.StartRecording(@"d:\Dyslexia_Reading_Test\dyslexia_test_record.WAV");
            //audioRecorder.StartRecording(@"d:\Dyslexia_Reading_Test\dyslexia_test_record1.OGG");
            //startAudio_Recording();
        }
        private void SubmitButtonClicked(object sender, RoutedEventArgs e)
        {
            stopImageSaving();
            audioRecorder.StopRecording();
        }

        public void GetAxisPoint(double gX, double gY)
        {
            //Console.WriteLine("x...>" + gX + "...y..>" + gY);
            //Thread thread = new Thread(() => myCanvas_Draw(gX / 3, gY / 3));
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
            
            if (isRecording)
            {
                EyeAxis eyeAxis = new EyeAxis();
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                eyeAxis.Time = timeStamp;
                eyeAxis.XAxis = gX.ToString();
                eyeAxis.YAxis = gY.ToString();
                
                eyeAxisList.Add(eyeAxis);

                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(() => BackCanvas_Draw(gX, gY), DispatcherPriority.Render);
                }
                else
                {
                    BackCanvas_Draw(gX, gY);
                }
            }
            
            //var vtext = gX + "," + gY;
            //var progress = new Progress<Tuple<double, double>>(s => myCanvas_Draw(s.Item1, s.Item2));
            //await Task.Factory.StartNew(() => SecondThreadConcern.LongWork(progress, gX/3, gY/3), TaskCreationOptions.LongRunning);
        }

        public void BackCanvas_Draw(double gX, double gY)
        {
            //myCanvas.Children.Clear();
            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Fill = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(el, gX);
            Canvas.SetTop(el, gY);

            BackCanvas.Children.Add(el);
            //Console.WriteLine("children added " + gX + "..." + gY);
            //myCanvas.InvalidateVisual();
            //myCanvas.UpdateLayout();
        }

        public void startImageSaving()
        {
            //TimerCallback tcb = CreateBitmapFromVisual(readingTest, "");
            //Timer stateTimer = new Timer(tcb, autoEvent, 1000, 250);
            timer = new System.Timers.Timer(250);
            //timer.Elapsed += async (sender, e) => await HandleTimer();
            timer.Elapsed += new ElapsedEventHandler(CanvasDrawEvent);
            // Set it to go off every five seconds
            //timer.Interval = 250;
            // And start it        
            //timer.Enabled = true;
            timer.Start();
            isRecording = true;
        }

        public void stopImageSaving()
        {
            //var json = JsonConvert.SerializeObject(eyeAxisList);
            //Console.WriteLine("eyeAxisList...>" + "{\"body\":" + json + "}");

            isRecording = false;
            counter = 0;
            timer.Stop();
            submitbutton.IsEnabled = false;
            loadingImage.Visibility = Visibility.Visible;
            worker.RunWorkerAsync();
        }

        public void startAudio_Recording()
        {
            
            //int waveInDevices = WaveIn.DeviceCount;
            //for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            //{
            //    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
            //    Console.WriteLine("Device {0}: {1}, {2} channels", waveInDevice, deviceInfo.ProductName, deviceInfo.Channels);
            //}

            //writer = new WaveFileWriter(@"d:\Dyslexia_Reading_Test\test.wev", waveIn.WaveFormat);

            //Now we can write to the file as we receive notifications from the waveIn device:

            //waveIn = new WaveIn();
            //waveIn.DeviceNumber = 0;
            //waveIn.DataAvailable += waveIn_DataAvailable;
            //int sampleRate = 8000; // 8 kHz
            //int channels = 1; // mono
            //waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
            //waveIn.StartRecording();
            //waveIn.StopRecording();
        }


        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //if (recordingState == RecordingState.Recording)
                writer.Write(e.Buffer, 0, e.BytesRecorded);

            // ...
        }

        //void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        //{
        //    for (int index = 0; index < e.BytesRecorded; index += 2)
        //    {
        //        short sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
        //        float sample32 = sample / 32768f;
        //        //ProcessSample(sample32);
        //    }
        //}

        public static void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {
            using (WaveFileReader reader = new WaveFileReader(inPath))
            {
                using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
                {
                    int bytesPerMillisecond =
                        reader.WaveFormat.AverageBytesPerSecond / 1000;

                    int startPos = (int)cutFromStart.TotalMilliseconds *
                                   bytesPerMillisecond;
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)cutFromEnd.TotalMilliseconds *
                                   bytesPerMillisecond;
                    endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                    int endPos = (int)reader.Length - endBytes;

                    TrimWavFile(reader, writer, startPos, endPos);
                }
            }
        }

        private static void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CreateVideo();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            submitbutton.IsEnabled = true;
            loadingImage.Visibility = Visibility.Hidden;

            if (MessageBox.Show("Dyslexia Reading test saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK) == MessageBoxResult.OK)
            {
                testWindow.Close();
            }
        }

        private void CanvasDrawEvent(object source, ElapsedEventArgs e)
        {
            
            counter++;
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => CreateBitmapFromVisual(BackCanvasGrid, @"d:\Dyslexia_Reading_Test\reading" + String.Format("{0:0000}", counter) + ".jpg"), DispatcherPriority.Render);
            }
            else
            {
                CreateBitmapFromVisual(BackCanvasGrid, @"d:\Dyslexia_Reading_Test\" + String.Format("{0:0000}", counter) + ".jpg");
            }

        }

        public static void CreateBitmapFromVisual(Visual target, string fileName)
        {
            if (target == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

            RenderTargetBitmap renderTarget = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext context = visual.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(target);
                context.DrawRectangle(visualBrush, null, new Rect(new System.Windows.Point(), bounds.Size));
            }

            renderTarget.Render(visual);
            PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
            using (Stream stm = File.Create(fileName))
            {
                bitmapEncoder.Save(stm);
            }
            Console.WriteLine("SCREENSHOT added " + fileName);
        }

        public void CreateVideo()
        {
            VideoFileWriter writer = new VideoFileWriter();
            writer.Open(@"d:\Dyslexia_Reading_Test\dyslexia_readingtest_video.mp4", width, height, 4, VideoCodec.MPEG4, 1000000);
            
            Bitmap image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);

            g.DrawString("(c) 2016 by JAHANGIR KABIR", new System.Drawing.Font("Calibri", 30), System.Drawing.Brushes.White, 80, 240);
            g.Save();
            
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.jpg"))
            {
                Console.WriteLine("FILE Name " + file);
                image = (Bitmap) System.Drawing.Image.FromFile(file, true);
                System.Drawing.Image srcimage = System.Drawing.Image.FromFile(file, true);

                //writer.WriteVideoFrame(image);
                writer.WriteVideoFrame(ResizeImage(srcimage, width, height));

            }

            writer.Close();

            //var directory = new DirectoryInfo(@"d:\Dyslexia_Reading_Test\");
            //var firstImage = (from f in directory.GetFiles("*.jpg", SearchOption.AllDirectories)
            //                  orderby f.LastWriteTime ascending
            //                  select f).Last();
            //Console.WriteLine("Last IMAGE : " + firstImage.ToString());

            //var files = from file in Directory.GetFiles(@"d:\Dyslexia_Reading_Test\", "*.jpg")
            //            orderby file descending
            //            select file;
            //var biggest = files.First();
            //Console.WriteLine("Last IMAGE : " + biggest.ToString());
            //var lowest = files.Last();
            //Console.WriteLine("First IMAGE : " + lowest.ToString());

            //foreach (FileInfo f in Directory.GetFiles(folderPath, "*.jpg").Select(fn => new FileInfo(fn)).OrderBy(f => f.Name))
            //{
            //    Console.WriteLine("Sorted FILE Name " + f.Name);
            //}

            UploadMultimediaData();
        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public void UploadMultimediaData()
        {
            var json = JsonConvert.SerializeObject(eyeAxisList);
            string coordinate = "{\"body\":" + json + "}";
            Console.WriteLine("coordinate...>" + coordinate);

            var imageFiles = from file in Directory.GetFiles(@"d:\Dyslexia_Reading_Test\", "*.jpg")
                        orderby file descending
                        select file;
            var lastImage = imageFiles.First();
            Console.WriteLine("Last IMAGE : " + lastImage.ToString());
            var firstImage = imageFiles.Last();
            Console.WriteLine("First IMAGE : " + firstImage.ToString());

            finalImage = firstImage.ToString();
            string url = "";
            
            string videofileLocation = folderPath + "dyslexia_readingtest_video.mp4";
            string soundfileLocation = folderPath + "dyslexia_test_record.WAV";
            string imagefileLocation = finalImage;
            NameValueCollection values = new NameValueCollection();
            NameValueCollection files = new NameValueCollection();
            values.Add("licenceNumber", AppConstant.LicenceNo);
            values.Add("type", "Reading");
            values.Add("coordinate", coordinate);
            files.Add("basedata_image", imagefileLocation);
            files.Add("basedata_video", videofileLocation);

            files.Add("basedata_image1", imagefileLocation);
            files.Add("basedata_video1", soundfileLocation);
            if (AppConstant.StudentId.Length > 0)
            {
                url = "http://52.35.60.215//dyslexia_web/v01/media/uploadMPWithId";
                values.Add("id", AppConstant.StudentId);
            }
            else
            {
                url = "http://52.35.60.215/dyslexia_web/v01/media/uploadMP";
            }

            response = sendHttpRequest(url, values, files);
            Console.WriteLine("RESPONSE " + response);
        }

        private static string sendHttpRequest(string url, NameValueCollection values, NameValueCollection files = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            // The first boundary
            byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            // The last boundary
            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            // The first time it itereates, we need to make sure it doesn't put too many new paragraphs down or it completely messes up poor webbrick
            byte[] boundaryBytesF = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            // Create the request and set parameters
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            // Get request stream
            Stream requestStream = request.GetRequestStream();

            foreach (string key in values.Keys)
            {
                // Write item to stream
                byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}", key, values[key]));
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            if (files != null)
            {
                foreach (string key in files.Keys)
                {
                    if (File.Exists(files[key]))
                    {
                        int bytesRead = 0;
                        byte[] buffer = new byte[2048];
                        byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", key, files[key]));
                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        requestStream.Write(formItemBytes, 0, formItemBytes.Length);

                        using (FileStream fileStream = new FileStream(files[key], FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                // Write file content to stream, byte by byte
                                requestStream.Write(buffer, 0, bytesRead);
                            }

                            fileStream.Close();
                        }
                    }
                }
            }

            // Write trailer and close stream
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                
                return reader.ReadToEnd();
            };
        }
    }

    public class EyeTribeListener : IGazeListener
    {
        public ReadingTest rTest;


        public EyeTribeListener(ReadingTest rTest1)
        {
            GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);
            GazeManager.Instance.AddGazeListener(this);
            rTest = rTest1;
        }
        public void OnGazeUpdate(GazeData gazeData)
        {
            double gX = gazeData.SmoothedCoordinates.X;
            double gY = gazeData.SmoothedCoordinates.Y;

            //Console.WriteLine("->>> x..>" + gX + "|...|y..>" + gY);
            if (gX < 1228 && gY < 638)
            {
                if (gX > 0 && gY > 0)
                {
                    rTest.GetAxisPoint(gX, gY);
                }
                //rTest.GetAxisPoint(gX, gY);
            }
            
        }
    }

}

using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;

namespace DyslexiaDemo
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        bool isLoged = false;
        string licenseNo, studentId, response = "";
        List<Student> studentList;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        TestWindow testWindow;
        public LoginPage(TestWindow testWindow)
        {
            InitializeComponent();
            this.testWindow = testWindow;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        
        private void LoginButtonClicked(object sender, RoutedEventArgs e)
        {
            if (isLoged)
            {
                if (IdComboBox.SelectedValue.ToString().Contains("Select Student"))
                {
                    MessageBox.Show("Please selecct student to continue.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    studentId = studentList[IdComboBox.SelectedIndex-1].StudentId;
                    Console.WriteLine("Student ID  " + studentId);
                    Console.WriteLine("LICENSE NO  " + licenseNo);
                    AppConstant.StudentId = studentId;

                    loginButton.IsEnabled = false;
                    loadingImage.Visibility = Visibility.Visible;
                    worker.RunWorkerAsync();
                }
            }
            else
            {
                if (licenceTextBox.Text.Length < 2)
                {
                    MessageBox.Show("Please Enter Licence Number to Continue.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    licenseNo = licenceTextBox.Text;
                    Console.WriteLine("LICENSE NO  " + licenseNo);
                    AppConstant.LicenceNo = licenseNo;

                    loginButton.IsEnabled = false;
                    loadingImage.Visibility = Visibility.Visible;
                    worker.RunWorkerAsync();
                }
                
            }

            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
            if (isLoged)
            {
                JObject main = new JObject();
                JObject objbody = new JObject();

                objbody.Add("licenceNumber", licenseNo);
                objbody.Add("id", studentId);
                main.Add("body", objbody);
                Console.WriteLine("JSON " + main.ToString());
                
                response = sendHttpRequest("http://52.35.60.215/dyslexia_web/v01/auth/activeAppWithId", main.ToString());

                Console.WriteLine("RESPONSE " + response);
            }
            else
            {
                JObject main = new JObject();
                JObject objbody = new JObject();

                objbody.Add("licenceNumber", licenseNo);
                main.Add("body", objbody);

                Console.WriteLine("JSON " + main.ToString());
                response = sendHttpRequest("http://52.35.60.215/dyslexia_web/v01/auth/activeApp", main.ToString());
                Console.WriteLine("RESPONSE " + response);
                
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
            loadingImage.Visibility = Visibility.Hidden;
            loginButton.IsEnabled = true;

            if (isLoged)
            {
                if (response.Contains("1111"))
                {
                    //JObject root = JObject.Parse(response);
                    //JObject studentObj = (JObject)root.GetValue("body");
                    //string isValidR = studentObj.GetValue("isValidR").ToString();
                    //if (isValidR.Contains("true"))
                    //{
                        ReadingTest rtest = new ReadingTest(this.testWindow);
                        this.NavigationService.Navigate(rtest);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Invalid Licence Number.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //}
                }
            }
            else
            {
                if (response.Contains("3333"))
                {
                    isLoged = true;
                    loginButton.Content = "Next";
                    licenceTextBox.IsEnabled = false;
                    IdComboBox.Visibility = Visibility.Visible;
                    studentLabel.Visibility = Visibility.Visible;

                    JObject root = JObject.Parse(response);
                    JArray bodyArray = (JArray)root["body"];
                    JObject studentObj;

                    studentList = new List<Student>();
                    IdComboBox.Items.Add(new Student().StudentName = "Select Student");
                    for (int i = 0; i < bodyArray.Count; i++) //loop through rows
                    {
                        Student s = new Student();
                        studentObj = (JObject)bodyArray[i];
                        string id = studentObj.GetValue("id").ToString();
                        string name = studentObj.GetValue("name").ToString();

                        s.StudentId = id;
                        s.StudentName = name;
                        studentList.Add(s);

                        IdComboBox.Items.Add(s);
                        Console.WriteLine("Name: {0}, Id: {1}", name, id);
                    }
                    IdComboBox.SelectedIndex = 0;
                }
                else if (response.Contains("1111"))
                {
                    ReadingTest rtest = new ReadingTest(this.testWindow);
                    this.NavigationService.Navigate(rtest);
                }
            }
        }

        private static string sendHttpRequest(string url, string jsonString)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "POST";
  
            // Get request stream
            //Stream requestStream = request.GetRequestStream();

            //byte[] postBytes = Encoding.UTF8.GetBytes(jsonString);

            //requestStream.Write(postBytes, 0, postBytes.Length);
            //requestStream.Close();

            using (Stream s = request.GetRequestStream())
            {
                using (StreamWriter sw = new StreamWriter(s))
                    sw.Write(jsonString);
            }

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return reader.ReadToEnd();
            };
        }
    }
}

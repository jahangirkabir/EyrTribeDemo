using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DyslexiaDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        TestWindow testWindow;
        public TestWindow()
        {
            InitializeComponent();
            testWindow = this;
            //ReadingTest rtest = new ReadingTest(this);
            LoginPage rtest = new LoginPage(this);
            testFrame.NavigationService.Navigate(rtest);
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = 0;
            this.Top = 0;
        }
    }
}

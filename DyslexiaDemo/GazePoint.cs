using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TETCSharpClient;
using TETCSharpClient.Data;

namespace DyslexiaDemo
{
    public class GazePoint : IGazeListener
    {
        MainWindow mainWindow;
        public GazePoint()
        {
            mainWindow = new MainWindow();
            // Connect client
            GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);

            // Register this class for events
            GazeManager.Instance.AddGazeListener(this);

            Thread.Sleep(5000); // simulate app lifespan (e.g. OnClose/Exit event)
            
            // Disconnect client
            GazeManager.Instance.Deactivate();
        }

        public void OnGazeUpdate(GazeData gazeData)
        {
            double gX = gazeData.SmoothedCoordinates.X;
            double gY = gazeData.SmoothedCoordinates.Y;

            Console.WriteLine( "->>> x..>"+gX+"|...|y..>"+gY );

            //mainWindow.GetAxisPoint(gX,gY);

            // Move point, do hit-testing, log coordinates etc.
        }
    }
}

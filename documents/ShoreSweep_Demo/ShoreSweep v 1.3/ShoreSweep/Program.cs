using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SplashScreen;
namespace ShoreSweep
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
          SplashScreen.SplashScreen.ShowSplashScreen();
            Application.DoEvents();
            SplashScreen.SplashScreen.SetStatus("Loading.");
            System.Threading.Thread.Sleep(500);
            SplashScreen.SplashScreen.SetStatus("Loading...");
            System.Threading.Thread.Sleep(300);
            SplashScreen.SplashScreen.SetStatus("Loading.....");
            System.Threading.Thread.Sleep(900);
            SplashScreen.SplashScreen.SetStatus("Loading......");
            System.Threading.Thread.Sleep(100);
            SplashScreen.SplashScreen.SetStatus("Loading.........");
            System.Threading.Thread.Sleep(400);
            SplashScreen.SplashScreen.SetStatus("Loading.............");
            System.Threading.Thread.Sleep(50);
            SplashScreen.SplashScreen.SetStatus("Loading..................");
            System.Threading.Thread.Sleep(240);
            SplashScreen.SplashScreen.SetStatus("Loading.......................");
            System.Threading.Thread.Sleep(900);
            SplashScreen.SplashScreen.SetStatus("Loading...............................");
            System.Threading.Thread.Sleep(240);
            SplashScreen.SplashScreen.SetStatus("Loading............................................");
            System.Threading.Thread.Sleep(10000);
            SplashScreen.SplashScreen.CloseForm();
            
            Application.Run(new Login());
           
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
    {
    static class Program
        {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// [STAThread]
        //[Obsolete]
        //private static void Main(string[] args)
        //{
        //    //Console.WriteLine("Starting (ganttproj1 " + Application.ProductVersion + ")...\n" + AppDomain.CurrentDomain.BaseDirectory.ToString());
        //    //Application.EnableVisualStyles();
        //    //Application.SetCompatibleTextRenderingDefault(true);
        //    var menu = new Central();
        //    menu.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
        //    menu.WindowState = FormWindowState.Maximized;
        //    menu.ShowDialog();
        //}
        [STAThread]
        //[Obsolete]
        static void Main()
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWnd());
            }
        }
    }
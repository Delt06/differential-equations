using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DEAssignment.Charts.Factories;
using DEAssignment.Methods;
using DEAssignment.Methods.Factories;

namespace DEAssignment
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
//            var method = new EulerMethod(Utils.RightSideFunction);
//            var step = 0.2d;
//            var x = Utils.Ivp.X0;
//            var y = Utils.Ivp.Y0;
//            for (int i = 0; i < 500; i++)
//            {
//                var ivp = new Ivp(x, y);
//                y = method[step, ivp, 1];
//                Console.WriteLine(y);
//                x += step;
//            }

            var methodFactory = new UtilsMethodFactory();
            var chartFactory = new ConfiguredMethodChartFactory();
            var mainForm = new MainForm(chartFactory, methodFactory);
            Application.Run(mainForm);
        }
    }
}
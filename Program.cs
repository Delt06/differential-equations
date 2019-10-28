using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DEAssignment.Charts.Factories;
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

            var methodFactory = new UtilsMethodFactory();
            var chartFactory = new ConfiguredMethodChartFactory();
            var mainForm = new MainForm(chartFactory, methodFactory);
            Application.Run(mainForm);
        }
    }
}
using System.Windows.Forms;
using System;

namespace FunctionGraph
{
    class Program
    {
        public static void Main(string[] args)
	{
	    Application.EnableVisualStyles();
	    Application.SetCompatibleTextRenderingDefault(false);
	    Application.Run(new MainWindow());
	}
    }
}
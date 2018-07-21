using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemperatureIconMeter
{
	static class Program
	{
		// mutex object for ensuring only single instance of application is allowed
		static Mutex mutex = new Mutex(true, "TemperatureIconMeter-windlknwgcouhq");

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (mutex.WaitOne(TimeSpan.Zero, true))
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new FormMain());
			}
		}
	}
}

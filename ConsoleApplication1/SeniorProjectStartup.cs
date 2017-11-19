using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using EncryptionLogic;
using System.Windows;
using SeniorProject;
using System.Diagnostics;

namespace ConsoleApplication1
{
	class SeniorProjectStartup
	{
		[STAThread]
		static void Main(string[] args)
		{
			/*setRegistry();
			*/
			try
			{
				MainWindow window = new MainWindow();
				window.Show();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				MessageBox.Show(e.InnerException.ToString());
			}
		}

		

		private static void startEncryption()
		{
			
		}
	}
}

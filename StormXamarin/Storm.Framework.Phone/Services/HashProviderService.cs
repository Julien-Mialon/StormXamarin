using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Storm.Framework.Cryptography;

namespace Storm.Framework.Services
{
	public class HashProviderService : IHashProvider
	{
		private MD5 m_md5;

		public HashProviderService()
		{
			this.m_md5 = new MD5();
		}

		public string ComputeMD5(string input)
		{
			this.m_md5.Value = input;
			return this.m_md5.FingerPrint;
		}
	}
}

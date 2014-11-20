using Storm.Framework.Cryptography;

namespace Storm.Framework.Services
{
	public class HashProviderService : IHashProvider
	{
		private MD5 m_md5;

		public HashProviderService()
		{
			m_md5 = new MD5();
		}

		public string ComputeMD5(string input)
		{
			m_md5.Value = input;
			return m_md5.FingerPrint;
		}
	}
}

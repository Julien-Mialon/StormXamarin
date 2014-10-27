using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Framework.Services
{
	public interface IHashProvider
	{
		string ComputeMD5(string input);
	}
}

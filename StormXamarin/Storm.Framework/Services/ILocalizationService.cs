using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Framework.Services
{
	public interface ILocalizationService
	{
		string GetString(string uid);

		string GetString(string uid, string property);
	}
}

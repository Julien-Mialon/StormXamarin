namespace Storm.Mvvm.Services
{
	public interface ILocalizationService
	{
		string GetString(string uid);

		string GetString(string uid, string property);
	}
}

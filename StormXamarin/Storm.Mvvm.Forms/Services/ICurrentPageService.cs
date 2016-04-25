using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public interface ICurrentPageService
	{
		Page CurrentPage { get; }

		void Push(Page newPage);

		void Pop();
	}
}

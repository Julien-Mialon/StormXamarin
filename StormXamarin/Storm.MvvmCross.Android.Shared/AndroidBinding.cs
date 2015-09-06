using Cirrious.CrossCore;
using Storm.MvvmCross.Android.Services;
using Storm.MvvmCross.Interfaces;

namespace Storm.MvvmCross.Android
{
    public static class AndroidBinding
    {
	    public static void Initialize()
	    {
		    Mvx.RegisterSingleton(typeof(IBindingSpecificPropertyService), () => new AndroidBindingSpecificPropertyService());
	    }
    }
}

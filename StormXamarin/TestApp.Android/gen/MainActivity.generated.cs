//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.34014
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestApp.Android.Activities
{
	using System.Collections.Generic;
	using Storm.Mvvm.Android.Bindings;
	
	
	public partial class MainActivity
	{
		
		protected override List<BindingObject> GetBindingPaths()
		{
			List<BindingObject> result = new List<BindingObject>();
			BindingObject o0 = new BindingObject("MyButton");
			result.Add(o0);
			BindingExpression e0 = new BindingExpression("text", "ButtonText");
			o0.AddExpression(e0);
			BindingExpression e1 = new BindingExpression("click", "ButtonCommand");
			o0.AddExpression(e1);
			return result;
		}
	}
}

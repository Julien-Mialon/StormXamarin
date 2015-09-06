namespace Storm.MvvmCross.Bindings.Internal
{
	internal abstract class BindingBase
	{
		protected object DataContext { get; private set; }

		protected object TargetObject { get; private set; }

		protected BindingExpression Expression { get; private set; }

		protected BindingBase(BindingExpression expression, object targetObject)
		{
			Expression = expression;
			TargetObject = targetObject;
		}

		public virtual void UpdateContext(object context)
		{
			DataContext = context;
		}

		public abstract void Initialize();
		public abstract void UpdateValue(object value);
	}
}

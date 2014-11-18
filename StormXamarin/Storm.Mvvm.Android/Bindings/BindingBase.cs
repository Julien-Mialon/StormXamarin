namespace Storm.Mvvm.Android.Bindings
{
	abstract class BindingBase
	{
		protected object Context { get; private set; }

		protected object TargetObject { get; private set; }

		protected BindingExpression Expression { get; private set; }

		protected BindingBase(BindingExpression expression, object targetObject)
		{
			Expression = expression;
			TargetObject = targetObject;
		}

		public virtual void UpdateContext(object context)
		{
			Context = context;
		}

		public abstract void Initialize();
		public abstract void UpdateValue(object value);
	}
}

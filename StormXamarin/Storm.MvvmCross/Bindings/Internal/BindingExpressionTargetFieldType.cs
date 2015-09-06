namespace Storm.MvvmCross.Bindings.Internal
{
	public enum BindingExpressionTargetFieldType
	{
		Undefined,
		Event, // create event to command binding (only one way/one time mode accepted)
		Property
	}
}

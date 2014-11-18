using System;
using System.Reflection;
using System.Windows.Input;

namespace Storm.Mvvm.Android.Bindings
{
	/// <summary>
	/// This class provide highlevel method to handle update of Command associated to the event
	/// </summary>
	class EventBinding : BindingBase
	{
		private EventToCommandProxy _eventProxy;

		public EventBinding(BindingExpression expression, object targetObject) : base(expression, targetObject)
		{

		}

		public override void Initialize()
		{
			//Get the event info to access event in the target object
			EventInfo eventInfo = TargetObject.GetType().GetEvent(Expression.TargetField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

			if (eventInfo == null)
			{
				throw new Exception("EventBinding : event with name " + Expression.TargetField + " does not exists in element of type " + TargetObject.GetType());
			}

			_eventProxy = new EventToCommandProxy(TargetObject, eventInfo);
			_eventProxy.Attach();
		}

		public override void UpdateValue(object value)
		{
			if (_eventProxy == null)
			{
				throw new Exception("EventBinding has not been initialized");
			}

			_eventProxy.Command = value as ICommand;
		}
	}
}

using System;

namespace Storm.Mvvm.Messaging
{
	class MessageReceiver : IMessageReceiver
	{
		private readonly Action<object, object> _callback;

		public object Token { get; private set; }
		public object Receiver { get; private set; }


		public MessageReceiver(object receiver, Action<object, object> callback)
			: this(receiver, null, callback)
		{
			
		}

		public MessageReceiver(object receiver, object token, Action<object, object> callback)
		{
			Token = token;
			Receiver = receiver;
			_callback = callback;
		}

		public void HandleReceive(object token, object parameter)
		{
			if (_callback != null)
			{
				_callback(token, parameter);
			}
		}
	}
}

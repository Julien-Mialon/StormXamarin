namespace Storm.Mvvm.Messaging
{
	interface IMessageReceiver
	{
		object Token { get; }

		object Receiver { get; }

		void HandleReceive(object token, object parameter);
	}
}
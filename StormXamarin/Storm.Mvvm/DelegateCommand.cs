using System;
using System.Windows.Input;

namespace Storm.Mvvm
{
	public abstract class CommandBase : ICommand
	{
		private Action<object> m_executeCallback;
		private Func<object, bool> m_canExecuteCallback;

		public event EventHandler CanExecuteChanged;

		public CommandBase(Action<object> _executeCallback)
		{
			m_executeCallback = _executeCallback;
			m_canExecuteCallback = o => true;
		}

		public CommandBase(Action<object> _executeCallback, Func<object, bool> _canExecuteCallback)
		{
			m_executeCallback = _executeCallback;
			m_canExecuteCallback = _canExecuteCallback;
		}

		public void RaiseCanExecuteChanged()
		{
			EventHandler handler = CanExecuteChanged;
			if(handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public bool CanExecute(object _parameter)
		{
			return m_canExecuteCallback(_parameter);
		}

		public void Execute(object _parameter)
		{
			m_executeCallback(_parameter);
		}
	}

	public class DelegateCommand : CommandBase
	{
		public DelegateCommand(Action _executeCallback)
			: base(o => _executeCallback())
		{

		}

		public DelegateCommand(Action _executeCallback, Func<bool> _canExecuteCallback)
			: base(o => _executeCallback(), o => _canExecuteCallback())
		{

		}
	}

	public class DelegateCommand<T> : CommandBase
	{
		public DelegateCommand(Action<T> _executeCallback)
			: base(o => _executeCallback((T)o))
		{

		}

		public DelegateCommand(Action<T> _executeCallback, Func<T, bool> _canExecuteCallback)
			: base(o => _executeCallback((T)o), o => _canExecuteCallback((T)o))
		{

		}
	}
}

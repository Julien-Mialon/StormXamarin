using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Storm.Mvvm
{
	public class EventToCommand : TriggerAction<DependencyObject>
	{
		#region Private fields

		#endregion

		#region Dependency properties

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null, OnCommandChanged));
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand), new PropertyMetadata(null, OnCommandParameterChanged));

		#endregion

		#region Properties

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		#endregion

		#region Methods

		protected override void Invoke(object parameter)
		{
			if (IsElementDisabled())
			{
				return;
			}

			ICommand command = Command;
			object commandParameter = CommandParameter;

			if (command != null && command.CanExecute(commandParameter))
			{
				command.Execute(commandParameter);
			}
		}

		protected Control GetAssociatedObject()
		{
			return AssociatedObject as Control;
		}

		protected bool IsElementDisabled()
		{
			Control element = GetAssociatedObject();
			if (element == null)
			{
				return false;
			}

			return element.IsEnabled;
		}

		protected void UpdateElementDisabledState()
		{
			Control element = GetAssociatedObject();
			if (element == null || Command == null)
			{
				return;
			}

			element.IsEnabled = Command.CanExecute(CommandParameter);
		}

		#endregion

		#region Dependency properties event changed handler

		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			EventToCommand evt = d as EventToCommand;
			if (evt == null)
			{
				return;
			}

			evt.OnCommandChanged(e.OldValue as ICommand, e.NewValue as ICommand);
		}

		private void OnCommandChanged(ICommand oldValue, ICommand newValue)
		{
			if (oldValue != null)
			{
				oldValue.CanExecuteChanged -= OnCanExecuteChanged;
			}

			if (newValue != null)
			{
				newValue.CanExecuteChanged += OnCanExecuteChanged;
			}

			UpdateElementDisabledState();
		}

		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			UpdateElementDisabledState();
		}

		private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			EventToCommand evt = d as EventToCommand;

			if (evt != null)
			{
				evt.UpdateElementDisabledState();
			}
		}

		#endregion
	}
}

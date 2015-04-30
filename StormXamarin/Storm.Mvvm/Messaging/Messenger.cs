using System;
using System.Collections.Generic;
using System.Linq;
using Storm.Mvvm.Extensions;

namespace Storm.Mvvm.Messaging
{
	public static class Messenger
	{
		private static readonly Dictionary<string, List<IMessageReceiver>> _recipients = new Dictionary<string, List<IMessageReceiver>>();

		public static void Register(string key, object receiver, Action callback)
		{
			RegisterCallback(key, receiver, null, (token, param) =>
			{
				if (callback != null)
				{
					callback();
				}
			});
		}

		public static void Register(string key, object receiver, Action<object> callback, object token)
		{
			RegisterCallback(key, receiver, null, (tok, param) =>
			{
				if (callback != null)
				{
					callback(tok);
				}
			});
		}

		public static void Register<T>(string key, object receiver, Action<T> callback)
		{
			RegisterCallback(key, receiver, null, (tok, param) =>
			{
				if (callback != null)
				{
					callback((T)param);
				}
			});
		}

		public static void Register<T>(string key, object receiver, Action<object, T> callback, object token)
		{
			RegisterCallback(key, receiver, null, (tok, param) =>
			{
				if (callback != null)
				{
					callback(tok, (T)param);
				}
			});
		}

		private static void RegisterCallback(string key, object receiver, object token, Action<object, object> callback)
		{
			if (!_recipients.ContainsKey(key))
			{
				_recipients.Add(key, new List<IMessageReceiver>());
			}
			_recipients[key].Add(new MessageReceiver(receiver, token, callback));
		}

		public static void Send(string key, object token = null)
		{
			SendCallback(key, token, null);
		}

		public static void Send<T>(string key, T param, object token = null)
		{
			SendCallback(key, token, param);
		}

		private static void SendCallback(string key, object token, object parameter)
		{
			if (_recipients.ContainsKey(key))
			{
				_recipients[key].Where(x => Equals(token, x.Token)).ForEach(item => item.HandleReceive(token, parameter));
			}
		}

		public static void Unregister(object receiver)
		{
			foreach (KeyValuePair<string, List<IMessageReceiver>> items in _recipients)
			{
				items.Value.RemoveWhere(x => Equals(receiver, x.Receiver));
			}
		}

		public static void Unregister(object receiver, string key)
		{
			if (_recipients.ContainsKey(key))
			{
				_recipients[key].RemoveWhere(x => Equals(receiver, x.Receiver));
			}
		}
	}
}

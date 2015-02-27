using System;
using System.Collections.Generic;
using System.Linq;

namespace Storm.Binding.AndroidTarget.Compiler
{
	public abstract class Expression
	{
		private string[] _keys;
		private readonly Dictionary<string, Expression> _attributes = new Dictionary<string, Expression>();

		public abstract ExpressionType Type { get; }

		public string[] AvailableKeys { get { return _keys ?? (_keys = InternalAvailableKeys); } }

		public IReadOnlyDictionary<string, Expression> Attributes { get { return _attributes; } }

		protected abstract string ContentKey { get; }

		protected abstract string[] InternalAvailableKeys { get; }
		
		public bool IsCorrectKey(string key)
		{
			return key == null || AvailableKeys.Any(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase));
		}

		public void Add(string key, Expression value)
		{
			string correctKey = key == null ? ContentKey : AvailableKeys.FirstOrDefault(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase));
			if (correctKey != null)
			{
				_attributes.Add(correctKey, value);
			}
		}

		public void Replace(string key, Expression value)
		{
			string correctKey = key == null ? ContentKey : AvailableKeys.FirstOrDefault(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase));
			if (correctKey != null)
			{
				_attributes.Remove(correctKey);
				_attributes.Add(correctKey, value);
			}
		}

		public bool Has(string key)
		{
			return _attributes.ContainsKey(key);
		}

		public T Get<T>(string key) where T : Expression
		{
			if (Has(key))
			{
				return _attributes[key] as T;
			}
			return null;
		}

		public string GetValue(string key)
		{
			TextExpression expr = Get<TextExpression>(key);
			if (expr != null)
			{
				return expr.Value;
			}
			return null;
		}

		public void SetContent(Expression value)
		{
			Add(null, value);
		}

		public Expression this[string key]
		{
			get { return Get<Expression>(key); }
		}

		public bool CheckCorrectness()
		{
			Dictionary<string, IEnumerable<ExpressionType>> expected = GetExpectedValueType();
			foreach (string key in Attributes.Keys)
			{
				Expression child = Attributes[key];
				if (!expected[key].Any(child.IsOfType) || !child.CheckCorrectness())
				{
					return false;
				}
				
			}
			return CheckConstraints();
		}

		public bool IsOfType(ExpressionType type)
		{
			return Type == type;
		}

		protected virtual Dictionary<string, IEnumerable<ExpressionType>> GetExpectedValueType()
		{
			return new Dictionary<string, IEnumerable<ExpressionType>>();
		}

		protected virtual bool CheckConstraints()
		{
			return true;
		}
	}
}

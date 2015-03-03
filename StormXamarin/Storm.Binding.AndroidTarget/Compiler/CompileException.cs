using System;
using System.Runtime.Serialization;

namespace Storm.Binding.AndroidTarget.Compiler
{
	[Serializable]
	public class CompileException : Exception
	{
		public CompileException()
		{
		}

		public CompileException(string message) : base(message)
		{
		}

		public CompileException(string message, Exception inner) : base(message, inner)
		{
		}

		protected CompileException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}

}

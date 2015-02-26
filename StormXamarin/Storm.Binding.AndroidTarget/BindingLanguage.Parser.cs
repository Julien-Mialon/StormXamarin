using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Storm.Binding.AndroidTarget
{
    internal partial class BindingLanguageParser
    {
        public BindingLanguageParser() : base(null) { }

        public void Parse(string s)
        {
            byte[] inputBuffer = System.Text.Encoding.Default.GetBytes(s);
            MemoryStream stream = new MemoryStream(inputBuffer);
            this.Scanner = new BindingLanguageScanner(stream);
            this.Parse();
        }
    }
}

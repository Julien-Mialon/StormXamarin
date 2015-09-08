using System.Collections.Generic;

namespace Storm.MvvmCross.Android.Target.CodeGenerator.Model
{
	class DependencyNode
	{
		public string Id { get; set; }

		public List<DependencyNode> Children { get; private set; }

		public List<DependencyNode> Dependencies { get; private set; }

		public bool IsMarked { get; set; }

		public DependencyNode()
		{
			Children = new List<DependencyNode>();
			Dependencies = new List<DependencyNode>();
		}

		public DependencyNode(string id) : this()
		{
			Id = id;
		}

		public void Add(DependencyNode child)
		{
			Children.Add(child);
			child.Dependencies.Add(this);
		}
	}
}

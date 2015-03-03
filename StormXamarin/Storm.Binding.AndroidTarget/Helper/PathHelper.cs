using System;
using System.IO;

namespace Storm.Binding.AndroidTarget.Helper
{
	public static class PathHelper
	{
		public static string ProjectDirectory
		{
			get { return Normalize(Environment.CurrentDirectory) + Path.DirectorySeparatorChar; }
		}

		public static string Normalize(string path)
		{
			return Path.GetFullPath(new Uri(Path.GetFullPath(path)).LocalPath)
				.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}

		public static string GetRelativePath(string filePath)
		{
			string normalized = Normalize(filePath);
			if (normalized.StartsWith(ProjectDirectory, StringComparison.OrdinalIgnoreCase))
			{
				return filePath.Substring(ProjectDirectory.Length);
			}

			BindingPreprocess.Logger.LogError("Error, unable to get relative path from {0} compare to {1}", ProjectDirectory, filePath);
			throw new Exception();
		}
	}
}

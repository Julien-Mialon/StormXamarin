using System;
using System.IO;

namespace Storm.Binding.AndroidTarget.Helper
{
	public static class PathHelper
	{
		public static string Normalize(string path)
		{
			return Path.GetFullPath(new Uri(Path.GetFullPath(path)).LocalPath)
				.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}

		public static string GetRelativePath(string relativePath, string filePath)
		{
			string normalized = Normalize(filePath);
			if (normalized.StartsWith(relativePath, StringComparison.OrdinalIgnoreCase))
			{
				return filePath.Substring(relativePath.Length);
			}

			BindingPreprocess.Logger.LogError("Error, unable to get relative path from {0} compare to {1}", relativePath, filePath);
			throw new Exception();
		}
	}
}

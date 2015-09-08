using System;
using System.Linq;
using Storm.MvvmCross.Android.Target.Configuration;
using Storm.MvvmCross.Android.Target.Model;

namespace Storm.MvvmCross.Android.Target.Helper
{
	public static class ParsingHelper
	{
		private const string USING_URI_START = "using:";
		private const string RESOURCE_TAG = "Resources";
		private const string FRAGMENT_TAG = "Fragment";
		private const string ID_ATTRIBUTE = "id";
		private const string RESOURCE_KEY_ATTRIBUTE = "key";
		private const string DATATEMPLATE_TAG = "DataTemplate";
		private const string STYLE_TAG = "Style";

		/// <summary>
		/// Check if an xml namespace is in fact a using uri to specify clr-namespace
		/// </summary>
		/// <param name="namespaceUri">the namespace uri to check</param>
		/// <returns>true if it's a using uri, false otherwise</returns>
		public static bool IsUsingUri(string namespaceUri)
		{
			return namespaceUri.StartsWith(USING_URI_START);
		}

		/// <summary>
		/// Extract namespace from a using uri
		/// </summary>
		/// <param name="namespaceUri">the using uri</param>
		/// <returns>the namespace contained in the using uri</returns>
		public static string GetUsingNamespace(string namespaceUri)
		{
			return namespaceUri.Substring(USING_URI_START.Length);
		}

		#region Xml tag matching region

		public static bool IsResourceTag(XmlElement element)
		{
			return IsTagNamed(element, RESOURCE_TAG);
		}

		public static bool IsFragmentTag(XmlElement element)
		{
			return IsTagNamed(element, FRAGMENT_TAG);
		}

		public static bool IsDataTemplateTag(XmlElement element)
		{
			return IsTagNamed(element, DATATEMPLATE_TAG);
		}

		public static bool IsStyleTag(XmlElement element)
		{
			return IsTagNamed(element, STYLE_TAG);
		}

		private static bool IsTagNamed(XmlElement element, string name)
		{
			return name.Equals(element.LocalName, StringComparison.InvariantCultureIgnoreCase);
		}

		#endregion

		#region Xml attribute matching region 

		public static bool IsIdAttribute(XmlAttribute attribute)
		{
			return IsAttributeNamed(attribute, ID_ATTRIBUTE);
		}

		public static bool IsCustomAttribute(XmlAttribute attribute)
		{
			return DefaultConfiguration.CustomAttribute.Any(x => IsAttributeNamed(attribute, x));
		}

		public static bool IsXmlOnlyAttribute(XmlAttribute attribute)
		{
			return DefaultConfiguration.XmlOnlyAttributes.Any(x => IsAttributeNamed(attribute, x));
		}

		public static bool IsResourceKeyAttribute(XmlAttribute attribute)
		{
			return IsAttributeNamed(attribute, RESOURCE_KEY_ATTRIBUTE);
		}

		public static bool IsStyleAttribute(XmlAttribute attribute)
		{
			return IsAttributeNamed(attribute, DefaultConfiguration.XmlStyleAttribute);
		}

		private static bool IsAttributeNamed(XmlAttribute attribute, string name)
		{
			return name.Equals(attribute.LocalName, StringComparison.InvariantCultureIgnoreCase);
		}

		public static XmlAttribute GetResourceKeyAttribute(XmlElement element)
		{
			return element.Attributes.FirstOrDefault(IsResourceKeyAttribute);
		}

		#endregion

		public static bool IsAttributeWithExpression(XmlAttribute attribute)
		{
			return IsExpressionValue(attribute.Value);
		}

		public static bool IsExpressionValue(string value)
		{
			string trimmed = value.Trim();
			return trimmed.StartsWith("{") && trimmed.EndsWith("}");
		}
	}
}

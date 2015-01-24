using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Localization
{
	public static class LocalizationHelper
	{
		private const string UidPropertyName = "Uid";
		private const string PropertiesPropertyName = "Properties";

		public static ILocalizationService ResourceService;

		public static string GetUid(DependencyObject obj)
		{
			return (string)obj.GetValue(UidProperty);
		}

		public static void SetUid(DependencyObject obj, string value)
		{
			obj.SetValue(UidProperty, value);
		}

		public static string GetProperties(DependencyObject obj)
		{
			return (string)obj.GetValue(PropertiesProperty);
		}

		public static void SetProperties(DependencyObject obj, string value)
		{
			obj.SetValue(PropertiesProperty, value);
		}

		public static readonly DependencyProperty UidProperty = DependencyProperty.RegisterAttached(UidPropertyName, typeof(string), typeof(LocalizationHelper), new PropertyMetadata(null, UidChangedCallback));
		public static readonly DependencyProperty PropertiesProperty = DependencyProperty.RegisterAttached(PropertiesPropertyName, typeof(string), typeof(LocalizationHelper), new PropertyMetadata(null, PropertiesChangedCallback));

		private static void UidChangedCallback(object sender, DependencyPropertyChangedEventArgs args)
		{
#if DEBUG
			if (DesignerProperties.IsInDesignTool)
			{
				return;
			}
#endif
			var attachedObject = sender as DependencyObject;
			var uid = args.NewValue as string;

			if (attachedObject != null)
			{
				string properties = GetProperties(attachedObject);

				if (!String.IsNullOrEmpty(properties))
				{
					UpdateProperties(attachedObject, properties, uid);
				}
			}
		}

		private static void PropertiesChangedCallback(object sender, DependencyPropertyChangedEventArgs args)
		{
#if DEBUG
			if (DesignerProperties.IsInDesignTool)
			{
				return;
			}
#endif
			var attachedObject = sender as DependencyObject;
			var properties = args.NewValue as string;

			if (attachedObject != null)
			{
				string uid = GetUid(attachedObject);

				UpdateProperties(attachedObject, properties, uid);
			}
		}

		private static void UpdateProperties(DependencyObject attachedObject, string properties, string uid)
		{
#if DEBUG
			if (DesignerProperties.IsInDesignTool)
			{
				return;
			}
#endif
			IEnumerable<string> props = properties.Split(',');

			Type attachedType = attachedObject.GetType();
			bool hasProperties = props.Any();

			foreach (PropertyInfo propInfo in props.Select(attachedType.GetRuntimeProperty).Where(propInfo => propInfo != null))
			{
				string value = hasProperties ? ResourceService.GetString(uid, properties) : ResourceService.GetString(uid);
				propInfo.SetValue(attachedObject, value);
			}
		}
	}
}

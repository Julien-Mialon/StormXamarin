using System;
using System.Collections.Generic;
using System.Linq;
using Storm.Binding.AndroidTarget.Helper;
using Storm.Binding.AndroidTarget.Model;

namespace Storm.Binding.AndroidTarget.Preprocessor
{
	class ViewFileProcessor
	{
		public Tuple<List<XmlAttribute>, List<IdViewObject>> ExtractExpressions(XmlElement element)
		{
			List<IdViewObject> viewsId = new List<IdViewObject>();
			List<XmlAttribute> expressionAttributes = new List<XmlAttribute>();
			_ExtractExpressions(element, expressionAttributes, viewsId);

			return new Tuple<List<XmlAttribute>, List<IdViewObject>>(expressionAttributes, viewsId);
		}

		public List<Resource> ExtractResources(XmlElement element)
		{
			List<Resource> result = new List<Resource>();
			List<XmlElement> toRemove = new List<XmlElement>();

			foreach (XmlElement child in element.Children)
			{
				if (ParsingHelper.IsResourceTag(child))
				{
					// parse content to get resources
					foreach (XmlElement resourceElement in child.Children)
					{
						XmlAttribute keyAttribute = ParsingHelper.GetResourceKeyAttribute(resourceElement);
						if (keyAttribute == null)
						{
							BindingPreprocess.Logger.LogError("You have a resource of type {0} with no key", string.IsNullOrWhiteSpace(resourceElement.NamespaceName) ? resourceElement.LocalName : string.Format("{0}:{1}", resourceElement.NamespaceName, resourceElement.LocalName));
						}
						else
						{
							Resource resource = new Resource(keyAttribute.Value)
							{
								ResourceElement = resourceElement,
								Type = resourceElement.LocalName,
							};
							foreach (XmlAttribute attribute in resourceElement.Attributes.Where(x => !ParsingHelper.IsResourceKeyAttribute(x)))
							{
								resource.Properties.Add(attribute.LocalName, attribute.Value);
							}
							result.Add(resource);
						}
					}

					// and then mark it for remove from the tree since it does not need to be there anymore
					toRemove.Add(child);
				}
				else
				{
					result.AddRange(ExtractResources(child));
				}
			}

			foreach (XmlElement toRemoveChild in toRemove)
			{
				element.Children.Remove(toRemoveChild);
			}

			return result;
		}

		public List<Resource> ExtractGlobalResources(XmlElement element)
		{
			List<Resource> result = new List<Resource>();
			
			if (ParsingHelper.IsResourceTag(element))
			{
				// parse content to get resources
				foreach (XmlElement resourceElement in element.Children)
				{
					XmlAttribute keyAttribute = ParsingHelper.GetResourceKeyAttribute(resourceElement);
					if (keyAttribute == null)
					{
						BindingPreprocess.Logger.LogError("You have a resource of type {0} with no key", string.IsNullOrWhiteSpace(resourceElement.NamespaceName) ? resourceElement.LocalName : string.Format("{0}:{1}", resourceElement.NamespaceName, resourceElement.LocalName));
					}
					else
					{
						Resource resource = new Resource(keyAttribute.Value)
						{
							ResourceElement = resourceElement,
							Type = resourceElement.LocalName,
						};
						foreach (XmlAttribute attribute in resourceElement.Attributes.Where(x => !ParsingHelper.IsResourceKeyAttribute(x)))
						{
							resource.Properties.Add(attribute.LocalName, attribute.Value);
						}
						result.Add(resource);
					}
				}
			}
			else
			{
				throw new Exception("Global resource files which do not start with a resource tag");
			}
			
			return result;
		}

		private void _ExtractExpressions(XmlElement element, List<XmlAttribute> expressionAttributes, List<IdViewObject> viewsId)
		{
			if (ParsingHelper.IsResourceTag(element))
			{
				//will be processed later so do not touch it for now
				return;
			}

			string id = null;
			bool isFragment = ParsingHelper.IsFragmentTag(element);
			List<XmlAttribute> bindings = new List<XmlAttribute>();
			
			foreach (XmlAttribute attribute in element.Attributes)
			{
				if (ParsingHelper.IsXmlOnlyAttribute(attribute))
				{
					continue;
				}

				if (ParsingHelper.IsIdAttribute(attribute))
				{
					if (id != null)
					{
						throw new Exception("Multiple id for same element");
					}

					id = attribute.Value;
					// add @+id/ to use auto declare mechanism in Android
					attribute.Value = "@+id/" + id;

					//check isFragment cause we will do a findViewById next and it will not works
					if (!isFragment)
					{
						viewsId.Add(new IdViewObject(element.LocalName, id));
					}
				}
				else if (ParsingHelper.IsCustomAttribute(attribute))
				{
					bindings.Add(attribute);
				}
				else if(ParsingHelper.IsAttributeWithExpression(attribute))
				{
					bindings.Add(attribute);
				}
			}

			if (id == null && bindings.Any())
			{
				XmlAttribute attribute = new XmlAttribute();
				id = NameGeneratorHelper.GetViewId();
				attribute.Value = "@+id/" + id;
				attribute.FullName = "android:id";
				attribute.NamespaceUri = "http://schemas.android.com/apk/res/android";

				element.Attributes.Add(attribute);

				//check isFragment cause we will do a findViewById next and it will not works
				if (!isFragment)
				{
					viewsId.Add(new IdViewObject(element.LocalName, id));
				}
			}

			foreach (XmlAttribute attribute in bindings)
			{
				attribute.AttachedId = id;
				element.Attributes.Remove(attribute);
			}
			expressionAttributes.AddRange(bindings);

			foreach (XmlElement child in element.Children)
			{
				_ExtractExpressions(child, expressionAttributes, viewsId);
			}
		}
	}
}

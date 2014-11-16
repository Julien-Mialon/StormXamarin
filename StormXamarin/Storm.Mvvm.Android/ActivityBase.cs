using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using Storm.Mvvm.Android.Bindings;

namespace Storm.Mvvm.Android
{
	public class ActivityBase : Activity
	{
		protected ViewModelBase ViewModel { get; private set; }

		private ExpressionNode _rootExpressionNode;

		protected void SetViewModel(ViewModelBase viewModel, Type idContainerType)
		{
			DateTime startTime = DateTime.Now;
			ViewModel = viewModel;

			List<BindingObject> bindingObjects = GetBindingPaths();

			//Get ids values 
			Dictionary<string, int> ids = idContainerType.GetRuntimeFields().Where(field => field.IsLiteral).ToDictionary(field => field.Name, field => (int)field.GetRawConstantValue());

			foreach (BindingObject bindingObject in bindingObjects)
			{
				bindingObject.TargetObject = FindViewById(ids[bindingObject.TargetObjectName]);
				if (bindingObject.TargetObject == null)
				{
					throw new Exception("Can not get object " + bindingObject.TargetObjectName);
				}
				Type targetType = bindingObject.TargetObject.GetType();
				//Process all expressions
				foreach (BindingExpression expression in bindingObject.Expressions)
				{
					expression.BindingObject = bindingObject;
					//if a property exists, use it
					PropertyInfo property = targetType.GetProperty(expression.TargetField, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
					if (property != null)
					{
						expression.TargetPropertyHandler = property;
					}
					else
					{
						//otherwise supposed its an event
						EventInfo ev = targetType.GetEvent(expression.TargetField, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
						if (ev != null)
						{
							expression.TargetEventHandler = ev;
						}
						else
						{
							throw new Exception("ActivityBase : can not infer if binding expression " + expression.TargetField + " is an event or property in object of type " + targetType);
						}
					}
					
					if (!string.IsNullOrWhiteSpace(expression.UpdateEvent))
					{
						EventInfo ev = targetType.GetEvent(expression.UpdateEvent, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
						if (ev != null)
						{
							expression.UpdateEventHandler = ev;
						}
						else
						{
							throw new Exception("ActivityBase : no update event " + expression.UpdateEvent + " in type " + targetType);
						}
					}
				}
			}

			_rootExpressionNode = new ExpressionNode
			{
				PropertyName = "",
			};
			ParseExpressionNodes("", bindingObjects.SelectMany(x => x.Expressions), _rootExpressionNode);

			_rootExpressionNode.FullUpdate(ViewModel);

			DateTime endTime = DateTime.Now;
			TimeSpan diff = endTime - startTime;
			System.Diagnostics.Debug.WriteLine("===> Time to process binding and update values : " + diff.TotalMilliseconds);
		}

		private void ParseExpressionNodes(string prefix, IEnumerable<BindingExpression> expressions, ExpressionNode rootNode)
		{
			//RMQ : prefix do not contains final dot "."
			if (prefix == null)
			{
				prefix = "";
			}

			Dictionary<string, ExpressionNode> newNodes = new Dictionary<string, ExpressionNode>();
			string fullPrefix = string.IsNullOrEmpty(prefix) ? prefix : prefix + ".";
			List<BindingExpression> validExpressions = expressions.Where(x => x.SourcePath.StartsWith(prefix)).ToList();
			foreach (BindingExpression expression in validExpressions)
			{
				if (expression.SourcePath == prefix)
				{
					// Add to bindingExpression list in the node
					rootNode.Expressions.Add(expression);
				}
				else
				{
					string prefixRemoved = expression.SourcePath.Substring(fullPrefix.Length);
					string currentBindingEvaluation = (prefixRemoved.Contains(".") ? prefixRemoved.Substring(0, prefixRemoved.IndexOf('.')) : prefixRemoved);

					if (!newNodes.ContainsKey(currentBindingEvaluation))
					{
						ExpressionNode currentChildNode = new ExpressionNode
						{
							PropertyName = currentBindingEvaluation,
						};
						rootNode.Children.Add(currentChildNode.PropertyName, currentChildNode);
						newNodes.Add(currentBindingEvaluation, currentChildNode);
						ParseExpressionNodes(fullPrefix + currentBindingEvaluation, validExpressions, currentChildNode);
					}
				}
			}
		}

		protected virtual List<BindingObject> GetBindingPaths()
		{
			return new List<BindingObject>();
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiffMatchPatch;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace JsonDiffPatchDotNet
{
	public class JsonDiffPatch
	{
		private static readonly string[] FILE_MODELS_INDICATOR_FIELDS = 
		{
			"filePath", "uploadId", "file"
		};
		
        public string MainPropertyName = string.Empty;
        public bool IgnoreFilesData;

        private readonly Options _options;

		public JsonDiffPatch()
			: this(new Options())
		{
		}

		public JsonDiffPatch(Options options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			_options = options;
		}

		/// <summary>
		/// Diff two JSON objects.
		/// 
		/// The output is a JObject that contains enough information to represent the
		/// delta between the two objects and to be able perform patch and reverse operations.
		/// </summary>
		/// <param name="left">The base JSON object</param>
		/// <param name="right">The JSON object to compare against the base</param>
		/// <returns>JSON Patch Document</returns>
		public JToken Diff(JToken left, JToken right, string mainPropertyName = "", JObject parent = null)
		{
			if (left == null)
				left = new JValue("");
			if (right == null)
				right = new JValue("");

			if (left.Type == JTokenType.Object && right.Type == JTokenType.Object)
			{
				var leftObj = (JObject)left;
				if (leftObj.Properties().Any() && leftObj.Properties().First().Value.ToString().Contains("System.Byte[]"))
				{
					return (right.Last as JProperty).Value;
				}
				return ObjectDiff((JObject)left, (JObject)right, mainPropertyName);
			}

			if (_options.ArrayDiff == ArrayDiffMode.Efficient
				&& left.Type == JTokenType.Array
				&& right.Type == JTokenType.Array)
			{
				var leftAsJArray = (JArray) left;
				var rightAsJArray = (JArray) right;
				if (leftAsJArray.Count > 0  && IsFileModel(leftAsJArray.First as JObject) ||
				    rightAsJArray.Count > 0  && IsFileModel(rightAsJArray.First as JObject))
				{
					if (IgnoreFilesData)
						return null;
					
					var output = new JArray();
					foreach (var rightEntity in rightAsJArray)
					{
						if(NeedIncludeFile(rightEntity as JObject))
							output.Add(rightEntity);
					}
					
					return output;
				}
				
				return ArrayDiff(leftAsJArray, rightAsJArray, mainPropertyName, parent);
			}

			// if (_options.TextDiff == TextDiffMode.Efficient
			// 	&& left.Type == JTokenType.String
			// 	&& right.Type == JTokenType.String
			// 	&& (left.ToString().Length > _options.MinEfficientTextDiffLength || right.ToString().Length > _options.MinEfficientTextDiffLength))
			// {
			// 	var dmp = new diff_match_patch();
			// 	List<Patch> patches = dmp.patch_make(left.ToObject<string>(), right.ToObject<string>());
			// 	return patches.Any()
			// 		? new JArray(dmp.patch_toText(patches), 0, (int)DiffOperation.TextDiff)
			// 		: null;
			// }

			if (!JToken.DeepEquals(left, right))
				return right;

			return null;
		}

		/// <summary>
		/// Patch a JSON object
		/// </summary>
		/// <param name="left">Unpatched JSON object</param>
		/// <param name="patch">JSON Patch Document</param>
		/// <returns>Patched JSON object</returns>
		/// <exception cref="System.IO.InvalidDataException">Thrown if the patch document is invalid</exception>
		public JToken Patch(JToken left, JToken patch)
		{
			if (patch == null)
				return left;

			if (patch.Type == JTokenType.Object)
			{
				var patchObj = (JObject)patch;
				JProperty arrayDiffCanary = patchObj.Property("_t");

				if (left != null
					&& left.Type == JTokenType.Array
					&& arrayDiffCanary != null
					&& arrayDiffCanary.Value.Type == JTokenType.String
					&& arrayDiffCanary.Value.ToObject<string>() == "a")
				{
					return ArrayPatch((JArray)left, patchObj);
				}

				return ObjectPatch(left as JObject, patchObj);
			}

			if (patch.Type == JTokenType.Array)
			{
				var patchArray = (JArray)patch;

				if (patchArray.Count == 1)	// Add
				{
					return patchArray[0];
				}

				if (patchArray.Count == 2)	// Replace
				{
					return patchArray[1];
				}

				if (patchArray.Count == 3)	// Delete, Move or TextDiff
				{
					if (patchArray[2].Type != JTokenType.Integer)
						throw new InvalidDataException("Invalid patch object");

					int op = patchArray[2].Value<int>();

					if (op == 0)
					{
						return null;
					}

					if (op == 2)
					{
						if (left.Type != JTokenType.String)
							throw new InvalidDataException("Invalid patch object");

						var dmp = new diff_match_patch();
						List<Patch> patches = dmp.patch_fromText(patchArray[0].ToObject<string>());

						if (!patches.Any())
							throw new InvalidDataException("Invalid textline");

						object[] result = dmp.patch_apply(patches, left.Value<string>());
						var patchResults = (bool[])result[1];
						if (patchResults.Any(x => !x))
							throw new InvalidDataException("Text patch failed");

						string right = (string)result[0];
						return right;
					}

					throw new InvalidDataException("Invalid patch object");
				}

				throw new InvalidDataException("Invalid patch object");
			}

			return null;
		}

		/// <summary>
		/// Unpatch a JSON object
		/// </summary>
		/// <param name="right">Patched JSON object</param>
		/// <param name="patch">JSON Patch Document</param>
		/// <returns>Unpatched JSON object</returns>
		/// <exception cref="System.IO.InvalidDataException">Thrown if the patch document is invalid</exception>
		public JToken Unpatch(JToken right, JToken patch)
		{
			if (patch == null)
				return right;

			if (patch.Type == JTokenType.Object)
			{
				var patchObj = (JObject)patch;
				JProperty arrayDiffCanary = patchObj.Property("_t");

				if (right != null
					&& right.Type == JTokenType.Array
					&& arrayDiffCanary != null
					&& arrayDiffCanary.Value.Type == JTokenType.String
					&& arrayDiffCanary.Value.ToObject<string>() == "a")
				{
					return ArrayUnpatch((JArray)right, patchObj);
				}

				return ObjectUnpatch(right as JObject, patchObj);
			}

			if (patch.Type == JTokenType.Array)
			{
				var patchArray = (JArray)patch;

				if (patchArray.Count == 1)	// Add (we need to remove the property)
				{
					return null;
				}

				if (patchArray.Count == 2)	// Replace
				{
					return patchArray[0];
				}

				if (patchArray.Count == 3)	// Delete, Move or TextDiff
				{
					if (patchArray[2].Type != JTokenType.Integer)
						throw new InvalidDataException("Invalid patch object");

					int op = patchArray[2].Value<int>();

					if (op == 0)
					{
						return patchArray[0];
					}
					if (op == 2)
					{
						if (right.Type != JTokenType.String)
							throw new InvalidDataException("Invalid patch object");

						var dmp = new diff_match_patch();
						List<Patch> patches = dmp.patch_fromText(patchArray[0].ToObject<string>());

						if (!patches.Any())
							throw new InvalidDataException("Invalid textline");

						var unpatches = new List<Patch>();
						for (int i = patches.Count - 1; i >= 0; --i)
						{
							Patch p = patches[i];
							var u = new Patch
							{
								length1 = p.length1,
								length2 = p.length2,
								start1 = p.start1,
								start2 = p.start2
							};

							foreach (Diff d in p.diffs)
							{
								if (d.operation == Operation.DELETE)
								{
									u.diffs.Add(new Diff(Operation.INSERT, d.text));
								}
								else if (d.operation == Operation.INSERT)
								{
									u.diffs.Add(new Diff(Operation.DELETE, d.text));
								}
								else
								{
									u.diffs.Add(d);
								}
							}
							unpatches.Add(u);
						}

						object[] result = dmp.patch_apply(unpatches, right.Value<string>());
						var unpatchResults = (bool[])result[1];
						if (unpatchResults.Any(x => !x))
							throw new InvalidDataException("Text patch failed");

						string left = (string)result[0];
						return left;
					}
					throw new InvalidDataException("Invalid patch object");
				}

				throw new InvalidDataException("Invalid patch object");
			}

			return null;
		}

		#region String Overrides

		/// <summary>
		/// Diff two JSON objects.
		/// 
		/// The output is a JObject that contains enough information to represent the
		/// delta between the two objects and to be able perform patch and reverse operations.
		/// </summary>
		/// <param name="left">The base JSON object</param>
		/// <param name="right">The JSON object to compare against the base</param>
		/// <returns>JSON Patch Document</returns>
		public string Diff(string left, string right)
		{
			JToken obj = Diff(JToken.Parse(left ?? ""), JToken.Parse(right ?? ""));
			return obj?.ToString();
		}

		/// <summary>
		/// Patch a JSON object
		/// </summary>
		/// <param name="left">Unpatched JSON object</param>
		/// <param name="patch">JSON Patch Document</param>
		/// <returns>Patched JSON object</returns>
		/// <exception cref="System.IO.InvalidDataException">Thrown if the patch document is invalid</exception>
		public string Patch(string left, string patch)
		{
			JToken patchedObj = Patch(JToken.Parse(left ?? ""), JToken.Parse(patch ?? ""));
			return patchedObj?.ToString();
		}

		/// <summary>
		/// Unpatch a JSON object
		/// </summary>
		/// <param name="right">Patched JSON object</param>
		/// <param name="patch">JSON Patch Document</param>
		/// <returns>Unpatched JSON object</returns>
		/// <exception cref="System.IO.InvalidDataException">Thrown if the patch document is invalid</exception>
		public string Unpatch(string right, string patch)
		{
			JToken unpatchedObj = Unpatch(JToken.Parse(right ?? ""), JToken.Parse(patch ?? ""));
			return unpatchedObj?.ToString();
		}

		#endregion

		private JObject ObjectDiff(JObject left, JObject right, string mainPropertyName = "")
		{
			if (left == null)
				throw new ArgumentNullException(nameof(left));
			if (right == null)
				throw new ArgumentNullException(nameof(right));

			var diffPatch = new JObject();

            if (string.IsNullOrEmpty(mainPropertyName))
            {
                mainPropertyName = MainPropertyName.ToLower();
            }

            bool hasMainProperty = left.TryGetValue(mainPropertyName, out var mainPropertyValue);
            if (hasMainProperty)
            {
                JToken d = Diff(mainPropertyValue, right.GetValue(mainPropertyName));
                if (d != null)
                {
                    return right;
                }
                else
                {
                    diffPatch.Add(new JProperty(mainPropertyName, mainPropertyValue));
                }
            }else if (IsFileModel(right))
            {
	            return NeedIncludeFile(right) ? right : null;
            }

            // Find properties modified or deleted
			foreach (var lp in left.Properties())
			{
				if (lp.Name == mainPropertyName) continue;

				JProperty rp = right.Property(lp.Name);
				if (lp.Name == "$type")
				{
					continue;
				}
				// Property deleted
				if (rp == null)
				{
					diffPatch.Add(new JProperty(lp.Name, new JArray(lp.Value, 0, (int)DiffOperation.Deleted)));
					continue;
				}

				JToken d = Diff(lp.Value, rp.Value, string.Empty, left);
				if (d != null)
				{
					diffPatch.Add(new JProperty(lp.Name, d));
				}
			}

			// Find properties that were added 
			foreach (var rp in right.Properties())
			{
				if (rp.Name == mainPropertyName) continue;

				if (left.Property(rp.Name) != null)
					continue;

				diffPatch.Add(new JProperty(rp.Name, new JArray(rp.Value)));
			}

			if (diffPatch.Properties().Any())
				return diffPatch;

			return null;
		}
		
        private bool IsFileModel(JObject target)
        {
			if (target == null) return false;
            var allProps = target.Properties();
            return allProps.Any(x => FILE_MODELS_INDICATOR_FIELDS.Contains(x.Name));
        }

        private bool NeedIncludeFile(JObject target)
        {
            var needInclude = false;
			if (target.ContainsKey("$type")) target.Remove("$type");

			if (target != null)
            {
	            target.TryGetValue("state", out var state);
	            var val = state.Value<int>();
                needInclude =  val == 0 || val == 1;
            }

            return needInclude;
        }

        private JArray ArrayDiff(JArray leftArray, JArray rightArray, string mainPropertyName = "", JObject parent = null)
		{
            if (JToken.DeepEquals(leftArray, rightArray)) return null;

            if (leftArray.Count == 0 && rightArray.Count == 0) return null;

            var result = new JArray();

            // Try Get main property name if MainName not present 
            if (string.IsNullOrEmpty(mainPropertyName))
            {
                mainPropertyName = MainPropertyName;
            }

            JObject testObject = null;
            if (leftArray.Count > 0)
            {
                testObject = leftArray[0] as JObject;
            }
            else if(rightArray.Count > 0)
            {
                testObject = rightArray[0] as JObject;
            }

            if (testObject == null)//if it is primitive array like int[]
	            return rightArray;

			var testObjectProperties = testObject.Properties();

			var isManyToMany = testObjectProperties.All(p => p.Name != MainPropertyName)
				&& testObjectProperties.All(p => p.Name != "uploadId")
				&& testObjectProperties.Any(p => p.Name.EndsWith(MainPropertyName.FirstCharToUpper()));

			if (isManyToMany) // Sorry for this (not my fault)
            {
				var parentNameSpace = parent["$type"].Value<string>().Split(',').First();
                var lastDotIndex = parentNameSpace.LastIndexOf('.');
                var parentName = parentNameSpace.Substring(lastDotIndex + 1, parentNameSpace.Length - lastDotIndex - 1);
                var parentFkName = parentName.FirstCharToLower() + MainPropertyName.FirstCharToUpper();

				var allIdProperties = testObject.Properties().Where(p => p.Name.EndsWith(MainPropertyName.FirstCharToUpper())).ToArray();

				JProperty childFkProp = allIdProperties.FirstOrDefault(x=>x.Name != parentFkName);
                mainPropertyName = childFkProp.Name;
				Debug.Log("Parent: " + mainPropertyName);
            }

            if (!string.IsNullOrEmpty(mainPropertyName))
            {
                leftArray = ArrayOrderBy(leftArray, mainPropertyName);
                rightArray = ArrayOrderBy(rightArray, mainPropertyName);
            }

            HashSet<JToken> mainPropertiesSet = new HashSet<JToken>();

            for (int i = 0; i < rightArray.Count; i++)
            {
                JToken rightElement = rightArray[i];
                JToken leftElement = null;

                if (!string.IsNullOrEmpty(mainPropertyName))
                {
                    var rightMainValue = (rightElement as JObject).GetValue(mainPropertyName);
                    leftElement = leftArray.FirstOrDefault(j => Diff((j as JObject).GetValue(mainPropertyName), rightMainValue) == null);
                    mainPropertiesSet.Add(rightMainValue);
                }
                else if (i < leftArray.Count)
                {
                    leftElement = leftArray[i];
                }

                if (leftElement == null)
                {
					if (isManyToMany)
                    {
						var obj = new JObject();
						foreach (var item in (rightElement as JObject).Properties())
						{
							if (item.Name == "$type") continue;
							if (item.Name.EndsWith(MainPropertyName.FirstCharToUpper()) && item.Name != mainPropertyName)
							{
								obj.Add(item.Name, 0);
							}
							else
							{
								obj.Add(item.Name, item.Value);
							}
						}
                        result.Add(obj);
					}
                    else
                    {
                        result.Add(rightElement);
                    }
                    continue;
                }

				JToken diff = Diff(leftElement, rightElement, mainPropertyName, parent);
			
                if (diff != null)
                {
                    if (isManyToMany)
                    {
                        var jObject = new JObject();
                        var diffObject = diff as JObject;
                        var allIdProperties = (leftElement as JObject).Properties().Where(p => p.Name.EndsWith(MainPropertyName.FirstCharToUpper()));
                        var notMainProp = allIdProperties.First(x => x.Name != mainPropertyName);
                        var childFk = diffObject.Properties().First(x => x.Name == mainPropertyName);
						jObject.Add(childFk);
						jObject.Add(notMainProp);
						foreach (var property in (diff as JObject).Properties())
						{
							if (property.Name == childFk.Name || property.Name == notMainProp.Name) continue;
							jObject.Add(property.Name, property.Value);
						}
						result.Add(jObject);
					}
                    else
                    {
                        result.Add(diff);
					}
                }
            }

            if (!string.IsNullOrEmpty(mainPropertyName))
            {
                for (int i = 0; i < leftArray.Count; i++)
                {
                    JToken leftElement = leftArray[i];
                    var leftMainValue = (leftElement as JObject).GetValue(mainPropertyName);
                    if (mainPropertiesSet.Contains(leftMainValue))
                        continue;
                    var deletedElement = new JObject();
                    var leftValue = leftMainValue.Value<long>();
                    deletedElement.Add(mainPropertyName, -leftValue);
                    result.Add(deletedElement);
                }
            }

			return result;
		}

        private JObject ObjectPatch(JObject obj, JObject patch)
		{
			if (obj == null)
				obj = new JObject();
			if (patch == null)
				return obj;

			var target = (JObject)obj.DeepClone();

			foreach (var diff in patch.Properties())
			{
				JProperty property = target.Property(diff.Name);
				JToken patchValue = diff.Value;

				// We need to special case deletion when doing objects since a delete is a removal of a property
				// not a null assignment
				if (patchValue.Type == JTokenType.Array && ((JArray)patchValue).Count == 3 && patchValue[2].Value<int>() == 0)
				{
					target.Remove(diff.Name);
				}
				else
				{
					if (property == null)
					{
						target.Add(new JProperty(diff.Name, Patch(null, patchValue)));
					}
					else
					{
						property.Value = Patch(property.Value, patchValue);
					}
				}
			}

			return target;
		}

		private JArray ArrayPatch(JArray left, JObject patch)
		{
			var toRemove = new List<JProperty>();
			var toInsert = new List<JProperty>();
			var toModify = new List<JProperty>();

			foreach (JProperty op in patch.Properties())
			{
				if (op.Name == "_t")
					continue;

				var value = op.Value as JArray;

				if (op.Name.StartsWith("_"))
				{
					// removed item from original array
					if (value != null && value.Count == 3 && (value[2].ToObject<int>() == (int)DiffOperation.Deleted || value[2].ToObject<int>() == (int)DiffOperation.ArrayMove))
					{
						toRemove.Add(new JProperty(op.Name.Substring(1), op.Value));

						if (value[2].ToObject<int>() == (int)DiffOperation.ArrayMove)
							toInsert.Add(new JProperty(value[1].ToObject<int>().ToString(), new JArray(left[int.Parse(op.Name.Substring(1))].DeepClone())));
					}
					else
					{
						throw new Exception($"Only removal or move can be applied at original array indices. Context: {value}");
					}
				}
				else
				{
					if (value != null && value.Count == 1)
					{
						toInsert.Add(op);
					}
					else
					{
						toModify.Add(op);
					}
				}
			}


			// remove items, in reverse order to avoid sawing our own floor
			toRemove.Sort((x, y) => int.Parse(x.Name).CompareTo(int.Parse(y.Name)));
			for (int i = toRemove.Count - 1; i >= 0; --i)
			{
				JProperty op = toRemove[i];
				left.RemoveAt(int.Parse(op.Name));
			}

			// insert items, in reverse order to avoid moving our own floor
			toInsert.Sort((x, y) => int.Parse(y.Name).CompareTo(int.Parse(x.Name)));
			for (int i = toInsert.Count - 1; i >= 0; --i)
			{
				JProperty op = toInsert[i];
				left.Insert(int.Parse(op.Name), ((JArray)op.Value)[0]);
			}

			foreach (var op in toModify)
			{
				JToken p = Patch(left[int.Parse(op.Name)], op.Value);
				left[int.Parse(op.Name)] = p;
			}

			return left;
		}

		private JObject ObjectUnpatch(JObject obj, JObject patch)
		{
			if (obj == null)
				obj = new JObject();
			if (patch == null)
				return obj;

			var target = (JObject)obj.DeepClone();

			foreach (var diff in patch.Properties())
			{
				JProperty property = target.Property(diff.Name);
				JToken patchValue = diff.Value;

				// We need to special case addition when doing objects since an undo add is a removal of a property
				// not a null assignment
				if (patchValue.Type == JTokenType.Array && ((JArray)patchValue).Count == 1)
				{
					target.Remove(property.Name);
				}
				else
				{
					if (property == null)
					{
						target.Add(new JProperty(diff.Name, Unpatch(null, patchValue)));
					}
					else
					{
						property.Value = Unpatch(property.Value, patchValue);
					}
				}
			}

			return target;
		}

		private JArray ArrayUnpatch(JArray right, JObject patch)
		{
			var toRemove = new List<JProperty>();
			var toInsert = new List<JProperty>();
			var toModify = new List<JProperty>();

			foreach (JProperty op in patch.Properties())
			{
				if (op.Name == "_t")
					continue;

				var value = op.Value as JArray;

				if (op.Name.StartsWith("_"))
				{
					// removed item from original array
					if (value != null && value.Count == 3 && (value[2].ToObject<int>() == (int)DiffOperation.Deleted || value[2].ToObject<int>() == (int)DiffOperation.ArrayMove))
					{
						var newOp = new JProperty(value[1].ToObject<int>().ToString(), op.Value);

						if (value[2].ToObject<int>() == (int)DiffOperation.ArrayMove)
						{
							toInsert.Add(new JProperty(op.Name.Substring(1), new JArray(right[value[1].ToObject<int>()].DeepClone())));
							toRemove.Add(newOp);
						}
						else
						{
							toInsert.Add(new JProperty(op.Name.Substring(1), new JArray(value[0])));
						}
					}
					else
					{
						throw new Exception($"Only removal or move can be applied at original array indices. Context: {value}");
					}
				}
				else
				{
					if (value != null && value.Count == 1)
					{
						toRemove.Add(op);
					}
					else
					{
						toModify.Add(op);
					}
				}
			}

			// first modify entries
			foreach (var op in toModify)
			{
				JToken p = Unpatch(right[int.Parse(op.Name)], op.Value);
				right[int.Parse(op.Name)] = p;
			}

			// remove items, in reverse order to avoid sawing our own floor
			toRemove.Sort((x, y) => int.Parse(x.Name).CompareTo(int.Parse(y.Name)));
			for (int i = toRemove.Count - 1; i >= 0; --i)
			{
				JProperty op = toRemove[i];
				right.RemoveAt(int.Parse(op.Name));
			}

			// insert items, in reverse order to avoid moving our own floor
			toInsert.Sort((x, y) => int.Parse(x.Name).CompareTo(int.Parse(y.Name)));
			foreach (var op in toInsert)
			{
				right.Insert(int.Parse(op.Name), ((JArray)op.Value)[0]);
			}

			return right;
		}

        private JArray ArrayOrderBy(JArray source, string keyName)
        {
            string keyInLower = keyName.ToLower();
            
            var sorted = source.Children().OrderBy(k =>
            {
                var keyProperty = (k as JObject).Properties().FirstOrDefault(x => x.Name.ToLower() == keyInLower);
                if (keyProperty == null) return null; 

                return keyProperty.Value;
            });

            return new JArray(sorted);
        }
    }
}

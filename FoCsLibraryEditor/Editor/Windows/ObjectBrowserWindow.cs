﻿using System;
using System.Collections.Generic;
using System.Text;
using ForestOfChaosLibrary.Extensions;
using ForestOfChaosLibrary.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using SearchCompo = UnityEngine.Component;
using SearchAsset = UnityEngine.ScriptableObject;

namespace ForestOfChaosLibraryEditor.Windows
{
	[FoCsWindow]
	public class ObjectBrowserWindow: FoCsWindow<ObjectBrowserWindow>
	{
		private const           string            GUI_SELECTION_LABEL = "ObjectBrowserSelectItemID";
		private const           string            TITLE               = "Object Browser";
		private const           float             TYPE_WIDTH          = 0.3f;
		private static          Vector2           typeScrollPos       = Vector2.zero;
		private static          Vector2           sceneScrollPos      = Vector2.zero;
		private static          Vector2           assetsScrollPos     = Vector2.zero;
		private static          List<Type>        TypeList;
		private static          List<Object>      FoundSceneObjects  = new List<Object>();
		private static          List<Object>      FoundAssetsObjects = new List<Object>();
		public static           bool              Enable_Compo       = true;
		public static           bool              Enable_Asset       = true;
		private static          string            typeSearch         = "";
		private static          string            sceneSearch        = "";
		private static          string            assetSearch        = "";
		private static          int               activeIndex;
		private static readonly GUILayoutOption[] ToggleOp    = {GUILayout.ExpandWidth(true), GUILayout.Height(18)};
		private static readonly GUIContent        PingContent = new GUIContent("", "Ping Object");
		private static          int               typeLabelHighlightIndex;

		public static Type ActiveType
		{
			get { return TypeList[ActiveIndex]; }
			set { ActiveIndex = TypeList.IndexOf(value); }
		}

		private static string TypeSearch
		{
			get { return typeSearch; }
			set { EditorPrefs.SetString("FoCsOB.TypeSearch", typeSearch = value); }
		}

		private static string SceneSearch
		{
			get { return sceneSearch; }
			set { EditorPrefs.SetString("FoCsOB.SceneSearch", sceneSearch = value); }
		}

		private static string AssetSearch
		{
			get { return assetSearch; }
			set { EditorPrefs.SetString("FoCsOB.AssetSearch", assetSearch = value); }
		}

		private static int ActiveIndex
		{
			get { return activeIndex; }
			set { EditorPrefs.SetInt("FoCsOB.ActiveIndex", activeIndex = value); }
		}

		private static float TypeWidth => Screen.width * TYPE_WIDTH;

		[MenuItem("Forest Of Chaos/" + TITLE)]
		private static void Init()
		{
			GetWindowAndShow();
			Window.titleContent.text = TITLE;
		}

		private void OnEnable()
		{
			SceneSearch = EditorPrefs.GetString("FoCsOB.SceneSearch");
			AssetSearch = EditorPrefs.GetString("FoCsOB.AssetSearch");
			TypeSearch  = EditorPrefs.GetString("FoCsOB.TypeSearch");
			activeIndex = EditorPrefs.GetInt("FoCsOB.ActiveIndex");
			InitTypeList();
		}

		private void Update()
		{
			if(mouseOverWindow)
				Repaint();
		}

		private static void InitTypeList()
		{
			TypeList = ReflectionUtilities.GetInheritedClasses<SearchAsset>();
			TypeList.AddRange(ReflectionUtilities.GetInheritedClasses<SearchCompo>());

			for(var i = TypeList.Count - 1; i >= 0; i--)
			{
				if(TypeList[i].IsGenericType || TypeList[i].IsAbstract || (TypeList[i].Assembly == typeof(EditorWindow).Assembly) || TypeList[i].IsSubclassOf(typeof(UnityEditor.Editor)) || TypeList[i].IsSubclassOf(typeof(EditorWindow)))
					TypeList.RemoveAt(i);
			}

			TypeList.AddWithDuplicateCheck(typeof(Transform));
			TypeList.AddWithDuplicateCheck(typeof(GameObject));
			TypeList.TrimExcess();
		}

		protected override void OnGUI()
		{
			using(var cc = Disposables.ChangeCheck())
			{
				using(Disposables.HorizontalScope())
				{
					using(Disposables.VerticalScope(GUILayout.Width(TypeWidth), GUILayout.Width(120)))
					{
						if(DrawTypePanel())
							return;
					}

					using(Disposables.VerticalScope())
					{
						if(DrawObjectPanel())
							return;
					}
				}

				if(cc.changed)
					Repaint();
			}
		}

		private bool DrawTypePanel()
		{
			using(Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.Height(16)))
				FoCsGUI.Layout.Label("Type Selection", FoCsGUI.Styles.ToolbarButton, GUILayout.Height(16));

			if(DrawTypeSearchBox())
				return true;

			using(Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, ToggleOp))
			{
				Enable_Compo = FoCsGUI.Layout.Toggle(!Enable_Compo? "Components Hidden" : "Components Shown",                 Enable_Compo, FoCsGUI.Styles.ToolbarButton, GUILayout.Height(21));
				Enable_Asset = FoCsGUI.Layout.Toggle(!Enable_Asset? "Scriptable Objects Hidden" : "Scriptable Objects Shown", Enable_Asset, FoCsGUI.Styles.ToolbarButton, GUILayout.Height(21));
			}

			using(var scroll = Disposables.ScrollViewScope(typeScrollPos, true, false, true))
			{
				typeScrollPos           = scroll.scrollPosition;
				typeLabelHighlightIndex = 0;

				for(var i = 0; i < TypeList.Count; i++)
				{
					if(!Enable_Compo)
					{
						if(TypeList[i].IsSubclassOf(typeof(SearchCompo)))
							continue;
					}

					if(!Enable_Asset)
					{
						if(TypeList[i].IsSubclassOf(typeof(SearchAsset)))
							continue;
					}

					if(TypeSearch.IsNullOrEmpty())
						DrawTypeLabel(i);
					else
					{
						if(!SearchString(TypeList[i].Name, TypeSearch))
							continue;

						DrawTypeLabel(i);
					}
				}
			}

			return false;
		}

		private void DrawTypeLabel(int i)
		{
			GUI.SetNextControlName(GUI_SELECTION_LABEL);

			using(var cc = Disposables.ChangeCheck())
			{
				var @event = FoCsGUI.Layout.Toggle(TypeList[i].Name.SplitCamelCase(), ActiveIndex == i, (typeLabelHighlightIndex % 2) == 0? FoCsGUI.Styles.RowField : FoCsGUI.Styles.RowFieldOdd, ToggleOp);
				typeLabelHighlightIndex = (typeLabelHighlightIndex + 1) % 2;

				if(cc.changed && @event)
				{
					GUI.FocusControl(GUI_SELECTION_LABEL);
					ActiveIndex = i;
					ChangeObjectType();
				}
			}
		}

		private bool DrawTypeSearchBox()
		{
			using(Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.ExpandWidth(true)))
			{
				FoCsGUI.Layout.Label("Search", GUILayout.Width(60));

				using(var cc = Disposables.ChangeCheck())
				{
					var str = FoCsGUI.Layout.TextField(TypeSearch, GUILayout.ExpandWidth(true));

					if(!cc.changed)
						return false;

					TypeSearch = str;
					Repaint();

					return true;
				}
			}
		}

		private void DrawSceneSearchBox()
		{
			using(Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, ToggleOp))
			{
				using(Disposables.HorizontalScope(FoCsGUI.Styles.ToolbarButton, ToggleOp))
				{
					FoCsGUI.Layout.Label("Current Loaded Scene Objects", GUILayout.Height(18));
					FoCsGUI.Layout.Label("Search",                       GUILayout.Width(60), GUILayout.Height(18));

					using(var cc = Disposables.ChangeCheck())
					{
						var str = FoCsGUI.Layout.TextField(SceneSearch, FoCsGUI.Styles.Unity.ToolbarTextField, GUILayout.Width((Screen.width * 0.3f) - 120));

						if(!cc.changed)
							return;

						SceneSearch = str;
						Repaint();
					}
				}
			}
		}

		private void DrawAssetSearchBox()
		{
			using(Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, ToggleOp))
			{
				using(Disposables.HorizontalScope(FoCsGUI.Styles.ToolbarButton, ToggleOp))
				{
					FoCsGUI.Layout.Label("Assets", GUILayout.Height(18));
					FoCsGUI.Layout.Label("Search", GUILayout.Width(60), GUILayout.Height(18));

					using(var cc = Disposables.ChangeCheck())
					{
						var str = FoCsGUI.Layout.TextField(AssetSearch, FoCsGUI.Styles.Unity.ToolbarTextField, GUILayout.Width((Screen.width * 0.3f) - 120));

						if(!cc.changed)
							return;

						AssetSearch = str;
						Repaint();
					}
				}
			}
		}

		private bool DrawObjectPanel()
		{
			using(Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.Height(16)))
				FoCsGUI.Layout.Label(TypeList[ActiveIndex].Name.SplitCamelCase(), FoCsGUI.Styles.ToolbarButton, GUILayout.Height(16));

			if(!Enable_Compo && !Enable_Asset)
				FoCsGUI.Layout.GetControlRect(GUILayout.ExpandHeight(true));
			else if(!Enable_Compo && Enable_Asset)
				DrawFoundAsset(GUILayout.ExpandHeight(true));
			else
			{
				DrawFoundScene(GUILayout.MinHeight((Screen.height * 0.4f) + 1), GUILayout.ExpandHeight(true));
				DrawFoundAsset(GUILayout.ExpandHeight(true));
			}

			DrawInfo();

			return false;
		}

		private void DrawFoundScene(params GUILayoutOption[] options)
		{
			DrawSceneSearchBox();

			using(var scroll = Disposables.ScrollViewScope(sceneScrollPos, true, options))
			{
				using(Disposables.VerticalScope())
				{
					if(FoundSceneObjects.Count == 0)
						FoCsGUI.Layout.InfoBox(string.Format("No Objects of {0} In scene.", ActiveType.Name));
					else
					{
						if(SceneSearch.IsNullOrEmpty())
						{
							foreach(var foundObject in FoundSceneObjects)
								DrawFoundObject(foundObject);
						}
						else
						{
							foreach(var foundObject in FoundSceneObjects)
							{
								if(!SearchString(foundObject, SceneSearch))
									continue;

								DrawFoundObject(foundObject);
							}
						}
					}
				}

				sceneScrollPos = scroll.scrollPosition;
			}
		}

		private void DrawFoundAsset(params GUILayoutOption[] options)
		{
			DrawAssetSearchBox();

			using(var scroll = Disposables.ScrollViewScope(assetsScrollPos, true, options))
			{
				using(Disposables.VerticalScope(GUILayout.ExpandHeight(true)))
				{
					if(FoundAssetsObjects.Count == 0)
						FoCsGUI.Layout.InfoBox(string.Format("No Objects of {0}. Can be found in the Assets Database.", ActiveType.Name));
					else
					{
						if(AssetSearch.IsNullOrEmpty())
						{
							foreach(var foundObject in FoundAssetsObjects)
								DrawFoundObject(foundObject);
						}
						else
						{
							foreach(var foundObject in FoundAssetsObjects)
							{
								if(!SearchString(foundObject, AssetSearch))
									continue;

								DrawFoundObject(foundObject);
							}
						}
					}
				}

				assetsScrollPos = scroll.scrollPosition;
			}
		}

		private static bool SearchString(Object foundObject, string search) => SearchString(foundObject.name, search);
		private static bool SearchString(string foundObject, string search) => foundObject.ToLower().Contains(search.ToLower());

		private static void DrawFoundObject(Object foundObject)
		{
			using(Disposables.HorizontalScope())
			{
				var eventPingButton = FoCsGUI.Layout.Button(PingContent, FoCsGUI.Styles.Find, GUILayout.Width(16));
				FoCsGUI.Layout.Label("  ", GUILayout.Width(8));
				FoCsGUI.Layout.Button(foundObject.name,                                                             FoCsGUI.Styles.Unity.Label, GUILayout.Width(300));
				FoCsGUI.Layout.Button(string.Format("Full Type: {0}", foundObject.GetType().Name.SplitCamelCase()), FoCsGUI.Styles.Unity.Label);

				if(eventPingButton.Pressed)
					EditorGUIUtility.PingObject(foundObject);

				//TODO: Drag Drop Code
				//if(labelEvent.EventIsMouse0 && labelEvent.Event.type == EventType.mouseDown)
				//{
				//	DragAndDrop.PrepareStartDrag();
				//	DragAndDrop.objectReferences = new[]
				//								   {
				//									   foundObject
				//								   };
				//	DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
				//	DragAndDrop.StartDrag($"FoCsOB.Drag({foundObject.name})");
				//	Debug.Log(foundObject.name);
				//}
			}
		}

		private static void DrawInfo()
		{
			using(Disposables.VerticalScope(GUILayout.MaxHeight(16 * 3)))
			{
				using(Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.Height(16)))
					FoCsGUI.Layout.Label("Type Info", FoCsGUI.Styles.ToolbarButton, GUILayout.Height(16));

				using(Disposables.HorizontalScope())
				{
					FoCsGUI.Layout.Label("Assembly:");
					FoCsGUI.Layout.Label(TypeList[ActiveIndex].Assembly.GetName().Name);
				}

				using(Disposables.HorizontalScope())
				{
					var baseType = TypeList[ActiveIndex].BaseType;

					if(baseType != null)
					{
						FoCsGUI.Layout.Label("Inherits from:");

						if(baseType.IsGenericType)
						{
							var genArg = new StringBuilder();
							genArg.Append('<');
							var genArgArray = baseType.GetGenericArguments();

							for(var i = 0; i < genArgArray.Length; i++)
							{
								if(i != 0)
									genArg.Append(',');

								genArg.Append(genArgArray[i].Name);
							}

							genArg.Append('>');
							FoCsGUI.Layout.Label(string.Format("{0}{1}", TypeList[ActiveIndex].BaseType.Name.Replace("`1", ""), genArg));
						}
						else
							FoCsGUI.Layout.Label(TypeList[ActiveIndex].BaseType.Name.SplitCamelCase());
					}
				}
			}
		}

		private void ChangeObjectType()
		{
			SceneObjectChange();
			AssetsObjectsChange();
		}

		private static void AssetsObjectsChange()
		{
			if(FoundAssetsObjects == null)
				FoundAssetsObjects = new List<Object>();
			else
				FoundAssetsObjects.Clear();

			FoundAssetsObjects.AddRange(FoCsAssetFinder.FindAssetsByTypeWithScene(ActiveType));

			for(var i = FoundAssetsObjects.Count - 1; i >= 0; i--)
			{
				if(FoundAssetsObjects[i] == null)
				{
					FoundAssetsObjects.RemoveAt(i);

					continue;
				}

				if(AssetDatabase.Contains(FoundAssetsObjects[i]))
					continue;

				FoundSceneObjects.Add(FoundAssetsObjects[i]);
				FoundAssetsObjects.RemoveAt(i);
			}

			FoundSceneObjects.Sort(Sorter);
			FoundAssetsObjects.Sort(Sorter);
			FoundAssetsObjects.TrimExcess();
			FoundSceneObjects.TrimExcess();
		}

		private static int Sorter(Object x, Object y) => string.Compare(x.name, y.name, StringComparison.Ordinal);

		private static void SceneObjectChange()
		{
			if(FoundSceneObjects == null)
				FoundSceneObjects = new List<Object>();
			else
				FoundSceneObjects.Clear();
		}
	}
}

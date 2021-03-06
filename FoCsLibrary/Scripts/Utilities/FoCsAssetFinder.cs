﻿using System;
using System.Collections.Generic;
using System.Linq;
using ForestOfChaosLibrary.Extensions;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;
#endif

namespace ForestOfChaosLibrary.Utilities
{
	public static class FoCsAssetFinder
	{
		public static T[] FindAssetsByType<T>() where T: Object => FindAssetsByType(typeof(T)).Cast<T>().ToArray();

		public static Object[] FindAssetsByType(Type type)
		{
#if UNITY_EDITOR
			var assets = new List<Object>();
			var guids  = AssetDatabase.FindAssets(string.Format("t:{0}", type.ToString().Replace("UnityEngine.", "")));

			for(var i = 0; i < guids.Length; i++)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
				var subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

				foreach(var subAsset in subAssets)
				{
					if(subAsset.GetType() == type)
						assets.AddWithDuplicateCheck(subAsset);
				}
			}

			return assets.ToArray();
#else
			return Resources.FindObjectsOfTypeAll(type);
#endif
		}

		public static T[] FindAssetsByTypeWithScene<T>() where T: Object => FindAssetsByTypeWithScene(typeof(T)).Cast<T>().ToArray();

		public static Object[] FindAssetsByTypeWithScene(Type type)
		{
#if UNITY_EDITOR
			var assets = new List<Object>();
			var guids  = AssetDatabase.FindAssets(string.Format("t:{0}", type.ToString().Replace("UnityEngine.", "")));

			for(var i = 0; i < guids.Length; i++)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
				var subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

				foreach(var subAsset in subAssets)
				{
					if(subAsset.GetType() == type)
						assets.AddWithDuplicateCheck(subAsset);
				}
			}

			foreach(var o in Object.FindObjectsOfType(type))
				assets.AddWithDuplicateCheck(o);

			return assets.ToArray();
#else
			return Resources.FindObjectsOfTypeAll(type);
#endif
		}
	}
}

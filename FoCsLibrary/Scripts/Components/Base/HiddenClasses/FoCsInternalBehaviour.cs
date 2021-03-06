﻿using ForestOfChaosLibrary.Components.Linker;
using UnityEngine;

namespace ForestOfChaosLibrary.Base.HiddenClasses
{
	public abstract class FoCsInternalBehaviour: MonoBehaviour
	{
#region GetComponentAdvanced
		/// <summary>
		///     Its GetComponent, but also checks for attached Linkers
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetComponentAdvanced<T>() where T: class => gameObject.GetComponentAdvanced<T>();

		/// <summary>
		///     Its GetComponentInChildren, but also checks for attached Linkers
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetComponentInChildrenAdvanced<T>() where T: class => gameObject.GetComponentInChildrenAdvanced<T>();

		/// <summary>
		///     Its GetComponentInParent, but also checks for attached Linkers
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetComponentInParentAdvanced<T>() where T: class => gameObject.GetComponentInParentAdvanced<T>();

		/// <summary>
		///     Its GetComponents, but also checks for attached Linkers
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T[] GetComponentsAdvanced<T>() where T: class => gameObject.GetComponentsAdvanced<T>();

		/// <summary>
		///     Its GetComponentsInChildren, but also checks for attached Linkers
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T[] GetComponentsInChildrenAdvanced<T>() where T: class => gameObject.GetComponentsInChildrenAdvanced<T>();

		/// <summary>
		///     Its GetComponentsInParent, but also checks for attached Linkers
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T[] GetComponentsInParentAdvanced<T>() where T: class => gameObject.GetComponentsInParentAdvanced<T>();
#endregion
	}
}

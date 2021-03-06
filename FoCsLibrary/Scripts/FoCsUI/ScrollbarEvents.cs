using System;
using ForestOfChaosLibrary.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ForestOfChaosLibrary.FoCsUI
{
	[AddComponentMenu(FoCsStrings.COMPONENTS_UI_FOLDER_ + "Scrollbar Events")]
	[RequireComponent(typeof(Scrollbar))]
	public class ScrollbarEvents: FoCsBehaviour
	{
		public Scrollbar     _Scrollbar;
		public Action<float> onValueChanged;

		public float Value
		{
			get { return _Scrollbar.value; }
			set { _Scrollbar.value = value; }
		}

		private void OnEnable()
		{
			if(_Scrollbar == null)
				_Scrollbar = GetComponent<Scrollbar>();

			_Scrollbar.onValueChanged.AddListener(ValueChanged);
		}

		private void OnDisable()
		{
			_Scrollbar.onValueChanged.RemoveListener(ValueChanged);
		}

		private void ValueChanged(float value)
		{
			onValueChanged.Trigger(value);
		}
	}
}

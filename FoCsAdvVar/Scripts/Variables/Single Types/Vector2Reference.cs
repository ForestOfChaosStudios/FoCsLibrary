﻿using System;
using ForestOfChaosAdvVar.Base;
using UnityEngine;

namespace ForestOfChaosAdvVar
{
	[Serializable] [AdvFolderNameUnity] public class Vector2Reference: AdvReference<Vector2> { }

	[Serializable]
	public class Vector2Variable: AdvVariable<Vector2, Vector2Reference>
	{
		public static implicit operator Vector2Variable(Vector2 input)
		{
			var fR = new Vector2Variable {UseLocal = true, Value = input};

			return fR;
		}
	}
}

﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ForestOfChaosAdvVar.Base
{
	[Serializable]
	public class AdvListVariable<T, aT>: AdvListVariable where aT: AdvListReference<T>
	{
		[SerializeField] private List<T> ConstantValue;
		public                   bool    UseConstant = true;
		[SerializeField] private aT      Variable;
		public                   List<T> Value => UseConstant? ConstantValue : Variable.Value;
	}

	/// <summary>
	///     This is a base class so that as Unity needs a none generic base class for editors/property drawers
	/// </summary>
	public class AdvListVariable { }
}

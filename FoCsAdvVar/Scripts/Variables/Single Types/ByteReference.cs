﻿using System;
using ForestOfChaosAdvVar.Base;

namespace ForestOfChaosAdvVar
{
	[Serializable] [AdvFolderNameSystem] public class ByteReference: AdvReference<byte> { }

	[Serializable]
	public class ByteVariable: AdvVariable<byte, ByteReference>
	{
		public static implicit operator ByteVariable(byte input)
		{
			var fR = new ByteVariable {UseLocal = true, Value = input};

			return fR;
		}
	}
}

﻿using NUnit.Framework;
using UnityEngine;

namespace ForestOfChaosLib.AdvVar
{
	[Category("AdvVar")]
	internal static class Float_AdvVar_Testing
	{
		[Test(Author = "Jordan Miles", Description = "To Test the OnValueChanged Event")]
		public static void Float_Reference_OnChange_Event()
		{
			var b = false;
			var f = ScriptableObject.CreateInstance<FloatReference>();
			f.OnValueChange += () => b = true;
			f.Value         =  6;
			Assert.True(b);
		}

		[Test(Author = "Jordan Miles", Description = "To Test the OnValueChanged Event")]
		public static void Float_Constant_OnChange_Event()
		{
			var           b = false;
			FloatVariable f = 5;
			f.OnValueChange += () => b = true;
			f.Value         =  6;
			Assert.True(b);
		}

		[Test(Author = "Jordan Miles", Description = "To Test the OnValueChanged Event")]
		public static void Float_Global_OnChange_Event()
		{
			var b = false;
			var f = new FloatVariable {UseLocal = false};
			f.InternalData.GlobalReference =  ScriptableObject.CreateInstance<FloatReference>();
			f.OnValueChange                += () => b = true;
			f.Value                        =  6;
			Assert.True(b);
		}
	}
}

﻿using System;

namespace ForestOfChaosLibrary.Extensions
{
	public static class IntExtensions
	{
		public static bool IsZero(this           int i) => i == 0;
		public static bool IsZeroOrNegative(this int i) => i <= 0;
		public static bool IsNegative(this       int i) => i < 0;
		public static bool IsZeroOrPositive(this int i) => i >= 0;
		public static bool IsPositive(this       int i) => i > 0;
		public static int Abs(this               int i) => Math.Abs(i);

		public static int Clamp(this int i, int min = 0, int max = 1)
		{
			if(i < min)
				i = min;
			else if(i > max)
				i = max;

			return i;
		}
	}
}

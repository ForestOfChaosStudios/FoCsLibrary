﻿using UnityEngine;

namespace ForestOfChaosLibrary.Utilities.Enums
{
	public enum Direction_UD
	{
		Up,
		Down
	}

	public static class Direction_UD_Helpers
	{
		public const Direction_UD FIRST = Direction_UD.Up;
		public const Direction_UD LAST  = Direction_UD.Down;

		public static Direction_UD Next(this Direction_UD val)
		{
			switch(val)
			{
				case LAST: return FIRST;
				default:   return ++val;
			}
		}

		public static Direction_UD Previous(this Direction_UD val)
		{
			switch(val)
			{
				case FIRST: return LAST;
				default:    return --val;
			}
		}

		public static Vector3 EulerAngles(this Direction_UD val)
		{
			switch(val)
			{
				case Direction_UD.Up:   return Vector3.left  * 90;
				case Direction_UD.Down: return Vector3.right * 90;
				default:                return Vector3.zero;
			}
		}

		public static void Rotate(this Transform transform, Direction_UD dir)
		{
			transform.Rotate(dir.EulerAngles());
		}
	}
}

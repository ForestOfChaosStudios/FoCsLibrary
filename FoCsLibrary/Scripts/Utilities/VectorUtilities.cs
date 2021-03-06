﻿using UnityEngine;

namespace ForestOfChaosLibrary.Utilities
{
	public static class VectorUtilities
	{
#region GetPosOnY
#region Extensions
		public static Vector3 GetPosOnY(this Ray ray) => GetPosOnY(0, ray);
		public static Vector3 GetPosOnY(this Ray ray, float   yAxis) => GetPosOnYAxis(yAxis,   ray);
		public static Vector3 GetPosOnY(this Ray ray, Vector3 yAxis) => GetPosOnYAxis(yAxis.y, ray);
#endregion
		public static Vector3 GetPosOnY(float   yAxis, Ray ray) => GetPosOnYAxis(yAxis,   ray);
		public static Vector3 GetPosOnY(Vector3 yAxis, Ray ray) => GetPosOnYAxis(yAxis.y, ray);

		public static Vector3 GetPosOnYAxis(float yAxis, Ray ray)
		{
			var dst = (yAxis - ray.origin.y) / ray.direction.y;

			return ray.origin + (ray.direction * dst);
		}
#endregion
	}
}

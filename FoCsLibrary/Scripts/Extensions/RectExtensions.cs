﻿using System;
using ForestOfChaosLibrary.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ForestOfChaosLibrary.Extensions
{
	public static class RectExtensions
	{
		public static Vector2 GetRandomPosInRect(this Rect rect)
		{
			var pos = new Vector2(Random.Range(rect.min.x, rect.max.x), Random.Range(rect.min.y, rect.max.y));

			return pos;
		}

		public static Rect Edit(this Rect rect, RectEdit[] edits, params RectEdit[] editParams)
		{
			var output = new Rect(rect);

			foreach(var pair in edits)
				DoEdit(ref output, pair);

			foreach(var pair in editParams)
				DoEdit(ref output, pair);

			return output;
		}

		public static Rect Edit(this Rect rect, RectEdit edits, params RectEdit[] editParams)
		{
			var output = new Rect(rect);
			DoEdit(ref output, edits);

			foreach(var pair in editParams)
				DoEdit(ref output, pair);

			return output;
		}

		private static void DoEdit(ref Rect output, RectEdit pair)
		{
			switch(pair.Type)
			{
				case RectEdit.RectEditType.Add:
					RectEdit.Add(pair, ref output);

					break;
				case RectEdit.RectEditType.Change:
					RectEdit.Change(pair, ref output);

					break;
				case RectEdit.RectEditType.Set:
					RectEdit.Set(pair, ref output);

					break;
				case RectEdit.RectEditType.Multiply:
					RectEdit.Multiply(pair, ref output);

					break;
				case RectEdit.RectEditType.Subtract:
					RectEdit.Subtract(pair, ref output);

					break;
				case RectEdit.RectEditType.Divide:
					RectEdit.Divide(pair, ref output);

					break;
				case RectEdit.RectEditType.Modulo:
					RectEdit.Modulo(pair, ref output);

					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}
	}
}

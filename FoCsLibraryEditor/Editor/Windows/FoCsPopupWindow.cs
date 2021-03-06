﻿using System;
using ForestOfChaosLibraryEditor.Windows;
using ForestOfChaosLibrary.Extensions;

namespace ForestOfChaosLibraryEditor
{
	public class FoCsPopupWindow: FoCsWindow<FoCsPopupWindow>
	{
		private FoCsPopupWindowArguments currentArguments;

		private static void Init()
		{
			GetWindowAndOpenUtility();
		}

		public static void SetUpInstance(FoCsPopupWindowArguments Args)
		{
			Init();
			Window.titleContent.text       = Args.WindowTitle;
			Window.currentArguments        = Args;
			Window.currentArguments.Window = Window;

			if(Window.currentArguments.MinHeight != null)
			{
				var m = Window.minSize;
				m.y            = Window.currentArguments.MinHeight();
				Window.minSize = m;
			}

			if(Window.currentArguments.MinWidth != null)
			{
				var m = Window.minSize;
				m.x            = Window.currentArguments.MinWidth();
				Window.minSize = m;
			}

			if(Window.currentArguments.MaxHeight != null)
			{
				var m = Window.maxSize;
				m.y            = Window.currentArguments.MaxHeight();
				Window.maxSize = m;
			}

			if(Window.currentArguments.MaxWidth != null)
			{
				var m = Window.maxSize;
				m.x            = Window.currentArguments.MaxWidth();
				Window.maxSize = m;
			}
		}

		protected override void OnGUI()
		{
			if(currentArguments == null)
				return;

			Window.currentArguments.Window = Window;
			currentArguments.OnGUI.Trigger(currentArguments);
		}
	}

	public class FoCsPopupWindowArguments
	{
		public Func<float>                     MaxHeight;
		public Func<float>                     MaxWidth;
		public Func<float>                     MinHeight;
		public Func<float>                     MinWidth;
		public Action<FoCsPopupWindowArguments> OnClose;
		public Action<FoCsPopupWindowArguments> OnGUI;
		public FoCsPopupWindow                  Window;
		public string                          WindowTitle;
	}
}

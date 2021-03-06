﻿using System;
using ForestOfChaosLibraryEditor;
using ForestOfChaosLibraryEditor.Windows;
using ForestOfChaosLibrary.Extensions;
using UnityEditor;
using UnityEngine;

namespace ForestOfChaosAdvVarEditor
{
	//TODO : Make this window better, GE add the ability to add extra functionality eg submit more data
	public class SubmitStringWindow: FoCsWindow<SubmitStringWindow>
	{
		private const  string                GUI_SELECTION_LABEL = "SubmitStringWindowDataField";
		private static bool                  notSelectedLabel;
		private        SubmitStringArguments currentArguments;

		private static void Init()
		{
			GetWindowAndOpenUtility();
		}

		public static void SetUpInstance(SubmitStringArguments Args)
		{
			Init();
			Window.titleContent.text = Args.WindowTitle;
			Window.currentArguments  = Args;
			notSelectedLabel         = false;
		}

		protected override void OnGUI()
		{
			if(currentArguments == null)
				return;

			EditorGUILayout.LabelField(currentArguments.Title);
			GUI.SetNextControlName(GUI_SELECTION_LABEL);
			currentArguments.Data = EditorGUILayout.TextField(GUIContent.none, currentArguments.Data);

			if(!notSelectedLabel)
			{
				EditorGUI.FocusTextInControl(GUI_SELECTION_LABEL);
				notSelectedLabel = true;
			}

			using(Disposables.HorizontalScope())
			{
				if(FoCsGUI.Layout.Button(currentArguments.SubmitMessage))
				{
					currentArguments.OnSubmit.Trigger(currentArguments);
					Close();
					EndWindows();
				}

				if(FoCsGUI.Layout.Button(currentArguments.CancelMessage))
				{
					currentArguments.OnCancel.Trigger(currentArguments);
					Close();
					EndWindows();
				}
			}

			if(!currentArguments.HasAnotherButton)
				return;

			if(FoCsGUI.Layout.Button(currentArguments.SubmitAnotherMessage))
				currentArguments.OnSubmitAnother.Trigger(currentArguments);
		}

		public class SubmitStringArguments
		{
			public string                        CancelMessage;
			public string                        Data;
			public bool                          HasAnotherButton = false;
			public Action<SubmitStringArguments> OnCancel;
			public Action<SubmitStringArguments> OnSubmit;
			public Action<SubmitStringArguments> OnSubmitAnother;
			public string                        SubmitAnotherMessage;
			public string                        SubmitMessage;
			public string                        Title;
			public string                        WindowTitle;
		}
	}
}

﻿using System;
using Rhino;
using Rhino.Commands;
using ChatUI;
using System.Windows.Interop;
using System.Windows;

namespace ChatGPTforRhino
{
    public class ChatGPTWindowCommand : Command
    {
        public ChatGPTWindowCommand()
        {
            Instance = this;
        }

        public static ChatGPTWindowCommand Instance { get; private set; }

        public override string EnglishName => "ChatGPT_window";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (IsWindowOpen<ChatUI.MainWindow>()) return Result.Success;

            var window = new ChatUI.MainWindow();

            //Rhinoのウィンドウを親に設定
            var rhinoHandle = RhinoApp.MainWindowHandle();
            var helper = new WindowInteropHelper(window);
            helper.Owner = rhinoHandle;

			window.Show();
            return Result.Success;
        }

		// ジェネリックメソッドで、指定した型のウィンドウがすでに開かれているかをチェック
		private bool IsWindowOpen<T>() where T : Window
		{
			foreach (Window window in Application.Current.Windows)
			{
				if (window is T) return true;
			}
			return false;
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Annotations;
using System.Windows.Media.Animation;
using ChatUI.MVVM.Model;
using System.Runtime.Remoting.Messaging;
using System.Collections.ObjectModel;
using ChatUI.MVVM.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChatGPTConnection;

namespace ChatUI
{
	public partial class MainWindow : Window
	{
		public static string DllDirectory
		{
			get 
			{
				string dllPath = System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location);
				string dllDirectory = System.IO.Directory.GetParent(dllPath).FullName;
				return dllDirectory;
			}
		}

		public MainWindow()
		{
			InitializeComponent();
			var vm = this.DataContext as MainViewModel;
		}



		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();
			}
		}

		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void WindowStateButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.WindowState != WindowState.Maximized)
			{
				this.WindowState = WindowState.Maximized;
			}
			else
			{
				this.WindowState = WindowState.Normal;
				
			}
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void OptionButton_Click(object sender, RoutedEventArgs e)
		{
			var optionWindow = new OptionWindow();		
			optionWindow.Owner = this;
			optionWindow.ShowDialog();
		}


		//デバッグモード
		//ChatGPTのメッセージテキストが詳細化
		private bool _isDebagMode;

		public bool IsDebagMode
		{
			get => _isDebagMode;
			set
			{
				_isDebagMode = value;
			}
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ChangeDebagMode(!_isDebagMode);
			if (_isDebagMode) DebugButton.Background = Brushes.LightSlateGray;
			else DebugButton.Background = Brushes.Transparent;
		}

		private void ChangeDebagMode(bool state)
		{
			IsDebagMode = state;
			//メッセージのステートを変化させる
			var vm = this.DataContext as MainViewModel;
			foreach (MessageModel mm in vm.Messages)
			{
				mm.UseSubMessage = state;
			}
		}


		

	}
}

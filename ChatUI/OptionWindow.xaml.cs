using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ChatUI
{
	/// <summary>
	/// OptionWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class OptionWindow : Window
	{
		public OptionWindow()
		{
			InitializeComponent();
			//セッティングファイルをロード
			Settings settings = Settings.LoadSettings();
			if (settings != null)
			{
				TextBox_APIKey.Password= settings.APIKey;
				TextBox_SytemPrompt.Text = settings.SystemMessage;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string apiKey = TextBox_APIKey.Password;
			string systemMessage = TextBox_SytemPrompt.Text;
			if (apiKey == null || systemMessage == null) return;
			//セッティングファイルをセーブ
			Settings settings = new Settings(apiKey, systemMessage);
			settings.SaveSettings();
			this.Close();
		}
	}
}

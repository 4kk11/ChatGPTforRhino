using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace ChatUI
{

	public class Settings
	{
		private static readonly string FileName = Path.Combine(MainWindow.DllDirectory, "Settings.xml");
		public string APIKey { get; set; }

		public Settings(string apikey) 
		{
			APIKey = apikey;
		}

		public Settings() { }

		public void SaveSettings()
		{
			try
			{
				var writer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
				
				using (var file = new StreamWriter(FileName))
				{
					writer.Serialize(file, this);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static Settings LoadSettings()
		{
			if (File.Exists(FileName))
			{
				try
				{
					var reader = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
					
					using (var file = new StreamReader(FileName))
					{
						return reader.Deserialize(file) as Settings;
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			else { return null; }
		}
	}
}

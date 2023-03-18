using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ChatUI.MVVM.Model
{
	class MessageModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string Username { get; set; }
		public string UsernameColor { get; set; }
		public string ImageSource { get; set; }

		public bool UseSubMessage {
			get { return useSubMessage; }
			set { 
				useSubMessage = value;
				OnPropertyChanged("Message");
			}
		}
		private bool useSubMessage ;
		public string Message {
			get { 

				return (UseSubMessage && SubMessage != null)? SubMessage : MainMessage; 
			}
			set { MainMessage = value; }
		}
		
		public string MainMessage { get; set; }
		public string SubMessage { get; set; }
		public DateTime Time { get; set; }
		public bool IsMyMessage { get; set; }
		public bool IsLoadingSpinner { get; set; }

		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

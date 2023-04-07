using ChatGPTConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUI
{

	public delegate void ChatGPTResponseEventHandler(object sender, ChatGPTResponseEventArgs e);
	public class ChatGPTResponseEventArgs : EventArgs
	{
		public ChatGPTResponseModel Response { get; private set; }

		public ChatGPTResponseEventArgs(ChatGPTResponseModel response)
		{
			Response = response;
		}
	}
}

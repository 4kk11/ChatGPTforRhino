using ChatGPTConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUI
{
	public static class ChatGPTResponseEvent
	{
		public static event ChatGPTResponseEventHandler ResponseReceived;
		internal static void OnResponseReceived(ChatGPTResponseEventArgs e)
		{
			ResponseReceived?.Invoke(null, e);
		}
	}

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

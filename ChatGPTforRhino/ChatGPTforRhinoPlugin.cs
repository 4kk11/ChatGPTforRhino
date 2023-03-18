using Rhino;
using Rhino.PlugIns;
using System;

namespace ChatGPTforRhino
{

	public class ChatGPTforRhinoPlugin : Rhino.PlugIns.PlugIn
	{
		public ChatGPTforRhinoPlugin()
		{
			Instance = this;
		}

		public static ChatGPTforRhinoPlugin Instance { get; private set; }

		protected override LoadReturnCode OnLoad(ref string errorMessage)
		{
			
			return base.OnLoad(ref errorMessage);
		}


	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;

namespace ChatGPTforRhino
{
	public class RhinoCommand
	{
		string[] Commands;
		public RhinoCommand(string[] commands) 
		{
			if (commands == null || commands.Length == 0) throw new Exception("command is null or has no content");
			this.Commands = commands;
		}

		public void RunCommand()
		{
			foreach (string command in Commands)
			{
				RhinoApp.RunScript(command, false);
			}
		}
	}
}

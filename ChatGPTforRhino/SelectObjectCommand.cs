using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatGPTforRhino
{
    public class SelectObjectCommand : Command
    {
        public SelectObjectCommand()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static SelectObjectCommand Instance { get; private set; }

        public override string EnglishName => "SelectObject";
		
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
			string name = null;
			if (!InputString(out name)) return Result.Failure;
			//doc.Objects.F
			RhinoObject ro = doc.Objects.Where(k => k.Name == name).FirstOrDefault();
			doc.Objects.Select(ro.Id);
			return Result.Success;
        }

		private bool InputString(out string userMessage)
		{
			using (GetString gs = new GetString())
			{
				gs.SetCommandPrompt("Name");

				gs.GetLiteralString();
				if (gs.CommandResult() != Result.Success)
				{
					userMessage = null;
					return false;
				}
				userMessage = gs.StringResult();
			}

			return true;
		}
	}
}
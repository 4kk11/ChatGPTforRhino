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
    public class SetNameCommand : Command
    {
        public SetNameCommand()
        {
            Instance = this;
        }

        public static SetNameCommand Instance { get; private set; }

        public override string EnglishName => "SetName";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (doc.Objects.Count == 0) return Result.Failure;
			string newName = null;
			if (!InputString(out newName)) return Result.Failure;

			var enm = doc.Objects.GetEnumerator();
			enm.MoveNext();
			RhinoObject ro = enm.Current;
			ro.Name = newName;
			ro.CommitChanges();

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
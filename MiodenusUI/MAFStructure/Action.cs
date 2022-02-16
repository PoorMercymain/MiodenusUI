using System.Collections.Generic;

namespace MiodenusUI.MAFStructure
{
    public class Action
    {
        public string Name { get; set; } = string.Empty;
        public List<ActionState> States { get; set; } = new ();
    }
}
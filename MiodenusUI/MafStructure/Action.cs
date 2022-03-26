﻿using System.Collections.Generic;

namespace MiodenusUI.MafStructure
{
    public class Action
    {
        public string Name { get; set; } = DefaultMafParameters.Action.Name;
        public List<ActionState> States { get; set; } = new ();
    }
}
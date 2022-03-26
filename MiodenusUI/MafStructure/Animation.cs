using System.Collections.Generic;

namespace MiodenusUI.MafStructure
{
    public class Animation
    {
        public AnimationInfo AnimationInfo { get; set; } = new ();
        public List<ModelInfo> ModelsInfo { get; set; } = new ();
        public List<Action> Actions { get; set; } = new ();
        public List<Binding> Bindings { get; set; } = new ();
    }
}
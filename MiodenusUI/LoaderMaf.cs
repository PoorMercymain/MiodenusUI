using System;
using System.IO;
using Newtonsoft.Json;

namespace MiodenusUI
{
    public class LoaderMaf
    {
        public MafStructure.Animation Read(in string filename)
        {
            var content = File.ReadAllText(filename);
            var animation = JsonConvert.DeserializeObject<MafStructure.Animation>(content);
            
            return animation;
        }

        public String CreateMafString(MafStructure.Animation animation)
        {
            var animationString = JsonConvert.SerializeObject(animation);

            return animationString;
        }
    }
}
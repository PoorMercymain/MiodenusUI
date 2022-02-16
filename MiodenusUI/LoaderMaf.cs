using System;
using System.IO;
using Newtonsoft.Json;

namespace MiodenusUI
{
    public class LoaderMaf
    {
        public MAFStructure.Animation Read(in string filename)
        {
            var content = File.ReadAllText(filename);
            var animation = JsonConvert.DeserializeObject<MAFStructure.Animation>(content);
            
            return animation;
        }

        public String CreateMafString(MAFStructure.Animation animation)
        {
            var animationString = JsonConvert.SerializeObject(animation);

            return animationString;
        }
    }
}
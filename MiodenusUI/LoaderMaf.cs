using System.IO;
using Newtonsoft.Json;

namespace MiodenusUI
{
    public class LoaderMaf
    {
        public MAFStructure.Animation Load(in string filename)
        {
            var content = File.ReadAllText(filename);
            var animation = JsonConvert.DeserializeObject<MAFStructure.Animation>(content);
            
            return animation;
        }
    }
}
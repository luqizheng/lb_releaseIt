using System.Runtime.Serialization;

namespace ReleaseIt
{

    public class Setting
    {
        
        public string Name { get; set; }
        /// <summary>
        /// gets or sets Name of Command which executed. 
        /// </summary>
        public string[] Tags { get; set; }

        public string From { get; set; }
    }
}
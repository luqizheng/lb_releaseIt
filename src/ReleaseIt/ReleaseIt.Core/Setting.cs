using System.Runtime.Serialization;

namespace ReleaseIt
{

    public class Setting
    {
        
        public string Id { get; set; }
      
        public string[] Tags { get; set; }
        /// <summary>
        /// gets or sets Id of Command which executed. 
        /// </summary>
        public string From { get; set; }
    }
}
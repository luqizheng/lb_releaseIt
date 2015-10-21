using System.Runtime.Serialization;

namespace ReleaseIt
{

    public class Setting
    {
        private string _dependcies;
        private string _id;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value != null ? value.ToLower() : null;

            }
        }

        public string[] Tags { get; set; }

        /// <summary>
        /// gets or sets Id of Command which executed. 
        /// </summary>
        public string Dependcies
        {
            get { return _dependcies; }
            set {
                _dependcies = value != null ? value.ToLower() : null;
            }
        }
    }
}
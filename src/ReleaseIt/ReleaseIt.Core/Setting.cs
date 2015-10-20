using System.Runtime.Serialization;

namespace ReleaseIt
{

    public class Setting
    {
        private string _from;
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
        public string From
        {
            get { return _from; }
            set {
                _from = value != null ? value.ToLower() : null;
            }
        }
    }
}
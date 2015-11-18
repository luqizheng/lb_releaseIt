using System;
using System.Runtime.Serialization;

namespace ReleaseIt
{

    public class Setting : ICloneable
    {
        private string _dependency;
        private string _id;
        private string _goTo;
        private string[] _call;
        
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
        public string Dependency
        {
            get { return _dependency; }
            set
            {
                _dependency = value != null ? value.ToLower() : null;
            }
        }
        /// <summary>
        /// Goto command after finsih.
        /// </summary>
        public string GoTo
        {
            get { return _goTo; }
            set { _goTo = value != null ? value.ToLower() : null; ; }
        }
        /// <summary>
        /// call next command after command finish 
        /// </summary>
        public string[] Call
        {
            get { return _call; }
            set
            {

                if (value != null)
                {
                    for (var i = 0; i < value.Length; i++)
                    {
                        value[i] = value[i].ToLower();
                    }
                }
                _call = value;

            }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public Setting Clone()
        {
            return (Setting)MemberwiseClone();
        }

    }
}
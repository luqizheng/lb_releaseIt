using System;
using System.Runtime.Serialization;

namespace ReleaseIt
{
    [DataContract]
    public class CopySetting
    {
        [DataMember]
        public string TargetPath { get; set; }

        [DataMember]
        public string From { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        /// <summary>
        ///     use /d to copy file.
        /// </summary>
        [DataMember]
        public DateTime? LastCopyDate { get; set; }

        [DataMember]
        public bool UseDateCompareCopy { get; set; }
    }
}
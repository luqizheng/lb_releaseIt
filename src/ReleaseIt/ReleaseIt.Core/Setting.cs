using System.Runtime.Serialization;

namespace ReleaseIt
{
    [DataContract]
    public class Setting
    {
        [DataMember]
        public string Name { get; set; }
    }
}
using System.Runtime.Serialization;

namespace RestClientTemplate.Model
{
    [DataContract(Name = "TokenRequest")]
    public class ConcreteTokenRequest
    {
        [DataMember(Name = "oldToken", EmitDefaultValue = false)]
        public string OldToken { get; set; }
    }
}
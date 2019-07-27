using Newtonsoft.Json;
using System;

namespace SimpleDocumentStore
{
    public class DocumentBase : IDocumentBase
    {
        [JsonIgnore]
        public Guid Id_G
        {
            get
            {
                if (string.IsNullOrEmpty(Id))
                    return Guid.Empty;

                return Guid.Parse(Id);
            }
        }

        public virtual string Id { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as DocumentBase;
            if (other == null)
            {
                return false;
            }
            if (Id != other.Id)
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

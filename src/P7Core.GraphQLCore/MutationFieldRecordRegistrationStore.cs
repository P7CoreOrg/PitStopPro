using System.Collections.Generic;
using System.Linq;

namespace P7Core.GraphQLCore
{
    public class MutationFieldRecordRegistrationStore :
        IMutationFieldRegistrationStore
    {
        private List<IMutationFieldRegistration> _fieldRecordRegistrations;

        public MutationFieldRecordRegistrationStore(
            IEnumerable<IMutationFieldRegistration> fieldRecordRegistrations)
        {
            _fieldRecordRegistrations = fieldRecordRegistrations.ToList();
        }

        public int Count => _fieldRecordRegistrations.Count;

        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            foreach (var item in _fieldRecordRegistrations)
            {
                item.AddGraphTypeFields(mutationCore);
            }
        }
    }
}
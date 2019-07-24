using System.Collections.Generic;
using System.Linq;

namespace P7Core.GraphQLCore
{
    public class SubscriptionFieldRecordRegistrationStore :
        ISubscriptionFieldRegistrationStore
    {
        private List<ISubscriptionFieldRegistration> _fieldRecordRegistrations;

        public SubscriptionFieldRecordRegistrationStore(
            IEnumerable<ISubscriptionFieldRegistration> fieldRecordRegistrations)
        {
            _fieldRecordRegistrations = fieldRecordRegistrations.ToList();
        }

        public int Count => _fieldRecordRegistrations.Count;

        public void AddGraphTypeFields(SubscriptionCore subscriptionCore)
        {
            foreach (var item in _fieldRecordRegistrations)
            {
                item.AddGraphTypeFields(subscriptionCore);
            }
        }
    }
}
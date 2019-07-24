using GraphQL.Types;

namespace P7Core.GraphQLCore
{
    public class SubscriptionCore : ObjectGraphType<object>
    {
        private int _count;

        public SubscriptionCore(ISubscriptionFieldRegistrationStore fieldStore)
        {
            Name = "subscription";
            fieldStore.AddGraphTypeFields(this);
            _count = fieldStore.Count;
        }
        public int RegistrationCount
        {
            get
            {
                return _count;
            }
        }
    }
}
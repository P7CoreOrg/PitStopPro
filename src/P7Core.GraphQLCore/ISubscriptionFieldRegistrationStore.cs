namespace GQL.GraphQLCore
{
    public interface ISubscriptionFieldRegistrationStore
    {
        int Count { get; }
        void AddGraphTypeFields(SubscriptionCore subscriptionCore);
    }
}
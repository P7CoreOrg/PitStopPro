namespace GQL.GraphQLCore
{
    public interface ISubscriptionFieldRegistration
    {
        void AddGraphTypeFields(SubscriptionCore subscriptionCore);
    }
}
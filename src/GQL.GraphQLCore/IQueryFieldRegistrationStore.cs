namespace GQL.GraphQLCore
{
    public interface IQueryFieldRegistrationStore
    {
        int Count { get; }
        void AddGraphTypeFields(QueryCore queryCore);
    }
}
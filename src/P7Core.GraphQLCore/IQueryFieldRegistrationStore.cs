namespace P7Core.GraphQLCore
{
    public interface IQueryFieldRegistrationStore
    {
        int Count { get; }
        void AddGraphTypeFields(QueryCore queryCore);
    }
}
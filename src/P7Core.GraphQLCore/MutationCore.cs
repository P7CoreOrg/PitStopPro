using GraphQL.Types;

namespace P7Core.GraphQLCore
{
    public class MutationCore : ObjectGraphType<object>
    {
        private int _count;

        public MutationCore(IMutationFieldRegistrationStore fieldStore)
        {
            Name = "mutation";
            fieldStore.AddGraphTypeFields(this);
            _count = fieldStore.Count;
        }

        public int RegistrationCount => _count;
    }
}
using LiveMessengerApi.Models;

namespace LiveMessengerApi
{
    public class CassandraMappings : Cassandra.Mapping.Mappings
    {
        public CassandraMappings()
        {
            For<User>().TableName("users").PartitionKey(u => u.user_id)
                 .Column(x => x.user_id, cm => cm.WithName("user_id"))
                 .Column(x => x.first_name)
                 .Column(x => x.last_name)
                 .Column(x => x.phone_number)
                 .Column(x => x.password);
                
        }
    }
}

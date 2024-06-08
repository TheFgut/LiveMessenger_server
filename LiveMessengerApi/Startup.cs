using Cassandra.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LiveMessengerApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            MappingConfiguration.Global.Define<CassandraMappings>();
        }
    }
}

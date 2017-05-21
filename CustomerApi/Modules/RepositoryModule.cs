using Autofac;
using CustomerApi.Repositories.Customer;

namespace CustomerApi.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerRequest();
        }
    }
}
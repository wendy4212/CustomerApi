using Autofac;
using CustomerApi.Services.Customer;

namespace CustomerApi.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerRequest();
        }
    }
}
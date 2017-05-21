using CustomerApi.Models;
using CustomerApi.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApi.Repositories.Customer
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerDataServiceViewModel> GetCustomerData(CustomerDataInViewModel model, string connectString);
        CustomerDataServiceViewModel CreateCustomerData(CustomerDataInViewModel model, string connectString);
        CustomerDataServiceViewModel EditCustomerData(CustomerDataInViewModel model, string connectString);
        CustomerDataServiceViewModel DeleteCustomerData(CustomerDataInViewModel model, string connectString);
    }
}

using CustomerApi.Models.Customer;
using CustomerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApi.Services.Customer
{
    public interface ICustomerService
    {
        ApiStatusViewModel<IEnumerable<CustomerDataServiceViewModel>> GetCustomerData(CustomerDataInViewModel model);
        ApiStatusViewModel<CustomerDataServiceViewModel> CreateCustomerData(CustomerDataInViewModel model);
        ApiStatusViewModel<CustomerDataServiceViewModel> EditCustomerData(CustomerDataInViewModel model);
        ApiStatusViewModel<CustomerDataServiceViewModel> DeleteCustomerData(CustomerDataInViewModel model);

    }
}

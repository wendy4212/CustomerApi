using CustomerApi.Repositories.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomerApi.Models;
using CustomerApi.Models.Customer;
using System.Configuration;

namespace CustomerApi.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository CustomerRepository;

        public CustomerService() : this(new CustomerRepository())
        {

        }
        public CustomerService(ICustomerRepository customerRepository)
        {
            this.CustomerRepository = customerRepository;
        }

        public ApiStatusViewModel<CustomerDataServiceViewModel> CreateCustomerData(CustomerDataInViewModel model)
        {
            throw new NotImplementedException();
        }

        public ApiStatusViewModel<CustomerDataServiceViewModel> DeleteCustomerData(CustomerDataInViewModel model)
        {
            throw new NotImplementedException();
        }

        public ApiStatusViewModel<CustomerDataServiceViewModel> EditCustomerData(CustomerDataInViewModel model)
        {
            throw new NotImplementedException();
        }

        public ApiStatusViewModel<IEnumerable<CustomerDataServiceViewModel>> GetCustomerData(CustomerDataInViewModel model)
        {
            var returnViewModel = new ApiStatusViewModel<IEnumerable<CustomerDataServiceViewModel>>();
            var connectString = ConfigurationManager.ConnectionStrings["CustomerApiConnection"].ConnectionString;

            try
            {
                returnViewModel.Data = this.CustomerRepository.GetCustomerData(model, connectString);
                returnViewModel.code = "200";
            }
            catch (Exception e)
            {
                returnViewModel.code = "500";
                returnViewModel.message = e.ToString();
            }
            return returnViewModel;

        }
    }
}
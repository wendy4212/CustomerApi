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

            var connectString = ConfigurationManager.ConnectionStrings["CustomerApiConnection"].ConnectionString;
            var returnViewModel = new ApiStatusViewModel<CustomerDataServiceViewModel>();

            try
            {
                returnViewModel.Data = this.CustomerRepository.CreateCustomerData(model, connectString);
                if (returnViewModel.Data == null)
                {
                    throw new ApplicationException("建立顧客失敗(CreateCustomerData)");
                }
                returnViewModel.code = "200";
            }
            catch (Exception e)
            {
                returnViewModel.code = "500";
                returnViewModel.message = e.ToString();
            }

            return returnViewModel;

        }

        public ApiStatusViewModel<CustomerDataServiceViewModel> DeleteCustomerData(CustomerDataInViewModel model)
        {

            var connectString = ConfigurationManager.ConnectionStrings["CustomerApiConnection"].ConnectionString;
            var returnViewModel = new ApiStatusViewModel<CustomerDataServiceViewModel>();

            try
            {
                returnViewModel.Data = this.CustomerRepository.DeleteCustomerData(model, connectString);
                if (returnViewModel.Data == null)
                {
                    throw new ApplicationException("刪除顧客失敗(DeleteCustomerData)");
                }
                returnViewModel.code = "200";
            }
            catch (Exception e)
            {
                returnViewModel.code = "500";
                returnViewModel.message = e.ToString();
            }

            return returnViewModel;

        }

        public ApiStatusViewModel<CustomerDataServiceViewModel> EditCustomerData(CustomerDataInViewModel model)
        {

            var connectString = ConfigurationManager.ConnectionStrings["CustomerApiConnection"].ConnectionString;
            var returnViewModel = new ApiStatusViewModel<CustomerDataServiceViewModel>();

            try
            {
                returnViewModel.Data = this.CustomerRepository.EditCustomerData(model, connectString);
                if (returnViewModel.Data == null)
                {
                    throw new ApplicationException("編輯顧客失敗(EditCustomerData)");
                }
                returnViewModel.code = "200";
            }
            catch (Exception e)
            {
                returnViewModel.code = "500";
                returnViewModel.message = e.ToString();
            }

            return returnViewModel;

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
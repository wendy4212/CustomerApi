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
                DataClassCustomerDataContext CustomerDataContext = new DataClassCustomerDataContext();
                CustomerData customer = new CustomerData();
                customer.CustomerId = model.CustomerId;
                customer.CustomerName = model.CustomerName;
                customer.CustomerAddr = model.CustomerAddr;
                CustomerDataContext.CustomerData.InsertOnSubmit(customer);
                CustomerDataContext.SubmitChanges();
                returnViewModel.code = "200";
                returnViewModel.message = "建立顧客成功!!";
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
                DataClassCustomerDataContext CustomerDataContext = new DataClassCustomerDataContext();
                CustomerData customer = CustomerDataContext.CustomerData.Single(p => p.CustomerId == model.CustomerId);
                CustomerDataContext.CustomerData.DeleteOnSubmit(customer);
                CustomerDataContext.SubmitChanges();
                returnViewModel.message = "刪除顧客成功!!";

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
                DataClassCustomerDataContext CustomerDataContext = new DataClassCustomerDataContext();
                CustomerData customer = CustomerDataContext.CustomerData.Single(p => p.CustomerId == model.CustomerId);
                customer.CustomerName = "Andy";
                CustomerDataContext.SubmitChanges();
                returnViewModel.message = "修改顧客資料成功!!";

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
                DataClassCustomerDataContext CustomerDataContext = new DataClassCustomerDataContext();
                var result = CustomerDataContext.CustomerData.AsEnumerable();

                var finalresult = result.Select(p => new CustomerDataServiceViewModel()
                    {
                        CustomerName = p.CustomerName,
                        CustomerId = p.CustomerId,
                        CustomerAddr = p.CustomerAddr
                    });

                if (model.CustomerId.Trim() != "")
                {
                    finalresult = finalresult.Where(p => p.CustomerId == model.CustomerId);
                }
                if (model.KeyWord != "")
                {
                    finalresult = finalresult.Where(p => p.CustomerName.Contains(model.CustomerId)).AsQueryable();
                }
                returnViewModel.Data = finalresult;
                returnViewModel.code = "200";
                returnViewModel.message = "顧客資料存取成功!!";
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
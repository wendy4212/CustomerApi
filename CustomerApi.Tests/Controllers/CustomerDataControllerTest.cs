using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomerApi;
using CustomerApi.Controllers;
using CustomerApi.Controllers.Customer;
using CustomerApi.Models.Customer;
using CustomerApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomerApi.Tests.Controllers
{
    [TestClass]
    public class CustomerControllerTest
    {

        [TestMethod]
        public void Index()
        {
            // Arrange
            CustomerController controller = new CustomerController();
            var model = new CustomerDataInViewModel();
            model.CustomerId = "0000000001";
        
            var serviceViewModel = new CustomerDataServiceViewModel();
            serviceViewModel.CustomerId = "0000000001";
            serviceViewModel.CustomerName = "哈囉你好嗎股份有限公司";
            serviceViewModel.CustomerAddr = "我們這一家";

            // Act
            ActionResult result = controller.GetCustomerData(model) as ActionResult;
            JObject jo = JsonConvert.DeserializeObject<JObject>(result.ToString());
            JArray da = jo.GetValue("data").ToObject<JArray>();
            var actual = JsonConvert.DeserializeObject<IEnumerable<CustomerDataServiceViewModel>>(da.ToString());
            actual = actual.Where(p => p.CustomerId == model.CustomerId);

            // Assert
            Assert.AreEqual(serviceViewModel, actual);
        }

        [TestMethod]
        public void Test()
        {
            // Arrange
            int i = 1;
            int j = 1;

            // Act
        
            // Assert
            Assert.AreEqual(i,j);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerApi.Models.Customer
{
    public class CustomerDataInViewModel
    {
        public CustomerDataInViewModel()
        {
            this.CustomerId = "";
            this.KeyWord = "";
            this.CustomerName = "";
            this.CustomerAddr = "";
        }
        public string CustomerId;
        public string KeyWord;
        public string CustomerName;
        public string CustomerAddr;
    }
}
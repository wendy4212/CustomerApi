using CustomerApi.Models.Customer;
using CustomerApi.Services.Customer;
using System.Web.Mvc;

namespace CustomerApi.Controllers.Customer
{
    public class CustomerController : Controller
    {
        private ICustomerService CustomerService;
        public CustomerController():this(new CustomerService())
        {

        }
        public CustomerController(ICustomerService customerService)
        {
            this.CustomerService = customerService;
        }
        /// <summary>
        /// 取得顧客資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetCustomerData(CustomerDataInViewModel model)
        {
            var data = this.CustomerService.GetCustomerData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增顧客
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CreateCustomerData(CustomerDataInViewModel model)
        {
            model.CustomerId = "0000000001";
            model.CustomerName = "哈囉你好嗎股份有限公司";
            model.CustomerAddr = "我們這一家";
            var data = this.CustomerService.CreateCustomerData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 編輯顧客
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditCustomerData(CustomerDataInViewModel model)
        {
            var data = this.CustomerService.EditCustomerData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 刪除顧客
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult DeleteCustomerData(CustomerDataInViewModel model)
        {
            var data = this.CustomerService.DeleteCustomerData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}

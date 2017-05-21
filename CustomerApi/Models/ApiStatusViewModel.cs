using System;

namespace CustomerApi.Models
{
    public class ApiStatusViewModel
    {
        public ApiStatusViewModel()
        {
            this.code = "";
            this.message = "";
            this.datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        public string code;
        public string message;
        public string datetime;
    }
    public class ApiStatusViewModel<T> : ApiStatusViewModel
    {
        public T Data;
    }
}
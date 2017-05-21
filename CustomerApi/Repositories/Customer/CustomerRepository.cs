using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomerApi.Models;
using CustomerApi.Models.Customer;
using System.Text;
using System.Data.SqlClient;
using CustomerApi.Repositories.DataManager;

namespace CustomerApi.Repositories.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerRepository() { }

        public CustomerDataServiceViewModel CreateCustomerData(CustomerDataInViewModel model, string connectString)
        {
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"
            insert into CustomerData (CustomerId,CustomerName,CustomerAddr)
            values (@CustomerId,@CustomerName,@CustomerAddr) 
            
            IF (@@ROWCOUNT > 0)
                BEGIN
                select @CustomerId AS CustomerId,@CustomerName AS CustomerName,@CustomerAddr AS CustomerAddr                select CustomerId,CustomerName,CustomerAddr FROM CustomerData WITH(NOLOCK) WHERE  CustomerId=@CustomerId
                END
            ELSE
                BEGIN
                SELECT NULL
                END");

            var sqlExecutor = new SqlExecutor(new SqlConnection(connectString));
            var result = sqlExecutor.Query<CustomerDataServiceViewModel>(sqlcmd.ToString(), model).FirstOrDefault();

            return result;
        }

        public CustomerDataServiceViewModel DeleteCustomerData(CustomerDataInViewModel model, string connectString)
        {
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"
            DELETE CustomerData WHERE CustomerId=@CustomerId
            
            IF (@@ROWCOUNT > 0)
                BEGIN 
                select @CustomerId AS CustomerId,@CustomerName AS CustomerName,@CustomerAddr AS CustomerAddr
                END
            ELSE
                BEGIN
                SELECT NULL
                END");

            var sqlExecutor = new SqlExecutor(new SqlConnection(connectString));
            var result = sqlExecutor.Query<CustomerDataServiceViewModel>(sqlcmd.ToString(), model).FirstOrDefault();

            return result;
        }

        public CustomerDataServiceViewModel EditCustomerData(CustomerDataInViewModel model, string connectString)
        {
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"
            UPDATE CustomerData
            SET CustomerName=@CustomerName,CustomerAddr=@CustomerAddr WHERE CustomerId=@CustomerId
            
            IF (@@ROWCOUNT > 0)
                BEGIN 
                select @CustomerId AS CustomerId,@CustomerName AS CustomerName,@CustomerAddr AS CustomerAddr
                END
            ELSE
                BEGIN
                SELECT NULL
                END");

            var sqlExecutor = new SqlExecutor(new SqlConnection(connectString));
            var result = sqlExecutor.Query<CustomerDataServiceViewModel>(sqlcmd.ToString(), model).FirstOrDefault();

            return result;
        }

        public IEnumerable<CustomerDataServiceViewModel> GetCustomerData(CustomerDataInViewModel model, string connectString)
        {
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"
            select CustomerId,CustomerName,CustomerAddr FROM CustomerData WITH(NOLOCK) WHERE 1=1 ");

            if (model.CustomerId.Trim() != "")
            { sqlcmd.Append(" CustomerId=@CustomerId "); }
            if (model.KeyWord != "")
            { sqlcmd.Append(" CustomerName LIKE '%'+@CustomerId+'%' "); }

            var sqlExecutor = new SqlExecutor(new SqlConnection(connectString));
            var result = sqlExecutor.Query<CustomerDataServiceViewModel>(sqlcmd.ToString(), model).AsEnumerable();

            return result;
        }
    }
}
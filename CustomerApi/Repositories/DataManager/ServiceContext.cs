using ErpNextApiInterface.Repositiry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using System.Web;

namespace ErpNextApiInterface.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceContext
    {
        private Dictionary<string,SqlConnection> _sqlDicList;
        private CommittableTransaction _ct;
        private TransactionOptions _ops;
        private SqlExecutor _sqlExe;
        private bool _done = false;
        private System.Transactions.IsolationLevel _level = System.Transactions.IsolationLevel.ReadCommitted;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public enum OPERATION { SELECT, UPDATE, DELETE, INSERT };

        /// <summary>
        /// 
        /// </summary>
        public ServiceContext()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        public ServiceContext(System.Transactions.IsolationLevel level)
        {
            _ops = new TransactionOptions();
            _ops.IsolationLevel = _level;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dal"></param>
        /// <returns></returns>
        public IEnumerable<T> ExcuteTransationResult<T>(CommonDal<T> dal,OPERATION OPER) where T : IModel
        {

            if (_ct == null) {
                TransactionOptions ops = new TransactionOptions();
                ops.IsolationLevel = _level;
                _ct = new CommittableTransaction(ops);
                _sqlDicList = new Dictionary<string, SqlConnection>();
            }

            string ConnStr = dal.getConnString();
            SqlConnection sqlCon = null;
            foreach (var item in _sqlDicList)
            {
                if (ConnStr == item.Key)
                {
                    sqlCon = item.Value;
                    break;
                }
            }

            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(ConnStr);
                sqlCon.Open();
                _sqlDicList.Add(ConnStr, sqlCon);
                sqlCon.EnlistTransaction(_ct);
            }

            _sqlExe = new SqlExecutor(sqlCon);

            try
            {
                switch (OPER)
                {
                    case OPERATION.SELECT:
                        return dal.ExcuteSelect(_sqlExe);
                    case OPERATION.DELETE:
                        return dal.ExcuteDelete(_sqlExe);
                    case OPERATION.INSERT:
                        return dal.ExcuteInsert(_sqlExe);
                    case OPERATION.UPDATE:
                        return dal.ExcuteUpdate(_sqlExe);
                    default:
                        return dal.ExcuteSelect(_sqlExe);
                }
            }
            catch (Exception ex)
            {
                logger.Error(GetExceptionDetails(ex));
                _ct.Rollback();
                _done = true;
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dal"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExcuteResult<T>(CommonDal<T> dal,OPERATION OPER,List<string> field=null) where T : IModel
        {
            var sqlCon = new SqlConnection(dal.getConnString());
            var sqlExe = new SqlExecutor(sqlCon);
            try
            {
                switch (OPER)
                {
                    case OPERATION.SELECT:
                        return dal.ExcuteSelect(sqlExe, field);
                    case OPERATION.DELETE:
                        return dal.ExcuteDelete(sqlExe);
                    case OPERATION.INSERT:
                        return dal.ExcuteInsert(sqlExe);
                    case OPERATION.UPDATE:
                        return dal.ExcuteUpdate(sqlExe);
                    default:
                        return dal.ExcuteSelect(sqlExe);
                }
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                var detail = dal.getParaLogDetail();
                if (detail != null)
                {
                    var dbExisted = dal.getConnString().Split(';').Length > 0;
                  
                    if (dbExisted) { message.AppendLine("連線: " + dal.getConnString().Split(';')[0]); }
                    message.AppendLine("語法: " + dal.getCommandString());
                    message.AppendLine("參數: ");
                    foreach (var item in detail)
                    {
                        message.AppendLine(item.Key+"=>"+item.Value);
                    }
                }
                message.AppendLine(GetExceptionDetails(ex));
                logger.Error(message);
                return null;
            }
            

        }

        /// <summary>
        /// 
        /// </summary>
        public void Commit()
        {
            if(!_done && _ct !=null )
            {
                _ct.Commit();
                _done = true;
            }  
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReSet()
        {
            _ct = new CommittableTransaction(_ops);
            _done = false;
        }


        public static string GetExceptionDetails(Exception ex) {
            Exception logException = ex;
            if(ex.InnerException != null)
            {
                //logException = ex.InnerException;
            }

            StringBuilder message = new StringBuilder();
            message.AppendLine();
            message.AppendLine("要求原始URL: " + HttpContext.Current.Request.RawUrl);
            message.AppendLine("例外類型: " + logException.GetType().Name);
            message.AppendLine("例外訊息: " + logException.Message);
            message.AppendLine("例外來源: " + logException.Source);
            message.AppendLine("Stack Trace: " + logException.StackTrace);
            return message.ToString();
        }

    }
}
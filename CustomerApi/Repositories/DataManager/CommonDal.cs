using ErpNextApiInterface.Repositiry;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Reflection;
using CustomerApi.Repositories.DataManager;

namespace ErpNextApiInterface.Interface
{
    /// <summary>
    ///
    /// </summary>
    public class CommonDal<T> where T : IModel
    {

        /// <summary>
        /// 
        /// </summary>
        private string _connString = "";
        protected T _model = null;
        protected T _extendModel = null;
        protected IEnumerable<T> _models = null;
        protected IEnumerable<T> _extendModels = null;
        protected string _commandString = "";

        /// <summary>
        /// 建構式for 單個model當Where條件使用
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="model"></param>
        public CommonDal(string connstring, T model,T extendModel = null)
        {
            _connString = connstring;
            _model = model;
            _extendModel = extendModel;
        }

        /// <summary>
        /// 建構式for 複數個model當Where條件使用
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="models"></param>
        public CommonDal(string connstring, IEnumerable<T> models, IEnumerable<T> extendModels = null)
        {
            _connString = connstring;

            if (models !=null)
            {
                _model = models.FirstOrDefault();
            }

            if (extendModels != null)
            {
                _extendModel = extendModels.FirstOrDefault();
            }

            _models = models;
            _extendModels = extendModels;
        }

        /// <summary>
        /// 取得連線字串
        /// </summary>
        /// <returns></returns>
        public string getConnString()
        {

            return _connString;
        }

        /// <summary>
        /// 供ServiceContext寫入log時可以取得SQL語法
        /// </summary>
        /// <returns></returns>
        public string getCommandString()
        {

            return _commandString;
        }


        /// <summary>
        /// 取得model屬性的字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getParaLogDetail()
        {
            if (_model != null)
            {
                return _model.getParaLogDetail();
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 因為update語法 有部分是在update set a=@a 另一部分是Where b=@b 所以需整合成一個ViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        private T CombindModelAndUpdateModel(T model, T updateModel)
        {
            var updateModelprops = updateModel.GetType().GetProperties();

            foreach (PropertyInfo prop in updateModelprops)
            {
                var value = prop.GetValue(updateModel);
                if (value != null)
                {
                    model.GetType().GetProperty(prop.Name).SetValue(model, value);
                }
            }

            return model;
        }

        /// <summary>
        /// 執行Select
        /// </summary>
        public IEnumerable<T> ExcuteSelect(SqlExecutor search,List<string> field =null)
        {
            _commandString = generateSelectCommand(field);
            if (_commandString == "")
            {
                return null;
            }

            if (_models != null)
            {
                var para = IModel.getPara(_models);
                var result = search.Query<T>(_commandString, para);
                return result;
            }
            else if (_model != null)
            {
                var result = search.Query<T>(_commandString, _model);
                return result;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 執行Insert
        /// </summary>
        public IEnumerable<T> ExcuteInsert(SqlExecutor search)
        {
            _commandString = generateInsertCommand();
            if (_commandString == "")
            {
                return null;
            }

            if (_models != null)
            {
                var result = new List<T>();
                foreach (var item in _models)
                {
                    result.Add(search.Query<T>(_commandString, item).FirstOrDefault());
                }
                return result;
            }
            else if (_model != null)
            {

                var result = search.Query<T>(_commandString, _model);
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 執行Update
        /// </summary>
        public IEnumerable<T> ExcuteUpdate(SqlExecutor search)
        {
            generateUpdateCommand();
            if (_models == null)
            {
                if (_model != null && _extendModel != null)
                {
                    var model = CombindModelAndUpdateModel(_model, _extendModel);
                    var result = search.Query<T>(_commandString, model);
                    return result;
                }
                else
                {
                    return null;
                }
            }

            if (_extendModels == null)
            {
                return null;
            }

            if (_models.Count() != _extendModels.Count())
            {
                return null;
            }
            else
            {
                var result = new List<T>();
                for (int i = 0; i <= _models.Count() - 1; i++)
                {
                    var model = CombindModelAndUpdateModel(_models.ElementAt(i), _extendModels.ElementAt(i));
                    result.Add(search.Query<T>(_commandString, model).FirstOrDefault());

                }
                return result;
            }
        }


        /// <summary>
        /// 執行Delete
        /// </summary>
        public IEnumerable<T> ExcuteDelete(SqlExecutor search)
        {
            generateDeleteCommand();
            if (_commandString == "")
            {
                return null;
            }

            if (_models != null)
            {
                var result = new List<T>();
                foreach (var item in _models)
                {
                    result.Add(search.Query<T>(_commandString, item).FirstOrDefault());
                }
                return result;
            }
            else if (_model != null)
            {

                var result = search.Query<T>(_commandString, _model);
                return result;
            }
            else
            {
                return null;
            }
        }



        private string getTableName()
        {
            string tableName = typeof(T).Name;
            if (tableName.IndexOf("ViewModel")>0)
            {
                tableName = tableName.Substring(0, tableName.Length - 9);
            }
            return tableName;

        }

        /// <summary>
        /// 產生Select語法
        /// </summary>
        /// <returns></returns>
        public virtual string generateSelectCommand(List<string> field)
        {
            StringBuilder strField = new StringBuilder();
            if (field != null)
            {
                for (int i = 0; i < field.Count(); i++)
                {
                    if (i != 0)
                    {
                        strField.Append(" , ");
                    }

                    strField.Append(field[i]);
                }
            }
            else
            {
                strField.Append("*");
            }


            StringBuilder cmd = new StringBuilder();
            cmd.Append("SELECT ");
            cmd.Append(strField.ToString());
            cmd.Append(" FROM ");
            cmd.Append(getTableName());
            cmd.Append(" with (nolock)  ");

            if (_models != null)
            {
                if (_models.Count() > 0)
                {
                    cmd.Append(" WHERE ");
                    cmd.Append(_model.getDefaultParaString("in")); //a=@a and b=@b and c=@c
                }
            }
            else if(_model!=null)
            {
                if (_model.getParaList().Count() > 0)
                {
                    cmd.Append(" WHERE ");
                    cmd.Append(_model.getDefaultParaString("in")); //a in @a and b in @b and c in @c
                }
            }

            return cmd.ToString();
        }

        /// <summary>
        /// 執行Update語法
        /// </summary>
        /// <returns></returns>
        public virtual string generateUpdateCommand()
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append("UPDATE ");
            cmd.Append(getTableName());
            cmd.Append(" SET  ");
            cmd.Append(_model.getDefaultParaString("="));
            cmd.Append(" WHERE ");
            cmd.Append(_extendModel.getDefaultParaString("="));//a=@a and b=@b and c=@c
            return cmd.ToString();
        }

        /// <summary>
        /// 執行Delete語法
        /// </summary>
        /// <returns></returns>
        public virtual string generateDeleteCommand()
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append("DELETE ");
            cmd.Append(getTableName());
            cmd.Append(" WHERE ");
            cmd.Append(_model.getDefaultParaString("="));//a=@a and b=@b and c=@c
            return cmd.ToString();
        }

        /// <summary>
        /// 執行Insert語法
        /// </summary>
        /// <returns></returns>
        public virtual string generateInsertCommand()
        {
            StringBuilder cmd = new StringBuilder();
            cmd.Append("INSERT  ");
            cmd.Append(getTableName());
            cmd.Append(_model.getDefaultInsertString(""));// (a,b,c)
            cmd.Append(" VALUES   ");
            cmd.Append(_model.getDefaultInsertString("@"));// (@a,@b,@c)

            return cmd.ToString();
        }


    }

  }
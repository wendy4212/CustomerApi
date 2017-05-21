using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ErpNextApiInterface.Interface
{
    /// <summary>
    ///
    /// </summary>
    public class IModel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static dynamic getPara(IEnumerable<IModel> model)
        {
            var expando = new ExpandoObject() as IDictionary<string, Object>;
            var props = model.FirstOrDefault().GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var value = model.Select(x => x.GetType().GetProperty(prop.Name).GetValue(x));
                expando.Add(prop.Name, value);
            }

            return expando;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<string> getParaList()
        {
            var props = this.GetType().GetProperties();
            var paraList = new List<string>();

            foreach (PropertyInfo prop in props)
            {
                var value = this.GetType().GetProperty(prop.Name).GetValue(this);
                if (value != null)
                {
                    paraList.Add(prop.Name);
                }
            }
            return paraList;
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> getParaLogDetail()
        {
            var props = this.GetType().GetProperties();
            var paraList = new Dictionary<string,string>();

            foreach (PropertyInfo prop in props)
            {
                var value = this.GetType().GetProperty(prop.Name).GetValue(this);
                if (value != null)
                {
                    paraList.Add(prop.Name, value.ToString());
                }
            }
            return paraList;
        }

        /// <summary>
        /// ex a=@a,b=@b or a in @a ,b in @b
        /// </summary>
        /// <param name="Operation"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string getDefaultParaString(string operation)
        {
            var items = getParaList();

            StringBuilder str = new StringBuilder();
            foreach (var item in items)
            {
                str.Append(str.Length == 0 ? "" : " AND ");
                str.Append(item +" "+ operation + " @" + item);
            }
    

            return str.ToString();
        }

        /// <summary>
        /// ex (a,b,c,d)  or (@a,@b,@c)
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string getDefaultInsertString(string prefix)
        {

            StringBuilder tmpSb = new StringBuilder();
            tmpSb.Append("(");
            var items = getParaList();
            for (int i = 0; i <= items.Count() - 1; i++)
            {
                if (i > 0)
                {
                    tmpSb.Append(",");
                }
                tmpSb.Append(prefix);
                tmpSb.Append(items[i].Trim());

            }
            tmpSb.Append(")");
            return tmpSb.ToString();
        }
    }

   
}
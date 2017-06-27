using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;

namespace BLL
{
   public class BLLManager
    {
       DAL.DALServer dll = new DAL.DALServer();
       public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
       {
           return dll.GetListByPage(strWhere,orderby,startIndex,endIndex);
       }
       public int GetRecordCount( string strWhere)
       {
           return dll.GetRecordCount(strWhere);
       }


       public static string GetStrJson(string strWhere, string orderby, int startIndex, int endIndex)//static有无
       {
           DAL.DALServer dll = new DAL.DALServer();//C#非静态的字段要求对象引用

           DataSet ds = dll. GetListByPage(strWhere, orderby, startIndex, endIndex);
           int count =dll. GetRecordCount(strWhere);
           string strJson = ToJson.Dataset2Json(ds,count);
           return strJson;
           //throw new NotImplementedException();
       }
    }
}

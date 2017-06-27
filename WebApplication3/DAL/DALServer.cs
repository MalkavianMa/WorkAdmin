using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DAL
{
  public  class DALServer
    {
      public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
      {
          //String sql = ConfigurationManager.ConnectionStrings["BookShopconnStr"].ToString();

          StringBuilder strSql = new StringBuilder();
          strSql.Append("SELECT * FROM ( ");
          strSql.Append(" SELECT ROW_NUMBER() OVER (");
          if (!string.IsNullOrEmpty(orderby.Trim()))
          {
              strSql.Append("order by T." + orderby);
          }
          else
          {
              strSql.Append("order by T.AdminID desc");
          }
          strSql.Append(")AS Row, T.*  from V_admin_MgPersonFiles T ");
          if (!string.IsNullOrEmpty(strWhere.Trim()))
          {
              strSql.Append(" WHERE " + strWhere);
          }
          strSql.Append(" ) TT");
          strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);

          return SqlHelper.ExecuteDataSet(SqlHelper.Connstring, strSql.ToString(), null);
          // throw new NotImplementedException();
      }

      /// <summary>
      /// 获取记录总数 
      /// </summary>
      /// <param name="strWhere">查询过滤条件字段</param>
      /// <returns></returns>
      public int GetRecordCount(string strWhere)
      {
          StringBuilder strSql = new StringBuilder();
          strSql.Append("select count(1) FROM V_admin_MgPersonFiles");
          if (strWhere.Trim() != "")
          {
              strSql.Append(" where " + strWhere);
          }
          object obj = SqlHelper.GetSingle(strSql.ToString());
          if (obj == null)
          {
              return 0;
          }
          else
          {
              return Convert.ToInt32(obj);
          }
      }
  }
}

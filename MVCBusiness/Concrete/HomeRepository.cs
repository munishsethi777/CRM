using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcJqGrid;
using MVCModels;
using System.Data;
using System.Collections;

namespace MVCBusiness
{
    public class HomeRepository : IHomeRepository
    {

        private AccountsEntities objContext = new AccountsEntities();
        string sSql = "";
        #region Events

        public IEnumerable CalendarDataForEvents()
        {
            try
            {
                var result = from a in objContext.CalendarEvents.AsEnumerable()
                    select new { 
                        id = a.id,
                        title =  a.title, 
                        start = a.startdate.ToString("yyyy-MM-ddThh:mm:ss"),
                        end = a.enddate.ToString("yyyy-MM-ddThh:mm:ss") 
                    };


                return result.ToArray();

            }
            catch (Exception ex)
            {

            }
            return null;
        }

            public object GridDataForEvents(GridSettings objGrdSettings)
            {
                try
                {
                    List<CalendarEvent> objOrderDashboard = GetEventList(objGrdSettings);
                    Int64 objTot = objContext.CalendarEvents.Count();
                    if (objOrderDashboard == null)
                        return null;

                    var jsonData = new
                    {
                        total = objTot / objGrdSettings.PageSize + 1,
                        page = objGrdSettings.PageIndex,
                        records = objTot,
                        rows = (
                            from c in objOrderDashboard.AsEnumerable()
                            select new
                            {
                                id = c.id,
                                cell = new object[] 
                                    { 
                                        c.id, // primary key
                                        c.title,
                                        c.description,
                                        c.startdate.ToShortDateString() + " " + c.startdate.ToShortTimeString(),
                                        c.enddate.ToShortDateString() + " " + c.enddate.ToShortTimeString(),
                                        c.isrepeated.ToString()  == "True" ? "Repeated" : "Not Repeated",
                                    }
                            }).ToArray()
                    };
                    return jsonData;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }

            //Private 
            private List<CalendarEvent> GetEventList(GridSettings objGrdSettings)
            {
                try
                {
                    string[] conditions = getCondition(objGrdSettings);
                    //sql query result
                    string sSql = string.Format("select  * from (select ROW_NUMBER() OVER({0}) as rowid ,c.* from CalendarEvents c {1}) as temp {2} ", conditions[0], conditions[1], conditions[2]);
                    AccountsEntities objContext = new AccountsEntities();
                    var list = objContext.CalendarEvents.SqlQuery(sSql).ToList();
                    return list;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }

            }

        #endregion
        #region Users
            private List<User> GetUsersList(GridSettings objGrdSettings)
            {
                try
                {
                    string[] conditions = getCondition(objGrdSettings);
                    //sql query result
                    string sSql = string.Format("select  * from (select ROW_NUMBER() OVER({0}) as rowid ,c.* from Users c {1}) as temp {2} ", conditions[0], conditions[1], conditions[2]);
                    AccountsEntities objContext = new AccountsEntities();
                    var list = objContext.Users.SqlQuery(sSql).ToList();
                    return list;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }

            }
            
            public object GridDataForUsers(GridSettings objGrdSettings)
            {
                try
                {
                    List<User> objOrderDashboard = GetUsersList(objGrdSettings);
                    Int64 objTot = objContext.Users.Count();
                    if (objOrderDashboard == null)
                        return null;

                    var jsonData = new
                    {
                        total = objTot / objGrdSettings.PageSize + 1,
                        page = objGrdSettings.PageIndex,
                        records = objTot,
                        rows = (
                            from c in objOrderDashboard.AsEnumerable()
                            select new
                            {
                                id = c.id,
                                cell = new object[] 
                                    { 
                                        c.id, // primary key
                                        c.name,
                                        c.email,
                                        c.phone,
                                        c.city,
                                        c.state,
                                        c.isactive == 1 ? "Active" : "InActive",
                                    }
                            }).ToArray()
                    };
                    return jsonData;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }

            #endregion
           
            public int Delete(string iParams,String iSql)
            {
                int iResult = -1;
                try
                {
                    //Delete 

                    sSql = string.Format(iSql, iParams);
                    iResult = objContext.Database.ExecuteSqlCommand(sSql);

                }
                catch (Exception Ex)
                {
                    iResult = -1;
                }
                return iResult;
            }
            
        private string[] getCondition(GridSettings objGrdSettings)
        {

            string WhereCondition = "";
            string sOrderBy = "", sSearchString = "";
            //for sorting
            string orderBy = "";
            string pagination = "";
            if (!string.IsNullOrEmpty(objGrdSettings.SortColumn))
            {
                if (objGrdSettings.SortColumn.Contains("date") || objGrdSettings.SortColumn.ToLower().Contains("time"))
                    sOrderBy += string.Format(" CONVERT(VARCHAR, ISNULL({0},0), 101) ", objGrdSettings.SortColumn, objGrdSettings.SortOrder);
                else
                    sOrderBy += string.Format(" CONVERT(VARCHAR, ISNULL({0},0)) {1} ", objGrdSettings.SortColumn, objGrdSettings.SortOrder);

                orderBy = string.Format(" order by  {0}", sOrderBy);
            }

            //for search
            sSearchString = "";
            WhereCondition += "where 1=1";
            if (objGrdSettings.Where != null)
            {
                foreach (var item in objGrdSettings.Where.rules)
                {
                    if (item.field.Contains("date") || item.field.ToLower().Contains("time"))
                        sSearchString += string.Format(" and convert(varchar, {0}, 101) like '{1}%' ", item.field, item.data);
                    else
                        sSearchString += string.Format(" and CONVERT(VARCHAR, ISNULL({0},0)) like '{1}%' ", item.field, item.data);
                }
                WhereCondition += string.Format("{0}", sSearchString);
            }


            // for paging
            if (objGrdSettings.PageSize > 0)
            {
                pagination = string.Format(" where rowid BETWEEN {0} AND {1}", ((objGrdSettings.PageIndex - 1) * objGrdSettings.PageSize) + 1, (objGrdSettings.PageIndex * objGrdSettings.PageSize));
            }
            string[] filter = new string[3];
            filter[0] = orderBy;
            filter[1] = WhereCondition;
            filter[2] = pagination;
            return filter;
        }

    }
}

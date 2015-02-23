using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcJqGrid;
using System.Collections;
namespace MVCBusiness
{
   public interface IHomeRepository
    {
        
        object GridDataForEvents(GridSettings objGrdSettings);
        object GridDataForUsers(GridSettings objGrdSettings);
        IEnumerable CalendarDataForEvents();
        int Delete(string iParams, String iSql);
    }
}

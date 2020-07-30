using Ruanmou.Framework.Models;
using Ruanmou.Remote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Remote.Interface
{
    public interface ISearchService
    {
        PageResult<CommodityModel> QueryCommodityPage(int pageIndex, int pageSize, string keyword, List<int> categoryIdList, string priceFilter, string priceOrderBy);
    }
}

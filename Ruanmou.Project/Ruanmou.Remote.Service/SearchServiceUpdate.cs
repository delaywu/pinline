using Ruanmou.Framework.Models;
using Ruanmou.Remote.Interface;
using Ruanmou.Remote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Remote.Service
{
    /// <summary>
    /// 封装了对分布式搜索服务
    /// 支持分页查询，
    /// </summary>
    public class SearchServiceUpdate : ISearchService
    {
        public PageResult<CommodityModel> QueryCommodityPage(int pageIndex, int pageSize, string keyword, List<int> categoryIdList, string priceFilter, string priceOrderBy)
        {
            Console.WriteLine("1234567SearchServiceUpdate");
            LuceneSearchService.SearcherClient client = null;
            try
            {
                client = new LuceneSearchService.SearcherClient();
                string result = client.QueryCommodityPage(pageIndex, pageSize, keyword, categoryIdList?.ToArray(), priceFilter, priceOrderBy);

                client.Close();//会关闭链接，但是如果网络异常了，会抛出异常而且关闭不了
                return Newtonsoft.Json.JsonConvert.DeserializeObject<PageResult<CommodityModel>>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (client != null)
                    client.Abort();
                throw ex;
            }

        }
    }
}

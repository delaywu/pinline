using Ruanmou.Bussiness.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Bussiness.Service
{
    public class CommodityService : BaseService, ICommodityService
    {
        public CommodityService(DbContext context) : base(context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.EF6.CodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (CodeFirstFromDBContext context = new CodeFirstFromDBContext())
                {
                    JDCommodity001 commodity001 = context.JD_Commodity_001.Find(123);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}

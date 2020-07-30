using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.EF6.CodeFirstFromDB
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (CodeFirstFromDBContext context = new CodeFirstFromDBContext())
                {
                    context.Database.Log += s => Console.WriteLine($"当前执行sql:{s}");

                    JDCommodity001 commodity1 = context.JD_Commodity_001.Find(123);

                    JDCommodity002 commodity2 = context.JD_Commodity_002.Find(123);

                    JDCommodity003 commodity3 = context.JD_Commodity_003.Find(123);

                    User user = context.Users.Find(17);//主键查询
                    User user28 = context.Users.FirstOrDefault(u => u.Id == 28);
                    var list = context.Users.Where(u => u.Id > 100);

                    User userNew = new User()
                    {
                        Account = "Admin",
                        State = 0,
                        CompanyId = 4,
                        CompanyName = "万达集团",
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        Email = "57265177@qq.com",
                        LastLoginTime = null,
                        LastModifierId = 0,
                        LastModifyTime = DateTime.Now,
                        Mobile = "18664876671",
                        Name = "yoyo",
                        Password = "12356789",
                        UserType = 1
                    };
                    context.Users.Add(userNew);
                    context.SaveChanges();//表示保存

                    userNew.Name = "CodeMan";
                    context.SaveChanges();//表示保存

                    context.Users.Remove(userNew);
                    context.SaveChanges();//表示保存
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


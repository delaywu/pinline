using Ruanmou.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Project
{
    public class ContextLifetimeTest
    {
        public static void Show()
        {
            #region 多个数据修改，一次SaveChange,开始事务保存
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    User userNew = new User()
            //    {
            //        Account = "Admin",
            //        State = 0,
            //        CompanyId = 4,
            //        CompanyName = "万达集团",
            //        CreateTime = DateTime.Now,
            //        CreatorId = 1,
            //        Email = "57265177@qq.com",
            //        LastLoginTime = null,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //        Mobile = "18664876671",
            //        Name = "yoyo",
            //        Password = "12356789",
            //        UserType = 1
            //    };
            //    dbContext.Users.Add(userNew);

            //    User user17 = dbContext.Users.FirstOrDefault(u => u.Id == 17);
            //    user17.Name += "aaa";

            //    User user18 = dbContext.Set<User>().Find(18);
            //    user18.Name += "bbb";

            //    Company company2019 = dbContext.Set<Company>().Find(2019);
            //    dbContext.Companies.Remove(company2019);

            //    dbContext.SaveChanges();
            //}
            #endregion

            #region 多个数据操作一次savechange，任何一个失败直接全部失败
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    User userNew = new User()
            //    {
            //        Account = "Admin",
            //        State = 0,
            //        CompanyId = 4,
            //        CompanyName = "万达集团",
            //        CreateTime = DateTime.Now,
            //        CreatorId = 1,
            //        Email = "57265177@qq.com",
            //        LastLoginTime = null,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //        Mobile = "18664876671",
            //        Name = "yoyo",
            //        Password = "12356789",
            //        UserType = 1
            //    };
            //    dbContext.Users.Add(userNew);

            //    User user17 = dbContext.Users.FirstOrDefault(u => u.Id == 17);
            //    user17.Name += "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            //    User user18 = dbContext.Set<User>().Find(18);
            //    user18.Name += "bbb";

            //    //Company company2019 = dbContext.Set<Company>().Find(2019);
            //    //dbContext.Companies.Remove(company2019);

            //    dbContext.SaveChanges();
            //}
            #endregion


            //能不能整个进程就一个context实例呀？ 当然不可以
            //多线程能不能是一个实例呢？ 一般来说不行(除非自己很明确状况)

            //那每个数据操作都去来个context实例？也不好;
            //1 内存消耗大，没法缓存
            //2 多context实例 join 不行，因为上下文环境不一样；除非把数据都查到内存，再去linq
            //3 多context的事务也麻烦点
            using (JDDbContext dbContext1 = new JDDbContext())
            using (JDDbContext dbContext2 = new JDDbContext())
            {
                var list = from u in dbContext1.Users
                           join c in dbContext2.Companies on u.CompanyId equals c.Id
                           where new int[] { 1, 2, 3, 4, 6, 7, 10 }.Contains(u.Id)
                           select new
                           {
                               Account = u.Account,
                               Pwd = u.Password,
                               CompanyName = c.Name
                           };
                foreach (var user in list)
                {
                    Console.WriteLine("{0} {1}", user.Account, user.Pwd);
                }
            }
            //context使用建议：
            //DbContext是个上下文环境，里面内置对象跟踪，会开启链接(就等于一个数据库链接)
            //一次请求，最好是一个context； 
            //多个请求 /多线程最好是多个实例； 
            //用完尽快释放； 
        }
    }
}

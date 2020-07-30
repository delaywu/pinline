using Ruanmou.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ruanmou.Project
{
    /// <summary>
    /// 1 这句话执行完，没有数据库查询
    /// 2 迭代遍历数据才去数据库查询--在真实需要使用数据时，才去数据库查询的
    /// 这就是延迟查询,好处：
    /// 可以叠加多次查询条件，一次提交给数据库；可以按需获取数据；
    /// 要注意的是：
    /// a 迭代使用时，用完了关闭连接  b 脱离context作用域
    /// 
    /// </summary>
    public class EFQueryAdvancedTest
    {
        public static void Show()
        {
            //IQueryable<User> sources = null;
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    sources = dbContext.Set<User>().Where(u => u.Id > 20);


            //    var userList = dbContext.Set<User>().Where(u => u.Id > 10);//1 这句话执行完，没有数据库查询

            //    foreach (var user in userList)//2 迭代遍历数据才去数据库查询--在真实需要使用数据时，才去数据库查询的
            //    {
            //        Console.WriteLine(user.Name);
            //    }
            //    //IEnumerator
            //    //这就是延迟查询,可以叠加多次查询条件，一次提交给数据库；可以按需获取数据；
            //    userList = userList.Where(u => u.Id < 100);
            //    userList = userList.Where(u => u.State < 3);
            //    userList = userList.OrderBy(u => u.Name);

            //    var list = userList.ToList<User>();//ToList()  迭代器  Count() FitstOrDefalut()

            //    Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
            //    //延迟查询也要注意：a 迭代使用时，用完了关闭连接  b 脱离context作用域
            //}
            //foreach (var user in sources)//这个时候查询，已经超出作用域。会异常
            //{
            //    Console.WriteLine(user.Name);
            //}


            {
                List<int> intList = new List<int>() { 123, 4354, 3, 23, 3, 4, 4, 34, 34, 3, 43, 43, 4, 34, 3 };
                var list = intList.Where(i =>
                 {
                     Thread.Sleep(i);
                     return i > 10;
                 });//没有过滤
                foreach (var i in list)//才去过滤
                {
                    Console.WriteLine(i);
                }
                Console.WriteLine("*********************");
                //这里是延迟的，利用的是迭代器的方式，每次去迭代访问时，才去筛选一次，委托+迭代器
            }
            //楼上是IEnumerable类型，数据其实已经在内存里，有个迭代器的实现，用的是委托
            //楼下的IQueryable类型，数据在数据库里面，这个list里面有表达式目录树---返回值类型--IQueryProvider(查询的支持工具，sqlserver语句的生成)
            //其实userList只是一个包装对象，里面有表达式目录树，有结果类型，有解析工具，还有上下文，真需要数据的时候才去解析sql，执行sql，拿到数据的---因为表达式目录树可以拼装；
            {
                using (JDDbContext dbContext = new JDDbContext())
                {
                    var userList = dbContext.Set<User>().Where(u => u.Id > 10);

                    foreach (var user in userList)
                    {
                        Console.WriteLine(user.Name);
                    }
                }
            }
        }
    }
}

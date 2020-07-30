using Ruanmou.EF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Project
{
    /// <summary>
    /// SaveChanges是以context为标准的，如果监听到任何数据的变化，然后会一次性的保存到数据库去，而且会开启事务！
    /// 关注下EntityState相互转换
    /// 
    /// 可以直接Attach增加监听
    /// </summary>
    public class EFStateTest
    {
        public static void Show()
        {
            //User userNew = new User()
            //{
            //    Account = "Admin",
            //    State = 0,
            //    CompanyId = 4,
            //    CompanyName = "万达集团",
            //    CreateTime = DateTime.Now,
            //    CreatorId = 1,
            //    Email = "57265177@qq.com",
            //    LastLoginTime = null,
            //    LastModifierId = 0,
            //    LastModifyTime = DateTime.Now,
            //    Mobile = "18664876671",
            //    Name = "yoyo",
            //    Password = "12356789",
            //    UserType = 1
            //};
            //using (JDDbContext context = new JDDbContext())
            //{
            //    Console.WriteLine(context.Entry<User>(userNew).State);//实体跟context没关系 Detached
            //    userNew.Name = "小鱼";
            //    context.SaveChanges();//Detached啥事儿不发生

            //    context.Users.Add(userNew);
            //    Console.WriteLine(context.Entry<User>(userNew).State);//Added
            //    context.SaveChanges();//插入数据(自增主键在插入成功后，会自动赋值过去)
            //    Console.WriteLine(context.Entry<User>(userNew).State);//Unchanged（跟踪，但是没变化）

            //    userNew.Name = "加菲猫";//修改----内存clone 
            //    Console.WriteLine(context.Entry<User>(userNew).State);//Modified
            //    context.SaveChanges();//更新数据库，因为状态是Modified
            //    Console.WriteLine(context.Entry<User>(userNew).State);//Unchanged（跟踪，但是没变化）

            //    context.Users.Remove(userNew);
            //    Console.WriteLine(context.Entry<User>(userNew).State);//Deleted
            //    context.SaveChanges();//删除数据，因为状态是Deleted
            //    Console.WriteLine(context.Entry<User>(userNew).State);//Detached已经从内存移除了


            //    {
            //        User user5 = context.Users.Find(5);
            //        user5.Name += "12";
            //        user5.State += 1;
            //        context.SaveChanges();//以什么为凭据，去生成的sql呢？
            //    }
            //}

            //{
            //    //是不是必须得先查询再更新
            //    User user = null;
            //    using (JDDbContext context = new JDDbContext())
            //    {
            //        User user20 = context.Users.Find(20);
            //        Console.WriteLine(context.Entry<User>(user20).State);
            //        user = user20;

            //        //User user21 = context.Users.Where(u => u.Id == 21).AsNoTracking().FirstOrDefault();
            //        //Console.WriteLine(context.Entry<User>(user21).State);
            //    }

            //    user.Name = "滑猪小板123456789";
            //    using (JDDbContext context = new JDDbContext())
            //    {
            //        context.Users.Attach(user);
            //        Console.WriteLine(context.Entry<User>(user).State);

            //        //user.Name = "滑猪小板";//只能更新这个字段
            //        context.Entry<User>(user).State = EntityState.Modified;//全字段更新
            //        Console.WriteLine(context.Entry<User>(user).State);

            //        //User userUpdate = context.Users.Find(user.Id);//查出来自然是监听
            //        //userUpdate.Name = user.Name;
            //        context.SaveChanges();
            //    }
            //    //EF本身是依赖监听变化，然后更新的；
            //    //平时业务都一次查询，然后用户修改，然后提交，
            //    //把实体传到EF，然后context.Entry<User>(user).State = EntityState.Modified;

            //    //会被长度校验失败
            //    //using (JDDbContext context = new JDDbContext())
            //    //{
            //    //    User userUpdate = new User() { Id = 22 };
            //    //    context.Users.Attach(userUpdate);

            //    //    Console.WriteLine(context.Entry<User>(userUpdate).State);
            //    //    userUpdate.Name = "滑猪小板98765432";//只能更新这个字段
            //    //    Console.WriteLine(context.Entry<User>(userUpdate).State);

            //    //    //User userUpdate = context.Users.Find(user.Id);//查出来自然是监听
            //    //    //userUpdate.Name = user.Name;
            //    //    context.SaveChanges();
            //    //}

            //    //{
            //    //    using (JDDbContext context = new JDDbContext())
            //    //    {
            //    //        User userUpdate = context.Users.Find(user.Id);//查出来自然是监听
            //    //        context.Users.de
            //    //        userUpdate.Name = user.Name;
            //    //        context.SaveChanges();
            //    //    }
            //    //}
            //}
            {
                using (JDDbContext context = new JDDbContext())
                {
                    var userList = context.Users.Where(u => u.Id > 10).ToList();
                    //var userList = context.Users.Where(u => u.Id > 10).AsNoTracking().ToList();
                    Console.WriteLine(context.Entry<User>(userList[3]).State);
                    Console.WriteLine("*********************************************");
                    var user5 = context.Users.Find(5);
                    Console.WriteLine("*********************************************");
                    var user1 = context.Users.Find(30);
                    Console.WriteLine("*********************************************");
                    var user2 = context.Users.FirstOrDefault(u => u.Id == 30);
                    Console.WriteLine("*********************************************");
                    var user3 = context.Users.Find(30);
                    Console.WriteLine("*********************************************");
                    var user4 = context.Users.FirstOrDefault(u => u.Id == 30);
                }
                //Find可以使用缓存，优先从内存查找(限于context)
                //但是linq时不能用缓存，每次都是要查询的
                //AsNoTracking() 如果数据不会更新，加一个可以提升性能
            }
            {
                //按需更新--只更新修改过的字段
                using (JDDbContext context = new JDDbContext())
                {
                    //var user5 = context.Users.Find(5);
                    //user5.Name += "abc";
                    //context.SaveChanges();

                    //User user = new User()
                    //{
                    //    Id = 123,
                    //    Name = "",
                    //};//不能实体传递
                    //传递json--主键：值，更改属性：值--来解读赋值

                    //var user5 = context.Users.Find(5);
                    //context.Entry<User>(user5).Property("Name").IsModified = true;//指定某字段被改过
                }
            }
        }
    }
}


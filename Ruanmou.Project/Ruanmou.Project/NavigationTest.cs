using Ruanmou.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ruanmou.Project
{
    public class NavigationTest
    {
        public static void ShowQuery()
        {
            //一般来说，主外键表，主表有个子表的集合，导航属性
            //子表里面还有个主表的实例，引用属性
            //二班来说，不是主外键也可以这么玩的

            //1 默认情况下，导航属性是延迟查询；
            //条件是virtaul属性+默认配置
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    var companyList = dbContext.Set<Company>().Where(c => c.Id < 20);
            //    foreach (var company in companyList)//只差company
            //    {
            //        Console.WriteLine(company.Name);
            //        foreach (var user in company.Users)//再去查用户
            //        {
            //            Console.WriteLine(user.Name);
            //        }
            //    }
            //}

            ////2 关闭延迟加载，子表数据就没了
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    dbContext.Configuration.LazyLoadingEnabled = false;//关闭延迟查询
            //    var companyList = dbContext.Set<Company>().Where(c => c.Id < 20);
            //    foreach (var company in companyList)//只差company
            //    {
            //        Console.WriteLine(company.Name);
            //        foreach (var user in company.Users)//再去查用户
            //        {
            //            Console.WriteLine(user.Name);
            //        }
            //    }
            //}

            //3 预先加载  Include 查询主表时就把子表数据一次性查出来
            //其实自己join也可以的
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    dbContext.Configuration.LazyLoadingEnabled = false;//是否关闭无所谓
            //    var companyList = dbContext.Set<Company>().Include("Users").Where(c => c.Id < 20);
            //    foreach (var company in companyList)//只差company
            //    {
            //        Console.WriteLine(company.Name);
            //        foreach (var user in company.Users)//再去查用户
            //        {
            //            Console.WriteLine(user.Name);
            //        }
            //    }
            //}

            ////4 关闭延迟查询后,如果需要子表数据，可以显示加载
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    dbContext.Configuration.LazyLoadingEnabled = false;//关闭延迟查询
            //    var companyList = dbContext.Set<Company>().Where(c => c.Id < 20);
            //    foreach (var company in companyList)//只查company
            //    {
            //        Console.WriteLine(company.Name);
            //        dbContext.Entry<Company>(company).Collection(c => c.Users).Load();
            //        //dbContext.Entry<Company>(company).Reference(c => c.Users).Load();
            //        foreach (var user in company.Users)//再去查用户
            //        {
            //            Console.WriteLine(user.Name);
            //        }
            //    }
            //}
            //我觉得，其实导航属性的东西，自己join也是可以搞定的

            //非主外键 也可以导航属性
            using (JDDbContext dbContext = new JDDbContext())
            {
                var list = dbContext.Set<SysRoleMenuMapping>().Where(m => m.Id > 5);
                foreach (var mapping in list)
                {
                    Console.WriteLine($"{mapping.Id}--{mapping.SysRoleId}--{mapping.SysMenuId}");
                    Console.WriteLine(mapping.SysMenu.Text);
                    Console.WriteLine(mapping.SysRole.Text);

                    foreach (var m in mapping.SysMenu.SysRoleMenuMappingList)
                    {
                        Console.WriteLine($"{m.Id}--{m.SysRoleId}--{m.SysMenuId}");
                    }
                }
            }

        }

        public static void ShowInsert()
        {
            //数据插入：A表--B表(包含A的ID)--ID是自增的
            #region 一次savechange，如果是主外键，可以自动使用自增id；如果不是，就用不到
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    Company company = new Company()
            //    {
            //        Name = "软谋教育高级班1",
            //        CreateTime = DateTime.Now,
            //        CreatorId = 1,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //    };

            //    User userNew1 = new User()
            //    {
            //        Account = "Admin",
            //        State = 0,
            //        CompanyId = company.Id,
            //        CompanyName = company.Name,
            //        CreateTime = DateTime.Now,
            //        CreatorId = 1,
            //        Email = "57265177@qq.com",
            //        LastLoginTime = null,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //        Mobile = "18664876671",
            //        Name = "民工甲1",
            //        Password = "12356789",
            //        UserType = 1
            //    };
            //    User userNew2 = new User()
            //    {
            //        Account = "Admin",
            //        State = 0,
            //        CompanyId = company.Id,
            //        CompanyName = company.Name,
            //        CreateTime = DateTime.Now,
            //        CreatorId = 1,
            //        Email = "57265177@qq.com",
            //        LastLoginTime = null,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //        Mobile = "18664876671",
            //        Name = "逐梦1",
            //        Password = "12356789",
            //        UserType = 2
            //    };


            //    dbContext.Set<User>().Add(userNew1);
            //    dbContext.Set<User>().Add(userNew2);

            //    dbContext.Set<Company>().Add(company);

            //    dbContext.SaveChanges();
            //}

            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    User userNew1 = new User()
            //    {
            //        Account = "Admin",
            //        State = 0,
            //        CompanyId = 2031,
            //        CompanyName = "软谋",
            //        CreateTime = DateTime.Now,
            //        CreatorId = 1,
            //        Email = "57265177@qq.com",
            //        LastLoginTime = null,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //        Mobile = "18664876671",
            //        Name = "民工甲1",
            //        Password = "12356789",
            //        UserType = 1
            //    };
            //    SysLog sysLog = new SysLog()
            //    {
            //        CreateTime = DateTime.Now,
            //        CreatorId = userNew1.Id,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //        Detail = "12345678",
            //        Introduction = "sadsfghj",
            //        LogType = 1,
            //        UserName = "zhangsan"
            //    };
            //    dbContext.Set<User>().Add(userNew1);
            //    dbContext.Set<SysLog>().Add(sysLog);
            //    dbContext.SaveChanges();
            //}
            #endregion

            #region 能成功，但是有事务问题
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    User userNew1 = new User()
            //    {
            //        Account = "Admin",
            //        State = 0,
            //        CompanyId = 2031,
            //        CompanyName = "软谋",
            //        CreateTime = DateTime.Now,
            //        CreatorId = 1,
            //        Email = "57265177@qq.com",
            //        LastLoginTime = null,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //        Mobile = "18664876671",
            //        Name = "民工甲12",
            //        Password = "12356789",
            //        UserType = 1
            //    };
            //    dbContext.Set<User>().Add(userNew1);
            //    dbContext.SaveChanges();

            //    SysLog sysLog = new SysLog()
            //    {
            //        CreateTime = DateTime.Now,
            //        CreatorId = userNew1.Id,
            //        LastModifierId = 0,
            //        LastModifyTime = DateTime.Now,
            //        Detail = "12345678",
            //        Introduction = "sadsfghj",
            //        LogType = 1,
            //        UserName = "zhangsan2"
            //    };
            //    dbContext.Set<SysLog>().Add(sysLog);
            //    dbContext.SaveChanges();
            //}
            #endregion

            #region MyRegion
            //using (JDDbContext dbContext = new JDDbContext())
            //{
            //    using (TransactionScope trans = new TransactionScope())
            //    {
            //        User userNew1 = new User()
            //        {
            //            Account = "Admin",
            //            State = 0,
            //            CompanyId = 2031,
            //            CompanyName = "软谋",
            //            CreateTime = DateTime.Now,
            //            CreatorId = 1,
            //            Email = "57265177@qq.com",
            //            LastLoginTime = null,
            //            LastModifierId = 0,
            //            LastModifyTime = DateTime.Now,
            //            Mobile = "18664876671",
            //            Name = "民工甲123333333",
            //            Password = "12356789",
            //            UserType = 1
            //        };
            //        dbContext.Set<User>().Add(userNew1);
            //        dbContext.SaveChanges();

            //        SysLog sysLog = new SysLog()
            //        {
            //            CreateTime = DateTime.Now,
            //            CreatorId = userNew1.Id,
            //            LastModifierId = 0,
            //            LastModifyTime = DateTime.Now,
            //            Detail = "12345678",
            //            Introduction = "sadsfghj",
            //            LogType = 1,
            //            UserName = "zhangsanan2zhangsanan2zhangsanan2zhangsanan2zhangsanan2zhangsanan2"
            //        };
            //        dbContext.Set<SysLog>().Add(sysLog);
            //        dbContext.SaveChanges();

            //        trans.Complete();//能执行这个，就表示成功了；
            //    }
            //}
            #endregion


            #region MyRegion
            using (JDDbContext dbContext1 = new JDDbContext())
            using (JDDbContext dbContext2 = new JDDbContext())
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    User userNew1 = new User()
                    {
                        Account = "Admin",
                        State = 0,
                        CompanyId = 2031,
                        CompanyName = "软谋",
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        Email = "57265177@qq.com",
                        LastLoginTime = null,
                        LastModifierId = 0,
                        LastModifyTime = DateTime.Now,
                        Mobile = "18664876671",
                        Name = "民工甲123333333",
                        Password = "12356789",
                        UserType = 1
                    };
                    dbContext1.Set<User>().Add(userNew1);
                    dbContext1.SaveChanges();

                    SysLog sysLog = new SysLog()
                    {
                        CreateTime = DateTime.Now,
                        CreatorId = userNew1.Id,
                        LastModifierId = 0,
                        LastModifyTime = DateTime.Now,
                        Detail = "12345678",
                        Introduction = "sadsfghj",
                        LogType = 1,
                        UserName = "zhangsanan2zhangsanan2zhangsanan2zhangsanan2333333333"
                    };
                    dbContext2.Set<SysLog>().Add(sysLog);
                    dbContext2.SaveChanges();

                    trans.Complete();//能执行这个，就表示成功了；
                }
            }
            #endregion

        }

        /// <summary>
        /// 针对主外键
        /// 
        /// 级联删除：主表数据删除后，子表是删除/不变/改成默认值&null
        /// 级联更新：应该用不上，
        /// </summary>
        public static void ShowDelete()
        {
            //1 数据库设置级联删除  只需要删除主表
            //2 如果没有级联删除--而且lazyload为false--还得数据库允许外键不存在--就可以只删除主表
            //3 其实工作中这种已经很少见了，大部分数据都是假删除的，搞个状态
            int id = 0;
            using (JDDbContext dbContext = new JDDbContext())
            {
                Company company = new Company()
                {
                    Name = "软谋教育高级班12222",
                    CreateTime = DateTime.Now,
                    CreatorId = 1,
                    LastModifierId = 0,
                    LastModifyTime = DateTime.Now,
                };
                User userNew1 = new User()
                {
                    Account = "Admin",
                    State = 0,
                    CompanyId = company.Id,
                    CompanyName = company.Name,
                    CreateTime = DateTime.Now,
                    CreatorId = 1,
                    Email = "57265177@qq.com",
                    LastLoginTime = null,
                    LastModifierId = 0,
                    LastModifyTime = DateTime.Now,
                    Mobile = "18664876671",
                    Name = "民工甲12222",
                    Password = "12356789",
                    UserType = 1
                };
                User userNew2 = new User()
                {
                    Account = "Admin",
                    State = 0,
                    CompanyId = company.Id,
                    CompanyName = company.Name,
                    CreateTime = DateTime.Now,
                    CreatorId = 1,
                    Email = "57265177@qq.com",
                    LastLoginTime = null,
                    LastModifierId = 0,
                    LastModifyTime = DateTime.Now,
                    Mobile = "18664876671",
                    Name = "逐梦12222",
                    Password = "12356789",
                    UserType = 2
                };
                dbContext.Set<User>().Add(userNew1);
                dbContext.Set<User>().Add(userNew2);
                dbContext.Set<Company>().Add(company);
                dbContext.SaveChanges();

                id = company.Id;
            }
            using (JDDbContext dbContext = new JDDbContext())
            {
                dbContext.Configuration.LazyLoadingEnabled = false;
                Company company = dbContext.Companies.Find(id);
                dbContext.Set<Company>().Remove(company);
                dbContext.SaveChanges();//只删除company，没有删除user+没有级联删除，主表删除了，但是子表还在，子表的外键id不存在了。。
            }
        }

    }
}

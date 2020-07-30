using Ruanmou.EF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Project
{
    public class EFQueryTest
    {
        public static void Show()
        {
            #region 其他查询
            using (JDDbContext dbContext = new JDDbContext())
            {
                //dbContext.Database.Log += c => Console.WriteLine($"sql：{c}");
                {
                    var list = dbContext.Users.Where(u => new int[] { 1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 14, 17 }.Contains(u.Id));//in查询
                    foreach (var user in list)
                    {
                        Console.WriteLine(user.Name);
                    }
                }
                {
                    //没有任何差别，只有写法上的熟悉
                    var list = from u in dbContext.Users
                               where new int[] { 1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 14 }.Contains(u.Id)
                               select u;

                    foreach (var user in list)
                    {
                        Console.WriteLine(user.Name);
                    }
                }
                {
                    var list = dbContext.Users.Where(u => new int[] { 1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 14, 18, 19, 20, 21, 22, 23 }.Contains(u.Id))
                                              .OrderBy(u => u.Id)
                                              .Select(u => new
                                              {
                                                  Account = u.Account,
                                                  Pwd = u.Password
                                              }).Skip(3).Take(5);
                    foreach (var user in list)
                    {
                        Console.WriteLine(user.Pwd);
                    }
                }
                {
                    var list = (from u in dbContext.Users
                                where new int[] { 1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 14 }.Contains(u.Id)
                                orderby u.Id
                                select new
                                {
                                    Account = u.Account,
                                    Pwd = u.Password
                                }).Skip(3).Take(5);

                    foreach (var user in list)
                    {
                        Console.WriteLine(user.Account);
                    }
                }

                {
                    var list = dbContext.Users.Where(u => u.Name.StartsWith("小") && u.Name.EndsWith("新"))
                                               .Where(u => u.Name.EndsWith("新"))
                                               .Where(u => u.Name.Contains("小新"))
                                               .Where(u => u.Name.Length < 5)
                                               .OrderBy(u => u.Id);

                    foreach (var user in list)
                    {
                        Console.WriteLine(user.Name);
                    }
                }
                {
                    var list = from u in dbContext.Users
                               join c in dbContext.Companies on u.CompanyId equals c.Id
                               where new int[] { 1, 2, 3, 4, 6, 7, 10 }.Contains(u.Id)
                               select new
                               {
                                   Account = u.Account,
                                   Pwd = u.Password,
                                   CompanyName = c.Name
                               };//).Skip(3).Take(5);
                    foreach (var user in list)
                    {
                        Console.WriteLine("{0} {1}", user.Account, user.Pwd);
                    }
                }
                {
                    var list = from u in dbContext.Users
                               join c in dbContext.Categories on u.CompanyId equals c.Id
                               into ucList
                               from uc in ucList.DefaultIfEmpty()
                               where new int[] { 1, 2, 3, 4, 6, 7, 10 }.Contains(u.Id)
                               select new
                               {
                                   Account = u.Account,
                                   Pwd = u.Password
                               };
                    foreach (var user in list)
                    {
                        Console.WriteLine("{0} {1}", user.Account, user.Pwd);
                    }
                }
            }
            using (JDDbContext dbContext = new JDDbContext())
            {
                {
                    DbContextTransaction trans = null;
                    try
                    {
                        trans = dbContext.Database.BeginTransaction();
                        string sql = "Update [User] Set Name='小新' WHERE Id=@Id";
                        SqlParameter parameter = new SqlParameter("@Id", 1);
                        dbContext.Database.ExecuteSqlCommand(sql, parameter);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                }
                {
                    DbContextTransaction trans = null;
                    try
                    {
                        trans = dbContext.Database.BeginTransaction();
                        string sql = "SELECT * FROM [User] WHERE Id=@Id";
                        SqlParameter parameter = new SqlParameter("@Id", 1);
                        List<User> userList = dbContext.Database.SqlQuery<User>(sql, parameter).ToList<User>();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                }
            }
            #endregion
        }
    }
}

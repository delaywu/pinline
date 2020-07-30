using Microsoft.Practices.Unity.Configuration;
using Ruanmou.Bussiness.Interface;
//using Ruanmou.Bussiness.Service;
using Ruanmou.EF.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Ruanmou.Project
{
    public class IOCTest
    {
        public static void Show()
        {
            {
                //using (JDDbContext dbContext = new JDDbContext())
                //{
                //    //既然分层封装，就不再跨层访问
                //    User user = dbContext.Users.Find(1);
                //}

                //{
                //    //1 通过Service来完成对数据库的访问
                //    IUserService iUserService = new UserService();
                //    iUserService.Add();
                //}
            }

            {
                //2 访问Company  访问Commodity  
                //每个service都提供增删改查基本方法？ 来个父类继承一下
                //接口也继承一下
                //然后把常规API放入接口和实现
            }

            {
                //Base接口的定义解析
                //Join晚点再说
            }

            {
                //DbContext dbContext = new JDDbContext();
                //using (IUserService iUserService = new UserService(dbContext))
                //{
                //    User user = iUserService.Find<User>(123);
                //    //....
                //}
                //using (ICompanyService iCompanyService = new CompanyService(new JDDbContext()))
                //{
                //    Company company = iCompanyService.Find<Company>(123);
                //    //....
                //}
                ////1 每张表都来一个Service?
                ////需要join怎么办？ 还有一个操作删除公司+用户   更新公司+用户  
                ////这是业务逻辑，不应该在上端写的，应该在service
                //using (IUserService iUserService = new UserService(new JDDbContext()))
                //using (ICompanyService iCompanyService = new CompanyService(new JDDbContext()))
                //{
                //    var result = from u in iUserService.Set<User>()
                //                 join c in iCompanyService.Set<Company>() on u.CompanyId equals c.Id
                //                 where u.Id > 100
                //                 select u;//这是错的 不同的上下文

                //}
                ////2 单表单Service很多时候无法满足业务的需求，业务经常是跨表的
                ////所以在封装Service时，需要划分边界，完成组合。一个Service完成一个有机整体的全部操作
                //using (IUserCompanyService iUserCompanyService = new UserCompanyService(new JDDbContext()))
                //{
                //    User user = iUserCompanyService.Find<User>(123);
                //    Company company = iUserCompanyService.Find<Company>(123);

                //    iUserCompanyService.UpdateLastLogin(user);
                //    iUserCompanyService.RemoveCompanyAndUser(company);
                //}
                ////难就难在怎么划分边界？！  根据项目的实际情况，
                ////1 主外键关系一般在一个Service   
                ////2 Mapping式一般也可以放在一起  
                ////3 单表单Service不算错比如日志
                ////频繁互动的  可以考虑合并一下

                ////上端难免还是要多个Service(多个context)共同操作，不要Join！事务UnitOfWork---TranscationScope
                //UnitOfWork.Invoke(() =>
                //{
                //    using (IUserCompanyService iUserCompanyService = new UserCompanyService(new JDDbContext()))
                //    {
                //        //增删改
                //    }
                //});
            }

            {
                //IOC：去掉细节依赖，降低耦合，增强扩展性
                //ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                //fileMap.ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "CfgFiles\\Unity.Config");//找配置文件的路径
                //Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                //UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);
                //IUnityContainer container = new UnityContainer();
                //section.Configure(container, "ruanmouContainer");

                IUnityContainer container = ContainerFactory.GetContainer();
                using (IUserCompanyService iUserCompanyService = container.Resolve<IUserCompanyService>())
                {
                    User user = iUserCompanyService.Find<User>(12);
                    Company company = iUserCompanyService.Find<Company>(23);
                }
            }
        }
    }
}

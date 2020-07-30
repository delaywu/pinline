using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Unity;

namespace Ruanmou.MVC5.Utility
{
    /// <summary>
    /// 保证这个容器只初始化一次
    /// </summary>
    public class DIFactory
    {
        private static IUnityContainer _Container = null;
        private readonly static object DIFactoryLock = new object();
        public static IUnityContainer GetContainer()
        {
            if (_Container == null)
            {
                lock (DIFactoryLock)
                {
                    if (_Container == null)
                    {
                        //container.RegisterType
                        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                        fileMap.ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "CfgFiles\\Unity.Config");
                        Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                        UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);
                        _Container = new UnityContainer();
                        section.Configure(_Container, "ruanmouContainer");
                    }
                }
            }
            return _Container;
        }
    }
}
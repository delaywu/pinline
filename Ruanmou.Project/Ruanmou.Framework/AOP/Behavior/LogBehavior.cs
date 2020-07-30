using Microsoft.Practices.Unity.InterceptionExtension;
using Ruanmou.Framework.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Ruanmou.Framework.AOP.Behavior
{
    /// <summary>
    /// 不需要特性
    /// </summary>
    public class LogBehavior : IInterceptionBehavior
    {
        private Logger logger = Logger.CreateLogger(typeof(LogBehavior));

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            logger.Debug($"LogBehavior {input.MethodBase.Name}");
            return getNext().Invoke(input, getNext);
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}

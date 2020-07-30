using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ruanmou.Project
{
    public class UnitOfWork
    {
        public static void Invoke(Action action)
        {
            TransactionScope transaction = null;
            try
            {
                transaction = new TransactionScope();
                action.Invoke();
                transaction.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}

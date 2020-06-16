using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Data.Infrastructures
{
    public interface IUnitOfWork
    {
        void Commit();
        void Dispose();
        IRepository<T> GetRepository<T>() where T : class;
    }        
}

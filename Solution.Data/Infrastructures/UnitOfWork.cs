using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Data.Infrastructures
{
    public class UnitOfWork: IUnitOfWork
    {
        IDataBaseFactory dBFactory;
        public UnitOfWork(IDataBaseFactory dBf)
        {
            dBFactory = dBf;
        }
        public void Commit()
        {
            dBFactory.Ctxt.SaveChanges();
        }
        public void Dispose()
        {
            dBFactory.Dispose();
        }
        public IRepository<T> GetRepository<T>() where T:class
        {
            return new Repository<T>(dBFactory);
        }
    }
}

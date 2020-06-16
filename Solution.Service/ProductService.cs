using Solution.Data;
using Solution.Data.Infrastructures;
using Solution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Service
{
   public class ProductService : Service<Product> , IProductService
    {
        static MyContext ctxt = new MyContext();
        static IDataBaseFactory dbFactory = new DataBaseFactory(ctxt);
        static IUnitOfWork uow = new UnitOfWork(dbFactory);
        public ProductService() : base(uow)
        {

        }

        
    }
}

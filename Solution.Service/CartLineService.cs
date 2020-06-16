using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solution.Data;
using Solution.Data.Infrastructures;
using Solution.Domain.Entities;
using System.Data.SqlClient;


namespace Solution.Service
{
    public class CartLineService : Service<CartLine>, ICartLineService
    {
        static MyContext ctxt = new MyContext();
        static IDataBaseFactory dbFactory = new DataBaseFactory(ctxt);
        static IUnitOfWork uow = new UnitOfWork(dbFactory);

        public CartLineService() : base(uow)
        {

        }





    }
}
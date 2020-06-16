using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Data.Infrastructures
{
    public class DataBaseFactory: Disposable, IDataBaseFactory
    {
        MyContext ctxt;
    
        public DataBaseFactory()
        {
            ctxt = new MyContext();
        }

        public DataBaseFactory(MyContext context)
        {
            this.ctxt = context;
        }

        public MyContext Ctxt { get { return ctxt; } }
        public override void DisposeCore()
        {
            if (ctxt != null)
                ctxt.Dispose();
        }
    }
}

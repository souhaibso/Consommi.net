using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Data.Infrastructures
{
    public interface IRepository<T> where T:class
    {
        void Add(T obj);
        void Update(T obj);
        void Delete(T obj);
        void Delete(Expression<Func<T, bool>> Condition);
        void Commit();
        T GetById(string id);
        T GetById(int id);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> Condition = null, Expression<Func<T, bool>> orderBy = null);
        IEnumerable<T> GetAll();



        //ghada
        T GetCartByUserId(int userId);
        //ghada
        void DeleteCartLine(int cartLineId);
        //ghada
        void CartConfirmPurchase(int userId);

        //ghada  
        List<T> GetCartLinesByCartId(int cartId);

        //ghada
        List<T> GetAllCartByUserId(int userId);
        //ghada
        List<T> GetAllProduct();
    }
}

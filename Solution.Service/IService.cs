using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Solution.Domain.Entities;

namespace Solution.Service
{
    public interface IService<T> where T : class
    {
        void Add(T obj);
        void Update(T obj);
        void Delete(T obj);
        void Delete(Expression<Func<T, bool>> Condition);
        void Commit();
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
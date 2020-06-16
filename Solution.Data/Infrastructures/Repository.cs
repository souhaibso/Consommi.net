using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Solution.Data;
using Solution.Domain;
using Solution.Domain.Entities;
using System.Data.SqlClient;

namespace Solution.Data.Infrastructures
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private MyContext ctxt;
        private DbSet<T> dbSet;
        public Repository(IDataBaseFactory dbFactory)
        {

            this.ctxt = dbFactory.Ctxt;
            this.dbSet = ctxt.Set<T>();
        }
        public void Add(T obj)
        {
            dbSet.Add(obj);
        }
        public void Update(T obj)
        {
            dbSet.Attach(obj);
            ctxt.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(T obj)
        {
            dbSet.Remove(obj);
        }
        public void Delete(Expression<Func<T, bool>> Condition)
        {
            var result = dbSet.Where(Condition);
            if (result != null)
                foreach (T obj in result)
                    dbSet.Remove(obj);
        }
        public void Commit()
        {
            ctxt.SaveChanges();
        }
        public T GetById(string id)
        {
            return dbSet.Find(id);
        }
        public T GetById(int id)
        {
            return dbSet.Find(id);
        }
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> condition = null, Expression<Func<T, bool>> orderBy = null)
        {
            if (condition != null)
            {
                if (orderBy != null)
                    return dbSet.Where(condition).OrderBy(orderBy);
                return dbSet.Where(condition);
            }
            if (orderBy != null)
                return dbSet.OrderBy(orderBy);
            return dbSet.AsEnumerable();
        }
        public IEnumerable<T> GetAll()
        {
            return dbSet.AsEnumerable();
        }


        //ghada GetCartByUserId
        public T GetCartByUserId(int userId)
        {
            //var cartLine = dbSet.SqlQuery("Select * from Carts where userId=@userId ", new SqlParameter("@userId", userId)).ToList();
            var cartLine = dbSet.SqlQuery("Select * from Carts where userId=@userId and status = 'true'", new SqlParameter("@userId", userId)).FirstOrDefault();
            return cartLine;
        }
        //ghada DeleteCartLine
        public void DeleteCartLine(int cartLineId)
        {
            ctxt.Database.ExecuteSqlCommand("DELETE FROM CartLines WHERE id = @cartLineId", new SqlParameter("@cartLineId", cartLineId));


        }
        //ghada CartConfirmPurchase
        public void CartConfirmPurchase(int userId)
        {
            ctxt.Database.ExecuteSqlCommand("UPDATE Carts SET status = 'false' WHERE userId = @userId", new SqlParameter("@userId", userId));


        }

        //ghada CartLineByCartId
        public List<T> GetCartLinesByCartId(int cartId)
        {
            return dbSet.SqlQuery("Select * from CartLines where CartId=@cartId", new SqlParameter("@cartId", cartId)).ToList<T>();
        }

        //ghada
        public List<T> GetAllCartByUserId(int userId)
        {
            return dbSet.SqlQuery("Select * from Carts where userId=@userId and status = 'false'", new SqlParameter("@userId", userId)).ToList<T>();
        }

        //ghada all product
        public List<T> GetAllProduct()
        {
            return dbSet.SqlQuery("Select * from Products").ToList<T>();
        }

    }
}

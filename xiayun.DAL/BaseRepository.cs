using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xiayun.IDAL;
using System.Linq.Expressions;
using PagedList;
namespace xiayun.DAL
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected MyContext netDiskContext = ContextFactory.GetDbContext() as MyContext;
        public TEntity Add(TEntity entity)
        {


            netDiskContext.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Added;
            return entity;
        }

        public int Count(Expression<Func<TEntity, bool>> where)
        {
            return netDiskContext.Set<TEntity>().Count(where);
        }

        public bool Delete(TEntity entity)
        {
            netDiskContext.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Deleted;
            return true;
        }

        public void Dispose()
        {
            if (netDiskContext != null)
            {
                netDiskContext.Dispose();
                GC.SuppressFinalize(netDiskContext);
            }

        }

        public bool Exist(Expression<Func<TEntity, bool>> where)
        {
            return netDiskContext.Set<TEntity>().Any(where);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> where)
        {

            return netDiskContext.Set<TEntity>().FirstOrDefault(where);
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> where)
        {
            return netDiskContext.Set<TEntity>().Where(where);
        }

        public IQueryable<TEntity> FindAll<SEntity>(Expression<Func<TEntity, bool>> where, bool isAsc, Expression<Func<TEntity, SEntity>> orderlanbda)
        {
            var lst = netDiskContext.Set<TEntity>().Where<TEntity>(where);
            if (!isAsc)
            {
                lst = lst.OrderByDescending<TEntity, SEntity>(orderlanbda);
            }
            return lst;
        }

        public IQueryable<TEntity> FindPaged<SEntity>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<TEntity, bool>> where, bool isAsc, Expression<Func<TEntity, SEntity>> orderLambda)
        {
            var lst = netDiskContext.Set<TEntity>().Where<TEntity>(where);
            totalRecord = lst.Count();
            if (!isAsc)
            {
                lst = lst.OrderByDescending<TEntity, SEntity>(orderLambda);
            }
            return lst.Skip<TEntity>((pageIndex - 1) * pageIndex).Take(pageSize);
        }
        public IPagedList<TEntity> GetPaged(Func<TEntity, bool> where, Func<TEntity, object> order, int pageIndex, int pageSize, bool isasc)
        {
            if (isasc)
                return netDiskContext.Set<TEntity>().Where<TEntity>(where).OrderBy(order).ToPagedList(pageIndex, pageSize);
            else
                return netDiskContext.Set<TEntity>().Where<TEntity>(where).OrderByDescending(order).ToPagedList(pageIndex, pageSize);
        }

        public int SaveChanges()
        {
            return netDiskContext.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            TEntity tentity = netDiskContext.Set<TEntity>().Attach(entity);
            netDiskContext.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Modified;
            return tentity;
        }
    }
}

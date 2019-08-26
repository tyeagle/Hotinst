using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOTINST.COMMON.DB.Infrastructure
{
    /// <summary>  
    ///     单元操作实现  
    /// </summary>  
    public class BaseUnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity:class
    {
        #region 构造析构
        /// <summary>
        /// 使用DbContext和DbSet<Tentity>构造一个UnitOfWorkContext
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbSet">要操作的表对象</param>
        public BaseUnitOfWork(DbContext dBContext)
        {
            this.HasBeenCommitted = true;
            this.Context = dBContext;
            //this.DataSet = dbSet;
        }
        ~BaseUnitOfWork()
        {
            Dispose(false);
        }

        #endregion
        #region 属性

        /// <summary>  
        /// 获取 当前使用的数据访问上下文对象  
        /// </summary>  
        protected DbContext Context { get; set; }
        /// <summary>
        /// 获取当前操作的表对象
        /// </summary>
        //protected DbSet<TEntity> DataSet { get; set; }

        /// <summary>  
        ///     获取 当前单元操作是否已被提交  
        /// </summary>  
        public bool HasBeenCommitted { get; private set; }

        #endregion
        #region IUnitOfWork

        /// <summary>  
        ///     提交当前单元操作的结果  
        /// </summary>  
        /// <returns></returns>  
        public int Commit()
        {
            if (HasBeenCommitted)
            {
                return 0;
            }
            try
            {
                int result = Context.SaveChanges();
                HasBeenCommitted = true;
                return result;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException.InnerException is SqlException)
                {
                    SqlException sqlEx = e.InnerException.InnerException as SqlException;
                }
                throw;
            }
        }

        /// <summary>  
        ///     把当前单元操作回滚成未提交状态  
        /// </summary>  
        public void Rollback()
        {
            HasBeenCommitted = false;
        }

        #endregion
        #region IUnitOfWorkContext<Tentity>
        /// <summary>  
        ///   为指定的类型返回 System.Data.Entity.DbSet，这将允许对上下文中的给定实体执行 CRUD 操作。  
        /// </summary>  
        /// <typeparam name="TEntity"> 应为其返回一个集的实体类型。 </typeparam>  
        /// <returns> 给定实体类型的 System.Data.Entity.DbSet 实例。 </returns>  
        public DbSet<TEntity> Set()
        {
            return Context.Set<TEntity>();
        }

        /// <summary>  
        ///     注册一个新的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        public void RegisterNew(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            EntityState state = Context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
            HasBeenCommitted = false;
        }

        /// <summary>  
        ///     批量注册多个新的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entities"> 要注册的对象集合 </param>  
        public void RegisterNew(IEnumerable<TEntity> entities)
        {
            try
            {
                //Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterNew(entity);
                }
            }
            finally
            {
                //Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>  
        ///     注册一个更改的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        public void RegisterModified(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
            }
            else if (EntityState.Unchanged == Context.Entry(entity).State)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            HasBeenCommitted = false;
        }

        /// <summary>  
        ///   注册一个删除的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        public void RegisterDeleted(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            HasBeenCommitted = false;
        }

        /// <summary>  
        ///   批量注册多个删除的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entities"> 要注册的对象集合 </param>  
        public void RegisterDeleted(IEnumerable<TEntity> entities)
        {
            try
            {
                //Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterDeleted(entity);
                }
            }
            finally
            {
                //Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }


        #endregion
        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!HasBeenCommitted)
            {
                Commit();
            }

            if (disposing)
            {
                
            }
        }


        #endregion
    }
}

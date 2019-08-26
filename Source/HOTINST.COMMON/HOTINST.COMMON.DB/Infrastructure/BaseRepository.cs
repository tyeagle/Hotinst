using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HOTINST.COMMON.DB.Infrastructure;

namespace HOTINST.COMMON.DB.Infrastructure
{
    /// <summary>  
    ///     EntityFramework仓储操作基类  
    /// </summary>  
    /// <typeparam name="TEntity">动态实体类型</typeparam>  
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class 
    {
        #region 属性


        /// <summary>  
        ///     获取或设置 EntityFramework的数据仓储上下文  
        /// </summary>  
        protected IUnitOfWork<TEntity> UnitOfWork { get; set; }
        

        /// <summary>  
        ///     获取 当前实体的查询数据集  
        /// </summary>  
        public virtual IQueryable<TEntity> Entities
        {
            get { return UnitOfWork.Set(); }
        }

        #endregion

        #region 构造函数

        public BaseRepository(IUnitOfWork<TEntity> unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            if (this.UnitOfWork == null)
            {
                throw new ArgumentException($"param(unitOfWork){unitOfWork} is not of type {UnitOfWork}");
            }
        }
        

        #endregion

        #region 公共方法

        /// <summary>
        /// 从数据库查询并返回所有对象
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> All()
        {
            return UnitOfWork.Set().AsQueryable();
        }

        /// <summary>
        /// 使用过滤器从数据库查询并返回符合条件的对象列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return UnitOfWork.Set().Where(predicate);
        }

        /// <summary>
        /// 使用过滤器,分页方式从数据库查询并返回对象
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="total">符合过滤条件的总对象数目</param>
        /// <param name="index">分页序号</param>
        /// <param name="size">每页包含的行数</param>
        /// <returns></returns>
        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var querySet = null ==filter? All(): Filter(filter);
            total = querySet.Count();

            querySet = skipCount == 0 ? querySet.Take(size) : querySet.Skip(skipCount).Take(size);
            
            return querySet.AsQueryable();
        }

        /// <summary>
        /// 指定主键查找对象
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public TEntity Find(params object[] keys)
        {
            return UnitOfWork.Set().Find(keys);
        }

        /// <summary>
        /// 使用判定表达式查询对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return UnitOfWork.Set().FirstOrDefault(predicate);
        }

        /// <summary>  
        ///     插入实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Insert(TEntity entity, bool isSave = false)
        {
            UnitOfWork.RegisterNew(entity);
            return isSave ? UnitOfWork.Commit() : 0;
        }

        /// <summary>  
        ///     批量插入实体记录集合  
        /// </summary>  
        /// <param name="entities"> 实体记录集合 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Insert(IEnumerable<TEntity> entities, bool isSave = false)
        {
            UnitOfWork.RegisterNew(entities);
            return isSave ? UnitOfWork.Commit() : 0;
        }

        /// <summary>  
        ///     删除指定编号的记录  
        /// </summary>  
        /// <param name="id"> 实体记录编号 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Delete(object id, bool isSave = false)
        {
             TEntity entity = UnitOfWork.Set().Find(id);
            return entity != null ? Delete(entity, isSave) : 0;
        }

        /// <summary>  
        ///     删除实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Delete(TEntity entity, bool isSave = false)
        {
            UnitOfWork.RegisterDeleted(entity);
            return isSave ? UnitOfWork.Commit() : 0;
        }

        /// <summary>  
        ///     删除实体记录集合  
        /// </summary>  
        /// <param name="entities"> 实体记录集合 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Delete(IEnumerable<TEntity> entities, bool isSave = false)
        {
            UnitOfWork.RegisterDeleted(entities);
            return isSave ? UnitOfWork.Commit() : 0;
        }

        /// <summary>  
        ///     删除所有符合特定表达式的数据  
        /// </summary>  
        /// <param name="predicate"> 查询条件谓语表达式 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = false)
        {
            IEnumerable<TEntity> entities = UnitOfWork.Set().Where(predicate);
            return entities.Count() > 0 ? Delete(entities, isSave) : 0;
        }

        /// <summary>  
        ///     更新实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Update(TEntity entity, bool isSave = false)
        {
            UnitOfWork.RegisterModified(entity);
            return isSave ? UnitOfWork.Commit() : 0;
        }

        /// <summary>  
        ///     查找指定主键的实体记录  
        /// </summary>  
        /// <param name="key"> 指定主键 </param>  
        /// <returns> 符合编号的记录，不存在返回null </returns>  
        public virtual TEntity GetByKey(object key)
        {
            return UnitOfWork.Set().Find(key);
        }


        public void Save()
        {
            UnitOfWork.Commit();
        }

        #endregion
    }
}

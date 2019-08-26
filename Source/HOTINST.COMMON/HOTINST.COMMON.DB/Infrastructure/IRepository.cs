using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HOTINST.COMMON.DB.Infrastructure
{
    /// <summary>  
    ///     定义仓储模型中的数据标准操作  
    /// </summary>  
    /// <typeparam name="TEntity">动态实体类型</typeparam>  
    public interface IRepository<TEntity> where TEntity : class
    {
        #region 属性

        /// <summary>  
        ///     获取 当前实体的查询数据集  
        /// </summary>  
        IQueryable<TEntity> Entities { get; }

        #endregion

        #region 公共方法
        /// <summary>
        /// 从数据库查询并返回所有对象
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> All();
        /// <summary>
        /// 使用过滤器从数据库查询并返回符合条件的对象列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 使用过滤器,分页方式从数据库查询并返回对象
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="total">符合过滤条件的总对象数目</param>
        /// <param name="index">分页序号</param>
        /// <param name="size">每页包含的行数</param>
        /// <returns></returns>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50);
        /// <summary>
        /// 指定主键查找对象
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        TEntity Find(params object[] keys);
        /// <summary>
        /// 使用判定表达式查询对象
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>  
        ///     插入实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        int Insert(TEntity entity, bool isSave = false);

        /// <summary>  
        ///     批量插入实体记录集合  
        /// </summary>  
        /// <param name="entities"> 实体记录集合 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        int Insert(IEnumerable<TEntity> entities, bool isSave = false);

        /// <summary>  
        ///     删除指定编号的记录  
        /// </summary>  
        /// <param name="id"> 实体记录编号 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        int Delete(object id, bool isSave = false);

        /// <summary>  
        ///     删除实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        int Delete(TEntity entity, bool isSave = false);

        /// <summary>  
        ///     删除实体记录集合  
        /// </summary>  
        /// <param name="entities"> 实体记录集合 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        int Delete(IEnumerable<TEntity> entities, bool isSave = false);

        /// <summary>  
        ///     删除所有符合特定表达式的数据  
        /// </summary>  
        /// <param name="predicate"> 查询条件谓语表达式 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = false);

        /// <summary>  
        ///     更新实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否立即执行保存，默认不立即保存，应该统一由UnitOfWork进行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        int Update(TEntity entity, bool isSave = false);

        /// <summary>  
        ///     查找指定主键的实体记录  
        /// </summary>  
        /// <param name="key"> 指定主键 </param>  
        /// <returns> 符合编号的记录，不存在返回null </returns>  
        TEntity GetByKey(object key);

        void Save();

        #endregion
    }
}

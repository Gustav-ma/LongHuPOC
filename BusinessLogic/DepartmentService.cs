﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using LongHu.DataAccess;
using LongHu.Framework;
using LongHu.Framework.Utility;
using System.Linq.Expressions;
using System;
using My.FrameWork.Utilities;
using My.FrameWork.Utilities.Cache;

namespace LongHu.BusinessLogic
{
    public class DepartmentService
    {
        #region Save method
        /// <summary>
        ///  save Entity Method
        /// </summary>
        /// <param name="svarDepartment"></param>
        public Decimal Add(ObjectModel.Department svarDepartment)
        {
		    svarDepartment.IsActive = "1";
            var rmodel = new ConvertModel();
            var sDepartment = rmodel.ReturnModel<Department, ObjectModel.Department>(svarDepartment);
            var dao = new DepartmentRepository();
            var newItem=dao.Insert(sDepartment);
			return newItem.Id;
        }

        #endregion

        #region Update method
        /// <summary>
        ///  Edit Entity Method
        /// </summary>
        /// <param name="evarDepartment"></param>
        public void Update(ObjectModel.Department evarDepartment)
        {
            var rmodel = new ConvertModel();
            var eDepartment = rmodel.ReturnModel<Department, ObjectModel.Department>(evarDepartment);
            var dao = new DepartmentRepository();
            var dataDepartment = dao.Query(s => s.Id == evarDepartment.Id).FirstOrDefault();
            //eDepartment.CreatedOn =dataDepartment.CreatedOn;
            //eDepartment.CreatedByEmployeeId =dataDepartment.CreatedByEmployeeId;
            //eDepartment.ModifiedByEmployeeId =ConstantManager.GetCurrentUserId();
            //eDepartment.ModifiedOn =DateTime.Now;
    		eDepartment.IsActive =dataDepartment.IsActive;
            dao.Update(eDepartment, c => c.Id == eDepartment.Id);

        }

        #endregion

        #region Remove method
        /// <summary>
        ///  Delete Entity Method
        /// </summary>
        /// <param name="dvarDepartment"></param>    
        public void Remove(ObjectModel.Department dvarDepartment)
        {
            var dao = new DepartmentRepository();
            dao.Delete(s => s.Id == dvarDepartment.Id);
        }

        #endregion

        #region Get Single
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelDepartment"></param>
        /// <returns></returns>
        public ObjectModel.Department GetDepartmentByID(ObjectModel.Department modelDepartment)
        {
            var rmodel = new ConvertModel();
            var dao = new DepartmentRepository();
            var dataDepartment = dao.Query(s => s.Id == modelDepartment.Id).FirstOrDefault();

            return rmodel.ReturnModel<ObjectModel.Department, Department>(dataDepartment);
        }

        #endregion

        #region Get Query
        /// <summary>
        ///  Get Entity List 
        /// </summary>
        /// <returns>List</returns>
        public IList<ObjectModel.Department> FindAll()
        {
            var rmodel = new ConvertModel();
            var list = new List<ObjectModel.Department>();
            var daDepartment = new DepartmentRepository();
            foreach (var vartemp in daDepartment.Query(c => c.IsActive == "1"))
            {
                var omDepartment = rmodel.ReturnModel<ObjectModel.Department, Department>(vartemp);
                list.Add(omDepartment);
            }
            return list;
        }

        #endregion

        #region Get Page Query
        /// <summary>
        ///  Get Entity Page Query
        /// </summary>
        /// <returns>List</returns>
        public IList<ObjectModel.Department> QueryByPage<TKey>(Expression<Func<ObjectModel.Department, bool>> filter, Expression<Func<ObjectModel.Department, TKey>> orderBy, int orderType, int pageSize, int pageIndex, out int recordsCount)
        {
		    var newFilter = ExpressionConverter<Department>.Convert(filter);
            var newOrderBy = ExpressionConverter<Department>.Convert(orderBy);
            var dao = new DepartmentRepository();
            var rmodel = new ConvertModel();
            var list = new List<ObjectModel.Department>();
            var dataList= dao.QueryByPage(newFilter, newOrderBy, orderType, pageSize, pageIndex, out recordsCount);
            if (null == dataList) return null;
            foreach (var vartemp in dataList)
            {
                var omDepartment = rmodel.ReturnModel<ObjectModel.Department, Department>(vartemp);
                list.Add(omDepartment);
            }
            return list;

        }
		private IList<ObjectModel.Department> QueryData(Expression<Func<Department, bool>> filter, Expression<Func<Department, ObjectModel.Department>> selector)
        {
             var newfilter = ExpressionConverter<Department>.Convert(filter);
            var dao = new DepartmentRepository();
         
            var dataList = dao.Query(newfilter, p => new ObjectModel.Department { 
            //write something
            }).ToList();
            return dataList;
        }
        public IList<ObjectModel.Department> Query(Expression<Func<ObjectModel.Department, bool>> filter)
        {
		    var newfilter = ExpressionConverter<Department>.Convert(filter);
            var dao = new DepartmentRepository();
            var rmodel = new ConvertModel();
            var list = new List<ObjectModel.Department>();
            var dataList = dao.Query(newfilter).ToList();
            if (null == dataList) return null;
            foreach (var vartemp in dataList)
            {
                var omDepartment = rmodel.ReturnModel<ObjectModel.Department, Department>(vartemp);
                list.Add(omDepartment);
            }
            return list;
        }

		public IList<ObjectModel.Department> QueryByPage(Expression<Func<ObjectModel.Department, bool>> filter, string orderBy, int pageSize, int pageIndex, out int recordsCount)
        {         
            var newfilter = ExpressionConverter<Department>.Convert(filter);
			var dao = new DepartmentRepository();
            var dataList = dao.QueryByPage(newfilter, orderBy, pageSize, pageIndex, out recordsCount).ToList();           
            if (null == dataList) return null;
            var list = new List<ObjectModel.Department>();
            var rmodel = new ConvertModel();
            foreach (var vartemp in dataList)
            {
                var omDepartment = rmodel.ReturnModel<ObjectModel.Department, Department>(vartemp);
                list.Add(omDepartment);
            }
            return list;

        }
		public IList<ObjectModel.Department> GetDepartmentFromCache()
        {
            var key = "Department_key";
			var query=CacheHelper.Get(key) as IList<ObjectModel.Department>;
            if (null == query)
            {
                query = this.Query(q => q.IsActive=="1");
				DateTime ExpirationTime = DateTime.Now.AddMinutes(ConstantManager.CacheCurrentUserExpirationTime);
                CacheHelper.Insert(key, query, ExpirationTime);
            }
            return query;
        }
		public ObjectModel.Department GetSingleOrDefault(Expression<Func<ObjectModel.Department, bool>> filter, string orderBy)
        {
            var newfilter = ExpressionConverter<Department>.Convert(filter);
            var dao = new DepartmentRepository();
            int recordCount = 0;
            var data = dao.QueryByPage(newfilter, orderBy, 1, 1, out recordCount).ToList().FirstOrDefault();
            if (null == data) return null;
            var list = new List<ObjectModel.Department>();
            var rmodel = new ConvertModel();
            return rmodel.ReturnModel<ObjectModel.Department, Department>(data);

        }
        #endregion
    }
}




﻿using Ruanmou.Bussiness.Interface;
using Ruanmou.EF.Model;
using Ruanmou.Framework.Caching;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Bussiness.Service
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(DbContext context)
            : base(context)
        {
        }

        public void Show()
        {
            Console.WriteLine("123456789");
            throw new Exception("This is CategoryService Exception");
            //Console.WriteLine("123456789");
        }

        //public void Add(Category category)
        //{
        //    base.Add<Category>(category);
        //}
        //.....

        public override void Dispose()
        {
            Console.WriteLine("其他对象的释放");
            base.Dispose();
        }

        #region Query
        /// <summary>
        /// 用code获取当前类及其全部子孙类别的id
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<int> GetDescendantsIdList(string code)
        {
            return this.CacheAllCategory().Where(c => c.Code.StartsWith(code)).Select(c => c.Id);
        }

        /// <summary>
        /// 根据类别编码找子类别集合  找一级类用默认code  root
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<Category> GetChildList(string code = "root")
        {
            return this.CacheAllCategory().Where(c => c.ParentCode.StartsWith(code));
        }

        /// <summary>
        /// 查询并缓存全部数据
        /// 在服务层，任何数据操作都是要经过服务层，
        /// 所有如果有数据更新 可以清除缓存
        /// </summary>
        /// <returns></returns>
        public List<Category> CacheAllCategory()
        {
            return CacheManager.Get<List<Category>>("AllCategory",
                () => base.Set<Category>().ToList());
        }
        #endregion Query
    }
}

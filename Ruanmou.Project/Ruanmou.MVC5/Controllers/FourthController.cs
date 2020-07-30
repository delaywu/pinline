using PagedList;
using Ruanmou.Bussiness.Interface;
using Ruanmou.EF.Model;
using Ruanmou.Framework.ExtendExpression;
using Ruanmou.Framework.Models;
using Ruanmou.MVC5.Utility;
using Ruanmou.MVC5.Utility.Filter;
using Ruanmou.Remote.Interface;
using Ruanmou.Remote.Model;
using Ruanmou.Web.Core.Extension;
using Ruanmou.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Unity;

namespace Ruanmou.MVC5.Controllers
{
    /// <summary>
    /// 1 列表绑定、增删改查
    /// 2 WCF搜索引擎的封装调用和AOP的整合
    /// 3 Ajax删除、Ajax表单提交、Ajax列表、Ajax三级联动
    /// 4 Http请求的本质，各种ActionResult扩展订制
    /// 
    /// 怎么样完成一个功能：
    /// a Bussiness增加接口+实现
    /// b IOC配置文件
    /// c 注入到控制器
    /// d 查询数据库，传递到前端，绑定一下
    /// e 接受参数,拼装参数----
    /// f 参数ViewBag传递到前端再绑定
    /// g 分页使用pagedlist---返回数据用StaticPagedList--前端分页url带上参数--接受分页参数
    /// h 分页点击后重置页码数，就是设置表单的action
    /// 
    /// 接口服务查询，建议封装一下；
    /// 建议跟数据库查询独立分开；
    /// 也是接口+实现+model,然后就IOC
    /// 应用程序的配置文件需要加上服务相关
    /// 
    /// Ruanmou.Framework：通用的帮助类库，这里面放的是任何一个项目都可能用上的，这个类库可以被任何类库引用，但是自身不引用任何类库；只要是我用的东西，都得写在我自己里面；如果必须用到别的类库的东西，可以通过委托传递进来；
    /// Ruanmou.Web.Core：专门为MVC网站服务的通用的帮助
    /// 
    /// 1 增删改一览，Ajax删除、Ajax表单提交、Ajax列表、Ajax三级联动
    /// 2 HttpGet HttpPost Bind ChildActionOnly特性解读
    /// 3 Http请求的本质，各种ActionResult扩展订制
    /// 4 用户登录/退出功能实现
    /// 
    /// Create时，会有两次请求，地址都是一个，也就是Action相同，
    /// 一个HttpGet  一次HttpPost
    /// MVC怎么识别呢？不能依赖于参数识别(参数来源太多不稳定)
    /// 必须通过HttpVerbs来识别，
    /// 如果没有标记，那么就用方法名称来识别
    /// [ChildActionOnly] 用来指定该Action不能被单独请求，只能是子请求
    /// [Bind]指定只从前端接收哪些字段，其他的不要，防止数据的额外提交
    /// [ValidateAntiForgeryToken] 防重复提交，在cookie里加上一个key，提交的时候先校验这个
    /// 。。。filter特性
    /// MVC支持了非常多的特性，靠的全部是反射，就能额外的去识别特性，去做点有意义的事儿
    /// 
    /// Ajax请求数据响应格式：
    /// 一个项目组必须是统一的，前端才知道怎么应付
    /// 还有很多其他情况，比如异常了--exceptionfilter--按照固定格式返回
    ///                   比如没有权限--authorization--按照固定格式返回
    ///                   
    /// Http请求的本质：
    /// 请求--应答式，响应可以那么丰富？
    /// 不同的类型其实方式一样的，只不过有个contenttype的差别
    /// html---text/html
    /// json---application/json
    /// xml---application/xml
    /// js----application/javascript
    /// ico----image/x-icon
    /// image/gif   image/jpeg   image/png
    /// 
    /// 这个等于是Http协议的约定，Web框架和浏览器共同支持的
    /// 其实是服务器告诉浏览器如何处理这个数据
    /// 从页面下载pdf  或者页面展示pdf 靠的就是contenttype
    /// application/pdf     application/octet-stream
    /// 
    /// MVC各种Result的事儿
    /// Json方法实际上是new JsonResult 然后ExecuteResult
    /// 指定ContentType-application/json  然后将Data序列化成字符串写入stream
    /// 我们可以随意扩展的，只需要把数据放入response  指定好contenttype
    /// 
    /// 到底该怎么样看源码？
    /// 自上而下，先看流程，再看细节，剥洋葱式，一层层深入
    /// 先看流程--找入口--只找节点--不深入细节(看名字猜意思)--搞定过程
    /// 深入一个点的时候也不钻牛角尖--还是一层层的看---
    /// 熟知常规套路----看懂命名----了解常规的设计模式
    /// 耐心
    /// </summary>
    public class FourthController : Controller
    {
        #region Identity
        private ICommodityService _iCommodityService = null;
        private ICategoryService _iCategoryService = null;
        private ISearchService _iSearchService = null;
        public FourthController(ICommodityService commodityService, ISearchService searchService, ICategoryService categoryService)
        {
            this._iCommodityService = commodityService;
            this._iSearchService = searchService;
            this._iCategoryService = categoryService;
            //ISearchService searchService2 = DIFactory.GetContainer().Resolve<ISearchService>("update");
        }
        private int pageSize = 20;
        //private int pageIndex = 1;
        #endregion

        #region Index普通查询
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ViewResult Index(string searchString, string url, int? pageIndex)
        {
            #region 查询条件
            //searchString = base.HttpContext.Request.Form["searchString"];
            searchString = base.HttpContext.Request["searchString"];
            Expression<Func<JDCommodity, bool>> funcWhere = null;
            //可能多个条件，可能有可能没有  
            //A   4种可能再拼装  都不为空-A为空-B为空-都为空   不可取，条件多了没法做
            //B  就把dbset放出来,没有性能问题，但是有隐患
            //if (!string.IsNullOrWhiteSpace(searchString))
            //{
            //    var list = this._iCommodityService.Set<JDCommodity>().Where(c => c.Title.Contains(searchString));
            //}
            //if (!string.IsNullOrWhiteSpace(url))
            //{
            //    list = this._iCommodityService.Set<JDCommodity>().Where(c => c.Url.Contains(url));
            //}
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                funcWhere = funcWhere.And(c => c.Title.Contains(searchString));
                base.ViewBag.SearchString = searchString;
            }
            if (!string.IsNullOrWhiteSpace(url))
            {
                funcWhere = funcWhere.And(c => c.Url.Contains(url));
                base.ViewBag.Url = url;
            }
            //这里还可以延伸一下，做一个动态封装查询条件--
            //var commodityList = this._iCommodityService.Query<JDCommodity>(funcWhere);
            #endregion

            #region 排序
            Expression<Func<JDCommodity, int>> funcOrderby = c => c.Id;
            #endregion

            int index = pageIndex ?? 1;

            PageResult<JDCommodity> commodityList = this._iCommodityService.QueryPage(funcWhere, pageSize, index, funcOrderby, true);

            StaticPagedList<JDCommodity> pageList = new StaticPagedList<JDCommodity>(commodityList.DataList, index, pageSize, commodityList.TotalCount);

            return View(pageList);
        }
        #endregion

        #region Lucene查询
        public ActionResult SearchIndex()
        {
            //PageResult<CommodityModel> result = this._iSearchService.QueryCommodityPage(1, 20, "女人", null, "", "");
            //ISearchService searchService1 = DIFactory.GetContainer().Resolve<ISearchService>();
            //ISearchService searchService2 = DIFactory.GetContainer().Resolve<ISearchService>("update");
            //return View(result);

            //三级列表
            ViewBag.FirstCategory = BuildCategoryList(this._iCategoryService.CacheAllCategory().Where(c => c.CategoryLevel == 1));
            ViewBag.SecondCategory = BuildCategoryList(null);
            ViewBag.ThirdCategory = BuildCategoryList(null);
            return View();
        }
        /// <summary>
        /// 列表页：局部页的方式
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="pageIndex"></param>
        /// <param name="firstCategory"></param>
        /// <param name="secondCategory"></param>
        /// <param name="thirdCategory"></param>
        /// <returns></returns>
        //[ChildActionOnly]//不能单独请求
        public ActionResult SearchPartialList(string searchString, int pageIndex = 1, int firstCategory = -1, int secondCategory = -1, int thirdCategory = -1)
        {
            int id = thirdCategory == -1 ?
                        secondCategory == -1 ?
                   firstCategory == -1 ? -1 : firstCategory : secondCategory : thirdCategory;
            if (id == -1 && string.IsNullOrWhiteSpace(searchString))
            {
                searchString = "男装";
            }
            List<int> categoryIdList = null;
            if (id != -1)
            {
                Category category = this._iCategoryService.CacheAllCategory().FirstOrDefault(c => c.Id == id);
                if (category != null)
                    categoryIdList = this._iCategoryService.CacheAllCategory().Where(c => (c.ParentCode.StartsWith(category.Code) || c.Id == category.Id)).Select(c => c.Id).ToList();
            }

            PageResult<CommodityModel> remoteCommodityList = this._iSearchService.QueryCommodityPage(pageIndex, pageSize, searchString, categoryIdList, null, null);
            StaticPagedList<CommodityModel> pageList = new StaticPagedList<CommodityModel>(remoteCommodityList.DataList, pageIndex, pageSize, remoteCommodityList.TotalCount);
            return View(pageList);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.categoryList = BuildCategoryList();
            return View();
        }
        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Post)] //[HttpPost]
        //[HttpPost]
        //[HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId, CategoryId, Title, Price, Url, ImageUrl")]JDCommodity commodity)
        {
            string title1 = this.HttpContext.Request.Params["title"];
            string title2 = this.HttpContext.Request.QueryString["title"];
            string title3 = this.HttpContext.Request.Form["title"];
            if (ModelState.IsValid)//数据校验
            {
                JDCommodity newCommodity = this._iCommodityService.Insert(commodity);
                return RedirectToAction("Index");
            }
            else
            {
                throw new Exception("ModelState未通过检测");
            }
        }

        [HttpPost]
        public ActionResult AjaxCreate([Bind(Include = "ProductId, CategoryId, Title, Price, Url, ImageUrl")]JDCommodity commodity)
        {
            JDCommodity newCommodity = this._iCommodityService.Insert(commodity);
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Success,
                PromptMsg = "插入成功"
            };
            return Json(ajaxResult);
        }
        #endregion Create

        #region Details Edit Delete
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("need commodity id");
            }
            JDCommodity commodity = this._iCommodityService.Find<JDCommodity>(id ?? -1);
            if (commodity == null)
            {
                throw new Exception("Not Found Commodity");
            }
            return View(commodity);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("need commodity id");
            }
            JDCommodity commodity = this._iCommodityService.Find<JDCommodity>(id ?? -1);
            if (commodity == null)
            {
                throw new Exception("Not Found");
            }
            ViewBag.categoryList = BuildCategoryList();
            return View(commodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, CategoryId, Title, Price,Url,ImageUrl")] JDCommodity commodity)
        {
            if (ModelState.IsValid)
            {
                JDCommodity commodityDB = this._iCommodityService.Find<JDCommodity>(commodity.Id);
                commodityDB.ProductId = commodity.ProductId;
                commodityDB.CategoryId = commodity.CategoryId;
                commodityDB.Title = commodity.Title;
                commodityDB.Price = commodity.Price;
                commodityDB.Url = commodity.Url;
                commodityDB.ImageUrl = commodity.ImageUrl;
                this._iCommodityService.Update(commodityDB);
                return RedirectToAction("Index");
            }
            else
                throw new Exception("ModelState未通过检测");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Not Found");
            }
            JDCommodity commodity = this._iCommodityService.Find<JDCommodity>(id ?? -1);
            if (commodity == null)
            {
                throw new Exception("Not Found");
            }
            else
            {
                this._iCommodityService.Delete(commodity);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// ajax删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxDelete(int id)
        {
            this._iCommodityService.Delete<JDCommodity>(id);
            AjaxResult ajaxResult = new AjaxResult()
            {
                Result = DoResult.Success,
                PromptMsg = "删除成功"
            };
            return Json(ajaxResult);
        }
        #endregion Details Edit Delete

        #region ajax请求
        /// <summary>
        /// 响应下拉框动作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CategoryDropdown(int id = -1)
        {
            Category category = this._iCategoryService.CacheAllCategory().FirstOrDefault(c => c.Id == id);
            AjaxResult ajaxResult = new AjaxResult();
            if (category != null)
            {
                var categoryList = this._iCategoryService.CacheAllCategory().Where(c => c.ParentCode.Equals(category.Code));
                ajaxResult.RetValue = BuildCategoryList(categoryList);
                ajaxResult.Result = DoResult.Success;
            }
            else
            {
                ajaxResult.Result = DoResult.Failed;
                ajaxResult.PromptMsg = "类型查询失败";
            }
            return Json(ajaxResult);// JsonHelper.ToJson<AjaxResult>(ajaxResult);
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// 拼装下拉框
        /// </summary>
        /// <param name="categoryList"></param>
        /// <returns></returns>
        private IEnumerable<SelectListItem> BuildCategoryList(IEnumerable<Category> categoryList)
        {
            List<SelectListItem> selectList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Selected=true,
                    Text="--请选择--",
                    Value="-1"
                }
            };
            if (categoryList != null && categoryList.Count() > 0)
            {
                selectList.AddRange(categoryList.Select(c => new SelectListItem()
                {
                    Selected = false,
                    Text = c.Name,
                    Value = c.Id.ToString()
                }));
            }
            return selectList;
        }

        private IEnumerable<SelectListItem> BuildCategoryList()
        {
            var categoryList = this._iCategoryService.GetChildList();
            if (categoryList.Count() > 0)
            {
                return categoryList.Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = string.Format("{0}_{1}", c.Id, c.Code)
                });
            }
            else return null;
        }
        #endregion PrivateMethod

        #region TestResult
        /// <summary>
        /// 返回ActionResult--MVC框架会执行其ExecuteResult方法---
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonResultIn()
        {
            return Json(new AjaxResult()
            {
                Result = DoResult.Success,
                DebugMessage = "这里是JsonResult"
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回ActionResult--MVC框架会执行其ExecuteResult方法---
        /// </summary>
        /// <returns></returns>
        public NewtonJsonResult JsonResultNewton()
        {
            return new NewtonJsonResult(new AjaxResult()
            {
                Result = DoResult.Success,
                DebugMessage = "这里是JsonResult"
            });
        }

        /// <summary>
        /// 不是ActionResult---直接当结果写入stream，默认的contenttype是html
        /// </summary>
        /// <returns></returns>
        [CustomActionFilterAttribute]
        public string JsonResultString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new AjaxResult()
            {
                Result = DoResult.Success,
                DebugMessage = "这里是JsonResult"
            });
        }
        /// <summary>
        /// 没有返回--直接写入stream
        /// </summary>
        [CustomActionFilterAttribute]
        public void JsonResultVoid()
        {
            HttpResponseBase response = base.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(
                Newtonsoft.Json.JsonConvert.SerializeObject(new AjaxResult()
                {
                    Result = DoResult.Success,
                    DebugMessage = "这里是JsonResult"
                }));
        }

        public XmlResult XmlResult()
        {
            return new XmlResult(new AjaxResult()
            {
                Result = DoResult.Success,
                DebugMessage = "这里是JsonResult"
            });
        }
        //ExcelResult--NPOI---
        #endregion
    }
}

public class NewtonJsonResult : ActionResult
{
    private object Data = null;
    public NewtonJsonResult(object data)
    {
        this.Data = data;
    }
    public override void ExecuteResult(ControllerContext context)
    {
        HttpResponseBase response = context.HttpContext.Response;
        response.ContentType = "application/json";
        response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(this.Data));
    }
}

//public class XmlResult : ActionResult
//{
//    private object Data = null;
//    public XmlResult(object data)
//    {
//        this.Data = data;
//    }
//    public override void ExecuteResult(ControllerContext context)
//    {
//        HttpResponseBase response = context.HttpContext.Response;
//        response.ContentType = "application/json";
//        response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(this.Data));
//    }
//}


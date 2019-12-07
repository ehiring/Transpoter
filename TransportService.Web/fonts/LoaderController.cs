using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TransportService.Web.Models;
using System.Data.SqlClient;
using TransportService.Web.BusinessLayer;

namespace TransportService.Web.Controllers
{
    public class LoaderController : Controller
    {
        TripBusinessLayer _tripBusinessLayer;
        public LoaderController()
        {
            _tripBusinessLayer = new TripBusinessLayer();
        }
        #region "Old Code"

        
        // GET: Loader
        public ActionResult Old_Index(int? page)
        {
            Transpoter data = new Transpoter();

            
            StaticPagedList<Transpoter> itemAsIPagedList;
            itemAsIPagedList = Old_GridLoadList(page, "", "");
            data.TransDetails = itemAsIPagedList;


            JobDbContext _db = new JobDbContext();
            IEnumerable<subtripDetails> result1 = _db.DBsubtripDetails.SqlQuery(@"exec GetSubTripDetails").ToList<subtripDetails>();
            data.SubTDetails = result1;


            IEnumerable<RouteDetails> result2 = _db.DBRouteDetails.SqlQuery(@"exec GetTripRouteDetails").ToList<RouteDetails>();
            data.RouteList = result2;

            ViewData["CityList"] = Old_binddropdown("CityList", 0);

            // return View();
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Index", data) : View("Index", data);
        }
        public StaticPagedList<Transpoter> Old_GridLoadList(int? page, string Source = "", string Destination = "")
        {
            JobDbContext _db = new JobDbContext();
            var pageIndex = (page ?? 1);
            const int pageSize = 4;
            int totalcount = 4;
            Transpoter Llist = new Transpoter();
            IEnumerable<Transpoter> result = _db.DBTransporter.SqlQuery(@"exec USP_GetTripDetails @pPageIndex, @pPageSize,@Source,@Destination",
                                                                        new SqlParameter("@pPageIndex", pageIndex),
                                                                        new SqlParameter("@pPageSize", pageSize),
                                                                        new SqlParameter("@Source", Source == null ? (object)DBNull.Value : Source),
                                                                        new SqlParameter("@Destination", Destination == null ? (object)DBNull.Value : Destination)).ToList<Transpoter>();
            totalcount = 0;
            if (result.Count() > 0)
            {
                totalcount = Convert.ToInt32(result.FirstOrDefault().TotalRows);

            }

            var itemAsIPagedList = new StaticPagedList<Transpoter>(result, pageIndex, pageSize, totalcount);
            return itemAsIPagedList;
        }
        public List<SelectListItem> Old_binddropdown(string action, int val = 0, int TripId = 0)
        {
            JobDbContext _db = new JobDbContext();

            var res = _db.Database.SqlQuery<SelectListItem>("exec USP_BindDropDown @action , @val, @TripId",
                    new SqlParameter("@action", action),
                    new SqlParameter("@val", val),
                    new SqlParameter("@TripId", TripId))
                   .ToList()
                   .AsEnumerable()
                   .Select(r => new SelectListItem
                   {
                       Text = r.Text.ToString(),
                       Value = r.Value.ToString(),
                       Selected = r.Value.Equals(Convert.ToString(val))
                   }).ToList();

            return res;
        }
        #endregion

        #region "New Code"
        public ActionResult Index(int? page)
        {
            Transpoter _transpoter = new Transpoter();
            

            _transpoter.TransDetails = _tripBusinessLayer.GetTripDetails(page, "", "");
            _transpoter.SubTDetails = _tripBusinessLayer.GetSubTripDetails();
            _transpoter.RouteList = _tripBusinessLayer.GetTripRouteDetails();

            ViewData["CityList"] = _tripBusinessLayer.GetDropDownData("CityList",0);

            // return View();
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Index", _transpoter) : View("Index", _transpoter);
        }

        public ActionResult AddTrip()
        {

            ViewData["CityList"] = _tripBusinessLayer.GetDropDownData("CityList", 0);
            ViewData["VehicalTypeList"] = _tripBusinessLayer.GetDropDownData("VehicalTypeList", 0);
            return View();
        }
        #endregion
    }
}
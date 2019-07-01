﻿using GridMvc.Demo.Models;
using GridMvc.Pagination;
using GridMvc.Server;
using GridShared;
using GridShared.Filtering;
using GridShared.Sorting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GridMvc.Demo.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly NorthwindDbContext _context;

        public HomeController(NorthwindDbContext context, ICompositeViewEngine compositeViewEngine) : base(compositeViewEngine)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            ViewBag.ActiveMenuTitle = "Demo";

            Action<IGridColumnCollection<Order>> columns = c =>
            {
                /* Adding not mapped column, that renders body, using inline Razor html helper */
                c.Add()
                    .Encoded(false)
                    .Sanitized(false)
                    .SetWidth(30)
                    .Css("hidden-xs") //hide on phones
                    .RenderValueAs(o => $"<b><a class='modal_link' href='/Home/Edit/{o.OrderID}'>Edit</a></b>");

                /* Adding "OrderID" column: */

                c.Add(o => o.OrderID)
                    .Titled("Number")
                    .SetWidth(100);

                /* Adding "OrderDate" column: */
                c.Add(o => o.OrderDate, "OrderCustomDate")
                    .Titled("Date")
                    .SortInitialDirection(GridSortDirection.Descending)
                    .SetCellCssClassesContraint(o => o.OrderDate.HasValue && o.OrderDate.Value >= DateTime.Parse("1997-01-01") ? "red" : "")
                    .Format("{0:yyyy-MM-dd}")
                    .SetWidth(110)
                    .Max(true).Min(true);

                /* Adding "CompanyName" column: */
                c.Add(o => o.Customer.CompanyName)
                    .Titled("Company")
                    .SetWidth(250)
                    .ThenSortByDescending(o => o.OrderID)
                    .SetInitialFilter(GridFilterType.StartsWith, "a")
                    .SetFilterWidgetType("CustomCompanyNameFilterWidget")
                    .Max(true).Min(true);

                /* Adding "ContactName" column: */
                c.Add(o => o.Customer.ContactName).Titled("ContactName").SetWidth(250)
                    .Max(true).Min(true);

                /* Adding "Freight" column: */
                c.Add(o => o.Freight)
                    .Titled("Freight")
                    .SetWidth(100)
                    .Format("{0:F}")
                    .Sum(true).Average(true).Max(true).Min(true);

                /* Adding "Vip customer" column: */
                c.Add(o => o.Customer.IsVip)
                    .Titled("Is Vip")
                    .SetWidth(70)
                    .Css("hidden-xs") //hide on phones
                    .RenderValueAs(o => o.Customer.IsVip ? "Yes" : "No");
            };
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var locale = requestCulture.RequestCulture.UICulture.TwoLetterISOLanguageName;

            var repository = new OrdersRepository(_context);
            var server = new GridServer<Order>(repository.GetAll(), Request.Query, false, "ordersGrid", 
                columns, 10, locale)
                .SetRowCssClasses(item => item.Customer.IsVip ? "success" : string.Empty)
                .Sortable()
                .Filterable()
                .WithMultipleFilters()
                .Searchable(true, false)
                .WithGridItemsCount();

            return View(server.Grid);
        }

        public ActionResult About()
        {
            ViewBag.ActiveMenuTitle = "About";
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetOrder(int id)
        {
            var repository = new OrdersRepository(_context);
            Order order = repository.GetById(id);
            if (order == null)
                return Json(new { status = 0, message = "Not found" });

            string content = await RenderAsync("_OrderInfo", order);
            return Json(new { status = 1, message = "Ok", content });
        }

        [HttpGet]
        public ActionResult GetCustomersNames()
        {
            var repository = new CustomersRepository(_context);
            var allItems = repository.GetAll().Select(c => c.CompanyName);
            return Json(new { items = allItems });
        }

        [HttpGet]
        public ActionResult AjaxPaging()
        {
            var repository = new OrdersRepository(_context);
            var model = new SGrid<Order>(repository.GetAll(), Request.Query, false, GridPager.DefaultAjaxPagerViewName);

            ViewBag.ActiveMenuTitle = "AjaxPaging";
            return View(model);
        }

        [HttpPost]
        public ActionResult GetOrdersGridRows()
        {
            var repository = new OrdersRepository(_context);
            var model = new SGrid<Order>(repository.GetAll(), Request.Query, false, GridPager.DefaultAjaxPagerViewName);

            return PartialView("_OrdersGrid", model);
        }

        [HttpPost]
        public ActionResult GetOrderDetailsGrid(int OrderId)
        {
            Action<IGridColumnCollection<OrderDetail>> columns = c =>
            {
                /* Adding "OrderID" column: */
                c.Add(o => o.OrderID)
                    .Titled("Order Number")
                    .SortInitialDirection(GridSortDirection.Descending)
                    .SetWidth(100);

                /* Adding "ProductID" column: */
                c.Add(o => o.ProductID)
                    .Titled("Product Number")
                    .ThenSortByDescending(o => o.ProductID)
                    .SetWidth(100);

                /* Adding "ProductName" column: */
                c.Add(o => o.Product.ProductName)
                    .Titled("Product Name")
                    .SetWidth(250);

                /* Adding "Quantity" column: */
                c.Add(o => o.Quantity)
                    .Titled("Quantity")
                    .SetCellCssClassesContraint(o => o.Quantity >= 50 ? "red" : "")
                    .SetWidth(100)
                    .Format("{0:F}");

                /* Adding "UnitPrice" column: */
                c.Add(o => o.UnitPrice)
                    .Titled("Unit Price")
                    .SetWidth(100)
                    .Format("{0:F}");
            };

            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var locale = requestCulture.RequestCulture.UICulture.TwoLetterISOLanguageName;
            var orderDetails = (new OrderDetailsRepository(_context)).GetForOrder(OrderId);

            var server = new GridServer<OrderDetail>(orderDetails, Request.Query,
                    false, "orderDetailsGrid" + OrderId.ToString(), columns, 10, locale)
                        .SetRowCssClasses(item => item.Quantity > 10 ? "success" : string.Empty)
                        .Sortable()
                        .Filterable()
                        .WithMultipleFilters()
                        .WithGridItemsCount();

            return PartialView("_SubGrid", server.Grid);
        }

        [HttpGet]
        public ActionResult AjaxPagingAntiForgery()
        {
            ViewBag.ActiveMenuTitle = "AjaxPagingAntiForgery";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetOrdersGridRowsAntiForgery()
        {
            return ViewComponent("AjaxGrid");
        }

        //
        // POST /Account/SetLanguage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            try
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }
            catch (Exception)
            {
            }
            return LocalRedirect(returnUrl);
        }

    }
}
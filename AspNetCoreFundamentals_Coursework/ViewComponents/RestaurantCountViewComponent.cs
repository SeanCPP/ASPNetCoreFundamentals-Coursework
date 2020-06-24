using Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreFundamentals_Coursework.ViewComponents
{
    public class RestaurantCountViewComponent
        : ViewComponent
    {
        private readonly IRestaurantData data;

        public RestaurantCountViewComponent(IRestaurantData data)
        {
            this.data = data;
        }
        public IViewComponentResult Invoke()
        {
            var count = data.Count();
            return View(count);
        }
    }
}

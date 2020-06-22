using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreFundamentals_Coursework.Pages.Restaurants
{
    public class DetailModel : PageModel
    {
        public Restaurant Restaurant { get; set; }

        private readonly IRestaurantData _data;
        public DetailModel(IRestaurantData data)
        {
            _data = data;
        }
        public IActionResult OnGet(int restaurantId)
        {
            Restaurant = _data.GetById(restaurantId);
            if(Restaurant is null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}
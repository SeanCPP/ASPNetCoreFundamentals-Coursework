using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreFundamentals_Coursework.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData data;
        private readonly IHtmlHelper helper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(IRestaurantData data, IHtmlHelper helper)
        {
            this.data = data;
            this.helper = helper;
        }

        
        public IActionResult OnGet(int? restaurantId)
        {
            Cuisines = helper.GetEnumSelectList<CuisineType>();
            if (restaurantId.HasValue)
            {
                Restaurant = data.GetById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();
            }
            if(Restaurant is null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Cuisines = helper.GetEnumSelectList<CuisineType>();
                return Page();
            }
            if (Restaurant.Id > 0)
            {
                data.Update(Restaurant);
            }
            else
            {
                data.Create(Restaurant);
            }
            data.Commit();
            TempData["Message"] = "Restaurant saved!";
            return RedirectToPage("./Detail/", new { restaurantId = Restaurant.Id,  });
        }
    }
}
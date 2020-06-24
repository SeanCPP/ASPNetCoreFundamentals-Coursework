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
    public class DeleteModel : PageModel
    {
        private readonly IRestaurantData data;

        public Restaurant Restaurant { get; set; }
        public DeleteModel(IRestaurantData data)
        {
            this.data = data;
        }
        public IActionResult OnGet(int restaurantId)
        {
            Restaurant = data.GetById(restaurantId);
            if(Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
        public IActionResult OnPost(int restaurantId)
        {
            var restaurant = data.Delete(restaurantId);
            data.Commit();
            if(restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            TempData["Message"] = $"{restaurant.Name} has been deleted.";
            return RedirectToPage("./List");
        }
    }
}
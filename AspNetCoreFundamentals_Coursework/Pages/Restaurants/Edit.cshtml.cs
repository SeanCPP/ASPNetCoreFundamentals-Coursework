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

        
        public IActionResult OnGet(int restaurantId)
        {
            Cuisines = helper.GetEnumSelectList<CuisineType>();
            Restaurant = data.GetById(restaurantId);
            if(Restaurant is null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        } 

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                data.Update(Restaurant);
                data.Commit();
                return RedirectToPage("./List");
            }
            Cuisines = helper.GetEnumSelectList<CuisineType>();
            return Page();
        }
    }
}
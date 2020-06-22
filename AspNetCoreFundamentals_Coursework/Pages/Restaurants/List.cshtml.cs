using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreFundamentals_Coursework.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IRestaurantData _data;
        private readonly IConfiguration config;

        public IEnumerable<Restaurant> Restaurants { get; set; }
        public ListModel(IConfiguration configuration, IRestaurantData data)
        {
            config = configuration;
            _data = data;
        }
        public void OnGet()
        {
            Restaurants = _data.GetAll();
        }
    }
}
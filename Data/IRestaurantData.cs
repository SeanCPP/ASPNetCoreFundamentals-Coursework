using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetByName(string name=null);
    }
}


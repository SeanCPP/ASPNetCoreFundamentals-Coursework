using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetByName(string name=null);
        Restaurant GetById(int id);
        Restaurant Update(Restaurant itemToUpdate);
        Restaurant Create(Restaurant newItem);
        int Commit();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface ICityRepositories
    {
        List<City> FetchAllcity();

        List<State> GetAllstate();
        
        bool AddCity(City city);

        bool UpdateCity(City city);
        City FetchByCityId(int cityId);
        void deleteCity(int id);
    }
}
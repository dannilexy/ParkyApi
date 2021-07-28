using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.IRepository
{
  public  interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationaParks();

        NationalPark GetNationalPark(int NationalParkId);

        bool NationalParkExists(string name);


        bool NationalParkExists(int id);

        bool CreateNationalPark(NationalPark nationalPark);


        bool UpdateNationalPark(NationalPark nationalPark);


        bool DeleteNationalPark(NationalPark nationalPark);

        bool Save();


        

    }
}

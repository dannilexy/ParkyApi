using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.IRepository
{
  public  interface ITrailRepository
    {
        ICollection<Trail> GetTrails();

        ICollection<Trail> GetTrainInPark(int npId);

        Trail GetTrail(int trailId);

        bool TrailExists(string name);


        bool TrailExists(int id);

        bool CreateTrail(Trail trail);


        bool UpdateTrail(Trail trail);


        bool DeleteTrail(Trail trail);

        bool Save();


        

    }
}

using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository
{
    public class TrailRepository : ITrailRepository
    {

        private readonly ParkyApiContext _db;
        public TrailRepository(ParkyApiContext db)
        {
            _db = db;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trail.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trail.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int TrailId)
        {
            return _db.Trail.Include(c => c.NationalPark).FirstOrDefault(c=>c.Id == TrailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trail.Include(c => c.NationalPark).OrderBy(a => a.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trail.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trail.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trail.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrainInPark(int npId)
        {
            return _db.Trail.Include(c => c.NationalPark).Where(c=>c.NationalParkId == npId).ToList();
        }
    }
}

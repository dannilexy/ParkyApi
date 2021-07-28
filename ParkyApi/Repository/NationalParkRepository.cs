using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {

        private readonly ParkyApiContext _db;
        public NationalParkRepository(ParkyApiContext db)
        {
            _db = db;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalPark.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.NationalPark.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int NationalParkId)
        {
            return _db.NationalPark.FirstOrDefault(c=>c.Id == NationalParkId);
        }

        public ICollection<NationalPark> GetNationaParks()
        {
            return _db.NationalPark.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _db.NationalPark.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            return _db.NationalPark.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalPark.Update(nationalPark);
            return Save();
        }
    }
}

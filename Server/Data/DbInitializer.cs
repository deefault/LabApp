using System.Globalization;
using System.Linq;
using LabApp.Server.Data.Models;

namespace LabApp.Server.Data
{
    public class DbInitializer
    {
        private readonly AppDbContext _db;

        public DbInitializer(AppDbContext db)
        {
            _db = db;
        }

        public void Initialize()
        {
            // if (_db.Database.EnsureCreated() || !_db.Countries.Any())
            // {
            //     InitializeCountries();
            // }
        }

        public void InitializeCountries()
        {
            /*try
            {
                _db.Countries.AddRange(
                    CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID))
                        .Select(x => new Country(x.Name.ToLower(), x.EnglishName))
                );
            }
            catch (CultureNotFoundException)
            {
                _db.Countries.Add(new Country("us", "USA"));
                _db.Countries.Add(new Country("ru", "Russia"));
                _db.Countries.Add(new Country("fr", "France"));
            }

            _db.SaveChanges();*/
        }
    }
}
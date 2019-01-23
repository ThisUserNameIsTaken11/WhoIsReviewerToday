using System.Linq;
using WhoIsReviewerToday.Domain;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext
{
    internal class DbInitializer : IDbInitializer
    {
        private readonly IAppDbContext _appDbContext;

        public DbInitializer(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            _appDbContext.EnsureDatabaseCreated();
        }

        public void SeedIfNeeded()
        {
            var wereDevelopersAdded = TrySeedDevelopers();

            if (wereDevelopersAdded)
                _appDbContext.SaveChanges();
        }

        private bool TrySeedDevelopers()
        {
            if (_appDbContext.Developers.Any())
                return false;

            _appDbContext.AddRange(
                new[]
                {
                    new Developer
                    {
                        Team = Team.Desktop,
                        FullName = "Pavel Petrov",
                        UserName = "@petrov_p"
                    },
                    new Developer
                    {
                        Team = Team.Desktop,
                        FullName = "Dastan Uskembayev",
                        UserName = "@uskembayev"
                    },
                    new Developer
                    {
                        Team = Team.Desktop,
                        FullName = "Azamat Kazhgali",
                        UserName = "@kazhgali"
                    },
                    new Developer
                    {
                        Team = Team.Mobile,
                        FullName = "Sergey Molotkov",
                        UserName = "@molotkov_sergey"
                    },
                    new Developer
                    {
                        Team = Team.Desktop,
                        FullName = "Rustem Makhanov",
                        UserName = "@Hecntv"
                    },
                    new Developer
                    {
                        Team = Team.Mobile,
                        FullName = "Daniyar Kokabayev",
                        UserName = "@dkokabayev"
                    },
                    new Developer
                    {
                        Team = Team.Mobile,
                        FullName = "Mikhail Kazakov",
                        UserName = "@MikhailKazakov"
                    },
                    new Developer
                    {
                        Team = Team.Mobile,
                        FullName = "Sergey Borisov",
                        UserName = "@borisovsergey"
                    },
                    new Developer
                    {
                        Team = Team.Mobile,
                        FullName = "Alexey Ershov",
                        UserName = "@aoershov"
                    },
                    new Developer
                    {
                        Team = Team.Desktop,
                        FullName = "Евгений Ступин",
                        UserName = "@CoolJMx"
                    },
                    new Developer
                    {
                        Team = Team.Mobile,
                        FullName = "Dmitry Zarubin",
                        UserName = "@dmitry_zarubin"
                    },
                    new Developer
                    {
                        Team = Team.Mobile,
                        FullName = "ilya mazurenko",
                        UserName = "@imazurenko"
                    },
                    new Developer
                    {
                        Team = Team.Desktop,
                        FullName = "Khadzhimustafov Alexey",
                        UserName = "@khaleksei"
                    },
                    new Developer
                    {
                        Team = Team.Desktop,
                        FullName = "Ruslan Aliev",
                        UserName = "@Speshial1985"
                    }
                });

            return true;
        }
    }
}
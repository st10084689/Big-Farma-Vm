using Big_Farma.Data;
using Big_Farma.Models;
using Big_Farma.Repository.IRepository;

namespace Big_Farma.Repository
{
    public class ApplicationRepository : Repository<ApplicationUser>, IApplicationRepository
    {
        private ApplicationDbContext _db;

        public ApplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}

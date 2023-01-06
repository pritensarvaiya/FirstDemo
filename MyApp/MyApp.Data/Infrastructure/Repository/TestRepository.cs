using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class TestRepository : Repository<Test>,ITestRepository
    {
        private ApplicationDbContext _context;
        public TestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

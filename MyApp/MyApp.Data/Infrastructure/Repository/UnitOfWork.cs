using MyApp.DataAccessLayer.Infrastructure.IRepository;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }        
        public ITestRepository Test { get; private set; }        

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
            Test= new TestRepository(context);
        }

        void IUnitOfWork.Save()
        {
            _context.SaveChanges();
        }
    }
}

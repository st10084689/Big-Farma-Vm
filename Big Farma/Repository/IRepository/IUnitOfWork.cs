namespace Big_Farma.Repository.IRepository
{
   
        public interface IUnitOfWork
        {

            ICategoryRepository category { get; }

            IProductRepository product { get; }

        IApplicationRepository application { get; }

            void Save();
        }
    }


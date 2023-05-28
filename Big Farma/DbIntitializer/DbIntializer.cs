using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Big_Farma.Data;
using Microsoft.EntityFrameworkCore;
using Big_Farma.Models;

namespace Big_Farma.DbIntitializer
{
    public class DbIntializer : IDbIntializer
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbIntializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void intializer()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin@gmail.com",
                    Email = "Admin@gmail.com",
                    Name = "Admin123",
                    PhoneNumber = "1",
                    StreetAdress = "testing",
                    State = "123",
                    PostalCode = "123",
                    City = "123",
                }, "Admin123!").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "Admin@gmail.com");

                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();


                var category1 = new Category
                {
                    CategoryName = "vegetables"
                };

                var category2 = new Category
                {
                    CategoryName = "meat"
                };

                _db.Categories.Add(category1);
                _db.Categories.Add(category2);
                _db.SaveChanges();

                // Create two products associated with the admin user and the categories
                var product1 = new Product
                {
                    ProductName = "Ostriches meat",
                    ProductDescription = "Ostriches meat tastes good and is very healthy",
                    Price = 34,
                    CategoryId = category1.ID,
                    ApplicationIdentity = user.Id
                };

                var product2 = new Product
                {
                    ProductName = "Wheat",
                    ProductDescription = "wheat fresh and organic",
                    Price = 56,
                    CategoryId = category2.ID,
                    ApplicationIdentity = user.Id
                };

                _db.Products.Add(product1);
                _db.Products.Add(product2);
                _db.SaveChanges();
            

        }
                return;

        }
        
    }
}

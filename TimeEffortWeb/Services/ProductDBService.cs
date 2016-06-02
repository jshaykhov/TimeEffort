using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortWeb.Entities;

namespace TimeEffortCore.Services
{
    public class ProductDBService 
    {
        private DBEntities db;
        public ProductDBService()
        {
            db = new DBEntities();
        }
        public void Delete(int itemId)
        {
            var item = db.Product.FirstOrDefault(p => p.Id == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a null product");
            db.Product.Remove(item);
            db.SaveChanges();
        }

        public List<Product> GetAll()
        {
            return db.Product.ToList();
        }

        public Product GetById(int id)
        {
            var item = db.Product.FirstOrDefault(p => p.Id == id);
            if (item == null)
                throw new ArgumentNullException("Product does not exist");
            return item;
        }

        public void Insert(Product item)
        {
            item.Created = DateTime.Now;
            db.Product.Add(item);
            db.SaveChanges();
        }

        public void Update(Product item)
        {
            var dbItem = db.Product.FirstOrDefault(p => p.Id == item.Id);
            if (dbItem == null)
                throw new ArgumentNullException("Product does not exist");
            dbItem.Title = item.Title;
            dbItem.Price = item.Price;
            dbItem.Description = item.Description;
            dbItem.Manufacturer = item.Manufacturer;
            dbItem.Brand = item.Brand;
            dbItem.TitleRu = item.TitleRu;
            dbItem.PriceRu = item.PriceRu;
            dbItem.DescriptionRu = item.DescriptionRu;
            dbItem.ManufacturerRu = item.ManufacturerRu;
            dbItem.BrandRu = item.BrandRu;
            dbItem.CategoryId = item.CategoryId;
            dbItem.Updated = DateTime.Now;
            db.SaveChanges();
        }
        
        //Category related methods

        //Check whether there are products referring
        //to the category in the database
        public bool CategoryHasProducts(int categoryId)
        {
            return db.Product.Any(p => p.CategoryId == categoryId);
        }
        public void DeleteCategory(int itemId)
        {
            var item = db.Category.FirstOrDefault(c => c.Id == itemId);
            if (item == null)
                throw new Exception("You cannot delete a null category");
            //If there are products refering to the category in the database
            //throw exception with respective error message
            if (CategoryHasProducts(itemId))
            {
                throw new Exception("You cannot delete a category! There are products with this category in the database");
            }
            db.Category.Remove(item);
            db.SaveChanges();
        }

        public Category GetCategoryById(int id)
        {
            var item = db.Category.FirstOrDefault(c => c.Id == id);
            if (item == null)
                throw new ArgumentNullException("Category does not exist");
            return item;
        }

        //Method returns true if there are no records in category table with the same category name
        private bool IsUnique(Category item)
        {
            return !db.Category.Any(c => c.CategoryName.ToLower() == item.CategoryName.ToLower());
        }
        public void InsertCategory(Category item)
        {
            //Find if there is no such category that is going to be inserted
            //if category name is not unique throw exception
            if (!IsUnique(item))
            {
                throw new Exception("Category already exists in the database");
            }
            db.Category.Add(item);
            db.SaveChanges();
        }

        public void UpdateCategory(Category item)
        {
            var dbItem = db.Category.FirstOrDefault(c => c.Id == item.Id);
            if (dbItem == null)
                throw new ArgumentNullException("Category does not exist");
            dbItem.CategoryName = item.CategoryName;
            dbItem.CategoryNameRu = item.CategoryNameRu;
            db.SaveChanges();
        }

        public List<Category> GetAllCategories()
        {
            return db.Category.ToList();
        }
    }
}

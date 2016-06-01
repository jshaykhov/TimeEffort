using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;

namespace TimeEffortCore.Services
{
    public class ProjectDBService
    {
        private time_trackerEntities db;
        public ProjectDBService()
        {
            db = new time_trackerEntities();
        }
        //public void Delete(int itemId)
        //{
        //    var item = db.Project.FirstOrDefault(p => p.Id == itemId);
        //    if (item == null)
        //        throw new ArgumentNullException("You cannot delete a null Project");
        //    db.Project.Remove(item);
        //    db.SaveChanges();
        //}

        //public List<Project> GetAll()
        //{
        //    return db.Project.ToList();
        //}

        //public Project GetById(int id)
        //{
        //    var item = db.Project.FirstOrDefault(p => p.Id == id);
        //    if (item == null)
        //        throw new ArgumentNullException("Project does not exist");
        //    return item;
        //}

        //public void Insert(Project item)
        //{
        //    item.Created = DateTime.Now;
        //    db.Project.Add(item);
        //    db.SaveChanges();
        //}

        //public void Update(Project item)
        //{
        //    var dbItem = db.Project.FirstOrDefault(p => p.Id == item.ID);
        //    if (dbItem == null)
        //        throw new ArgumentNullException("Project does not exist");
        //    dbItem.Title = item.Title;
        //    dbItem.Price = item.Price;
        //    dbItem.Description = item.Description;
        //    dbItem.Manufacturer = item.Manufacturer;
        //    dbItem.Brand = item.Brand;
        //    dbItem.TitleRu = item.TitleRu;
        //    dbItem.PriceRu = item.PriceRu;
        //    dbItem.DescriptionRu = item.DescriptionRu;
        //    dbItem.ManufacturerRu = item.ManufacturerRu;
        //    dbItem.BrandRu = item.BrandRu;
        //    dbItem.CategoryId = item.CategoryId;
        //    dbItem.Updated = DateTime.Now;
        //    db.SaveChanges();
        //}
    }
}

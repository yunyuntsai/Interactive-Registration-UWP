using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace IoTRegistApi.Models
{
    public class PhotoModel
    {
        public class DetailModel
        {         
            public int VisitorId { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public int PhotoId { get; set; }
            public string PhotoUrl { get; set; }
            public string PhotoName { get; set; }
        }

        public class CreateModel
        {

            [Required]
            public string PhotoName { get; set; }
            [Required]
            public string PhotoUrl { get; set; }
            [Required]
            public int VisitorId { get; set; }
            //public bool IsComplete { get; set; }
        }

        public List<DetailModel> GetAll()
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                var L2Enty = from  p in dbEntity.Visitors_Photo
                             orderby p.VisitorId descending
                             join c in dbEntity.Visitors_Profile on p.VisitorId equals c.VisitorId into ps
                             from c in ps.DefaultIfEmpty()
                             select new { c.VisitorId, c.VisitorName, c.VisitorCompany, p.PhotoId, p.PhotoUrl, p.PhotoName };
                return L2Enty.Select(s => new DetailModel()
                {
                    VisitorId = s.VisitorId,
                    Name = s.VisitorName,
                    Company = s.VisitorCompany,
                    PhotoId = s.PhotoId,
                    PhotoUrl = s.PhotoUrl,
                    PhotoName = s.PhotoName,
                }).Take(50).ToList<DetailModel>();
            }
        }

        public List<DetailModel> GetbyId(int VisitorID)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                var L2Enty = from p in dbEntity.Visitors_Photo
                             orderby p.VisitorId descending
                             join c in dbEntity.Visitors_Profile on p.VisitorId equals c.VisitorId into ps
                             from c in ps.DefaultIfEmpty()
                             where p.VisitorId == VisitorID
                             select new { c.VisitorId, c.VisitorName, c.VisitorCompany, p.PhotoId, p.PhotoUrl, p.PhotoName };
             
                return L2Enty.Select(s => new DetailModel()
                {
                    VisitorId = s.VisitorId,
                    Name = s.VisitorName,
                    Company = s.VisitorCompany,
                    PhotoId = s.PhotoId,
                    PhotoUrl = s.PhotoUrl,
                    PhotoName = s.PhotoName,
                }).Take(50).ToList<DetailModel>();
            }
        }

        public void Create(CreateModel photoModel)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                Visitors_Photo event1 = new Visitors_Photo();
                event1.PhotoName = photoModel.PhotoName;
                event1.PhotoUrl = photoModel.PhotoUrl;
                event1.VisitorId = photoModel.VisitorId;
                dbEntity.Visitors_Photo.Add(event1);
                dbEntity.SaveChanges();
            }
        }

        public void Delete(Int32 id)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                Visitors_Photo e = dbEntity.Visitors_Photo.Find(id);
                dbEntity.Visitors_Photo.Remove(e);
                Debug.WriteLine(e.PhotoName);
                dbEntity.SaveChanges();

            }
        }

        public void DeleteAll()
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                foreach (Visitors_Photo item in dbEntity.Visitors_Photo)
                {
                    dbEntity.Visitors_Photo.Remove(item);
                    dbEntity.SaveChanges();
                }

            }
        }
    }
}
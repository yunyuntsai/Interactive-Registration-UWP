using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace IoTRegistApi.Models
{
    public class VisitorModel
    {
        public class DetailModel
        {
 
            public int VisitorId { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string Arrived { get; set; }
            public DateTime CreateTime { get; set; }
            public DateTime? UpdateTime { get; set; }
            public long NFCid { get; set; }
            public string TagId { get; set; }
            public int EventId { get; set; }
        }
        public class CreateModel
        {
            [Required]
            public int VisitorId { get; set; }
            [Required]
            public string VisitorName { get; set; }
            [Required]
            public string VisitorCompany { get; set; }
            [Required]
            public string Arrived { get; set; }
            [Required]
            public int EventId { get; set; }
            //public bool IsComplete { get; set; }
        }

        public class UpdateModel
        {
            [Required]
            public long NFCid { get; set; }
            //public bool IsComplete { get; set; }
        }

        public List<DetailModel> GetAll()
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                var L2Enty = from c in dbEntity.Visitors_Profile
                             orderby c.VisitorId descending
                             join p in dbEntity.Tag_Profile on c.NFCid equals p.NfcId into ps
                             from p in ps.DefaultIfEmpty()
                             select new { c.VisitorId, c.VisitorName, c.VisitorCompany, c.Arrived, c.CreateTime, c.VisitTime, c.NFCid, c.EventId, p.TagId };
                return L2Enty.Select(s => new DetailModel()
                {
                    VisitorId = s.VisitorId,
                    Name = s.VisitorName,
                    Company = s.VisitorCompany,
                    Arrived = s.Arrived,
                    CreateTime = s.CreateTime,
                    UpdateTime = (DateTime?)s.VisitTime,
                    NFCid = (long)s.NFCid,
                    TagId = s.TagId,
                    EventId = s.EventId
                }).Take(50).ToList<DetailModel>();
            }
        }

        public List<DetailModel> GetbyId(int VisitorID)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                var L2Enty = from c in dbEntity.Visitors_Profile
                             orderby c.VisitorId descending
                             join p in dbEntity.Tag_Profile on c.NFCid equals p.NfcId into ps
                             from p in ps.DefaultIfEmpty()
                             where c.VisitorId == VisitorID
                             select new { c.VisitorId, c.VisitorName, c.VisitorCompany, c.Arrived, c.CreateTime, c.VisitTime, c.NFCid, c.EventId, p.TagId };

                return L2Enty.Select(s => new DetailModel()
                {
                    VisitorId = s.VisitorId,
                    Name = s.VisitorName,
                    Company = s.VisitorCompany,
                    Arrived = s.Arrived,
                    EventId = s.EventId,
                    CreateTime = s.CreateTime,
                    UpdateTime = (DateTime?)s.VisitTime,
                    NFCid = (long)s.NFCid,
                    TagId = s.TagId,
                }).Take(50).ToList<DetailModel>();
            }
        }

        public void Create(CreateModel dataModel)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                Visitors_Profile newVisitor = new Visitors_Profile();
                newVisitor.VisitorId = dataModel.VisitorId;
                newVisitor.VisitorName = dataModel.VisitorName;
                newVisitor.VisitorCompany = dataModel.VisitorCompany;
                newVisitor.Arrived = dataModel.Arrived;
                newVisitor.EventId = dataModel.EventId;
                newVisitor.CreateTime = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                newVisitor.VisitTime = null;
                newVisitor.NFCid = (Int64)0;
                dbEntity.Visitors_Profile.Add(newVisitor);
                dbEntity.SaveChanges();
            }
        }

        public void Update(int VisitorId, Visitors_Profile newUser, UpdateModel upmodel)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                Visitors_Profile origin_User = dbEntity.Visitors_Profile.Find(VisitorId);
                Debug.WriteLine( " " + origin_User.VisitorName + " " + origin_User.VisitTime);
                Debug.WriteLine("TagId : " + upmodel.NFCid);

                //dbEntity.Entry(newUser).CurrentValues.SetValues(origin_User);
                try
                {
                    origin_User.VisitorId = newUser.VisitorId;
                    origin_User.VisitorName = newUser.VisitorName;
                    origin_User.VisitorCompany = newUser.VisitorCompany;
                    origin_User.Arrived = newUser.Arrived;
                    origin_User.CreateTime = newUser.CreateTime;
                    origin_User.VisitTime = newUser.VisitTime;
                    origin_User.EventId = newUser.EventId;
                    origin_User.NFCid = upmodel.NFCid;
                    origin_User.EventId = newUser.EventId;
                    dbEntity.SaveChanges();
                    //Debug.WriteLine(origin_User.UserID + " " + origin_User.UserName + " " + origin_User.Enroll + " " + origin_User.UpdatedAt);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        public void Clear(Int32 id, Visitors_Profile newUser)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                Visitors_Profile origin_User = dbEntity.Visitors_Profile.Find(id);
                Debug.WriteLine( " " + origin_User.VisitorName + origin_User.VisitTime);

                //dbEntity.Entry(newUser).CurrentValues.SetValues(origin_User);
                try
                {
                    origin_User.VisitorId = newUser.VisitorId;
                    origin_User.VisitorCompany = newUser.VisitorCompany;
                    origin_User.VisitorName = newUser.VisitorName;
                    origin_User.Arrived = newUser.Arrived;
                    origin_User.CreateTime = newUser.CreateTime;
                    origin_User.VisitTime = newUser.VisitTime;
                    origin_User.EventId = newUser.EventId;
                    origin_User.NFCid = newUser.NFCid;

                    foreach (Visitors_Photo photo_item in dbEntity.Visitors_Photo)
                    {
                        if (photo_item.VisitorId == id)
                        {
                            dbEntity.Visitors_Photo.Remove(photo_item);
                        }
                    }
                    dbEntity.SaveChanges();
                    //Debug.WriteLine(origin_User.UserID + " " + origin_User.UserName + " " + origin_User.Enroll + " " + origin_User.UpdatedAt);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        public void Delete(Int32 id)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                Visitors_Profile origin_User = dbEntity.Visitors_Profile.Find(id);
                dbEntity.Visitors_Profile.Remove(origin_User);
                foreach(Visitors_Photo photo_item in dbEntity.Visitors_Photo)
                {
                    if(photo_item.VisitorId == id)
                    {
                        dbEntity.Visitors_Photo.Remove(photo_item);
                    }
                }
                dbEntity.SaveChanges();

            }
        }

        public void DeleteAll()
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                foreach (Visitors_Profile item in dbEntity.Visitors_Profile)
                {
                    dbEntity.Visitors_Profile.Remove(item);
                   
                }

                foreach (Visitors_Photo item1 in dbEntity.Visitors_Photo)
                {
                    dbEntity.Visitors_Photo.Remove(item1);

                }
                dbEntity.SaveChanges();

            }
        }
    
    }
}
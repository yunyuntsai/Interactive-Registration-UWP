using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace DotNetAppSqlDb.Models
{
    public class VisitorModel
    {
        public class DetailModel
        {
            public int Id { get; set; }
            public string VisitorId { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string Arrived { get; set; }
            public DateTime CreateTime { get; set; }
            public DateTime UpdateTime { get; set; }
            public long NFCid { get; set; }
            public string TagId { get; set; }
        }
        public class CreateModel
        {
            [Required]
            public int ID { get; set; }
            [Required]
            public string VisitorName { get; set; }
            [Required]
            public string VisitorCompany { get; set; }
            [Required]
            public string Arrived { get; set; }
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
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                var L2Enty = from c in dbEntity.Visitor_Profile
                             orderby c.ID descending
                             join p in dbEntity.Tag_Profile on c.NFCid equals p.NfcId into ps
                             from p in ps.DefaultIfEmpty()
                             select new { c.ID, c.VisitorId, c.VisitorName, c.VisitorCompany, c.Arrived, c.CreateTime, c.VisitTime, c.NFCid, p.TagId };
                return L2Enty.Select(s => new DetailModel()
                {
                    Id = s.ID,
                    VisitorId = s.VisitorId,
                    Name = s.VisitorName,
                    Company = s.VisitorCompany,
                    Arrived = s.Arrived,
                    CreateTime = s.CreateTime,
                    UpdateTime = s.VisitTime,
                    NFCid = (long)s.NFCid,
                    TagId = s.TagId
                }).Take(50).ToList<DetailModel>();
            }
        }

        public List<DetailModel> GetbyId(int VisitorID)
        {
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                var L2Enty = from c in dbEntity.Visitor_Profile
                             orderby c.ID descending
                             join p in dbEntity.Tag_Profile on c.NFCid equals p.NfcId into ps
                             from p in ps.DefaultIfEmpty()
                             where c.ID == VisitorID
                             select new { c.ID, c.VisitorId, c.VisitorName, c.VisitorCompany, c.Arrived, c.CreateTime, c.VisitTime, c.NFCid, p.TagId };

                return L2Enty.Select(s => new DetailModel()
                {
                    Id = s.ID,
                    VisitorId = s.VisitorId,
                    Name = s.VisitorName,
                    Company = s.VisitorCompany,
                    Arrived = s.Arrived,
                    CreateTime = s.CreateTime,
                    UpdateTime = s.VisitTime,
                    NFCid = (long)s.NFCid,
                    TagId = s.TagId
                }).Take(50).ToList<DetailModel>();
            }
        }

        public void Create(CreateModel dataModel)
        {
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                Visitor_Profile newVisitor = new Visitor_Profile();
                newVisitor.ID = dataModel.ID;
                newVisitor.VisitorId = "user" + dataModel.ID.ToString().PadLeft(3, '0');
                newVisitor.VisitorName = dataModel.VisitorName;
                newVisitor.VisitorCompany = dataModel.VisitorCompany;
                newVisitor.Arrived = dataModel.Arrived;
                newVisitor.CreateTime = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                newVisitor.VisitTime = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                newVisitor.NFCid = (Int64)0;
                dbEntity.Visitor_Profile.Add(newVisitor);
                dbEntity.SaveChanges();
            }
        }

        public void Update(int VisitorId, Visitor_Profile newUser, UpdateModel upmodel)
        {
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                Visitor_Profile origin_User = dbEntity.Visitor_Profile.Find(VisitorId);
                Debug.WriteLine(origin_User.VisitorId + " " + origin_User.VisitorName + " " + origin_User.VisitTime);
                Debug.WriteLine("TagId : " + upmodel.NFCid);

                //dbEntity.Entry(newUser).CurrentValues.SetValues(origin_User);
                try
                {
                    origin_User.ID = newUser.ID;
                    origin_User.VisitorId = newUser.VisitorId;
                    origin_User.VisitorName = newUser.VisitorName;
                    origin_User.VisitorCompany = newUser.VisitorCompany;
                    origin_User.Arrived = newUser.Arrived;
                    origin_User.CreateTime = newUser.CreateTime;
                    origin_User.VisitTime = newUser.VisitTime;
                    origin_User.NFCid = upmodel.NFCid;

                    dbEntity.SaveChanges();
                    //Debug.WriteLine(origin_User.UserID + " " + origin_User.UserName + " " + origin_User.Enroll + " " + origin_User.UpdatedAt);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        public void Clear(Int32 id, Visitor_Profile newUser)
        {
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                Visitor_Profile origin_User = dbEntity.Visitor_Profile.Find(id);
                Debug.WriteLine(origin_User.VisitorId + " " + origin_User.VisitorName + origin_User.VisitTime);

                //dbEntity.Entry(newUser).CurrentValues.SetValues(origin_User);
                try
                {
                    origin_User.ID = newUser.ID;
                    origin_User.VisitorId = newUser.VisitorId;
                    origin_User.VisitorCompany = newUser.VisitorCompany;
                    origin_User.VisitorName = newUser.VisitorName;
                    origin_User.Arrived = newUser.Arrived;
                    origin_User.CreateTime = newUser.CreateTime;
                    origin_User.VisitTime = newUser.VisitTime;
                    origin_User.NFCid = newUser.NFCid;

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
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                Visitor_Profile origin_User = dbEntity.Visitor_Profile.Find(id);
                dbEntity.Visitor_Profile.Remove(origin_User);
                dbEntity.SaveChanges();

            }
        }

        public void DeleteAll()
        {
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                foreach (Visitor_Profile item in dbEntity.Visitor_Profile)
                {
                    dbEntity.Visitor_Profile.Remove(item);
                    dbEntity.SaveChanges();
                }

            }
        }
    }
}
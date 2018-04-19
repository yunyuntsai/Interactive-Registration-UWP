﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DotNetAppSqlDb.Models
{
    public class PhotoModel
    {
        public class DetailModel
        {
            public int VisitorId { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string Arrived { get; set; }
            public DateTime CreateTime { get; set; }
            public DateTime UpdateTime { get; set; }
            public long NFCid { get; set; }
            public string TagId { get; set; }
            public int PhotoId { get; set; }
            public string PhotoUrl { get; set; }
            public string PhotoName { get; set; }
        }

        public class CreateModel
        {
            [Required]
            public int PhotoId { get; set; }
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
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                var L2Enty = from p in dbEntity.Visitor_Photo
                             orderby p.VisitorId descending
                             join c in dbEntity.Visitor_Profile on p.VisitorId equals c.ID into ps
                             from c in ps.DefaultIfEmpty()
                             join q in dbEntity.Tag_Profile on c.NFCid equals q.NfcId into qq
                             from q in qq.DefaultIfEmpty()
                             select new { c.ID, c.VisitorName, c.VisitorCompany, c.Arrived, c.CreateTime, c.VisitTime, c.NFCid, q.TagId,  p.PhotoId, p.PhotoUrl, p.PhotoName };
                return L2Enty.Select(s => new DetailModel()
                {
                    VisitorId = s.ID,
                    Name = s.VisitorName,
                    Company = s.VisitorCompany,
                    Arrived = s.Arrived,
                    CreateTime = s.CreateTime,
                    UpdateTime = s.VisitTime,
                    NFCid = (long)s.NFCid,
                    TagId = s.TagId,
                    PhotoId = s.PhotoId,
                    PhotoUrl = s.PhotoUrl,
                    PhotoName = s.PhotoName,
                }).Take(50).ToList<DetailModel>();
            }
        }

        public List<DetailModel> GetbyId(int VisitorID)
        {
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                var L2Enty = from p in dbEntity.Visitor_Photo
                             orderby p.VisitorId descending
                             join c in dbEntity.Visitor_Profile on p.VisitorId equals c.ID into ps
                             from c in ps.DefaultIfEmpty()
                             where p.VisitorId == VisitorID
                             select new { c.ID, c.VisitorName, c.VisitorCompany, p.PhotoId, p.PhotoUrl, p.PhotoName };

                return L2Enty.Select(s => new DetailModel()
                {
                    VisitorId = s.ID,
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
            using (WebApplication2201802_dbEntities dbEntity = new WebApplication2201802_dbEntities())
            {
                Visitor_Photo newPhoto = new Visitor_Photo();
                newPhoto.PhotoId = photoModel.PhotoId;
                newPhoto.PhotoName = photoModel.PhotoName;
                newPhoto.PhotoUrl = photoModel.PhotoUrl;
                newPhoto.VisitorId = photoModel.VisitorId;
                dbEntity.Visitor_Photo.Add(newPhoto);
                dbEntity.SaveChanges();
            }
        }
    }
}
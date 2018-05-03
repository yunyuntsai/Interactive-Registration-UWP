using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IoTRegistApi.Models
{
    public class historyModel
    {
        public class DetailModel
        {
            public int Id { get; set; }
            public int VisitorId { get; set; }
            public string VisitorName { get; set; }
            public string VisitorCompany { get; set; }
            public string Arrived { get; set; }
            public DateTime? VisitTime { get; set; }
            public string PhotoUrl { get; set; }
            public int EventID { get; set; }
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
            public DateTime? VisitTime { get; set; }
            [Required]
            public string PhotoUrl { get; set; }
            [Required]
            public int EventID { get; set; }
        }


        public List<DetailModel> GetAll()
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                var L2Enty = from p in dbEntity.History_Visitor_Detail
                             orderby p.Id descending
                             select p;

                return L2Enty.Select(s => new DetailModel()
                {
                    Id = s.Id,
                    VisitorId = s.VisitorId,
                    VisitorName = s.VisitorName,
                    VisitorCompany = s.VisitorCompany,
                    Arrived = s.Arrived,
                    VisitTime = s.VisitTime,
                    PhotoUrl = s.PhotoUrl,
                    EventID = s.EventID,
                }).Take(50).ToList<DetailModel>();
            }
        }

        public void Create(CreateModel historyModel)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                History_Visitor_Detail H = new History_Visitor_Detail();
                H.VisitorId = historyModel.VisitorId;
                H.VisitorName = historyModel.VisitorName;
                H.VisitorCompany = historyModel.VisitorCompany;
                H.Arrived = historyModel.Arrived;
                H.VisitTime = (DateTime?)historyModel.VisitTime;
                H.PhotoUrl = historyModel.PhotoUrl;
                H.EventID = historyModel.EventID;
                dbEntity.History_Visitor_Detail.Add(H);
                dbEntity.SaveChanges();
            }
        }
    }
}
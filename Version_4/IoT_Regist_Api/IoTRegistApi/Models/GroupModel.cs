using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IoTRegistApi.Models
{
    public class GroupModel
    {
        public class DetailModel
        {
            public int EventId { get; set; }
            public string EventName { get; set; }
            public int VisitorId { get; set; }
            public string VisitorName { get; set; }
            public string VisitorCompany { get; set; }
            public string Arrived { get; set; }
            public DateTime? VisitTime { get; set; }
            public string PhotoUrl { get; set; }
        }

        public List<DetailModel> GetbyId(int EventID)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                var L2Enty = from p in dbEntity.History_Visitor_Detail
                             orderby p.EventID descending
                             join c in dbEntity.Event_Profile on p.EventID equals c.eventID into ps
                             from c in ps.DefaultIfEmpty()
                             where p.EventID == EventID
                             select new { p.EventID, c.EventName, p.VisitorId, p.VisitorName, p.VisitorCompany, p.PhotoUrl, p.Arrived, p.VisitTime };

                return L2Enty.Select(s => new DetailModel()
                {
                    EventId = s.EventID,
                    EventName = s.EventName,
                    VisitorId = s.VisitorId,
                    VisitorName = s.VisitorName,
                    VisitorCompany = s.VisitorCompany,
                    Arrived = s.Arrived,
                    VisitTime = (DateTime?)s.VisitTime,
                    PhotoUrl = s.PhotoUrl
                }).Take(50).ToList<DetailModel>();
            }
        }
    }
}
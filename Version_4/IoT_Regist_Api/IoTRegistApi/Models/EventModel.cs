using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IoTRegistApi.Models
{
    public class EventModel
    {
        public class DetailModel
        {
            public int EventId { get; set; }
            public string EventName { get; set; }
            public string EventType { get; set; }
            public DateTime StartDate { get; set; }
            public TimeSpan StartTime { get; set; }
            public DateTime EndDate { get; set; }
            public TimeSpan EndTime { get; set; }
        }

        public class CreateModel
        {
            [Required]
            public int eventID { get; set; }
            [Required]
            public string EventName { get; set; }
            [Required]
            public string EventType { get; set; }
            [Required]
            public DateTime StartDate { get; set; }
            [Required]
            public TimeSpan StartTime { get; set; }
            [Required]
            public DateTime EndDate { get; set; }
            [Required]
            public TimeSpan EndTime { get; set; }
        }

        public List<DetailModel> GetAll()
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                var L2Enty = from p in dbEntity.Event_Profile
                             orderby p.eventID descending
                             select  p;
                return L2Enty.Select(s => new DetailModel()
                {
                    EventId = s.eventID,
                    EventName = s.EventName,
                    EventType = s.EventType,
                    StartDate = (DateTime)s.StartDate,
                    StartTime = (TimeSpan)s.StartTime,
                    EndDate = (DateTime)s.EndDate,
                    EndTime = (TimeSpan)s.EndTime,
                }).Take(50).ToList<DetailModel>();
            }
        }

        public List<DetailModel> GetbyId(int EventID)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                var L2Enty = from p in dbEntity.Event_Profile
                             orderby p.eventID descending
                             where p.eventID == EventID
                             select  p;

                return L2Enty.Select(s => new DetailModel()
                {
                    EventId = s.eventID,
                    EventName = s.EventName,
                    EventType = s.EventType,
                    StartDate = (DateTime)s.StartDate,
                    StartTime = (TimeSpan)s.StartTime,
                    EndDate = (DateTime)s.EndDate,
                    EndTime = (TimeSpan)s.EndTime,
                }).Take(50).ToList<DetailModel>();
            }
        }

        public void Create(CreateModel eventModel)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                Event_Profile event1 = new Event_Profile();
                event1.eventID = eventModel.eventID;
                event1.EventName = eventModel.EventName;
                event1.EventType = eventModel.EventType;
                event1.StartDate = eventModel.StartDate;
                event1.StartTime = eventModel.StartTime;
                event1.EndDate = eventModel.EndDate;
                event1.EndTime = eventModel.EndTime;
                dbEntity.Event_Profile.Add(event1);
                dbEntity.SaveChanges();
            }
        }

        public void Delete(Int32 id)
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                Event_Profile e = dbEntity.Event_Profile.Find(id);
                dbEntity.Event_Profile.Remove(e);
                dbEntity.SaveChanges();

            }
        }

        public void DeleteAll()
        {
            using (Registration_dbEntities dbEntity = new Registration_dbEntities())
            {
                foreach (Event_Profile item in dbEntity.Event_Profile)
                {
                    dbEntity.Event_Profile.Remove(item);
                    dbEntity.SaveChanges();
                }

            }
        }
    }
}
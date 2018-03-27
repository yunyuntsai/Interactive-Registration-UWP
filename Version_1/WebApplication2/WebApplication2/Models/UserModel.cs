using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Web;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace WebApplication2.Models
{
    public class UserModel
    {
        public class DetailModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string Arrived { get; set; }
            public DateTime CreateTime { get; set; }
            public DateTime UpdateTime { get; set; }
            public long TagId { get; set; }
        }
        public class CreateModel
        {
            [Required]
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public int Age { get; set; }
            [Required]
            public string Arrived { get; set; }
            //public bool IsComplete { get; set; }
        }

        public class UpdateModel
        {
            [Required]
            public long TagId { get; set; }
            //public bool IsComplete { get; set; }
        }

        public List<DetailModel> GetAll()
        {
            using (WebApplication2201802_dbEntities1 dbEntity = new WebApplication2201802_dbEntities1())
            {
                var L2Enty = from c in dbEntity.USERTABLE
                             orderby c.UserId descending
                             select c;
                return L2Enty.Select(s => new DetailModel()
                {
                    Id = (int)s.UserId,
                    Name = s.UserName,
                    Age = s.AGE,
                    Arrived = s.Arrived,
                    CreateTime = s.CreateAt,
                    UpdateTime = s.UpdateAt,
                    TagId = (long)s.TagID
                }).Take(50).ToList<DetailModel>();
            }
        }

        public List<DetailModel> GetbyId(int ID)
        {
            using (WebApplication2201802_dbEntities1 dbEntity = new WebApplication2201802_dbEntities1())
            {
                var L2Enty = from c in dbEntity.USERTABLE
                             orderby c.UserId descending
                             where c.UserId == ID
                             select c;
                return L2Enty.Select(s => new DetailModel()
                {
                    Id = (int)s.UserId,
                    Name = s.UserName,
                    Age = s.AGE,
                    Arrived = s.Arrived,
                    CreateTime = s.CreateAt,
                    UpdateTime = s.UpdateAt,
                    TagId = s.TagID
                }).Take(50).ToList<DetailModel>();
            }
        }

        public void Create(CreateModel dataModel)
        {
            using (WebApplication2201802_dbEntities1 dbEntity = new WebApplication2201802_dbEntities1())
            {
                USERTABLE newUser = new USERTABLE();
                newUser.UserId = dataModel.Id;
                newUser.UserName = dataModel.Name;
                newUser.AGE = dataModel.Age;
                newUser.Arrived = dataModel.Arrived;
                newUser.CreateAt = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                newUser.UpdateAt = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                //newOrder.Deleted = false;

                dbEntity.USERTABLE.Add(newUser);
                dbEntity.SaveChanges();
            }
        }

        public void Update(int id, USERTABLE newUser, UpdateModel upmodel)
        {
            using (WebApplication2201802_dbEntities1 dbEntity = new WebApplication2201802_dbEntities1())
            {             
                USERTABLE origin_User = dbEntity.USERTABLE.Find(id);
                Debug.WriteLine(origin_User.UserId + " " + origin_User.UserName  + " " + origin_User.UpdateAt);
                //dbEntity.USERTABLE.AddOrUpdate(User);

                /*update_User.UpdatedAt = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                
                if (update_User.Enroll == 0)
                {
                   update_User.Enroll = 1;
                }*/
                Debug.WriteLine("TagId : "+upmodel.TagId);
                Debug.WriteLine(newUser.UserId + " " + newUser.UserName  + " " + newUser.UpdateAt);
                //dbEntity.Entry(newUser).CurrentValues.SetValues(origin_User);
                try
                {
                    origin_User.UserId = newUser.UserId;
                    origin_User.UserName = newUser.UserName;
                    origin_User.AGE = newUser.AGE;
                    origin_User.Arrived = newUser.Arrived;
                    origin_User.CreateAt = newUser.CreateAt;
                    origin_User.UpdateAt = newUser.UpdateAt;
                    origin_User.TagID = upmodel.TagId;
                    //Debug.WriteLine(dbEntity.Entry(update_User).CurrentValues);
                    Debug.WriteLine(origin_User.UserId + " " + origin_User.UserName  + " " + origin_User.UpdateAt + " " + origin_User.TagID);


                    //dbEntity.USERTABLE.Attach(origin_User);
                    //dbEntity.Entry(origin_User).State = EntityState.Modified;
                    //dbEntity.SaveChangesAsync();
                    dbEntity.SaveChanges();
                    //Debug.WriteLine(origin_User.UserID + " " + origin_User.UserName + " " + origin_User.Enroll + " " + origin_User.UpdatedAt);
                }
                catch( Exception e)
               {
                    Debug.WriteLine(e);
                }                    
            }
        }

        public void Clear(Int32 id, USERTABLE newUser)
        {
            using (WebApplication2201802_dbEntities1 dbEntity = new WebApplication2201802_dbEntities1())
            {
                USERTABLE origin_User = dbEntity.USERTABLE.Find(id);
                Debug.WriteLine(origin_User.UserId + " " + origin_User.UserName  + origin_User.UpdateAt);
                //dbEntity.USERTABLE.AddOrUpdate(User);

                /*update_User.UpdatedAt = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                
                if (update_User.Enroll == 0)
                {
                   update_User.Enroll = 1;
                }*/
                Debug.WriteLine(newUser.UserId + " " + newUser.UserName  + " " + newUser.UpdateAt);
                //dbEntity.Entry(newUser).CurrentValues.SetValues(origin_User);
                try
                {
                    origin_User.UserId = newUser.UserId;
                    origin_User.UserName = newUser.UserName;
                    origin_User.AGE = newUser.AGE;
                    origin_User.Arrived = newUser.Arrived;
                    origin_User.CreateAt = newUser.CreateAt;
                    origin_User.UpdateAt = newUser.UpdateAt;
                    origin_User.TagID = newUser.TagID;
                    //Debug.WriteLine(dbEntity.Entry(update_User).CurrentValues);
                    Debug.WriteLine(origin_User.UserId + " " + origin_User.UserName  + " " + origin_User.UpdateAt + " "+origin_User.TagID);


                    //dbEntity.USERTABLE.Attach(origin_User);
                    //dbEntity.Entry(origin_User).State = EntityState.Modified;
                    //dbEntity.SaveChangesAsync();
                    dbEntity.SaveChanges();
                    //Debug.WriteLine(origin_User.UserID + " " + origin_User.UserName + " " + origin_User.Enroll + " " + origin_User.UpdatedAt);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
namespace TradingVLU.Models
{
    public class UserDB
    {
        vlutrading3545Entities db = null;
        public UserDB()
        {
            db = new vlutrading3545Entities();
        }
        public int Insert(user entity)
        {
            db.users.Add(entity);
            db.SaveChanges();
            return entity.id;
        }

        public bool Update(user entity)
        {
            try
            {
                var user = db.users.Find(entity.id);
                user.username = entity.username;
                user.email = entity.email;
                user.name = entity.name;
                user.answer_security_question = entity.answer_security_question;
                user.role = entity.role;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public IEnumerable<user> ListAll(string searchString, int page, int size)
        {
            IQueryable<user> model = db.users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.username.Contains(searchString) || x.name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.id).ToPagedList(page, size);
        }

        public user GetByID(string username)
        {
            return db.users.SingleOrDefault(x => x.username == username);
        }
        public user ViewDetail(int id)
        {
            return db.users.Find(id);
        }

        public bool Login(string user, string pass)
        {
            var result = db.users.Count(x => x.username == user && x.password == pass);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.users.Find(id);
                db.users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
        public int? ChangeStatus(int id)
        {
            var user = db.users.Find(id);
            if (user.is_active == 0)
            {                
                user.is_active = 1;
            }
            else
            {
                user.is_active = 0;
            }           
            db.SaveChanges();
            return user.is_active;
        }
    }
    }

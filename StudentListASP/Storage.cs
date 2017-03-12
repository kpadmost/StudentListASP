using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentListASP
{
    public class Storage
    {
        public List<Student> getStudents()
        {
            using (var db = new StorageContext())
            {
                return db.Students.Include("Group").ToList();
            }
        }

        public List<Group> getGroups()
        {
            using (var db = new StorageContext())
            {
                return db.Groups.ToList();
            }
        }
        //Student set actions
        public void createStudent(string firstName, string lastName
            , string indexNo, int groupId, DateTime birthDate, string birthPlace)
        {
            using (var db = new StorageContext())
            {
                var group = db.Groups.Find(groupId);
                var student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = birthDate
                    ,
                    BirthPlace = birthPlace,
                    IndexNo = indexNo,
                    Group = group,
                    IDGroup = groupId
                };
                db.Students.Add(student);
                db.SaveChanges();
            }
        }
        public void updateStudent(Student st)
        {
            using (var db = new StorageContext())
            {
                var original = db.Students.Find(st.IDStudent);
                if (Convert.ToBase64String(original.TimeStamp) != Convert.ToBase64String(st.TimeStamp))
                    throw new DbUpdateConcurrencyException();
                if (original != null)
                {
                    original.FirstName = st.FirstName;
                    original.LastName = st.LastName;
                    original.BirthDate = st.BirthDate;
                    original.BirthPlace = st.BirthPlace;
                    original.IDGroup = st.IDGroup;
                    original.IndexNo = st.IndexNo;

                    db.SaveChanges();
                }
            }
        }
        public void deleteStudent(Student st)
        {
            using (var db = new StorageContext())
            {
                var original = db.Students.Find(st.IDStudent);
                if (Convert.ToBase64String(original.TimeStamp) != Convert.ToBase64String(st.TimeStamp))
                    throw new DbUpdateConcurrencyException();
                if (original != null)
                {
                    db.Students.Remove(original);
                    db.SaveChanges();
                }
            }
        }

        //Group set actions
        public void createGroup(string groupName)
        {
            using (var db = new StorageContext())
            {
                var original = db.Groups.Single(g => g.Name == groupName);
                if(original == null)
                {
                    db.Groups.Add(new Group { Name = groupName });
                }
            }
        }

        public void updateGroup(Group group)
        {
            using (var db = new StorageContext())
            {
                var original = db.Groups.Find(group.IDGroup);
                if(original != null)
                {
                    original.Name = group.Name;

                    db.SaveChanges();
                }
            }
        }

        public void deleteGroup(Group gr)
        {
            using (var db = new StorageContext())
            {
                var original = db.Groups.Find(gr.IDGroup);
                if(original != null)
                {
                    db.Groups.Remove(original);
                    db.SaveChanges();
                }
            }
        }
    }
}

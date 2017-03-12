using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentListASP.Models
{


    public class StudentsViewModel
    {
        private FilterViewModel filter = new FilterViewModel();
        public Student SelectedStudent  { get; set; }
        public IEnumerable<Student> StudentList { get; set; }
        public GroupsViewModel GroupsVM { get; set; }
        public FilterViewModel Filter { get { return filter; } }
        
    }

    public class FilterViewModel
    {
        public string SelectedCity { get; set; }
        public Group SelectedGroup { get; set; }
    }

    public class GroupsViewModel
    {
        public Group SelectedGroup { get; set; }
        public List<Group> groupList { get; set; }
    }
}
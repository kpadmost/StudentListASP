using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentListASP.Models;
using System.Web.UI.WebControls;
using System.Data.Entity.Infrastructure;

namespace StudentListASP.Controllers
{
    public class ListController : Controller
    {
        private StudentsViewModel vm;
        private readonly Student defaultStudent;
        private readonly Group defaultGroup;
        private readonly log4net.ILog logger 
            = log4net.LogManager.GetLogger(System.Reflection
                .MethodBase.GetCurrentMethod().DeclaringType);
        public ListController()
        {
            var s = new Storage();
            defaultStudent = new Student
            {
                FirstName = "",
                LastName = "",
                IndexNo = "",
                BirthDate = DateTime.Today,
                BirthPlace = ""
            };
            defaultGroup = new Group
            {
                Name = ""
            };
            vm = new StudentsViewModel()
            {
                StudentList = s.getStudents(),
                SelectedStudent = defaultStudent,

            };
            vm.GroupsVM = new GroupsViewModel()
            {
                groupList = s.getGroups(),
                SelectedGroup = defaultGroup
            };
            
        }
        
        //
        // GET: /List/
        public ViewResult Index()
        {
            return View(vm);
            
        }

        [HttpPost]
        public ActionResult Filter(string city, string submit)
        {
            var s = new Storage();
            if(submit == "Clear")
            {
                vm.Filter.SelectedCity = "";
                vm.Filter.SelectedGroup = null;
                vm.SelectedStudent = defaultStudent;
                vm.StudentList = s.getStudents();
                vm.GroupsVM.groupList = s.getGroups();
                vm.GroupsVM.SelectedGroup = defaultGroup;
                return RedirectToAction("Index");
            }
            string name = Request["SelectedGroup.Name"]; 
            vm.Filter.SelectedCity = city;
            Predicate<string> birthPlaceFilter = x => city == "" || x
                           .ToLower().Contains(city.ToLower());
            Predicate<string> groupFilter = x => name == "" || x == name;


            try
            {
                   vm.Filter.SelectedGroup = s.getGroups().Single(g => g.Name == name);
            }
            catch (System.InvalidOperationException)
            {
                
            }
            vm.StudentList = s
                .getStudents()
                .Where(x => birthPlaceFilter(x.BirthPlace)
                    && groupFilter(x.Group.Name));


            return View("Index", vm);
        }

        [HttpPost]
        public ActionResult Select(string button)
        {
            var s = new Storage();
            if (button == null)
                vm.SelectedStudent = null;
            vm.SelectedStudent = s.getStudents()
                .Find(st => st.IDStudent == Convert.ToInt32(button));
            return View("Index", vm);
        }

        public ActionResult Edit(Student st, string edit)
        {
            var s = new Storage();
            logger.Info("modStud");
            if(ModelState.IsValid)
            {
                try 
                {	        
                    st.IDGroup = s.getGroups().Single(g => g.Name == st.Group.Name).IDGroup;
                    switch (edit)
                    {
                        case "Zapisz":
                            s.updateStudent(st);
                            break;
                        case "Nowy":
                            s.createStudent(st.FirstName, st.LastName, st.IndexNo,
                                    st.IDGroup, st.BirthDate, st.BirthPlace);
                            break;
                        case "Usun":
                            s.deleteStudent(st);
                            vm.SelectedStudent = defaultStudent;
                            break;
                     }
                }
                catch (DbUpdateConcurrencyException ex)
                {

                    ModelState.AddModelError(string.Empty,
                    "Rownolegle modyfikowanie!.");
                    logger.Error(ex.Message);
                }
                catch (InvalidOperationException e)
                {
                    ModelState.AddModelError(string.Empty,
                    "Prosze o wybranie wl grupy");
                    logger.Error(e.Message);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty,
                    "Nieoczekiwany wyjatek!");
                    logger.Error(e.Message);
                }
                
            }
          
            
            vm.StudentList = s.getStudents();
            return View("Index", vm);
        }

        public ActionResult Groups()
        {
            return View(vm.GroupsVM);
        }

        public ActionResult GroupEdit(string button)
        {
            var s = new Storage();
            string groupName = Request["SelectedGroup.IDGroup"];
            string groupNewName = Request["SelectedGroup.Name"];


            Group gr = new Group();
            switch (button)
            {
                case "Zapisz":
                    try
                    {
                        gr.IDGroup = s.getGroups().Single(gl => gl.Name == groupName).IDGroup;
                        gr.Name = groupNewName;
                        s.updateGroup(gr);
                    }
                    catch (InvalidOperationException ex)
                    {
                        ModelState.AddModelError(string.Empty, "Nie istnieje takiej grupy!");
                        logger.Error(ex.Message);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError(string.Empty, "Niespodiewany blad! Prosze o pow administratora!");
                        logger.Error(e.Message);
                    }
                        
                    break;
                case "Dodaj":
                        if(groupNewName != "")
                            try
                            {
                                s.createGroup(groupNewName);
                            }
                            catch (Exception e)
                            {
                                if (e.Message.Contains("UniqueConstraint"))
                                {
                                    ModelState.AddModelError(string.Empty, "Nazwa grupy powinna byc unikatowa!");
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Niespodziewany wyjatek!");
                                }
                            }
                        else
                            ModelState.AddModelError(string.Empty, "Nie mozna dodaj grupy o takiej nazwie");
                    break;
                case "Usun":
                    try
                    {
                        gr.IDGroup = s.getGroups().Single(gl => gl.Name == groupName).IDGroup;
                        s.deleteGroup(gr);
                    }
                    catch (InvalidOperationException)
                    {
                        ModelState.AddModelError(string.Empty, "Takiej grupy nie istnieje!");
                    }
                    catch (Exception e)
                    {
                        
                    }
                    break;
            }
            vm.GroupsVM.groupList = s.getGroups();
            return View("Groups", vm.GroupsVM);
        }
    }
}
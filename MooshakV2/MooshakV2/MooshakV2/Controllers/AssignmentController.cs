using Microsoft.AspNet.Identity;
using MooshakV2.DAL;
using MooshakV2.Services;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakV2.Controllers
{
    public class AssignmentController : Controller
    {
        private AssignmentService service;
        private CourseService courseService;
        private SubmissionService submissionService;
        private DatabaseDataContext dbContext;

        public AssignmentController()
        {
            service = new AssignmentService();
            courseService = new CourseService();
            submissionService = new SubmissionService();
            dbContext = new DatabaseDataContext();
        }
        /// <summary>
        /// Shows assignment list view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() { return RedirectToAction("List"); }

        /// <summary>
        /// Shows the create view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create()
        {
            //Creates a viewData
            prepareDropdown();
            return View("AdminTeacherViews/create", new AssignmentViewModel());
        }


        /// <summary>
        /// Adds assignment "newAssignment to the database.
        /// </summary>
        /// <param name="newAssignment"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create(AssignmentViewModel newAssignment)
        {
            // Error Check
            ModelState["id"].Errors.Clear();
            if (ModelState.IsValid)
            {
                AssignmentViewModel assignment = new AssignmentViewModel();
                //Gives assignment a courseId
                assignment.courseId = newAssignment.courseId;
                // If it is able to create a new assignment it redirects to list
                // else it goes back to the create view
                if (service.addAssignment(newAssignment))
                    return RedirectToAction("List");
            }

            return View("AdminTeacherViews/create", newAssignment);

        }
        /// <summary>
        /// Sýnir Assignment List view-ið Shows the Assignment List view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult list()
        {
            // Creates a viewData
            prepareDropdown();
            // Fetches every assignment from database
            var model = service.getAllAssignments();
            // Returns a different view for student and admin/teacher
            if (User.IsInRole("Student"))
                return View("StudentViews/list", model);

            return View("AdminTeacherViews/list", model);
        }

        /// <summary>
        /// Shows assignments that are in course "id"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult listAssignmentsInCourse(string id)
        {
            // Changes string id to int id
            int courseId = Convert.ToInt32(id);
            // Fetches every assignment that are in course "courseId"
            var model = service.getAllAssignmentsInCourse(courseId);
            if (model != null)
            {
                // Creates a viewData
                prepareDropdown();
                // Returns a different view for student and admin/teacher
                if (User.IsInRole("Student"))
                {
                    return View("StudentViews/list", model);
                }
                return View("AdminTeacherViews/list", model);
            }
            return RedirectToAction("List");
        }

        /// <summary>
        /// Shows history for assignment "id"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult showSubmissionsFromAssignment(string id)
        {
            // Changes string id to int id
            int courseId = Convert.ToInt32(id);
            // Fetches every submission from assignment that has "courseId"
            var model = submissionService.getAllSubmissionsFromAssignment(courseId);
            if (model != null)
            {
                // Creates a ViewData
                prepareAssignmentDropdown();
                // Returns history view
                return View("history", model);
            }
            return View("history");
        }

        /// <summary>
        /// Shows assignment view for "id"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(int? id)
        {
            // Checks if id is null
            if (id.HasValue)
            {
                //  Creates a new AssignmentViewModel from "id"
                var model = service.getAssignmentById(id);

                if (model != null)
                {
                    // Creates a new viewData
                    prepareDropdown();
                    // Returns the edit view
                    return View("AdminTeacherViews/edit", model);
                }

            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Changes the assignment that goes in
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(AssignmentViewModel assignment)
        {
            // Creates a viewData
            prepareDropdown();
            // Error Check
            if (ModelState.IsValid)
            {
                // If it updates the assignent it returns the view 
                // otherwise the edit view is returned
                if (service.updateAssignment(assignment))
                    return RedirectToAction("List");
            }

            return View("AdminTeacherViews/edit", assignment);
        }

        /// <summary>
        /// Shows the remove view for assignemtn "id"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(int? id)
        {
            // Check if id is null
            if (id.HasValue)
            {
                // Creates a viewmodel for assignment "id"
                var toRemove = service.getAssignmentById(id);
                // Checks if it can remove, redirects to list if it can
                if (toRemove != null)
                    return View("AdminTeacherViews/remove", toRemove);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Removes the "toRemove" assignment
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(AssignmentViewModel toRemove)
        {
            // Checks if it can remove the assignment, redirects to error if it fails, otherwise
            // back to list
            if (service.removeAssignment(toRemove.id))
                return RedirectToAction("List");

            return RedirectToAction("Error");

        }

        /// <summary>
        /// Shows detail view for "id"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult details(int? id)
        {
            // Check if id is null
            if (id.HasValue)
            {
                // Creates a viewModel by id
                var model = service.getAssignmentById(id);
                // Check if user is student, return to student view detail
                // else goes to admin/teacher student view detail
                if (User.IsInRole("Student"))
                    return View("StudentViews/details", model);

                return View("AdminTeacherViews/details", model);
            }
            return RedirectToAction("Error");
        }

        /// <summary>
        /// Shows submission history for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult userHistory()
        {
            // Fetches current user id
            var userId = User.Identity.GetUserId();

            // Creates a viewmodel containing history for the current user
            var model = submissionService.getAllHistoryViewModelsByID(userId);

            return View("userHistory", model);
        }

        /// <summary>
        /// Shows the error view
        /// </summary>
        /// <returns></returns>
        public ActionResult error() { return View(); }
        
        /// <summary>
        /// Creates viewdata for every course in the database
        /// </summary>
        private void prepareDropdown()
        {
            // Fetches every course
            var courseList = courseService.getAllCourses();
            // Creates a new list
            List<SelectListItem> courseDropDown = new List<SelectListItem>();

            // Adds every course title and id to the list
            foreach (var item in courseList)
            {
                courseDropDown.Add(new SelectListItem { Text = item.title, Value = item.id.ToString() });
            }

            // Assigns the list to a viewData
            ViewData["Courselist"] = courseDropDown;
        }

        /// <summary>
        /// Creates a viewData for every assignment
        /// </summary>
        private void prepareAssignmentDropdown()
        {
            // Fetches every assignment in the database
            var assignmentList = service.getAllAssignments();
            // Creates a new list
            List<SelectListItem> assignmentDropDown = new List<SelectListItem>();

            // Adds every assignment title and id to the list
            foreach (var item in assignmentList)
            {
                assignmentDropDown.Add(new SelectListItem { Text = item.title, Value = item.id.ToString() });
            }

            // Assigns the list to a viewData
            ViewData["Assignmentlist"] = assignmentDropDown;
        }


        /// <summary>
        /// Shows the upload view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult uploadFile()
        {
            return View();
        }
        
        /// <summary>
        /// Adds a new assignmentpart to "model"
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addPart(AssignmentViewModel model)
        {
            prepareDropdown();
            // Checks if model is null
            if (model != null)
            {
                // If assignment isn't in DB, create the assignment
                if (model.id <= 0)
                {
                    // Checks if it can add part to assignment then redirects to the correct view
                    if (true)
                    {
                        AssignmentViewModel assignment = new AssignmentViewModel();
                        assignment.courseId = model.courseId;
                        if (service.addAssignment(model))
                            return RedirectToAction("List");
                    }
                    return View("AdminTeacherViews/create", model);
                }

                service.addPart(model.assignmentParts[0], model.id);
                var updatedModel = service.getAssignmentById(model.id);
                // just return same model, it contains all necessary information
                return View("AdminTeacherViews/edit", updatedModel);
            }
            return View("Error");
        }

        /// <summary>
        /// Saves file locally and adds a new submission to the database
        /// </summary>
        /// <param name="theFile"></param>
        /// <returns></returns>
        public ActionResult uploadFile(FileUploadViewModel theFile)
        {
            // Check if model state is valid
            if (ModelState.IsValid)
            {
                // Fetches the newest id
                var id = (from i in dbContext.submissions
                          orderby i.Id descending
                          select i.Id).FirstOrDefault();
                // Creates a string containing the fileExtension
                var fileExtension = Path.GetExtension(theFile.file.FileName);
                // Checks if extension is .cpp returns to list if it isn't
                if(fileExtension != ".cpp")
                {
                    return RedirectToAction("list");
                }
                // Creates a string containging the filename as id + extension
                var fileName = id.ToString() + fileExtension;

                // Where the file is saved locally
                var path = Path.Combine(Server.MapPath("~/AllFiles"), fileName);
                // Saves the file
                theFile.file.SaveAs(path);
                // Creates a new database entry
                service.submitFile(theFile, User.Identity.GetUserId());
            }

            return RedirectToAction("list");
        }


        /// <summary>
        /// Removes the assignment view "model"
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult delPart(AssignmentViewModel model)
        {
            // Creates a viewData
            prepareDropdown();
            // Checks if model and assignemtnpart is null
            if(model != null && model.assignmentParts != null)
            {
                // Check if its removing the last assignmentpart
                if(service.removePart(model.assignmentParts[0]))
                {
                    var updatedModel = service.getAssignmentById(model.id);
                    return View("AdminTeacherViews/edit", updatedModel);
                }
            }
            return View("Error");
        }


        /// <summary>
        /// Show the history view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult history()
        {
            //Creates a viewData
            prepareAssignmentDropdown();
            // Model containing every submission
            var model = submissionService.getAllHistoryViewModels();

            return View("history", model);
        }

        [HttpPost]
        public ActionResult Compiler(FormCollection data)
        {
            // To simplify matters, we declare the code here.
            // The code would of course come from the student!
            var code = "#include <iostream>\n" +
                    "using namespace std;\n" +
                    "int main()\n" +
                    "{\n" +
                    "cout << \"Hello world\" << endl;\n" +
                    "cout << \"The output should contain two lines\" << endl;\n" +
                    "return 0;\n" +
                    "}";

            // Set up our working folder, and the file names/paths.
            // In this example, this is all hardcoded, but in a
            // real life scenario, there should probably be individual
            // folders for each user/assignment/milestone.
            var workingFolder = "C:\\Temp\\Mooshak2Code\\";
            var cppFileName = "Hello.cpp";
            var exeFilePath = workingFolder + "Hello.exe";

            // Write the code to a file, such that the compiler
            // can find it:
            System.IO.File.WriteAllText(workingFolder + cppFileName, code);

            // In this case, we use the C++ compiler (cl.exe) which ships
            // with Visual Studio. It is located in this folder:
            var compilerFolder = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\bin\\";
            // There is a bit more to executing the compiler than
            // just calling cl.exe. In order for it to be able to know
            // where to find #include-d files (such as <iostream>),
            // we need to add certain folders to the PATH.
            // There is a .bat file which does that, and it is
            // located in the same folder as cl.exe, so we need to execute
            // that .bat file first.

            // Using this approach means that:
            // * the computer running our web application must have
            //   Visual Studio installed. This is an assumption we can
            //   make in this project.
            // * Hardcoding the path to the compiler is not an optimal
            //   solution. A better approach is to store the path in
            //   web.config, and access that value using ConfigurationManager.AppSettings.

            // Execute the compiler:
            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            compiler.Start();
            compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
            compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + cppFileName);
            compiler.StandardInput.WriteLine("exit");
            string output = compiler.StandardOutput.ReadToEnd();
            compiler.WaitForExit();
            compiler.Close();

            // Check if the compile succeeded, and if it did,
            // we try to execute the code:
            if (System.IO.File.Exists(exeFilePath))
            {
                var processInfoExe = new ProcessStartInfo(exeFilePath, "");
                processInfoExe.UseShellExecute = false;
                processInfoExe.RedirectStandardOutput = true;
                processInfoExe.RedirectStandardError = true;
                processInfoExe.CreateNoWindow = true;
                using (var processExe = new Process())
                {
                    processExe.StartInfo = processInfoExe;
                    processExe.Start();
                    // In this example, we don't try to pass any input
                    // to the program, but that is of course also
                    // necessary. We would do that here, using
                    // processExe.StandardInput.WriteLine(), similar
                    // to above.

                    // We then read the output of the program:
                    var lines = new List<string>();
                    while (!processExe.StandardOutput.EndOfStream)
                    {
                        lines.Add(processExe.StandardOutput.ReadLine());
                    }

                    ViewBag.Output = lines;
                }
            }

            // TODO: We might want to clean up after the process, there
            // may be files we should delete etc.

            return View();
        }
    }
}
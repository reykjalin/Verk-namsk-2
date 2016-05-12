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

        public ActionResult Index() { return RedirectToAction("List"); }
        // Create assignment
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create()
        {
            prepareDropdown();
            return View("AdminTeacherViews/create", new AssignmentViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult create(AssignmentViewModel newAssignment)
        {
            ModelState["id"].Errors.Clear();
            if (ModelState.IsValid)
            {
                AssignmentViewModel assignment = new AssignmentViewModel();
                assignment.courseId = newAssignment.courseId;
                if (service.addAssignment(newAssignment))
                    return RedirectToAction("List");
            }

            return View("AdminTeacherViews/create", newAssignment);

        }

        [HttpGet]
        [Authorize]
        public ActionResult list()
        {
            prepareDropdown();
            var model = service.getAllAssignments();
            if (User.IsInRole("Student"))
                return View("StudentViews/list", model);

            return View("AdminTeacherViews/list", model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult listAssignmentsInCourse(string id)
        {
            int courseId = Convert.ToInt32(id);
            var model = service.getAllAssignmentsInCourse(courseId);
            if (model != null)
            {
                prepareDropdown();
                if (User.IsInRole("Student"))
                    return View("StudentViews/list", model);


                return View("AdminTeacherViews/list", model);
            }
            return RedirectToAction("List");
        }

        //Change assignment
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(int? id)
        {
            if (id.HasValue)
            {
                var model = service.getAssignmentById(id);

                if (model != null)
                {
                    prepareDropdown();
                    return View("AdminTeacherViews/edit", model);
                }

            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult edit(AssignmentViewModel assignment)
        {
            prepareDropdown();
            if (ModelState.IsValid)
            {
                if (service.updateAssignment(assignment))
                    return RedirectToAction("List");
            }

            return View("AdminTeacherViews/edit", assignment);
        }

        //Remove assignment
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(int? id)
        {
            if (id.HasValue)
            {
                var toRemove = service.getAssignmentById(id);
                if (toRemove != null)
                    return View("AdminTeacherViews/remove", toRemove);
            }
            return RedirectToAction("Error");
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult remove(AssignmentViewModel toRemove)
        {
            if (service.removeAssignment(toRemove.id))
                return RedirectToAction("List");

            return RedirectToAction("Error");

        }

        //Get details about the given id
        [HttpGet]
        [Authorize]
        public ActionResult details(int? id)
        {
            if (id.HasValue)
            {
                var model = service.getAssignmentById(id);
                if (User.IsInRole("Student"))
                    return View("StudentViews/details", model);

                return View("AdminTeacherViews/details", model);
            }
            return RedirectToAction("Error");
        }

        public ActionResult error() { return View(); }
        

        private void prepareDropdown()
        {
            var courseList = courseService.getAllCourses();
            List<SelectListItem> courseDropDown = new List<SelectListItem>();

            foreach (var item in courseList)
                courseDropDown.Add(new SelectListItem { Text = item.title, Value = item.id.ToString() });

            ViewData["Courselist"] = courseDropDown;
        }

        [HttpGet]
        public ActionResult uploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addPart(AssignmentViewModel model)
        {
            prepareDropdown();
            // Villucheck á model
            if(model != null)
            {
                // If assignment isn't in DB, create the assignment
                if(model.id <= 0)
                {
                    // TODO: Setja þetta í fall? alveg eins og í create...
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
        public ActionResult uploadFile(FileUploadViewModel theFile)
        {

            if (ModelState.IsValid)
            {
                service.submitFile(theFile, User.Identity.GetUserId());

                var id = (from i in dbContext.submissions
                          orderby i.Id descending
                          select i.Id).FirstOrDefault();
                var fileExtension = Path.GetExtension(theFile.file.FileName);
                var fileName = id.ToString() + fileExtension;

                var path = Path.Combine(Server.MapPath("~/AllFiles"), fileName);

                theFile.file.SaveAs(path);

            }

            return RedirectToAction("list");
        }


        [HttpPost]
        public ActionResult delPart(AssignmentViewModel model)
        {
            prepareDropdown();
            if(model != null && model.assignmentParts != null)
            {
                if(service.removePart(model.assignmentParts[0]))
                {
                    var updatedModel = service.getAssignmentById(model.id);
                    return View("AdminTeacherViews/edit", updatedModel);
                }
            }
            return View("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult history()
        {
            var model = submissionService.getAllSubmissions();

            return View("history", model);
        }


        public ActionResult Compiler()
        {
            return View("compiler");
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
            var workingFolder = "C:\\Users\\Jón\\Google Drive\\2.önn.2016\\Verk-namsk-2\\MooshakV2\\MooshakV2\\MooshakV2\\Test\\";
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
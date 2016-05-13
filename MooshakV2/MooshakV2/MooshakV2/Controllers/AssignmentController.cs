using Microsoft.AspNet.Identity;
using MooshakV2.DAL;
using MooshakV2.Services;
using MooshakV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
                {
                    return View("StudentViews/list", model);
                }
                return View("AdminTeacherViews/list", model);
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult showSubmissionsFromAssignment(string id)
        {
            int courseId = Convert.ToInt32(id);
            var model = submissionService.getAllSubmissionsFromAssignment(courseId);
            if (model != null)
            {
                prepareAssignmentDropdown();
                return View("history", model);
            }
            return View("history");
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
            {
                courseDropDown.Add(new SelectListItem { Text = item.title, Value = item.id.ToString() });
            }

            ViewData["Courselist"] = courseDropDown;
        }

        private void prepareAssignmentDropdown()
        {
            var assignmentList = service.getAllAssignments();
            List<SelectListItem> assignmentDropDown = new List<SelectListItem>();

            foreach (var item in assignmentList)
            {
                assignmentDropDown.Add(new SelectListItem { Text = item.title, Value = item.id.ToString() });
            }

            ViewData["Assignmentlist"] = assignmentDropDown;
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
            if (model != null)
            {
                // If assignment isn't in DB, create the assignment
                if (model.id <= 0)
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
            }
            return View("Error");
        }

        public ActionResult uploadFile(FileUploadViewModel theFile)
        {

            if (ModelState.IsValid)
            {
                var id = (from i in dbContext.submissions
                          orderby i.Id descending
                          select i.Id).FirstOrDefault();
                var fileExtension = Path.GetExtension(theFile.file.FileName);
                if(fileExtension != ".cpp")
                {
                    return RedirectToAction("list");
                }
                var fileName = id.ToString() + fileExtension;

                var path = Path.Combine(Server.MapPath("~/AllFiles"), fileName);
                theFile.file.SaveAs(path);

                service.submitFile(theFile, User.Identity.GetUserId());
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
            prepareAssignmentDropdown();
            var model = submissionService.getAllHistoryViewModels();

            return View("history", model);
        }

        //Did not manage to get the compiler to work properly, we first tried to implement it in AssignmentService.cs and then after same failed tries we
        //tried to implement it in AssignmentController.cs but we were to short on time.
        //This is the soon to be Compiler.
        [HttpPost]
        public void checkSuccess(FileUploadViewModel upload)
        {

            var path = Path.GetDirectoryName("~/AllFiles/");
            var workingFolder = path + "\\Temp\\";
            var fileName = upload.file.FileName;
            var noExtension = Path.GetFileNameWithoutExtension(fileName);
            var exeFilePath = workingFolder + noExtension + ".exe";

            var compilerFolder = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\bin\\";

            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            compiler.Start();
            compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
            compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + fileName);
            compiler.StandardInput.WriteLine("exit");
            string output = compiler.StandardOutput.ReadToEnd();
            compiler.WaitForExit();
            compiler.Close();

            var input = service.getInput(upload);

            var expectedOutput = service.getOutput(upload);

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
                    processExe.StandardInput.WriteLine(input);
                    processExe.StandardInput.Close();

                    // We then read the output of the program:
                    var lines = new List<string>();
                    while (!processExe.StandardOutput.EndOfStream)
                    {
                        lines.Add(processExe.StandardOutput.ReadLine());
                    }

                    StringBuilder builder = new StringBuilder();
                    foreach (string line in lines) // Loop through all strings
                    {
                        builder.Append(line); // Append string to StringBuilder
                    }
                    string result = builder.ToString(); // Get string from StringBuilder

                    if (result == expectedOutput)
                    {
                     
                    }

                }
            }

            return;
        }
    }
}
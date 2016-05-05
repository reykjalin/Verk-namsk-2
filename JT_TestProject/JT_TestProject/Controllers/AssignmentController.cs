using JT_TestProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JT_TestProject.Controllers
{
    public class AssignmentController : Controller
    {
		private AssignmentsServices _service = new AssignmentsServices();
        // GET: Assignment
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Details(int id)
		{
			var viewModel = _service.GetAssignmentByID(id);

			return View(viewModel);
		}
    }
}
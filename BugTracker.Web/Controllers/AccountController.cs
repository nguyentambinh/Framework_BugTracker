using System.Web.Mvc;
using BugTracker.Core.Dtos;
using BugTracker.Core.Interfaces;

namespace BugTracker.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserContext _userContext;

        public AccountController(
            IAuthService authService,
            IUserContext userContext)
        {
            _authService = authService;
            _userContext = userContext;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _authService.Login(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return RedirectToAction("Index", "Bug");
        }

        public ActionResult Logout()
        {
            _userContext.SignOut();
            return RedirectToAction("Login");
        }
    }
}
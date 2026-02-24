using System;
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            if (_authService == null || _userContext == null)
                throw new Exception("Unity chưa inject");

            return View(new LoginDto());
        }

        [AllowAnonymous]
        [HttpPost]
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
using AutoMapper;
using BLL.Filter;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Implementation;
using UI.Models;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading;

namespace UI.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private AppRoleManager RoleManager
        {
            get
            {
                return AppRoleManager.Create(null, HttpContext.GetOwinContext());
            }
        }
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public RoomsController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        public ActionResult Index(string search)
        {
            SetViewBag();

            if (Session["RoomsFilter"] != null)
            {
                Session["RoomsFilter"] = null;
            }

            Response.Cookies["id"].Value = Guid.NewGuid().ToString();

            var serv = _roomService.GetAllQuestRooms(null).ToList();

            var rooms = _mapper.Map<List<RoomViewModel>>(serv);
            if (String.IsNullOrEmpty(search))
            {
                return View(rooms);
            }

            return View(rooms.Where(x => x.Name.ToLower().Contains(search.ToLower()) || x.CompanyName.ToLower().Contains(search.ToLower())).ToList());
        }

        private void SetViewBag()
        {
            ViewBag.Types = _roomService.GetTypes();
            ViewBag.Ratings = _roomService.GetRatings();
            ViewBag.Companies = _roomService.GetCompanies();
        }

        public ActionResult Filter(string type, string value)
        {
            var filter = new RoomsFilter()
            {
                Name = value,
                Type = type
            };

            if (type == "type")
            {
                filter.Predicate = (x => x.DecorationType.Name == value);
            }
            else if (type == "rating")
            {
                filter.Predicate = (x => Math.Floor(x.Rating).ToString() == value);
            }
            else if (type == "company")
            {
                filter.Predicate = (x => x.Company.Name == value);
            }

            var filters = new List<RoomsFilter>();
            if (Session["RoomsFilter"] != null)
            {
                filters = Session["RoomsFilter"] as List<RoomsFilter>;
            }

            var found = filters.FirstOrDefault(f => f.Name == value && f.Type == type);
            if (found != null)
            {
                filters.Remove(found);
            }
            else
            {
                filters.Add(filter);
            }

            Session["RoomsFilter"] = filters;

            var games = _roomService.GetAllQuestRooms(filters);
            SetViewBag();

            var resultGames = _mapper.Map<List<RoomViewModel>>(games);

            if (resultGames.Count > 0)
                return PartialView("RoomsPartial", resultGames);
            else
                return PartialView("NotFound");
        }

        public ActionResult Room(int id)
        {
            var foundRoom = _roomService.GetRoom(id);
            ViewBag.Phone = foundRoom.Company.Phone;
            var mappedRoom = _mapper.Map<RoomViewModel>(foundRoom);

            return View("RoomDetails", mappedRoom);
        }

        [HttpGet]
        public ActionResult AddRoom()
        {
            ViewBag.Types = _roomService.GetTypes();
            ViewBag.Companies = _roomService.GetCompanies();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddRoom(RoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Types = _roomService.GetTypes();
                ViewBag.Companies = _roomService.GetCompanies();
                return View();
            }

            var roomToAdd = _mapper.Map<QuestRoom>(model);

            int? validationTypeId = _roomService.GetAllTypes().Where(x => x.Name == roomToAdd.DecorationType.Name).Select(x => x.Id).FirstOrDefault();
            int? validationCompanyId = _roomService.GetAllCompanies().Where(x => x.Name == roomToAdd.Company.Name).Select(x => x.Id).FirstOrDefault();

            if (validationTypeId != null && validationCompanyId != null)
            {
                roomToAdd.DecorationTypeId = int.Parse(validationTypeId.ToString());
                roomToAdd.DecorationType = null;

                roomToAdd.CompanyId = int.Parse(validationCompanyId.ToString());
                roomToAdd.Company = null;

                await _roomService.AddRoomAsync(roomToAdd);

                return RedirectToAction("Index");
            }

            return View("Error");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult UploadImage()
        {
            string fileName = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];

                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                if (!Directory.Exists(Server.MapPath("~/Files/Images")))
                    Directory.CreateDirectory(Server.MapPath("~/Files/Images"));

                file.SaveAs(Server.MapPath("~/Files/Images/" + fileName));
            }

            return Json("/Files/Images/" + fileName);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditRoom(int id)
        {
            var foundRoom = _mapper.Map<RoomViewModel>(_roomService.GetRoom(id));
            ViewBag.Types = _roomService.GetTypes();
            ViewBag.Companies = _roomService.GetCompanies();
            return View("EditRoom", foundRoom);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> EditRoom(RoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Types = _roomService.GetTypes();
                ViewBag.Companies = _roomService.GetCompanies();
                return View();
            }

            var roomToEdit = _mapper.Map<QuestRoom>(model);

            int? validationTypeId = _roomService.GetAllTypes().Where(x => x.Name == roomToEdit.DecorationType.Name).Select(x => x.Id).FirstOrDefault();
            int? validationCompanyId = _roomService.GetAllCompanies().Where(x => x.Name == roomToEdit.Company.Name).Select(x => x.Id).FirstOrDefault();

            if (validationTypeId != null && validationCompanyId != null)
            {
                roomToEdit.DecorationTypeId = int.Parse(validationTypeId.ToString());
                roomToEdit.DecorationType = null;

                roomToEdit.CompanyId = int.Parse(validationCompanyId.ToString());
                roomToEdit.Company = null;

                await _roomService.EditRoomAsync(roomToEdit);

                return RedirectToAction("Index");
            }

            return View("Error");
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            try
            {
                await _roomService.DeleteRoomAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult ControlDecorations()
        {
            ViewBag.Types = _roomService.GetTypes();
            return View("ControlDecorations");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddDecoration(DecorationTypeViewModel decoration)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Types = _roomService.GetTypes();
                return View("ControlDecorations");
            }

            await _roomService.AddTypeAsync(_mapper.Map<DecorationType>(decoration));
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteDecoration(DecorationTypeViewModel decoration)
        {
            int? validationTypeId = _roomService.GetAllTypes().Where(x => x.Name == decoration.Name).Select(x => x.Id).FirstOrDefault();
            if (validationTypeId != null)
            {
                await _roomService.DeleteTypeAsync(int.Parse(validationTypeId.ToString()));
                return RedirectToAction("Index");
            }
            else
                return View("Error");
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "admin")]
        public ActionResult ControlCompanies()
        {
            ViewBag.Companies = _roomService.GetCompanies();
            return View("ControlCompanies");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddCompany(CompanyViewModel company)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Companies = _roomService.GetCompanies();
                return View("ControlCompanies");
            }

            await _roomService.AddCompanyAsync(_mapper.Map<Company>(company));
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteCompany(CompanyViewModel company)
        {
            int? validationTypeId = _roomService.GetAllCompanies().Where(x => x.Name == company.Name).Select(x => x.Id).FirstOrDefault();
            if (validationTypeId != null)
            {
                await _roomService.DeleteCompanyAsync(int.Parse(validationTypeId.ToString()));
                return RedirectToAction("Index");
            }
            else
                return View("Error");
        }

        public ActionResult Cart(string data)
        {
            if (data != null)
            {
                var orders = data.Replace("[", "").Replace("]", "").Split(',').ToList();

                var foundRooms = _roomService.GetAllQuestRooms(null).Where(x =>
                {
                    foreach (var item in orders)
                    {
                        if (x.Id.ToString() == item)
                            return true;
                    }
                    return false;
                });

                var rooms = _mapper.Map<List<RoomViewModel>>(foundRooms);

                return View("Cart", rooms);
            }
            return View("Error");
        }

        [Authorize(Roles = "admin")]
        public ActionResult ControlUsers()
        {
            var users = _mapper.Map<List<ReadUserViewModel>>(UserManager.Users.ToArray());
            return View("ControlUsers", users);
        }

        [HttpPost]
        [Authorize(Roles = "system")]
        public async Task<JsonResult> DeleteUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                if (!UserManager.IsInRole(id, "system"))
                {
                    IdentityResult result = await UserManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return Json("User was deleted!");
                    }
                    else
                    {
                        return Json("Errors: " + result.Errors);
                    }
                }
                else
                {
                    return Json("Can not delete system admin account.");
                }
            }
            else
            {
                return Json("User not found!");
            }
        }

        [HttpGet]
        [Authorize(Roles = "system")]
        public async Task<ActionResult> EditUser(string id)
        {
            if (!UserManager.IsInRole(id, "system"))
            {
                var user = _mapper.Map<EditUserViewModel>(await UserManager.FindByIdAsync(id));
                if (user != null)
                {
                    var rolesList = RoleManager.Roles.Select(x => x.Name).ToList();
                    if (rolesList.Contains("system"))
                    {
                        rolesList.Remove("system");
                        ViewBag.Roles = rolesList;
                        return View(user);
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View("Error");
        }

        [HttpPost]
        [Authorize(Roles = "system")]
        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                if (!UserManager.IsInRole(model.Id, "system"))
                {
                    if (user.UserName != model.Name)
                        user.UserName = model.Name;
                    if (user.PhoneNumber != model.Phone)
                        user.PhoneNumber = model.Phone;
                    if (user.Email != model.Email)
                        user.Email = model.Email;

                    IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);

                    if (!validEmail.Succeeded)
                    {
                        ModelState.AddModelError("", string.Join(", ", validEmail.Errors));
                    }

                    IdentityResult validPass = null;
                    if (model.Password != string.Empty && model.Password != null && model.Password == model.ConfirmPassword)
                    {
                        validPass = await UserManager.PasswordValidator.ValidateAsync(model.Password);

                        if (validPass.Succeeded)
                        {
                            user.PasswordHash =
                                UserManager.PasswordHasher.HashPassword(model.Password);
                        }
                        else
                        {
                            ModelState.AddModelError("", string.Join(", ", validPass.Errors));
                        }
                    }

                    if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && model.Password != string.Empty && validPass.Succeeded))
                    {
                        if (model.Role == "user")
                        {
                            if (UserManager.IsInRole(user.Id, "admin"))
                                user.Roles.Remove(user.Roles.First(x => x.RoleId == RoleManager.FindByName("admin").Id));
                        }
                        else if (model.Role == "admin")
                        {
                            if (!UserManager.IsInRole(user.Id, "admin"))
                                user.Roles.Add(new IdentityUserRole { RoleId = RoleManager.FindByName("admin").Id, UserId = user.Id });
                        }

                        IdentityResult result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("ControlUsers");
                        }
                        else
                        {
                            ModelState.AddModelError("", string.Join(", ", result.Errors));
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Can not edit system admin account.");
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found.");
            }

            return View(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return View("Error");
            }
            return View("SignUp");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(SignUpUserViewModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return View("Error");
            }
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Name, Email = model.Email, PhoneNumber = model.Phone };
                IdentityResult resultCreate = await UserManager.CreateAsync(user, model.Password);
                if (RoleManager.RoleExists("user"))
                {
                    if (resultCreate.Succeeded)
                    {
                        IdentityResult resultRole = await UserManager.AddToRoleAsync((await UserManager.FindAsync(model.Name, model.Password)).Id, "user");
                        if (resultRole.Succeeded)
                            return RedirectToAction("SignIn");
                        else
                            ModelState.AddModelError("", string.Join(", ", resultRole.Errors));
                    }
                    else
                    {
                        ModelState.AddModelError("", string.Join(", ", resultCreate.Errors));
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return View("Error");
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(SignInUserViewModel details)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return View("Error");
            }

            var user = await UserManager.FindAsync(details.Name, details.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Wrong name or password.");
            }
            else
            {
                ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);
                return RedirectToAction("Index");
            }

            return View(details);
        }

        [Authorize]
        public ActionResult SignOut()
        {
            AuthManager.SignOut();

            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public ActionResult NotRegisteredOrder(string orderContainer)
        {
            ViewBag.OrderContainer = orderContainer;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NotRegisteredOrder(NotRegisteredOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                @ViewBag.OrderContainer = model.OrdersStringJSON;
                return View();
            }

            if (model.OrdersStringJSON == null)
            {
                ModelState.AddModelError("", "Error, empty Local Storage!");
                return View(model);
            }
            else
            {
                var orderContainer = new OrderContainer { DateTime = DateTime.Now, IsAccepted = false, NotRegisteredUser = true, Phone = model.Phone };

                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    ICollection<Order> orders = js.Deserialize<ICollection<Order>>(model.OrdersStringJSON);
                    orderContainer.Order = orders;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Local Storage error!");
                    return View(model);
                }

                await _roomService.AddOrderContainerAsync(orderContainer);

                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<JsonResult> RegisteredOrder(string OrdersStringJSON)
        {
            var orderContainer = new OrderContainer { DateTime = DateTime.Now, IsAccepted = false, NotRegisteredUser = false, UserId = User.Identity.GetUserId() };

            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                ICollection<Order> orders = js.Deserialize<ICollection<Order>>(OrdersStringJSON);
                orderContainer.Order = orders;
            }
            catch (Exception)
            {
                return Json("Local Storage error!");
            }

            await _roomService.AddOrderContainerAsync(orderContainer);

            return Json("Successful order!");
        }

        [Authorize(Roles = "user")]
        public ActionResult Cabinet()
        {
            if (Request.IsAuthenticated)
            {
                var orderContainers = _mapper.Map<List<OrderContainerViewModel>>(_roomService.GetAllOrderContainers().Where(x => x.UserId == User.Identity.GetUserId()));
                if (orderContainers.Count > 0)
                    return View(orderContainers);
                return View();
            }
            return View("Error");
        }

        [Authorize(Roles = "user")]
        public ActionResult OrderDetails(int id)
        {
            var orderContainer = _roomService.GetOrderContainer(id);
            if (orderContainer != null)
            {
                if (orderContainer.UserId == User.Identity.GetUserId() || User.IsInRole("admin"))
                {
                    var orderList = _mapper.Map<List<OrderDetailsViewModel>>(orderContainer.Order);
                    return View("OrderDetails", orderList);
                }
            }
            return View("Error");
        }

        [Authorize(Roles = "admin")]
        public ActionResult ControlOrderContainers()
        {
            var orderContainers = _mapper.Map<List<OrderContainerViewModel>>(_roomService.GetAllOrderContainers());
            if (orderContainers.Count > 0)
                return View(orderContainers);
            return View();
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AcceptOrder(int id)
        {
            var orderContainer = _roomService.GetOrderContainer(id);
            if (orderContainer != null)
            {
                if (orderContainer.IsAccepted != true)
                {
                    orderContainer.IsAccepted = true;
                    await _roomService.EditOrderContainerAsync(orderContainer);
                }
                return RedirectToAction("ControlOrderContainers");
            }
            return View("Error");
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var orderContainer = _roomService.GetOrderContainer(id);
            if (orderContainer != null)
            {
                await _roomService.DeleteOrderContainerAsync(id);
                return RedirectToAction("ControlOrderContainers");
            }
            return View("Error");
        }
    }
}
using AutoMapper;
using BLL.Filter;
using BLL.Interfaces;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomsController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        public ActionResult Index(string search)
        {
            SetViewBag();

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
        public ActionResult EditRoom(int id)
        {
            var foundRoom = _mapper.Map<RoomViewModel>(_roomService.GetRoom(id));
            ViewBag.Types = _roomService.GetTypes();
            ViewBag.Companies = _roomService.GetCompanies();
            return View("EditRoom", foundRoom);
        }

        [HttpPost]
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
        public ActionResult ControlDecorations()
        {
            ViewBag.Types = _roomService.GetTypes();
            return View("ControlDecorations");
        }

        [HttpPost]
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
        public ActionResult ControlCompanies()
        {
            ViewBag.Companies = _roomService.GetCompanies();
            return View("ControlCompanies");
        }

        [HttpPost]
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

        public ActionResult OpenCart(string data)
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
    }
}
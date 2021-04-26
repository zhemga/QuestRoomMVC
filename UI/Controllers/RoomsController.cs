using AutoMapper;
using BLL.Filter;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
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

            return View(rooms.Where(x => x.Name.Contains(search)).ToList());
        }

        private void SetViewBag()
        {
            ViewBag.Types = _roomService.GetTypes();
            ViewBag.Ratings = _roomService.GetRatings();
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
                filter.Predicate = (x => x.Type.Name == value);
            }
            else if (type == "rating")
            {
                filter.Predicate = (x => Math.Floor(x.Rating).ToString() == value);
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
            var foundedRoom = _mapper.Map<RoomViewModel>(_roomService.GetRoom(id));
            return View("RoomDetails", foundedRoom);
        }

        public ActionResult AddRoom()
        {
            ViewBag.Types = _roomService.GetTypes();
            return View("AddRoom");
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoomViewModel model)
        {
            // 1) якщо картинка:
            //    2) зберегти картинку на сервер
            // 2.1) конвертувати картинку
            //    3) записати шлях в модель
            if (!ModelState.IsValid)
            {
                return View();
            }

            //if (image != null)
            //{
            //    var fileName = Guid.NewGuid().ToString() + ".jpg";

            //    var bitmap = BitmapConvertor.Convert(image.InputStream, 200, 200);
            //    var serverPath = Server.MapPath($"~/Images/{fileName}");

            //    bitmap.Save(serverPath);
            //    model.Image = $"/Images/{fileName}";
            //}

            //await _gameService.AddGameAsync(_mapper.Map<Game>(model));

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
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
    }
}
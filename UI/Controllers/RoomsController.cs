﻿using AutoMapper;
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

        [HttpGet]
        public ActionResult AddRoom()
        {
            ViewBag.Types = _roomService.GetTypes();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddRoom(RoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Types = _roomService.GetTypes();
                return View();
            }

            await _roomService.AddRoomAsync(_mapper.Map<QuestRoom>(model));

            return RedirectToAction("Index");
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
            var foundedRoom = _mapper.Map<RoomViewModel>(_roomService.GetRoom(id));
            ViewBag.Types = _roomService.GetTypes();
            return View("EditRoom", foundedRoom);
        }

        [HttpPost]
        public async Task<ActionResult> EditRoom(RoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Types = _roomService.GetTypes();
                return View();
            }

            await _roomService.EditRoomAsync(_mapper.Map<QuestRoom>(model));

            return RedirectToAction("Index");
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
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Диплом.Data;
using Диплом.Models;

namespace Диплом.Controllers
{
    public class GameRoomsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GameRoomsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult List()
        {
            IEnumerable<GameRoom> objGameRoomList = _db.GameRooms;

            return View(objGameRoomList);
        }

        public IActionResult Rent(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var gameRoomFromDb = _db.GameRooms.Find(id);

            if (gameRoomFromDb == null)
            {
                return NotFound();
            }

            UserRent userRent = new UserRent()
            {
                gameRoom = gameRoomFromDb,
                rent = new Rent() { orgnizerEmail = LogInUser.email, roomNumber = gameRoomFromDb.number.Value },
                openRent = new OpenRent()
            };

            return View(userRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rent(UserRent obj)
        {
            obj.gameRoom = _db.GameRooms.Find(obj.rent.roomNumber);

            if (obj.rent.timeOf < DateTime.Now)
            {
                ModelState.AddModelError("rent.timeOf", "Указано прошедшее время");
            }

            if (obj.rent.timeOf.HasValue && obj.rent.duration.HasValue)
            {
                switch (obj.rent.timeOf.Value.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        ModelState.AddModelError("rent.timeOf", "Понедельник - выходной!");
                        break;
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        if (obj.rent.timeOf.Value.TimeOfDay < new TimeSpan(16, 0, 0) || new TimeSpan(24, 0, 0) < obj.rent.timeOf.Value.TimeOfDay.Add(new TimeSpan(obj.rent.duration.Value, 0, 0)))
                        {
                            ModelState.AddModelError("rent.timeOf", "Бронь не попадает в график работы");
                        }
                        break;
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        if (obj.rent.timeOf.Value.TimeOfDay < new TimeSpan(14, 0, 0) || new TimeSpan(24, 0, 0) < obj.rent.timeOf.Value.TimeOfDay.Add(new TimeSpan(obj.rent.duration.Value, 0, 0)))
                        {
                            ModelState.AddModelError("rent.timeOf", "Бронь не попадает в график работы");
                        }
                        break;
                }

                List<Rent> sameTimeRent = _db.Rents.Where(rent => obj.rent.timeOf.Value.Date == rent.timeOf.Value.Date && obj.rent.roomNumber == rent.roomNumber).ToList();

                if (sameTimeRent.Count > 0)
                {
                    sameTimeRent = sameTimeRent.Where(rent => obj.rent.timeOf.Value.TimeOfDay < rent.timeOf.Value.TimeOfDay.Add(new TimeSpan(rent.duration.Value, 0, 0)) && rent.timeOf.Value.TimeOfDay < obj.rent.timeOf.Value.TimeOfDay.Add(new TimeSpan(obj.rent.duration.Value, 0, 0))).ToList();

                    if (sameTimeRent.Count > 0)
                    {
                        ModelState.AddModelError("rent.timeOf", "Это время уже забронировано");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                obj.rent.price = _db.GameRooms.Find(obj.rent.roomNumber).rentPrice.Value * obj.rent.duration.Value;

                _db.Add(obj.rent);
                _db.SaveChanges();
                return RedirectToAction("Profile", "User");
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OpenRent(UserRent obj)
        {
            obj.gameRoom = _db.GameRooms.Find(obj.rent.roomNumber);

            if (obj.rent.timeOf < DateTime.Now)
            {
                ModelState.AddModelError("rent.timeOf", "Указано прошедшее время");
            }

            if (obj.rent.timeOf.HasValue && obj.rent.duration.HasValue)
            {
                switch (obj.rent.timeOf.Value.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        ModelState.AddModelError("rent.timeOf", "Понедельник - выходной!");
                        break;
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        if (obj.rent.timeOf.Value.TimeOfDay < new TimeSpan(16, 0, 0) || new TimeSpan(24, 0, 0) < obj.rent.timeOf.Value.TimeOfDay.Add(new TimeSpan(obj.rent.duration.Value, 0, 0)))
                        {
                            ModelState.AddModelError("rent.timeOf", "Бронь не попадает в график работы");
                        }
                        break;
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        if (obj.rent.timeOf.Value.TimeOfDay < new TimeSpan(14, 0, 0) || new TimeSpan(24, 0, 0) < obj.rent.timeOf.Value.TimeOfDay.Add(new TimeSpan(obj.rent.duration.Value, 0, 0)))
                        {
                            ModelState.AddModelError("rent.timeOf", "Бронь не попадает в график работы");
                        }
                        break;
                }

                List<Rent> sameTimeRent = _db.Rents.Where(rent => obj.rent.timeOf.Value.Date == rent.timeOf.Value.Date && obj.rent.roomNumber == rent.roomNumber).ToList();

                if (sameTimeRent.Count > 0)
                {
                    sameTimeRent = sameTimeRent.Where(rent => obj.rent.timeOf.Value.TimeOfDay < rent.timeOf.Value.TimeOfDay.Add(new TimeSpan(rent.duration.Value, 0, 0)) && rent.timeOf.Value.TimeOfDay < obj.rent.timeOf.Value.TimeOfDay.Add(new TimeSpan(obj.rent.duration.Value, 0, 0))).ToList();

                    if (sameTimeRent.Count > 0)
                    {
                        ModelState.AddModelError("rent.timeOf", "Это время уже забронировано");
                    }
                }
            }

            if (obj.gameRoom.places < obj.openRent.peopleCount)
            {
                ModelState.AddModelError("openRent.peopleCount", "Слишком много человек, в комнате недостаточно места");
            }

            if (ModelState.IsValid)
            {
                obj.rent.price = _db.GameRooms.Find(obj.rent.roomNumber).rentPrice.Value * obj.rent.duration.Value;

                _db.Add(obj.rent);
                _db.SaveChanges();

                obj.openRent.rentId = _db.Rents.Where(rent => rent.roomNumber == obj.rent.roomNumber && rent.timeOf == obj.rent.timeOf).First().id;

                _db.Add(obj.openRent);
                _db.SaveChanges();

                return RedirectToAction("Profile", "User");
            }

            return View("Rent", obj);
        }

        public IActionResult OpenRentsList()
        {
            List<OpenRentInfo> openRents = new List<OpenRentInfo>();

            foreach (var openRent in _db.OpenRents)
            {
                openRents.Add(new OpenRentInfo() { id = openRent.rentId, occupiedPlaces = openRent.peopleCount.Value, descriptionOf = openRent.descriptionOf });
            }

            openRents = openRents.Where(openRent => _db.Rents.Find(openRent.id).orgnizerEmail != LogInUser.email).ToList();

            foreach (var openRent in openRents)
            {
                Rent rent = _db.Rents.Find(openRent.id);

                openRent.gameRoom = _db.GameRooms.Find(rent.roomNumber);
                openRent.timeOf = rent.timeOf;
                openRent.duration = rent.duration;
            }

            foreach (var openRent in openRents)
            {
                foreach (var item in _db.Members.Where(member => member.rentId == openRent.id))
                {
                    openRent.occupiedPlaces += item.peopleCount.Value;
                }
            }

            openRents = openRents.Where(openRent => openRent.timeOf.Value > DateTime.Now).ToList();

            return View(openRents);
        }

        public IActionResult JoinToOpenRent(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var openRentFromDb = _db.OpenRents.Where(openRent => openRent.rentId == id).First();

            if (openRentFromDb == null)
            {
                return NotFound();
            }

            var RentFromDb = _db.Rents.Find(openRentFromDb.rentId);

            if (RentFromDb == null)
            {
                return NotFound();
            }

            OpenRentInfo openRent = new OpenRentInfo()
            {
                id = openRentFromDb.rentId,
                gameRoom = _db.GameRooms.Find(RentFromDb.roomNumber),
                occupiedPlaces = openRentFromDb.peopleCount.Value,
                timeOf = RentFromDb.timeOf,
                duration = RentFromDb.duration,
                descriptionOf = openRentFromDb.descriptionOf
            };

            foreach (var item in _db.Members.Where(member => member.rentId == openRent.id))
            {
                openRent.occupiedPlaces += item.peopleCount.Value;
            }

            JoinToRent userRent = new JoinToRent()
            {
                openRentInfo = openRent
            };

            return View(userRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult JoinToOpenRent(JoinToRent obj)
        {
            Rent rent = _db.Rents.Find(obj.openRentInfo.id);
            obj.openRentInfo.gameRoom = _db.GameRooms.Find(rent.roomNumber);

            if (_db.Members.Where(member => member.rentId == obj.openRentInfo.id && member.userEmail == LogInUser.email).Count() > 0)
            {
                ModelState.AddModelError("peopleCount", "Вы уже являетесь участником этой брони");
            }

            int? availablePlaces = obj.openRentInfo.gameRoom.places - obj.openRentInfo.occupiedPlaces;

            if (availablePlaces - obj.peopleCount < 0)
            {
                string word = "мест";

                if (availablePlaces > 0 && availablePlaces < 5)
                {
                    word = "место";

                    if (availablePlaces > 1)
                    {
                        word = "места";
                    }
                }

                ModelState.AddModelError("peopleCount", $"Слишком много человек, в комнате осталось {availablePlaces} {word}");
            }

            if (ModelState.IsValid)
            {
                Member member = new Member()
                {
                    rentId = obj.openRentInfo.id,
                    userEmail = LogInUser.email,
                    peopleCount = obj.peopleCount
                };

                _db.Add(member);
                _db.SaveChanges();
                return RedirectToAction("Profile", "User");
            }

            return View(obj);
        }
    }
}

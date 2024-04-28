using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Диплом.Data;
using Диплом.Models;

namespace Диплом.Controllers
{
    public class AdminToolsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminToolsController(ApplicationDbContext db)
        {
            _db = db;
        }

        //--------------------------------------------------------------------

        public IActionResult GameRoomsTable()
        {
            IEnumerable<GameRoom> objGameRoomList = _db.GameRooms;

            return View(objGameRoomList);
        }

        public IActionResult CreateGameRoom()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateGameRoom(GameRoom obj)
        {
            if (_db.GameRooms.Where(gameRoom => gameRoom.number == obj.number).Count() != 0)
            {
                ModelState.AddModelError("number", "Комната с таким номером уже существует");
            }

            if (ModelState.IsValid)
            {
                _db.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("GameRoomsTable");
            }

            return View(obj);
        }

        public IActionResult EditGameRoom(int? id)
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

            return View(gameRoomFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditGameRoom(GameRoom obj)
        {
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("GameRoomsTable");
            }

            return View(obj);
        }

        public IActionResult DeleteGameRoom(int? id)
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

            return View(gameRoomFromDb);
        }

        [HttpPost, ActionName("DeleteGameRoom")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteGameRoomPOST(int id)
        {
            var gameRoomFromDb = _db.GameRooms.Find(id);

            if (gameRoomFromDb == null)
            {
                return NotFound();
            }

            _db.Remove(gameRoomFromDb);
            _db.SaveChanges();
            return RedirectToAction("GameRoomsTable");
        }

        //--------------------------------------------------------------------

        public IActionResult BoardGamesTable()
        {
            IEnumerable<BoardGame> objGameRoomList = _db.BoardGames;

            return View(objGameRoomList);
        }

        public IActionResult CreateBoardGame()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBoardGame(BoardGame obj)
        {
            if (_db.BoardGames.Where(boardGame => boardGame.nameOf == obj.nameOf).Count() != 0)
            {
                ModelState.AddModelError("nameOf", "Такая настольная игра уже существует");
            }

            if (obj.minPlayers >= obj.maxPlayers)
            {
                ModelState.AddModelError("maxPlayers", "Максимальное количество игроков");
            }

            if (ModelState.IsValid)
            {
                _db.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("BoardGamesTable");
            }

            return View(obj);
        }

        public IActionResult EditBoardGame(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var BoardGameFromDb = _db.BoardGames.Find(id);

            if (BoardGameFromDb == null)
            {
                return NotFound();
            }

            return View(BoardGameFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBoardGame(BoardGame obj)
        {
            if (obj.minPlayers >= obj.maxPlayers)
            {
                ModelState.AddModelError("maxPlayers", "Максимальное количество игроков");
            }

            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("BoardGamesTable");
            }

            return View(obj);
        }

        public IActionResult DeleteBoardGame(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var BoardGameFromDb = _db.BoardGames.Find(id);

            if (BoardGameFromDb == null)
            {
                return NotFound();
            }

            return View(BoardGameFromDb);
        }

        [HttpPost, ActionName("DeleteBoardGame")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBoardGamePOST(string id)
        {
            var BoardGameFromDb = _db.BoardGames.Find(id);

            if (BoardGameFromDb == null)
            {
                return NotFound();
            }

            _db.Remove(BoardGameFromDb);
            _db.SaveChanges();
            return RedirectToAction("BoardGamesTable");
        }

        //--------------------------------------------------------------------

        public IActionResult RentsTable()
        {
            IEnumerable<Rent> objRentList = _db.Rents;

            return View(objRentList);
        }

        public IActionResult CreateRent()
        {
            NewRent newRent = new NewRent()
            {
                users = _db.Users.Where(user => user.roleOf != "admin"),
                gameRooms = _db.GameRooms
            };

            return View(newRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRent(NewRent obj)
        {
            obj.users = _db.Users.Where(user => user.roleOf != "admin");
            obj.gameRooms = _db.GameRooms;

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
                return RedirectToAction("RentsTable");
            }

            return View(obj);
        }

        public IActionResult EditRent(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var RentFromDb = _db.Rents.Find(id);

            if (RentFromDb == null)
            {
                return NotFound();
            }

            NewRent newRent = new NewRent()
            {
                users = _db.Users.Where(user => user.roleOf != "admin"),
                gameRooms = _db.GameRooms,
                rent = RentFromDb
            };

            return View(newRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRent(NewRent obj)
        {
            obj.users = _db.Users.Where(user => user.roleOf != "admin");
            obj.gameRooms = _db.GameRooms;

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

                List<Rent> sameTimeRent = _db.Rents.Where(rent => obj.rent.timeOf.Value.Date == rent.timeOf.Value.Date && obj.rent.roomNumber == rent.roomNumber && rent.id != obj.rent.id).ToList();

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

                _db.Update(obj.rent);
                _db.SaveChanges();
                return RedirectToAction("RentsTable");
            }

            return View(obj);
        }

        public IActionResult DeleteRent(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var RentFromDb = _db.Rents.Find(id);

            if (RentFromDb == null)
            {
                return NotFound();
            }

            NewRent newRent = new NewRent()
            {
                users = _db.Users.Where(user => user.roleOf != "admin"),
                gameRooms = _db.GameRooms,
                rent = RentFromDb
            };

            return View(newRent);
        }

        [HttpPost, ActionName("DeleteRent")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRentPOST(NewRent obj)
        {
            var RentFromDb = _db.Rents.Find(obj.rent.id);

            if (RentFromDb == null)
            {
                return NotFound();
            }

            _db.Remove(RentFromDb);
            _db.SaveChanges();
            return RedirectToAction("RentsTable");
        }

        //--------------------------------------------------------------------

        public IActionResult UsersTable()
        {
            IEnumerable<User> objUserList = _db.Users;

            return View(objUserList);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User obj)
        {
            if (_db.Users.Where(user => user.email == obj.email).Count() != 0)
            {
                ModelState.AddModelError("email", "Этот адрес уже зарегистрирован");
            }

            if (ModelState.IsValid)
            {
                _db.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("UsersTable");
            }

            return View(obj);
        }

        public IActionResult EditUser(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var userFromDb = _db.Users.Find(id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(User obj)
        {
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("UsersTable");
            }

            return View(obj);
        }

        public IActionResult DeleteUser(string id)
        {
            if (id == null || id == string.Empty)
            {
                return NotFound();
            }

            var userFromDb = _db.Users.Find(id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUserPOST(string id)
        {
            var userFromDb = _db.Users.Find(id);

            if (userFromDb == null)
            {
                return NotFound();
            }

            _db.Remove(userFromDb);
            _db.SaveChanges();
            return RedirectToAction("UsersTable");
        }

        //--------------------------------------------------------------------

        public IActionResult OpenRentsTable()
        {
            IEnumerable<OpenRent> objGameRoomList = _db.OpenRents;

            return View(objGameRoomList);
        }

        public IActionResult CreateOpenRent()
        {
            NewOpenRent openRent = new NewOpenRent()
            {
                rents = _db.Rents
            };

            return View(openRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOpenRent(NewOpenRent obj)
        {
            obj.rents = _db.Rents;

            if (_db.OpenRents.Where(openRent => openRent.rentId == obj.openRent.rentId).Count() != 0)
            {
                ModelState.AddModelError("openRent.rentId", "Эта бронь уже открыта");
            }

            if (_db.GameRooms.Find(_db.Rents.Find(obj.openRent.rentId).roomNumber).places < obj.openRent.peopleCount)
            {
                ModelState.AddModelError("openRent.peopleCount", "Слишком много человек, в комнате недостаточно места");
            }

            if (ModelState.IsValid)
            {
                _db.Add(obj.openRent);
                _db.SaveChanges();
                return RedirectToAction("OpenRentsTable");
            }

            return View(obj);
        }

        public IActionResult EditOpenRent(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var openRentFromDb = _db.OpenRents.Find(id);

            if (openRentFromDb == null)
            {
                return NotFound();
            }

            NewOpenRent openRent = new NewOpenRent()
            {
                rents = _db.Rents,
                openRent = openRentFromDb
            };

            return View(openRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOpenRent(NewOpenRent obj)
        {
            obj.rents = _db.Rents;

            if (_db.GameRooms.Find(_db.Rents.Find(obj.openRent.rentId).roomNumber).places < obj.openRent.peopleCount)
            {
                ModelState.AddModelError("openRent.peopleCount", "Слишком много человек, в комнате недостаточно места");
            }

            if (ModelState.IsValid)
            {
                _db.Update(obj.openRent);
                _db.SaveChanges();
                return RedirectToAction("OpenRentsTable");
            }

            return View(obj);
        }

        public IActionResult DeleteOpenRent(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var openRentFromDb = _db.OpenRents.Find(id);

            if (openRentFromDb == null)
            {
                return NotFound();
            }

            NewOpenRent openRent = new NewOpenRent()
            {
                rents = _db.Rents,
                openRent = openRentFromDb
            };

            return View(openRent);
        }

        [HttpPost, ActionName("DeleteOpenRent")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOpenRentPOST(NewOpenRent obj)
        {
            var openRentFromDb = _db.OpenRents.Find(obj.openRent.id);

            if (openRentFromDb == null)
            {
                return NotFound();
            }

            _db.Remove(openRentFromDb);
            _db.SaveChanges();
            return RedirectToAction("OpenRentsTable");
        }

        //--------------------------------------------------------------------

        public IActionResult MembersTable()
        {
            IEnumerable<Member> objMemberList = _db.Members;

            return View(objMemberList);
        }

        public IActionResult CreateMember()
        {
            NewMember newMember = new NewMember()
            {
                users = _db.Users.Where(user => user.roleOf != "admin"),
                openRents = _db.OpenRents
            };

            return View(newMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMember(NewMember obj)
        {
            obj.users = _db.Users.Where(user => user.roleOf != "admin");
            obj.openRents = _db.OpenRents;

            if (obj.member.userEmail == _db.Rents.Find(obj.member.rentId).orgnizerEmail)
            {
                ModelState.AddModelError("member.userEmail", "Этот пользователь является организатором");
            }

            if (_db.Members.Where(member => member.rentId == obj.member.rentId && member.userEmail == obj.member.userEmail).Count() > 0)
            {
                ModelState.AddModelError("member.userEmail", "Этот пользователь уже является участником брони");
            }

            int? occupiedPlaces = obj.openRents.Where(openRents => openRents.rentId == obj.member.rentId).First().peopleCount;

            foreach (var item in _db.Members.Where(member => member.rentId == obj.member.rentId))
            {
                occupiedPlaces += item.peopleCount;
            }

            int? availablePlaces = _db.GameRooms.Find(_db.Rents.Find(obj.member.rentId).roomNumber).places - occupiedPlaces;

            if (availablePlaces - obj.member.peopleCount < 0)
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

                ModelState.AddModelError("member.peopleCount", $"Слишком много человек, в комнате осталось {availablePlaces} {word}");
            }

            if (ModelState.IsValid)
            {
                _db.Add(obj.member);
                _db.SaveChanges();
                return RedirectToAction("MembersTable");
            }

            return View(obj);
        }

        public IActionResult EditMember(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var memberFromDb = _db.Members.Find(id);

            if (memberFromDb == null)
            {
                return NotFound();
            }

            NewMember newMember = new NewMember()
            {
                users = _db.Users.Where(user => user.roleOf != "admin"),
                openRents = _db.OpenRents,
                member = memberFromDb
            };

            return View(newMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMember(NewMember obj)
        {
            obj.users = _db.Users.Where(user => user.roleOf != "admin");
            obj.openRents = _db.OpenRents;

            int? occupiedPlaces = obj.openRents.Where(openRents => openRents.rentId == obj.member.rentId).First().peopleCount;

            foreach (var item in _db.Members.Where(member => member.rentId == obj.member.rentId && member.id != obj.member.id))
            {
                occupiedPlaces += item.peopleCount;
            }

            int? availablePlaces = _db.GameRooms.Find(_db.Rents.Find(obj.member.rentId).roomNumber).places - occupiedPlaces;

            if (availablePlaces - obj.member.peopleCount < 0)
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

                ModelState.AddModelError("member.peopleCount", $"Слишком много человек, в комнате осталось {availablePlaces} {word}");
            }

            if (ModelState.IsValid)
            {
                _db.Update(obj.member);
                _db.SaveChanges();
                return RedirectToAction("MembersTable");
            }

            return View(obj);
        }

        public IActionResult DeleteMember(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var memberFromDb = _db.Members.Find(id);

            if (memberFromDb == null)
            {
                return NotFound();
            }

            NewMember newMember = new NewMember()
            {
                users = _db.Users.Where(user => user.roleOf != "admin"),
                openRents = _db.OpenRents,
                member = memberFromDb
            };

            return View(newMember);
        }

        [HttpPost, ActionName("DeleteMember")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMemberPOST(NewMember obj)
        {
            var memberFromDb = _db.Members.Find(obj.member.id);

            if (memberFromDb == null)
            {
                return NotFound();
            }

            _db.Remove(memberFromDb);
            _db.SaveChanges();
            return RedirectToAction("MembersTable");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Диплом.Data;
using Диплом.Models;

namespace Диплом.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Profile()
        {
            if (String.IsNullOrEmpty(LogInUser.email))
            {
                return NotFound();
            }

            var userFromDb = _db.Users.Find(LogInUser.email);

            if (userFromDb == null)
            {
                return NotFound();
            }

            List<OpenRentInfo> joinRents = new List<OpenRentInfo>();

            foreach (var member in _db.Members.Where(member => member.userEmail == LogInUser.email))
            {
                joinRents.Add(new OpenRentInfo() { id = member.rentId });
            }


            foreach (var joinRent in joinRents)
            {
                OpenRent openRent = _db.OpenRents.Where(openRent => openRent.rentId == joinRent.id).FirstOrDefault();

                if (openRent != null)
                {
                    joinRent.occupiedPlaces = openRent.peopleCount.Value;
                    joinRent.descriptionOf = openRent.descriptionOf;
                }
            }

            foreach (var joinRent in joinRents)
            {
                Rent rent = _db.Rents.Find(joinRent.id);

                joinRent.gameRoom = _db.GameRooms.Find(rent.roomNumber);
                joinRent.timeOf = rent.timeOf;
                joinRent.duration = rent.duration;
            }

            joinRents = joinRents.Where(joinRent => joinRent.timeOf.Value.Date >= DateTime.Now.Date && joinRent.occupiedPlaces != 0).ToList();


            foreach (var joinRent in joinRents)
            {
                foreach (var item in _db.Members.Where(member => member.rentId == joinRent.id))
                {
                    joinRent.occupiedPlaces += item.peopleCount.Value;
                }
            }

            Profile profile = new Profile()
            {
                firstname = userFromDb.firstname,
                lastname = userFromDb.lastname,
                rents = _db.Rents.Where(rent => rent.orgnizerEmail == userFromDb.email && rent.timeOf.Value.Date >= DateTime.Now.Date).Select(_rent => new UserRent() { rent = _rent }).ToList(),
                joinRents = joinRents
            };

            foreach (var item in profile.rents)
            {
                item.gameRoom = _db.GameRooms.Find(item.rent.roomNumber);

                IEnumerable<OpenRent> openRent = _db.OpenRents.Where(openRent => openRent.rentId == item.rent.id);

                if (openRent.Count() > 0)
                {
                    item.openRent = openRent.First();

                    foreach (var member in _db.Members.Where(member => member.rentId == item.rent.id))
                    {
                        item.openRent.peopleCount += member.peopleCount.Value;
                    }
                }
            }

            return View(profile);
        }

        public IActionResult EditFullName()
        {
            if (String.IsNullOrEmpty(LogInUser.email))
            {
                return NotFound();
            }

            var userFromDb = _db.Users.Find(LogInUser.email);

            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFullName(User obj)
        {
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Profile");
            }

            return View(obj);
        }

        public IActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPassword(NewPassword obj)
        {
            IEnumerable<User> account = _db.Users.Where(user => user.email == LogInUser.email && user.passwordOf == obj.currentPassword);

            if (account.Count() == 0)
            {
                ModelState.AddModelError("currentPassword", "Неверный пароль");
            }

            if (ModelState.IsValid)
            {
                account.First().passwordOf = obj.passwordOf;

                _db.Update(account.First());
                _db.SaveChanges();

                return RedirectToAction("Profile");
            }

            return View(obj);
        }

        public IActionResult RentInfo(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var rentFromDb = _db.Rents.Find(id);

            if (rentFromDb == null)
            {
                return NotFound();
            }

            return View(rentFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RentInfo(Rent obj)
        {
            _db.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Profile");
        }

        public IActionResult OpenRentInfo(int? id)
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

            UserRent userRent = new UserRent()
            {
                rent = RentFromDb,
                openRent = _db.OpenRents.Where(openRent => openRent.rentId == RentFromDb.id).First()
            };

            return View(userRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OpenRentInfo(UserRent obj)
        {
            _db.Remove(obj.rent);
            _db.SaveChanges();
            return RedirectToAction("Profile");
        }

        public IActionResult JoinToRentInfo(int? id)
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

            UserRent userRent = new UserRent()
            {
                rent = RentFromDb,
                openRent = _db.OpenRents.Where(openRent => openRent.rentId == RentFromDb.id).First()
            };

            return View(userRent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult JoinToRentInfo(UserRent obj)
        {
            Member memberFromDb = _db.Members.Where(member => member.rentId == obj.rent.id && member.userEmail == LogInUser.email).First();

            if (memberFromDb == null)
            {
                return NotFound();
            }

            _db.Remove(memberFromDb);
            _db.SaveChanges();
            return RedirectToAction("Profile");
        }

        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Authorization(AuthorizedData obj)
        {
            IEnumerable<User> account = _db.Users.Where(user => user.email == obj.email && user.passwordOf == obj.passwordOf);

            if (account.Count() == 0)
            {
                ModelState.AddModelError("email", "Неверная электронная почта или пароль");
            }

            if (ModelState.IsValid)
            {
                LogInUser.email = account.First().email;
                LogInUser.role = account.First().roleOf;

                if (LogInUser.role == "admin")
                {
                    return RedirectToAction("GameRoomsTable", "AdminTools");
                }

                return RedirectToAction("Main", "Home");
            }

            return View(obj);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(RegistrationData obj)
        {
            if (_db.Users.Where(user => user.email == obj.email).Count() != 0)
            {
                ModelState.AddModelError("email", "Этот адрес уже зарегистрирован");
            }

            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    email = obj.email,
                    passwordOf = obj.passwordOf,
                    firstname = obj.firstname,
                    lastname = obj.lastname,
                    roleOf = "user"
                };

                _db.Add(newUser);
                _db.SaveChanges();

                LogInUser.email = newUser.email;
                LogInUser.role = newUser.roleOf;

                return RedirectToAction("Main", "Home");
            }

            return View(obj);
        }

        public IActionResult LogOut()
        {
            LogInUser.email = string.Empty;
            LogInUser.role = "User";
            return RedirectToAction("Main", "Home");
        }
    }
}

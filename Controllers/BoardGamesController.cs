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
    public class BoardGamesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BoardGamesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult List()
        {
            IEnumerable<BoardGame> objBoardGameList = _db.BoardGames;

            FiltredGames objFiltredGames = new FiltredGames();
            objFiltredGames.boardGameList = objBoardGameList;

            foreach (var obj in objBoardGameList.GroupBy(obj => obj.typeOf))
            {
                objFiltredGames.typeOf.Add(obj.Key, false);
            }

            foreach (var obj in objBoardGameList.GroupBy(obj => obj.manufacturer))
            {
                objFiltredGames.manufacturer.Add(obj.Key, false);
            }

            return View(objFiltredGames);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult List(FiltredGames objFiltredGames)
        {
            IEnumerable<BoardGame> objBoardGameList = _db.BoardGames;

            if (!String.IsNullOrEmpty(objFiltredGames.gameName))
            {
                objBoardGameList = objBoardGameList.Where(obj => obj.nameOf.ToLower().Contains(objFiltredGames.gameName.ToLower()));
            }

            if (objFiltredGames.typeOf.ContainsValue(true))
            {
                List<BoardGame> filtredGames = new List<BoardGame>();

                foreach (var item in objFiltredGames.typeOf)
                {
                    if (item.Value)
                    {
                        filtredGames.AddRange(objBoardGameList.Where(obj => obj.typeOf == item.Key));
                    }
                }

                objBoardGameList = filtredGames.Distinct();
            }

            if (objFiltredGames.minPlayers != 0 && objFiltredGames.maxPlayers != 0)
            {
                objBoardGameList = objBoardGameList.Where(obj => obj.minPlayers <= objFiltredGames.maxPlayers && obj.maxPlayers >= objFiltredGames.minPlayers);
            }

            if (objFiltredGames.gameDuration.ContainsValue(true))
            {
                List<BoardGame> filtredGames = new List<BoardGame>();

                foreach (var item in objFiltredGames.gameDuration)
                {
                    if (item.Value)
                    {
                        string[] range = item.Key.Split('-');

                        filtredGames.AddRange(objBoardGameList.Where(obj => obj.avgDuration >= int.Parse(range[0]) && obj.avgDuration <= int.Parse(range[1])));
                    }
                }

                objBoardGameList = filtredGames.Distinct();
            }

            if (objFiltredGames.ageCategory.ContainsValue(true))
            {
                List<BoardGame> filtredGames = new List<BoardGame>();

                foreach (var item in objFiltredGames.ageCategory)
                {
                    if (item.Value)
                    {
                        string[] range = item.Key.Split('-');

                        filtredGames.AddRange(objBoardGameList.Where(obj => obj.ageCategory <= int.Parse(range[1])));
                    }
                }

                objBoardGameList = filtredGames.Distinct();
            }

            if (objFiltredGames.manufacturer.ContainsValue(true))
            {
                List<BoardGame> filtredGames = new List<BoardGame>();

                foreach (var item in objFiltredGames.manufacturer)
                {
                    if (item.Value)
                    {
                        filtredGames.AddRange(objBoardGameList.Where(obj => obj.manufacturer == item.Key));
                    }
                }

                objBoardGameList = filtredGames.Distinct();
            }

            objFiltredGames.boardGameList = objBoardGameList;

            return View(objFiltredGames);
        }

        public IActionResult GamePage(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var boardGame = _db.BoardGames.Find(id);

            if (boardGame == null)
            {
                return NotFound();
            }

            return View(boardGame);
        }
    }
}
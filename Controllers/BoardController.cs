using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class BoardController : Controller {
    private readonly ILogger<BoardController> _logger;
    private IBoardRepository boardRepository;

    public BoardController(ILogger<BoardController> logger, IBoardRepository boardRepository) {
        this.boardRepository = boardRepository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        if(NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        if(UserIsAdmin()) return View(new GetBoardsViewModel(boardRepository.GetAll()));
        var loggedUserId = Convert.ToInt32(HttpContext.Session.GetString("Id"));
        return View(new GetBoardsViewModel(boardRepository.GetByUser(loggedUserId)));
    }

    [HttpGet]
    public IActionResult Add() => View(new AddBoardViewModel());

    [HttpPost]
    public IActionResult Add(AddBoardViewModel board) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var newBoard = new Board() {
            Name = board.Name,
            Description = board.Description,
            OwnerId = board.OwnerId
        };
        try {
            boardRepository.Add(newBoard);
        } catch (Exception e) {
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id) => View(new UpdateBoardViewModel(boardRepository.GetById(id)));

    [HttpPost]
    public IActionResult Update(UpdateBoardViewModel board) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            var targetBoard = boardRepository.GetById(board.Id);
            var updatedBoard = new Board() {
                Id = board.Id,
                OwnerId = targetBoard.OwnerId,
                Name = board.Name,
                Description = board.Description
            };
            boardRepository.Update(board.Id, updatedBoard);
        } catch (Exception e) {
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Delete(int id) {
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try {
            boardRepository.Delete(id);
        } catch (Exception e) {
            _logger.LogError(e.ToString());
        }
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool UserIsAdmin() => HttpContext.Session.GetString("Role") == Enum.GetName(Role.Admin);
    private bool NotLogged() => string.IsNullOrEmpty(HttpContext.Session.GetString("User")); 
}

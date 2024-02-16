using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;
using tl2_tp10_2023_InakiPoch.ViewModels;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class BoardController : Controller {
    private readonly ILogger<BoardController> _logger;
    private IBoardRepository boardRepository;
    private RoleCheck roleCheck;

    public BoardController(ILogger<BoardController> logger, IBoardRepository boardRepository, RoleCheck roleCheck) {
        this.boardRepository = boardRepository;
        this.roleCheck = roleCheck;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        if(roleCheck.IsAdmin()) {
            return View(new GetBoardsViewModel(
                boardRepository.GetByUser(roleCheck.LoggedUserId()),
                boardRepository.GetByTask(roleCheck.LoggedUserId()),
                boardRepository.GetAll()
            ));
        }
        return View(new GetBoardsViewModel(
            boardRepository.GetByUser(roleCheck.LoggedUserId()), 
            boardRepository.GetByTask(roleCheck.LoggedUserId()), 
            new List<Board>()    
        ));
    }

    [HttpGet]
    public IActionResult Add(string errorMessage = null) {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        var model = new AddBoardViewModel(roleCheck.LoggedUserId()) {
            ErrorMessage = errorMessage
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Add(AddBoardViewModel board) {
        try {
            if(!ModelState.IsValid) return RedirectToAction("Index");
            var newBoard = new Board() {
                Name = board.Name,
                Description = board.Description,
                OwnerId = board.OwnerId
            };
            if(boardRepository.BoardExists(newBoard)) {
                throw new Exception("Tablero ya existente");
            }
            boardRepository.Add(newBoard);
            return RedirectToAction("Index");
        } catch (Exception e) {
            board.ErrorMessage = "Tablero ya existente";
            _logger.LogError(e.ToString());
        }
        return View("Add", board);
    }

    [HttpGet]
    public IActionResult Update(int id, string errorMesagge = null) {
        if(roleCheck.NotLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        var model = new UpdateBoardViewModel(boardRepository.GetById(id)) {
            ErrorMessage = errorMesagge
        };
        return View(model);
    }

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
            if(AlreadyExists(updatedBoard, targetBoard)) {
                throw new Exception("No se puede actualizar el tablero. Tablero ya existente");
            }
            boardRepository.Update(board.Id, updatedBoard);
            return RedirectToAction("Index");
        } catch (Exception e) {
            board.ErrorMessage = "No se puede actualizar el tablero. Tablero ya existente";
            _logger.LogError(e.ToString());
        }
        return View("Update", board);
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

    private bool AlreadyExists(Board updatedBoard, Board board) => updatedBoard.Name != board.Name && boardRepository.BoardExists(updatedBoard);

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp09_2023_InakiPoch.Repositories;
using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.Controllers;

public class BoardController : Controller {
    private readonly ILogger<BoardController> _logger;
    BoardRepository boardRepository;

    public BoardController(ILogger<BoardController> logger) {
        boardRepository = new BoardRepository();
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() => View(boardRepository.GetAll());


    [HttpGet]
    public IActionResult Add() => View(new Board());

    [HttpPost]
    public IActionResult Add(Board Board) {
        boardRepository.Add(Board);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id) => View(boardRepository.GetById(id));

    [HttpPost]
    public IActionResult Update(Board board) {
        boardRepository.Update(board.Id, board);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Delete(int id) {
        boardRepository.Delete(id);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using Microsoft.AspNetCore.Mvc;
using minesweeper.Models.Requests;
using minesweeper.Services;

namespace minesweeper.Controllers
{
    [ApiController]
    [Route("")]
    public class MinesweeperController : ControllerBase
    {
        private readonly IGameService _gameService;

        public MinesweeperController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        [Route("/new")]
        public async Task<IActionResult> NewGame([FromBody] NewGameRequest request)
        {
            try
            {
                int width = request.Width;
                int height = request.Height;
                int minesCount = request.Mines_count;

                var obj = await _gameService.NewGame(width, height, minesCount);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return BadRequest(new { error = errorMessage });
            }
        }

        [HttpPost]
        [Route("/turn")]
        public async Task<IActionResult> CheckField([FromBody] CheckFieldRequest request)
        {
            try
            {
                var id = request.Game_id;
                var col = request.Col;
                var row = request.Row;

                await _gameService.CheckField(col, row, id);

                var obj = _gameService.GetVariables(id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                return BadRequest(new { error = errorMessage });
            }
        }
    }


}

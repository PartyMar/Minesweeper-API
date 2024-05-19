

using Microsoft.AspNetCore.Mvc;
using minesweeper.Models.Requests;

namespace minesweeper.Controllers
{
    [ApiController]
    [Route("")]
    public class MinesweeperController : ControllerBase
    {


        [HttpPost]
        [Route("/new")]
        public async Task<IActionResult> NewGame([FromBody] NewGameRequest request)
        {
            try
            {
                int width = request.Width;
                int height = request.Height;
                int minesCount = request.Mines_count;

                string newId = GameLogic.GenerateGameId();
                await GameLogic.NewGameVariables(newId, width, height, minesCount);
                var obj = GameLogic.GetVariables();

                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Произошла непредвиденная ошибка" });
            }
        }

        [HttpPost]
        [Route("/turn")]
        public async Task<IActionResult> CheckField([FromBody] CheckFieldRequest request)
        {
            try
            {
                //var id = request.Game_id;
                var col = request.Col;
                var row = request.Row;

                await GameLogic.CheckField(col, row);

                var obj =  GameLogic.GetVariables();

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

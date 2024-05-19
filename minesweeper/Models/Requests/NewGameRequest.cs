

using System.ComponentModel.DataAnnotations;

namespace minesweeper.Models.Requests

{
    public class NewGameRequest
    {
        [Required] public int Width { get; set; }
        [Required] public int Height { get; set; }
        [Required] public int Mines_count { get; set; }
    }
}

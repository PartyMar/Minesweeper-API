

namespace minesweeper.Models

{
    public class Game
    {
        public int Id { get; set; }

        public string game_id { get; set; } = "";

        public int width { get; set; }
        public int height { get; set; }
        public int mines_count { get; set; }
        public string[] mineField { get; set; } = [];
        public string[] playerField { get; set; } = [];
        public bool completed { get; set; }

    }
}

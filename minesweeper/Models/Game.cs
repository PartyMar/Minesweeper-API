

namespace minesweeper.Models

{
    public class Game
    {
        public string gameId { get; set; } = "";

        public int width { get; set; }
        public int height { get; set; }
        public int mines_count { get; set; }
        public string[][] field { get; set; } = [[]];
        public bool completed { get; set; }

        //public string[][]? player_field { get; set; }
    }
}

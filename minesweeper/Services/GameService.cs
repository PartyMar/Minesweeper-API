using minesweeper.Models;

namespace minesweeper.Services
{
    public interface IGameService
    {
        object GetVariables(string game_id);
        Task<object> NewGame(int newWidth, int newHeight, int newMinesCount);
        Task<string> CheckField(int col, int row, string game_id);
    }

    public class GameService : IGameService
    {
        private readonly DbGamesContext _context;

        public GameService(DbGamesContext context)
        {
            _context = context;
        }


        private Game GetGame(string game_id)
        {
            Game? game = _context.games.SingleOrDefault(g => g.game_id == game_id);

            if (game == null)
            {
                throw new Exception("игра с идентификатором " + game_id + " не была создана или устарела (неактуальна)");
            }
            return game;
        }

        public object GetVariables(string game_id)
        {
            Game game = GetGame(game_id);
            return new
            {
                game.game_id,
                game.width,
                game.height,
                game.mines_count,
                field = ArrayTolist(game.playerField),
                game.completed
            };
        }


        public async Task<object> NewGame(int newWidth, int newHeight, int newMinesCount)
        {

            Game game = new Game();
            List<List<string>> mineField = new List<List<string>>();
            List<List<string>> playerField = new List<List<string>>();

            game.game_id = GenerateGameId();
            game.width = newWidth;
            game.height = newHeight;
            game.mines_count = newMinesCount;
            game.completed = false;
            try
            {
                mineField = await GenerateMinefield(newHeight, newWidth, newMinesCount);
                for (int i = 0; i < newHeight; i++)
                {
                    playerField.Add(Enumerable.Repeat(" ", newHeight).ToList());
                }
            }
            catch (Exception ex)
            {
                return new Exception("Не получилось сгенерировать поле", ex);
            }
            game.mineField = ListToArray(mineField);
            game.playerField = ListToArray(playerField);

            _context.Add(game);
            _context.SaveChanges();
            return new
            {
                game.game_id,
                game.width,
                game.height,
                game.mines_count,
                field = playerField,
                game.completed
            };
        }


        public async Task<string> CheckField(int col, int row, string game_id)
        {
            try
            {
                Game game = GetGame(game_id);
                List<List<string>> mineField = ArrayTolist(game.mineField);
                List<List<string>> playerField = ArrayTolist(game.playerField);

                if (game_id != game.game_id)
                {
                    throw new Exception("игра с идентификатором " + game_id + " не была создана или устарела (неактуальна)");
                }
                if (row < 0 || col < 0 || row >= mineField.Count || col >= mineField[0].Count)
                {
                    throw new Exception("Вне границ");
                }
                if (game.completed)
                {
                    throw new Exception("Игра завершена");
                }
                if (playerField[row][col] != " ")
                {
                    throw new Exception("уже открытая ячейка");
                }


                if (mineField[row][col] == "M")
                {
                    game.completed = true;
                    mineField = mineField.Select(r => r.Select(cell => cell == "M" ? "X" : cell).ToList()).ToList();
                    playerField = mineField;

                    game.mineField = ListToArray(mineField);
                    game.playerField = ListToArray(playerField);
                    _context.SaveChanges();
                    return "Вы проиграли";
                }
                else
                {
                    await Task.Run(() =>
                    {
                        HashSet<string> visited = new HashSet<string>();
                        RevealEmptySpaces(row, col, visited, mineField, playerField);
                    });

                    //Проверка на победу
                    bool win = CheckWinConditions(mineField, playerField);
                    if (win)
                    {
                        playerField = mineField;
                    }

                    game.mineField = ListToArray(mineField);
                    game.playerField = ListToArray(playerField);
                    _context.SaveChanges();
                    return "Показаны клетки";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void RevealEmptySpaces(
            int row,
            int col,
            HashSet<string> visited,
            List<List<string>> mineField,
            List<List<string>> playerField
            )
        {

            if (row < 0 || col < 0 || row >= mineField.Count || col >= mineField[0].Count || visited.Contains($"{row}-{col}"))
            {
                return;
            }
            else if (mineField[row][col] != "0")
            {
                playerField[row][col] = mineField[row][col];
                return;
            }

            visited.Add($"{row}-{col}");
            playerField[row][col] = mineField[row][col];

            RevealEmptySpaces(row - 1, col, visited, mineField, playerField);
            RevealEmptySpaces(row + 1, col, visited, mineField, playerField);
            RevealEmptySpaces(row, col - 1, visited, mineField, playerField);
            RevealEmptySpaces(row, col + 1, visited, mineField, playerField);
        }


        private bool CheckWinConditions(List<List<string>> mineField, List<List<string>> playerField)
        {
            for (var i = 0; i < playerField.Count; i++)
            {
                for (var j = 0; j < playerField[i].Count; j++)
                {
                    if (playerField[i][j] == " " && mineField[i][j] != "M")
                    {
                        return false;
                    }
                }
            }
            return true;
        }




        //Utils
        private string GenerateGameId()
        {
            var uuid = Guid.NewGuid();
            var gameId = $"{uuid.ToString().Substring(0, 8)}-{uuid.ToString().Substring(9, 4)}-{uuid.ToString().Substring(14, 4)}-{uuid.ToString().Substring(19, 4)}-{uuid.ToString().Substring(24)}";

            return gameId.ToUpper();
        }
        private string[] ListToArray(List<List<string>> data)
        {
            string[] resultArray = new string[data.Count];

            for (int i = 0; i < data.Count; i++)
            {
                resultArray[i] = string.Concat(data[i]);
            }
            return resultArray;
        }
        private List<List<string>> ArrayTolist(string[] data)
        {
            List<List<string>> resultList = new List<List<string>>();

            foreach (string item in data)
            {
                List<string> charList = new List<string>();
                foreach (char ch in item)
                {
                    charList.Add(ch.ToString());
                }
                resultList.Add(charList);
            }
            return resultList;
        }
        private async Task<List<List<string>>> GenerateMinefield(int height, int width, int mineCount)
        {
            List<List<string>> minefield = new List<List<string>>();
            Random random = new Random();


            for (int i = 0; i < height; i++)
            {
                List<string> row = new List<string>();
                for (int j = 0; j < width; j++)
                {
                    row.Add("0");
                }
                minefield.Add(row);
            }

            int minesPlaced = 0;
            while (minesPlaced < mineCount)
            {
                int row = random.Next(0, height);
                int col = random.Next(0, width);

                if (minefield[row][col] != "M")
                {
                    minefield[row][col] = "M";
                    minesPlaced++;
                }
            }

            await Task.Run(() =>
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (minefield[i][j] == "M")
                        {
                            continue;
                        }

                        int count = 0;
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                if (i + x >= 0 && i + x < height && j + y >= 0 && j + y < width)
                                {
                                    if (minefield[i + x][j + y] == "M")
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                        minefield[i][j] = count.ToString();
                    }
                }
            }
        );

            return minefield;
        }
    }
}

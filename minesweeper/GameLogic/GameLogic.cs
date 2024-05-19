


namespace minesweeper
{
    public static class GameLogic
    {
        private static string game_id = "";
        private static int width = 0;
        private static int height = 0;
        private static int mines_count = 0;
        private static bool completed = false;
        private static List<List<string>> mineField = new List<List<string>>();
        private static List<List<string>> playerField = new List<List<string>>();

        public static object GetVariables()
        {
            return new
            {
                game_id,
                width,
                height,
                mines_count,
                field = playerField,
                completed
            };
        }

        public static string GenerateGameId()
        {
            var uuid = Guid.NewGuid();
            var gameId = $"{uuid.ToString().Substring(0, 8)}-{uuid.ToString().Substring(9, 4)}-{uuid.ToString().Substring(14, 4)}-{uuid.ToString().Substring(19, 4)}-{uuid.ToString().Substring(24)}";

            return gameId.ToUpper();
        }

        public static async Task<object> NewGameVariables(string newGameId, int newWidth, int newHeight, int newMinesCount)
        {
            game_id = newGameId;
            width = newWidth;
            height = newHeight;
            mines_count = newMinesCount;
            completed = false;
            try
            {
                mineField.Clear();
                playerField.Clear();
                mineField = await GenerateMinefield(height, width, mines_count);
                for (int i = 0; i < height; i++)
                {
                    playerField.Add(Enumerable.Repeat(" ", width).ToList());
                }
            }
            catch (Exception ex)
            {
                return new Exception("Не получилось сгенерировать поле", ex);
            }

            return GetVariables();
        }


        public static async Task<string> CheckField(int col, int row)
        {
            try
            {
                //if (id != gameId)
                //{
                //    throw new Exception("игра с идентификатором " + id + " не была создана или устарела (неактуальна)");
                //}
                if (row < 0 || col < 0 || row >= mineField.Count || col >= mineField[0].Count)
                {
                    throw new Exception("Вне границ");
                }
                if (completed)
                {
                    throw new Exception("Игра завершена");
                }
                if (playerField[row][col] != " ")
                {
                    throw new Exception("уже открытая ячейка");
                }


                if (mineField[row][col] == "M")
                {
                    completed = true;
                    mineField = mineField.Select(r => r.Select(cell => cell == "M" ? "X" : cell).ToList()).ToList();
                    playerField = mineField;
                    return "Вы проиграли";
                }
                else
                {
                    HashSet<string> visited = new HashSet<string>();
                    RevealEmptySpaces(row, col, visited);
                    bool win = CheckWinConditions();
                    if (win)
                    {
                        playerField = mineField;
                    }
                    return "Показаны пустые клетки";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void RevealEmptySpaces(int row, int col, HashSet<string> visited)
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

            RevealEmptySpaces((row - 1), col, visited);
            RevealEmptySpaces((row + 1), col, visited);
            RevealEmptySpaces(row, (col - 1), visited);
            RevealEmptySpaces(row, (col + 1), visited);
        }


        private static bool CheckWinConditions()
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

        private static async Task<List<List<string>>> GenerateMinefield(int height, int width, int mineCount)
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

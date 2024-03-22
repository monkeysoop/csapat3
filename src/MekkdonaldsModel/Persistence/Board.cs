using Mekkdonalds.Simulation;

namespace Mekkdonalds.Persistence;

internal class Board
{
    private Board(int x, int y, string t)
    {
        _board = new IMapObject?[x, y];
        Type = t;
    }

    private readonly IMapObject?[,] _board;

    private readonly string Type;

    public IMapObject? this[int i, int j]
    {
        get
        {
            if (i >= 0 && i < _board.GetLength(0) && j >= 0 && j < _board.GetLength(1))
                return _board[i, j];
            else if ((i < 0 || i >= _board.GetLength(0)) && (j < 0 || j >= _board.GetLength(1)))
                throw new IndexOutOfRangeException("Both indices are out if range");
            else if (i < 0 || i >= _board.GetLength(0))
                throw new IndexOutOfRangeException("The first index is out of range.");

            throw new IndexOutOfRangeException("The second index is out of range.");
        }
        internal set
        {
            if (i >= 0 && i < _board.GetLength(0) && j >= 0 && j < _board.GetLength(1))
                _board[i, j] = value;
            else if ((i < 0 || i >= _board.GetLength(0)) && (j < 0 || j >= _board.GetLength(1)))
                throw new IndexOutOfRangeException("Both indices are out if range");
            else if (i < 0 || i >= _board.GetLength(0))
                throw new IndexOutOfRangeException("The first index is out of range.");
            else
                throw new IndexOutOfRangeException("The second index is out of range.");
        }
    }

    internal static Board Load(string mapFile, string robotFile)
    {
        int h = -1 , w = -1;
        string t = "";
        Board b = new(1,1, "filler");

        using var fs = new StreamReader(mapFile);

        while (!fs.EndOfStream)
        {
            var line = fs.ReadLine() ?? throw new Exception("Empty file while reading from mapfile");

            if (line.StartsWith("type"))
            {
                t = line.Split(' ')[1];
            }
            else if (line.StartsWith("height"))
            {
                if (!int.TryParse(line.Split(' ')[1], out h)) throw new Exception("Wrong number format for height");

                if (h < 1) throw new Exception("Invalid number for height");
            }
            else if (line.StartsWith("width"))
            {
                if (!int.TryParse(line.Split(' ')[1], out w)) throw new Exception("Wrong number format for width");

                if (w < 1) throw new Exception("Invalid number for width");

                b = new(h, w, t);
            }
            else if (line == "map")
            {
                for (int i = 0; i < h; i++)
                {
                    line = fs.ReadLine() ?? throw new Exception("Empty file while reading from mapfile");

                    for (int j = 0; j < w; j++)
                    {
                        if (line[j] != '.')
                        {
                            b._board[i, j] = new Wall(i, j);
                        }
                    }
                }
            }

            throw new Exception("Unknown mapfile format");
        }

        using var fss = new StreamReader(robotFile);

        int l = -1;
        int id = 1;

        while (!fss.EndOfStream)
        {
            var line = fss.ReadLine() ?? throw new Exception("Empty file while reading from robotfile");

            if (!int.TryParse(line, out l)) throw new Exception("Wrong number format");

            var i = l / w;
            var j = l % w;

            b._board[i, j] = new Robot(id++, i, j);
        }

        return b;
    }
}

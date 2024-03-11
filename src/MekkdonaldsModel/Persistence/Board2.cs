#define NO_BORDER_CHEC
using System.Drawing;


namespace MekkdonaldsModel.Persistence
{
    internal class Board2
    {
        public const int EMPTY = 0;
        public const int WALL = 1;
        public const int SEARCHED = 2;
        public const int OPEN = 3;
        private int[] data;

        public int height { get; private set; }
        public int width { get; private set; }

        public Board2(int[] data, int height, int width)
        {
            this.data = new int[height * width];
            for (int i = 0; i < height * width; i++)
            {
                this.data[i] = data[i];
            }
            this.height = height;
            this.width = width;
        }

        public Board2(int[,] data, int height, int width)
        {
            this.data = new int[height * width];
            for (int y = 0; y < height; y++) 
            {
                for (int x = 0; x < width; x++)
                {
                    this.data[y * width + x] = data[y, x]; 
                }
            }

            this.height = height;
            this.width = width;
        }

        public bool IsPositionEmpty(Point position)
        {
#if NO_BORDER_CHECK
            return data[position.Y * width + position.X] == EMPTY;
#else
            return position.X >= 0 &&
                   position.X < width &&
                   position.Y >= 0 &&
                   position.Y < height &&
                   data[position.Y * width + position.X] == EMPTY;      
#endif
        }

        public bool IsPositionOpen(Point position)
        {
#if NO_BORDER_CHECK
            return data[position.Y * width + position.X] == OPEN;
#else
            return position.X >= 0 &&
                   position.X < width &&
                   position.Y >= 0 &&
                   position.Y < height &&
                   data[position.Y * width + position.X] == OPEN;      
#endif
        }

        public void SetPositionEmpty(Point position)
        {
            data[position.Y * width + position.X] = EMPTY;
        }

        public void SetPositionOpen(Point position)
        {
            data[position.Y * width + position.X] = OPEN;
        }

        public bool SetOpenIfEmpty(Point position)
        {
#if NO_BORDER_CHECK
            bool t = data[position.Y * width + position.X] == EMPTY;
#else
            bool t = position.X >= 0 &&
                     position.X < width &&
                     position.Y >= 0 &&
                     position.Y < height &&
                     data[position.Y * width + position.X] == EMPTY;
#endif
            if (t)
            {
                data[position.Y * width + position.X] = OPEN;
            }

            return t;
        }

        public bool SetSearchedIfEmpty(Point position)
        {
#if NO_BORDER_CHECK
            bool t = data[position.Y * width + position.X] == EMPTY;
#else
            bool t = position.X >= 0 &&
                     position.X < width &&
                     position.Y >= 0 &&
                     position.Y < height &&
                     data[position.Y * width + position.X] == EMPTY;
#endif
            if (t)
            {
                data[position.Y * width + position.X] = SEARCHED;
            }

            return t;
        }

    }
}

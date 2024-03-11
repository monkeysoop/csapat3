﻿#define BORDER_CHECK
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

        public int height { get; init; }
        public int width { get; init; }



        public Board2(int height, int width)
        {
            this.data = new int[height * width];
            this.height = height;
            this.width = width;
        }

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



        public void SetSearched(Point position)
        {
#if NO_BORDER_CHECK
            bool t = true;
#else
            bool t = position.X >= 0 &&
                     position.X < width &&
                     position.Y >= 0 &&
                     position.Y < height;
#endif
            if (t)
            {
                data[position.Y * width + position.X] = SEARCHED;
            }
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

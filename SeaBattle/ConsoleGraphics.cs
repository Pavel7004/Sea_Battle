using System;
using System.Collections.Generic;

namespace SeaBattle
{
    static class ConsoleGraphics
    {
        public static void showField(BoardInterface board, int positionLeft)
        {
            int coursorLeft = Console.CursorLeft;
            int coursorTop = Console.CursorTop;
            Console.SetCursorPosition(positionLeft, 0);
            Console.Write("# 1 2 3 4 5 6 7 8 9 10#");
            Console.SetCursorPosition(positionLeft - 1, 1);

            for (int i = 1; i <= 10; i++)
            {
                string label = (i / 10 == 1)? $"{i}" : $" {i}";
                Console.Write(label);
                for (int j = 1; j <= 10; j++)
                {
                    Position position = new Position(j, i);
                    try
                    {
                        switch (board.getStatus(position))
                        {
                            case ShipStatus.INTACT:
                                Console.Write(" *");
                                break;
                            case ShipStatus.HIT:
                                Console.Write(" X");
                                break;
                            case ShipStatus.SUNK:
                                Console.Write(" #");
                                break;
                        }
                    }
                    catch(InvalidPositionException e)
                    {
                        Console.Write("  ");
                    }
                }
                Console.Write($" {i}");
                Console.SetCursorPosition(positionLeft - 1, i + 1);
            }
            Console.CursorLeft += 1;
            Console.Write("# 1 2 3 4 5 6 7 8 9 10#");
            Console.SetCursorPosition(coursorLeft, coursorTop);
        }
        
        public static void showShootedField(Dictionary<Position, ShotStatus> shots, int cursourPositionLeft)
        {
            int coursorLeft = Console.CursorLeft;
            int coursorTop = Console.CursorTop;

            //Console.SetCursorPosition(cursourPositionLeft, 3);
            //Console.Write("X");

            Console.SetCursorPosition(cursourPositionLeft, 4);
            Console.Write("# 1 2 3 4 5 6 7 8 9 10#");
            Console.SetCursorPosition(cursourPositionLeft - 1 , 5);

            for (int i = 1; i <= 10; i++)
            {
                string label = (i / 10 == 1) ? $"{i}" : $" {i}";
                Console.Write(label);
                for (int j = 1; j <= 10; j++)
                {
                    Console.Write("  ");
                }
                Console.Write($" {i}");
                Console.SetCursorPosition(cursourPositionLeft - 1, i + 5);
            }
            Console.CursorLeft += 1;
            Console.Write("# 1 2 3 4 5 6 7 8 9 10#");

            foreach (KeyValuePair<Position, ShotStatus> kvp in shots)
            {
                Position pos = kvp.Key;
                Console.SetCursorPosition(cursourPositionLeft + pos.getY()*2 - 1, pos.getX() + 4);
                switch (kvp.Value)
                {
                    case ShotStatus.HIT:
                        Console.Write(" X");
                        break;
                    case ShotStatus.MISS:
                        Console.Write(" $");
                        break;
                    case ShotStatus.SUNK:
                        Console.Write(" #");
                        break;
                }
            }

            Console.SetCursorPosition(coursorLeft, coursorTop);
        }
    }
}

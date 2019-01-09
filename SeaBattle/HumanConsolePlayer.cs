using System;
using System.Collections.Generic;

namespace SeaBattle
{
    [Serializable]
    class HumanConsolePlayer : PlayerInterface
    {
        string Name { get; set; }
        Dictionary<Position, ShotStatus> Shots { get; set; }

        public HumanConsolePlayer(string name)
        {
            Name = name;
            Shots = new Dictionary<Position, ShotStatus>();
        }

        private bool checkIsVertical()
        {
            bool check = false;

            Console.WriteLine("Корабль будет расположен вертикально?(да/нет)");
            string answ = Console.ReadLine();

            if (answ == "да")
                check = true;
            else if (answ == "нет")
                check = false;
            else if (answ == "пауза")
                throw new PauseException();
            else
            {
                Console.WriteLine("Введено неверное значение");
                return checkIsVertical();
            }

            return check;
        }

        public Placement choosePlacement(ShipInterface ship, BoardInterface board)
        {
            ConsoleGraphics.showField(board, 70);

            bool isVertical = checkIsVertical();

            Console.WriteLine("Введите кординату X(1 - 10):");
            string answ = Console.ReadLine();
            if (answ == "пауза")
                throw new PauseException();
            int x = int.Parse(answ);

            Console.WriteLine("Введите кординату Y(1 - 10):");
            answ = Console.ReadLine();
            if (answ == "пауза")
                throw new PauseException();
            int y = int.Parse(answ);

            Position position = null;
            BoardInterface boardToShow = board.Clone() as BoardInterface;
            try
            {
                position = new Position(x, y);
                boardToShow.placeShip(ship, position, isVertical);
            }
            catch
            {
                Console.WriteLine("данные введены неправильно, введите заново");
                return choosePlacement(ship, board);
            }

            Console.Clear();
            ConsoleGraphics.showField(boardToShow, 70);
            
            while (true)
            {
                Console.WriteLine("Вы уверены в своём выборе?");
                answ = Console.ReadLine();
                if (answ == "да")
                    return new Placement(position, isVertical);
                else if (answ == "нет")
                    return choosePlacement(ship, board);
            }
        }

        public Position chooseShot()
        {
            ConsoleGraphics.showShootedField(Shots, 90);

            Console.WriteLine($"Игрок {Name}, введите кординаты выстрела");

            Console.WriteLine("Введите X:");
            string answ = Console.ReadLine();
            if (answ == "пауза")
                throw new PauseException();

            int x, y;
            try
            {
                x = int.Parse(answ);
            }
            catch
            {
                Console.WriteLine("Введено неверное значение");
                return chooseShot();
            }

            Console.WriteLine("Введите Y:");
            answ = Console.ReadLine();
            if (answ == "пауза")
                throw new PauseException();
            try
            {
                y = int.Parse(answ);
            }
            catch
            {
                Console.WriteLine("Введено неверное значение");
                return chooseShot();
            }

            Position position = null;
            try
            {
                position = new Position(y, x);
            }
            catch
            {
                Console.WriteLine("Введены неверные кординаты, введите кординаты заново");
                return chooseShot();
            }
            
            return position;
        }

        public void opponentShot(Position position)
        {
            Console.WriteLine($"Игрок {Name}, кординаты прошлого выстрела противника: {position.getX()},{position.getY()}");
        }

        public void shotResult(Position position, ShotStatus status)
        {
            Shots.Add(position, status);

            ConsoleGraphics.showShootedField(Shots, 90);

            string str = null;
            switch (status)
            {
                case ShotStatus.HIT:
                    str = "Попадание";
                    break;
                case ShotStatus.MISS:
                    str = "Промах";
                    break;
                case ShotStatus.SUNK:
                    str = "Потопил";
                    break;
            }
            Console.WriteLine($"{position.getX()},{position.getY()} {str}");
            Console.ReadLine();
        }

        public override string ToString()
        {
            return $"игрок {Name}";
        }
    }
}

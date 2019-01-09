using System;

namespace SeaBattle
{
    [Serializable]
    class ComputerPlayer : PlayerInterface
    {
        string Name { get; set; }

        public ComputerPlayer()
        {
            string[] names = { "Робот-Убийца", "Кибер-пират" };
            Random rnd = new Random();

            Name = names[rnd.Next(0, names.Length - 1)];
        }

        public Placement choosePlacement(ShipInterface ship, BoardInterface board)
        {
            Random rnd = new Random();
            while (true)
            {
                try
                {
                    Position position = new Position(rnd.Next(1, 10), rnd.Next(1, 10));
                    bool isVertical = rnd.Next(0, 1) == 1;
                    BoardInterface tempBoard = board.Clone() as BoardInterface;
                    tempBoard.placeShip(ship, position, isVertical);
                    return new Placement(position, isVertical);
                }
                catch { }
            }
        }

        public Position chooseShot()
        {
            Random rnd = new Random();
            while (true)
            {
                try
                {
                    Position position = new Position(rnd.Next(1, 10), rnd.Next(1, 10));
                    return position;
                }
                catch { }
            }
        }

        public void opponentShot(Position position)
        {
            
        }

        public void shotResult(Position position, ShotStatus status)
        {
            
        }

        public override string ToString()
        {
            return $"компьютер {Name}";
        }
    }
}

using System;

namespace SeaBattle
{
    [Serializable]
    class Ship : ShipInterface
    {
        ShipStatus[] ship;

        public Ship(int size)
        {
            ship = new ShipStatus[size];
            for(int i=0; i<ship.Length; i++)
            {
                ship[i] = ShipStatus.INTACT;
            }
        }

        public int getSize()
        {
            return ship.Length;
        }

        public ShipStatus getStatus(int offset)
        {
            if (offset < 0 || offset > ship.Length - 1)
                throw new InvalidPositionException("Неправильный параметр offset");

            return ship[offset];
        }

        public bool isSunk()
        {
            foreach(ShipStatus val in ship)
            {
                if (val != ShipStatus.SUNK)
                    return false;
            }
            return true;
        }

        public void shoot(int offset)
        {
            if (offset < 0 || offset > ship.Length - 1)
                throw new InvalidPositionException("Неправильный параметр offset");

            ship[offset] = ShipStatus.HIT;

            int count = 0;
            foreach (ShipStatus val in ship)
            {
                if (val == ShipStatus.HIT)
                    count++;
            }
            if (count == ship.Length)
                for (int i = 0; i < ship.Length; i++)
                    ship[i] = ShipStatus.SUNK;
        }
    }
}

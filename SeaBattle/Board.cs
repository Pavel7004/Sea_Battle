using System.Collections.Generic;
using System.Linq;
using System;

namespace SeaBattle
{
    [Serializable]
    class Board : BoardInterface
    {
        Dictionary<Placement, ShipInterface> dict;

        public Board()
        {
            dict = new Dictionary<Placement, ShipInterface>();                       
        }

        public bool allSunk()
        {
            foreach(KeyValuePair<Placement, ShipInterface> kvp in dict)
            {
                if (!kvp.Value.isSunk())
                    return false;
            }
            return true;
        }

        public object Clone()
        {
            Board newBoard = new Board();
            
            foreach (KeyValuePair<Placement, ShipInterface> kvp in dict)
            {
                Position position = kvp.Key.getPosition();
                ShipInterface ship = kvp.Value;
                newBoard.dict.Add(new Placement(new Position(position.getX(), position.getY()), kvp.Key.isVerticalValue()), ship);
            }

            return newBoard;
        }

        public ShipStatus getStatus(Position position)
        {
            bool check = false;
            int offset = -1;
            ShipInterface shootedShip = null;
            foreach (KeyValuePair<Placement, ShipInterface> kvp in dict)
            {
                Position pos = kvp.Key.getPosition();
                if (pos.getX() == position.getX() && pos.getY() == position.getY())
                {
                    IEnumerable<Placement> matches = dict.Where(pair => pair.Value == kvp.Value).Select(pair => pair.Key);
                    
                    foreach (Placement val in matches)
                    {
                        offset++;
                        if (position.getX() == val.getPosition().getX() && position.getY() == val.getPosition().getY())
                            break;
                    }

                    shootedShip = kvp.Value;
                    check = true;
                }               
            }

            if (!check)
                throw new InvalidPositionException("На этой клетке нет корабля");

            return shootedShip.getStatus(offset);
        }

        public void placeShip(ShipInterface ship, Position position, bool isVertical)
        {
            foreach (KeyValuePair<Placement, ShipInterface> kvp in dict)
            {
                for (int i = 0; i < ship.getSize(); i++)
                {
                    Position pos = null;

                    if (isVertical)
                        pos = new Position(position.getX(), position.getY() + i);
                    else
                        pos = new Position(position.getX() + i, position.getY());
                    
                    if (pos.getY() == kvp.Key.getPosition().getY() && pos.getX() == kvp.Key.getPosition().getX())
                        throw new ShipOverlapException("Место занято другим кораблём");
                }
            }
            
            for (int i = 0; i < ship.getSize(); i++)
            {
                if (isVertical)
                {
                    dict.Add(new Placement(new Position(position.getX(), position.getY() + i), isVertical), ship);
                }
                else
                {
                    dict.Add(new Placement(new Position(position.getX() + i, position.getY()), isVertical), ship);
                }
            }
        }

        public void shoot(Position position)
        {
            bool check = false;
            foreach(KeyValuePair<Placement, ShipInterface> kvp in dict)
            {  
                Position pos = kvp.Key.getPosition();
                if (pos.getX() == position.getX() && pos.getY() == position.getY())
                {
                    IEnumerable<Placement> matches = dict.Where(pair => pair.Value == kvp.Value).Select(pair => pair.Key);
                    int offset = -1;

                    foreach (Placement val in matches)
                    {
                        offset++;
                        if (position.getX() == val.getPosition().getX() && position.getY() == val.getPosition().getY())
                            break;
                    }

                    check = true;
                    kvp.Value.shoot(offset);
                }
            }

            if (!check)
                throw new InvalidPositionException("На этой клетке не расположен корабль");
        }
    }
}

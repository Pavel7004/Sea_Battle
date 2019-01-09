using System;

namespace SeaBattle
{
    [Serializable]
    class Placement
    {
        Position position;
        bool isVertical;

        public Placement(Position position, bool isVertical)
        {
            this.position = position;
            this.isVertical = isVertical;
        }

        public Position getPosition()
        {
            return position;
        }

        public bool isVerticalValue()
        {
            return isVertical;
        }
    }
}

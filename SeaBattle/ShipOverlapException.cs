using System;

namespace SeaBattle
{
    public class ShipOverlapException : Exception
    {
        public ShipOverlapException()
        {

        }

        public ShipOverlapException(string message) : base(message)
        { 
        }

    }
}

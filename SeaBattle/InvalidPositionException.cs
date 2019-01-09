using System;

namespace SeaBattle
{
    public class InvalidPositionException : Exception
    {

        public InvalidPositionException()
        {
        }

        public InvalidPositionException(String message) : base(message)
        {
        
        }
    
    }
}

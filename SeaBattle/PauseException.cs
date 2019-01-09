using System;

namespace SeaBattle
{
    public class PauseException : Exception
    {
        public PauseException()
        {

        }

        public PauseException(string message) :base(message)
        {
            
        }

        
    }
}

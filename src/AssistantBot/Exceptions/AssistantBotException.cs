using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssistantBot.Exceptions
{
    public class AssistantBotException : Exception
    {
        public AssistantBotException(string message) 
            : base(message) { }

        public AssistantBotException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

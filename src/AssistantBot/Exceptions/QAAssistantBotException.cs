using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAAssistantBot.Exceptions
{
    public class QAAssistantBotException : Exception
    {
        public QAAssistantBotException(string message) 
            : base(message) { }

        public QAAssistantBotException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

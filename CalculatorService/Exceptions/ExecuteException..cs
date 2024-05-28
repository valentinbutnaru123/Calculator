using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorService
{
    public class ExecuteException : Exception
    {
        public static string IsNotSingleNumberAndSimbols() 
        {
            return "Introduceti doar cifrele 1-9 si semnele respective : /,+,* -; ";
        }  
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorService.Exceptions
{
    public class WithoutParanthesisException :Exception
    {
        public static string MissingOpenParanthesis()
        {
            return "Lipseste paranteza deschisa '(' ";
        }

        public static string MissingCloseParanthesis()
        {
            return "Lipseste paranteza inchisa ')' ";


        }
    }
}

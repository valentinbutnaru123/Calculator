using CalculatorService;
using System.Globalization;

Console.WriteLine("Introduceti expresia :");

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

var expression = Console.ReadLine();
CalculationsService calcService = new CalculationsService(); 

try
{
    if (string.IsNullOrEmpty(expression))
        Console.WriteLine("Expresie invalida");
    else
    {
        var result = calcService.Execute(expression);

        Console.WriteLine(result);
    }
}

catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}



using CalculatorService.Exceptions;
using System.Data;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

//original

namespace CalculatorService;

public class CalculationsService
{
    public static char[] prioritizedOperand = { '*', '/' };

    public double Execute(string expression)
    {
        expression = expression.Trim().Replace(" ", "");

        double result = 0;
        if (!IsSingleNumberAndSimbols(expression))
        {
            throw new Exception("Only digit and symbols");
        }

        while (WithoutParenthesis(expression, out expression))
        {
        }

        result = IsSingleNumber(expression, out var num) ? num : Calculate(expression);


        return result;
    }

    public static double Calculate(string expression)
    {
        if (IsSingleNumber(expression, out double num))
            return num;

        expression = ExecutePrioritizedOperators(expression);

        Stack<char> operators = new Stack<char>();
        StringBuilder currentNumber = new StringBuilder();

        double? firstNumber = null;
        double secondNumber = 0;
        bool foundFirstNumber = false;

        for (int c = 0; c < expression.Length; c++)
        {
            if (char.IsDigit(expression[c]) || expression[c] == '.')
            {
                currentNumber.Append(expression[c]);

                if (firstNumber.HasValue)
                    secondNumber = double.Parse(currentNumber.ToString());
            }

            else
            {
                if (currentNumber.Length > 0)
                {
                    if (!firstNumber.HasValue)
                    {
                        firstNumber = double.Parse(currentNumber.ToString());
                        foundFirstNumber = true;
                    }
                    else
                    {
                        firstNumber = Operate(operators, firstNumber.Value, secondNumber);
                    }

                    currentNumber = new StringBuilder();
                }


                if (expression[c] == '-' && !foundFirstNumber)
                {
                    currentNumber.Append(expression[c]);
                }

                else
                {
                    operators.Push(expression[c]);
                }
            }
        }

        if (operators.Count > 0)
        {
            firstNumber = Operate(operators, firstNumber.Value, secondNumber);
        }

        return firstNumber.Value;
    }

    private static string ExecutePrioritizedOperators(string expression)
    {
        Stack<char> operators = new Stack<char>();
        StringBuilder currentNumber = new StringBuilder();

        double? firstNumber = null;
        double secondNumber = 0;
        bool foundFirstNumber = false;
        
        while (expression.Contains('*') || expression.Contains('/'))
        {
            int indexOper = expression.IndexOfAny(prioritizedOperand);
            operators.Push(expression[indexOper]);

            int indexOperLeft = indexOper - 1;

            while (indexOperLeft >= 0 && (expression[indexOperLeft] != '-' && expression[indexOperLeft] != '+'))
            {
                currentNumber.Append(expression[indexOperLeft]);
                indexOperLeft -= 1;
            }

            firstNumber = double.Parse(currentNumber.ToString());
            firstNumber = double.Parse(new string(firstNumber.ToString().Reverse().ToArray()));
            currentNumber = new StringBuilder();
            
            int indexOpeRight = 0;

            for (indexOpeRight = indexOper + 1; indexOpeRight < expression.Length; indexOpeRight++)
            {
                if (expression[indexOpeRight] == '-')
                {
                    operators.Push(expression[indexOpeRight]);
                }

                else
                {
                    while (indexOpeRight < expression.Length && (expression[indexOpeRight] != '-' &&
                                                                 expression[indexOpeRight] != '+' &&
                                                                 expression[indexOpeRight] != '*' &&
                                                                 expression[indexOpeRight] != '/'))
                    {
                        currentNumber.Append(expression[indexOpeRight]);
                        indexOpeRight += 1;
                    }

                    break;
                }
            }
            
            secondNumber = double.Parse(currentNumber.ToString());
            currentNumber = new StringBuilder();

            string result = Operate(operators, firstNumber.Value, secondNumber).ToString();

            expression = expression.Remove(indexOperLeft + 1, indexOpeRight - indexOperLeft - 1)
                .Insert(indexOperLeft + 1, result);
            
            if (IsSingleNumber(expression, out _))
                return expression;
        }

        return expression;
    }


    public static bool WithoutParenthesis(string expression, out string executedExpressions)
    {
        if (!expression.Any(x => x is '(' or ')'))
        {
            executedExpressions = expression;
            return false;
        }

        int lastOpenParenthesisIndex = -1;
        int firstClosedParenthesisIndex = -1;
        bool foundFirstPair = false;

        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == '(')
            {
                lastOpenParenthesisIndex = i;
            }
            else if (expression[i] == ')' && lastOpenParenthesisIndex != -1)
            {
                firstClosedParenthesisIndex = i;
                foundFirstPair = true;
                break;
            }
            else if (expression[i] == ')' && lastOpenParenthesisIndex == -1)
            {
                throw new Exception("Missing the pharantesis'(' ");
            }
        }


        if (!foundFirstPair)
        {
            throw new Exception("Missing the pharantesis ')' ");
        }

        string parenthesisExpression = expression
            .Substring(lastOpenParenthesisIndex, firstClosedParenthesisIndex - lastOpenParenthesisIndex)
            .Replace("(", "").Replace(")", "");


        string parenthesisExpressionResult = Calculate(parenthesisExpression).ToString();

        executedExpressions = expression.Remove(lastOpenParenthesisIndex,
                firstClosedParenthesisIndex - lastOpenParenthesisIndex + 1)
            .Insert(lastOpenParenthesisIndex, parenthesisExpressionResult);


        return true;
    }


    public static double Operate(Stack<char> operators, double number1, double number2)
    {
        char op = ' ';
        bool negativeNumber = false;


        if (HasInvalidCombination(operators))
        {
            throw new Exception("Invalid combination.");
        }

        if (operators.TryPeek(out char lasOperator) && lasOperator == '-')
        {
            char prevOperator = operators.ElementAt(operators.Count - 1);

            if ((prevOperator == '/' || prevOperator == '*'))
            {
                var ex = operators.Count(x => x == '-') % 2 == 0 ? negativeNumber = false : negativeNumber = true;

                operators.Pop();
                op = prevOperator;
            }
            else
            {
                op = operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';
            }
        }

        if (operators.Peek() == '*')
        {
            op = operators.Pop();
        }

        else if (operators.Peek() == '/')
        {
            op = operators.Pop();
        }

        else if (operators.Peek() == '+')
        {
            op = operators.Peek();
        }

        foreach (var c in operators)

            if (c == '-' && c == '+')
            {
                op = operators.Count(x => x == '-') % 2 == 0 ? '+' : '-';
            }


        operators.Clear();

        double result = 0;
        switch (op)
        {
            case '+':
                result = number1 + number2;
                break;
            case '-':
                result = number1 - number2;
                break;
            case '*':
                result = number1 * number2;
                break;
            case '/':
                result = number1 / number2;
                break;
        }

        if (double.IsPositiveInfinity(result))
            throw new DivideByZeroException("Infinity");

        if (negativeNumber)
            result *= -1;


        return result;
    }

    public static bool IsSingleNumber(string expression, out double num)
    {
        int startIndex = 0;
        var operators = new List<char>();

        while (startIndex < expression.Length && (expression[startIndex] == '-' || expression[startIndex] == '+'))
        {
            operators.Add(expression[startIndex]);
            startIndex++;
        }

        if (double.TryParse(expression.Substring(startIndex), out num))
        {
            num = operators.Count(x => x == '-') % 2 == 0 ? num : num * -1;

            return true;
        }

        return false;
    }

    public static bool HasInvalidCombination(Stack<char> stack)
    {
        string[] invalidCombination = { "**", "//", "*/", "/*", "+*", "*+", "/+", "+/", "/-", "*-" };

        string stackString = new string(stack.ToArray());

        foreach (string c in invalidCombination)
        {
            if (stackString.Contains(c))
            {
                return true;
            }
        }

        return false;
    }


    public static bool IsSingleNumberAndSimbols(string expression)
    {
        return !Regex.IsMatch(expression, @"[^0-9*/+\-()\.]");
    }
}
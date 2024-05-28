namespace CalculatorWebApp.Services
{
    public class CalculatorServicesMVC
    {
        private static readonly string[] operators = { "+", "-", "/", "*" };

        public string AddToExpression(string expressionInput, string value)
        {
            if (string.IsNullOrEmpty(expressionInput))
            {
                if (IsNumber(value) || value == "-" || value == "(")
                    expressionInput = value;
                else
                {
                    return expressionInput;
                }
            }
            else
            {
                string lastChar = expressionInput.Substring(expressionInput.Length - 1);
                string lastOperator = operators.Contains(lastChar) ? lastChar : "";

                if (expressionInput.Length > 1 &&
                    (expressionInput.Substring(expressionInput.Length - 2) == "/-" ||
                     expressionInput.Substring(expressionInput.Length - 2) == "*-") && !IsNumber(value))
                    return expressionInput;

                if (lastChar == "." && !IsNumber(value))
                    return expressionInput;

                if (operators.Contains(value))
                {
                    if (lastOperator != string.Empty)
                    {
                        if ((lastOperator == "*" || lastOperator == "/") && value == "-")
                        {
                            expressionInput += value;
                        }
                        else if (value == "-" && lastChar == "(")
                        {
                            expressionInput += value;
                        }
                        else
                        {
                            expressionInput = expressionInput.Substring(0, expressionInput.Length - 1) + value;
                        }
                    }
                    else
                    {
                        expressionInput += value;
                    }
                }
                else if (value == "(")
                {
                    if (string.IsNullOrEmpty(lastChar) || operators.Contains(lastChar) || lastChar == "(")
                    {
                        expressionInput += value;
                    }
                }
                else if (value == ")" && IsNumber(lastChar))
                {
                    expressionInput += value;
                }
                else if (IsNumber(value))
                {
                    expressionInput += value;
                }
                else if (value == ".")
                {
                    if (!IsNumber(lastChar))
                        return expressionInput;

                    var lastInput = new string(expressionInput.Reverse()
                        .TakeWhile(x => !operators.Contains(x.ToString())).ToArray());

                    if (lastInput.Count(x => x == '.') > 0)
                        return expressionInput;

                    expressionInput += value;
                }
            }

            return expressionInput;
        }

        private bool IsNumber(string value)
        {
            return double.TryParse(value, out double _);
        }

        public string RemoveLastCharacter(string expression)
        {
            return string.IsNullOrEmpty(expression) ? expression : expression.Remove(expression.Length - 1);
        }
    }
}
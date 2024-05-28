namespace TestProject
{
    using CalculatorService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CalculationServiceTest
    {

        //IsSingleNumberAndSymbols
        [TestMethod]
        public void IsSingleNumberAndSymbols_Test()
        {
            string expression = "3+4-10";

            var actual = CalculationsService.IsSingleNumberAndSimbols(expression);

            Assert.IsTrue(actual);

        }

        [TestMethod]
        public void IsNotOnlySingleNumberAndSymbols_ExceptionTest()
        {
            string expression = "3+!-A*10";

            var actual = CalculationsService.IsSingleNumberAndSimbols(expression);

            Assert.IsFalse(actual);

        }


        //IsSingleNumber
        [TestMethod]
        public void IsSingleNumber_PositiveNumberTest()
        {
            string expression = "33";

            bool actual = CalculationsService.IsSingleNumber(expression, out double expected);

            Assert.IsTrue(actual);
            Assert.AreEqual(33, expected);
        }


        [TestMethod]
        public void IsSingleNumber_NegativeNumberTest()
        {
            string expression = "-9";

            var actual = CalculationsService.IsSingleNumber(expression, out double expected);

            Assert.IsTrue(actual);
            Assert.AreEqual(-9, expected);
        }


        [TestMethod]
        public void IsNotSingleNumber_Test()
        {
            string expression = "2 + 4 ";

            var actual = CalculationsService.IsSingleNumber(expression, out double expected);

            Assert.IsFalse(actual);
            Assert.AreNotEqual(3, expected);
        }


        //HasInvalidCombination
        [TestMethod]
        public void HasInvalidCombination_Test()
        {
            Stack<char> stack = new Stack<char>();
            stack.Push('-');
            stack.Push('+');
            stack.Push('*');


            var actual = CalculationsService.HasInvalidCombination(stack);

            Assert.IsTrue(actual);  //trebuie si exception throw

        }
        [TestMethod]
        public void HasValidCombination_Test()
        {
            var stack = new Stack<char>();
            stack.Push('+');
            stack.Push('-');
            stack.Push('-');

            var actual = CalculationsService.HasInvalidCombination(stack);
            Assert.IsFalse(actual);

        }



        //Operate

        [TestMethod]
        public void Operate_ReturnPositiveNumberTest()
        {
            Stack<char> stack = new Stack<char>();
            stack.Push('+');
            stack.Push('-');
            stack.Push('-');

            int num1 = 4;
            int num2 = 2;


            double result = CalculationsService.Operate(stack, num1, num2);

            Assert.AreEqual(6, result);

        }

        [TestMethod]
        public void Operate_ReturnNegativeNumberTest()
        {
            Stack<char> stack = new Stack<char>();
            stack.Push('/');
            stack.Push('-');

            int num1 = 9;
            int num2 = 3;

            double result = CalculationsService.Operate(stack, num1, num2);

            Assert.AreEqual(-3, result);
        }


        [TestMethod]
        public void Operate_Addition_ReturnsCorrectResultTest()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('+');
            int number1 = 5;
            int number2 = 4;

            double result = CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void Operate_Subtraction_ReturnCorrectResultTest()
        {

            Stack<char> operators = new Stack<char>();
            operators.Push('-');
            int number1 = 5;
            int number2 = 4;

            double result = CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Operate_DivTest()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('/');
            int number1 = 20;
            int number2 = 4;

            double result = CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(5, result);
        }


        [TestMethod]
        public void Operate_DivTo0Test()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('/');
            int number1 = 20;
            int number2 = 0;

            Assert.ThrowsException<DivideByZeroException>(() => CalculationsService.Operate(operators, number1, number2));

        }


        [TestMethod]
        public void Operate_MultipleTest()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('*');
            int number1 = 6;
            int number2 = 4;

            double result = CalculationsService.Operate(operators, number1, number2);

            Assert.AreEqual(24, result);
        }

        //[TestMethod]
        public void Operate_InvalidCombinationTest()
        {
            Stack<char> operators = new Stack<char>();
            operators.Push('-');
            operators.Push('+');
            operators.Push('*');
            var number1 = 1;
            var number2 = 2;

            Assert.ThrowsException<Exception>(() => CalculationsService.Operate(operators, number1, number2));
        }


        //WithoutParanthesis
        [TestMethod]
        public void WithoutParanthesis_WithoutParanthesisTest()
        {
            string expression = "3+4--5";
            string executedExpression;

            var result = CalculationsService.WithoutParenthesis(expression, out executedExpression);

            Assert.IsFalse(result);
            Assert.AreEqual("3+4--5", executedExpression);
        }

        [TestMethod]
        public void WithoutParanthesis_WithParanthesisTest()
        {
            string expression = "(3+4-10)";
            string executedExpression;

            var result = CalculationsService.WithoutParenthesis(expression, out executedExpression);

            Assert.IsTrue(result);
            Assert.AreEqual("-3", executedExpression);
        }

        [TestMethod]
        public void WithoutParanthesis_MissingOpenPharantesis()
        {
            string expression = "6+5)";
            Assert.ThrowsException<Exception>(() => CalculationsService.WithoutParenthesis(expression, out expression));
        }

        [TestMethod]
        public void WithoutParanthesis_MissingClosePharantesis()
        {
            string expression = "(-6+5";

            Assert.ThrowsException<Exception>(() => CalculationsService.WithoutParenthesis(expression, out expression));

        }


        //calculate
        [TestMethod]
        public void Calculate_Test()
        {
            string expression = "3+6";

            var result = CalculationsService.Calculate(expression);

            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void Calculate_SingleNumberTest()
        {
            string expression = "1";

            var result = CalculationsService.Calculate(expression);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Calculate_NegativeNumberTest()
        {
            string expression = "-1-1";

            var result = CalculationsService.Calculate(expression);

            Assert.AreEqual(-2, result);
        }



    }

}
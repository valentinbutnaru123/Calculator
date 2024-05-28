using CalculatorService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using CalculatorWebApp.Services;
using Microsoft.Extensions.Primitives;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly CalculationsService _calculatorService;
        private readonly CalculatorServicesMVC _calcServicesMVC;

        public HomeController()
        {
            _calculatorService = new CalculationsService();
            _calcServicesMVC = new CalculatorServicesMVC();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToExpression(string expression, string buttonValue)
        {
            try
            {
                switch (buttonValue)
                {
                    case "AC":
                        expression = string.Empty;
                        break;
                    case "C":
                        expression = _calcServicesMVC.RemoveLastCharacter(expression);
                        break;
                    case "=":
                        expression = _calculatorService.Execute(expression).ToString();
                        break;
                    default:
                        expression = _calcServicesMVC.AddToExpression(expression, buttonValue);
                        break;
                }
            }
            catch (Exception e)
            {
                expression = e.Message;
            }

            ViewBag.Expression = expression;
            return View("Index");
        }
    }
}
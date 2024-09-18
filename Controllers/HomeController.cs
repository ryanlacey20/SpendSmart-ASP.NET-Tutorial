using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SpendSmart.Models;

namespace SpendSmart.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SpendSmartDbContext _context;
    public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
    {
        _logger = logger;
        _context = context;

    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Expenses()
    {
        var allExpenses = _context.Expenses.ToList();

        var totalExpenses = allExpenses.Sum(x => x.Value);

        ViewBag.Expenses = totalExpenses;

        return View(allExpenses);
    }
    public IActionResult CreateEditExpense(int? id)
    {
        if (id != null)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            return View(expenseInDb);

        }
        return View();
    }
    public IActionResult DeleteExpense(int id)
    {
        var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
        _context.Expenses.Remove(expenseInDb);
        _context.SaveChanges();
        return RedirectToAction("Expenses");
    }
    public IActionResult CreateEditExpenseForm(Expense expenseModel)
    {
        if (expenseModel.Id == 0)
        {
            _context.Expenses.Add(expenseModel);

        }
        else
        {
            _context.Expenses.Update(expenseModel);

        }

        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

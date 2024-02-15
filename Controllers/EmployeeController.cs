using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using TaskAssessment.Areas.Identity.Data;
using TaskAssessment.Data;

namespace TaskAssessment.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        private readonly TaskAssessmentContext _context;
        public EmployeeController(TaskAssessmentContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(String searchInput)
        {
            var emps = new List<Employee>();
            if (String.IsNullOrWhiteSpace(searchInput))
            {

                emps = await _context.Employees.ToListAsync();
            }
            else
            {
                emps = await _context.Employees.FromSqlRaw("EXEC SearchEmployee '" + searchInput + "'").ToListAsync();
            }

            return View(emps);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int Id)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(x => x.Id == Id);
            if (emp == null)
            {
                TempData["err"] = "Employee Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(x => x.Id == Id);
            if (emp == null)
            {
                TempData["err"] = "Employee Not Found";
                return RedirectToAction(nameof(Index));
            }
            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
            TempData["suc"] = "Employee Deleted.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int Id)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(x => x.Id == Id);
            if (emp == null)
            {
                TempData["err"] = "Employee Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            TempData["suc"] = "Employee Updated.";
            return RedirectToAction(nameof(Index));
        }
    }
}

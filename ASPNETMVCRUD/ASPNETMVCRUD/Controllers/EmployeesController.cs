using ASPNETMVCRUD.Data;
using ASPNETMVCRUD.Models;
using ASPNETMVCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ASPNETMVCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // list that will be shown to the user
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                //Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateTime = addEmployeeRequest.DateTime,

                Department = addEmployeeRequest.Department,
            };

            await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new updateEmployeeviewmodel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateTime = employee.DateTime,

                    Department = employee.Department,
                };
                return await Task.Run(() => View("View",viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(updateEmployeeviewmodel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            { 
                employee.Name = model.Name; 
                employee.Email = model.Email;   
                employee.Salary = model.Salary;
                employee.DateTime = model.DateTime;
                employee.Department = model.Department;

                await mvcDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");





        }

        [HttpPost]

        public async Task<IActionResult> Delete(updateEmployeeviewmodel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();  
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}

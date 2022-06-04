using EncryptedEmployeeMgmt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISalaryRepository _salaryRepository;

        public AdminController(IEmployeeRepository employeeRepository, ISalaryRepository salaryRepository)
        {
            _employeeRepository = employeeRepository;
            _salaryRepository = salaryRepository;
        }
        [HttpGet]
        [Route("ctc")]
        public IActionResult GetCTC(int eid)
        {
            return Ok(_employeeRepository.GetEmployeeCTC(eid));
        }
        [HttpGet]
        [Route("salary")]
        public IActionResult GetSalary(int eid)
        {
            return Ok(_employeeRepository.GetEmployeeSalary(eid));
        }
        [HttpGet]
        [Route("monthsalary")]
        public IActionResult GetMonthSalary(int eid, Month month)
        {
            int salary = _salaryRepository.GetEmployeeMonthSalary(eid, month);
            if (salary == 0)
            {
                return BadRequest("Employee Not Exist in the Month of " + month.ToString());
            }
            return Ok(salary);
        }
        [HttpGet]
        [Route("salarysum")]
        public IActionResult GetSalarySum(int eid, Month from, Month to)
        {
            return Ok(_salaryRepository.GetEmployeeSalarySum(eid, from, to));
        }
        [HttpPost]
        [Route("addemployee")]
        public IActionResult CreateEmployee(Employee employee)
        {
            return Ok(_employeeRepository.CreateEmployee(employee));
        }
        [HttpPost]
        [Route("creditsalary")]
        public IActionResult CreditSalary(MonthlySalary salary)
        {
            Employee emp=_employeeRepository.GetEmployee(salary.Employee.Id);
            salary.Employee=emp;
            _salaryRepository.AddSalary(salary);
            return Ok("Salary Credited to Employee " + salary.Employee.Id);
        }
        [HttpGet]
        [Route("salaryhistory")]
        public IActionResult EmployeeSalaryHistory(int Eid)
        {
            List<MonthlySalary> salaryHistory= _salaryRepository.GetEmployeeSalaryHistory(Eid); 
            return Ok(salaryHistory);
        }
        [HttpGet]
        [Route("allemployees")]
        public IActionResult AllEmployeesList()
        {
            List<Employee> employees=_employeeRepository.GetAllEmployees();
            return Ok(employees);
        }
    }
}

using EncryptedEmployeeMgmt.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _http;
        private  HttpResponseMessage _response=null;
        public EmployeeController()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("https://localhost:44350/api/admin/");
        }
        
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = new List<Employee> ();
            _response = await _http.GetAsync("allemployees");
            if (_response.IsSuccessStatusCode)
            {
                var result = _response.Content.ReadAsStringAsync().Result;
                employees= JsonConvert.DeserializeObject<List<Employee>>(result);
                return View(employees);
            }            
            return View();
        }
        
        public async Task<IActionResult> SalaryHistory(int Eid)
        {
            List<MonthlySalary> salaries = new List<MonthlySalary>();
            _response = await _http.GetAsync("salaryhistory/?Eid="+Eid);
            if (_response.IsSuccessStatusCode)
            {
                var result = _response.Content.ReadAsStringAsync().Result;
                salaries = JsonConvert.DeserializeObject<List<MonthlySalary>>(result);
                return View(salaries);
            }
            return View();
        }
        [HttpGet]
        public IActionResult DepositSalary()
        {
            MonthlySalary monthlySalary = new MonthlySalary();
            return View(monthlySalary);
        }
        [HttpPost]
        public async Task<IActionResult> DepositSalary(MonthlySalary monthlySalary)
        {
            StringContent content=new StringContent(JsonConvert.SerializeObject(monthlySalary),Encoding.UTF8,"application/json");
            _response =await _http.PostAsync("creditsalary", content);
            if (_response.IsSuccessStatusCode)
            {
                var result=_response.Content.ReadAsStringAsync().Result;
                ViewData["response"]=result;
                return View("~/views/shared/success.cshtml");
            }
            return View(monthlySalary);
        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            Employee employee = new Employee();
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
            _response = await _http.PostAsync("addemployee", content);
            if (_response.IsSuccessStatusCode)
            {
                var result = _response.Content.ReadAsStringAsync().Result;
                Employee emp=JsonConvert.DeserializeObject<Employee>(result);
                ViewData["response"] = "Emp Created With Emp Id = "+emp.Id;
                return View("~/views/shared/success.cshtml");
            }
            return View(employee);
        }
    }
}

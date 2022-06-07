using EmployeeLibrary.Utilities;
using EmployeeMVC.ViewModels;
using EncryptedEmployeeMgmt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Research.SEAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
                List<EmployeeViewModel> emps= new List<EmployeeViewModel> ();
                foreach (var employee in employees)
                {
                    var ctcCipher = Utilities.BuildCiphertextFromBase64String(employee.CTC,Utilities.context);
                    var salaryCipher=Utilities.BuildCiphertextFromBase64String(employee.Salary,Utilities.context);
                    var emp=new EmployeeViewModel()
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        DOJ= employee.DOJ,
                        CTC= Convert.ToInt32(Utilities.CiphertextToDouble(ctcCipher)),
                        Salary=Convert.ToInt32(Utilities.CiphertextToDouble(salaryCipher))
                    };
                    emps.Add(emp);
                }
                return View(emps);
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
            EmployeeViewModel employee = new EmployeeViewModel();
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel employeeViewModel)
        {
            var ctcCipher=Utilities.DoubleToCiphertext(employeeViewModel.CTC);
            var ctcString= Utilities.CiphertextToBase64String(ctcCipher);
            var salCipher = Utilities.DoubleToCiphertext((employeeViewModel.CTC)/12);
            var salString = Utilities.CiphertextToBase64String(salCipher);
            Employee employee = new Employee()
            {
                Name = employeeViewModel.Name,
                DOJ=employeeViewModel.DOJ,
                CTC=ctcString,
                Salary=salString
            };
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
        [HttpGet]
        public IActionResult CTC()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetCTC()
        {
            int Eid =Convert.ToInt32(HttpContext.Request.Form["eid"]);
            HttpResponseMessage res= await _http.GetAsync("ctc/?eid=" + Eid);
            if (res.IsSuccessStatusCode)
            {
                var result=res.Content.ReadAsStringAsync().Result;
                Ciphertext resultCipher= Utilities.BuildCiphertextFromBase64String(result,Utilities.context);
                double CTC = Utilities.CiphertextToDouble(resultCipher);
                ViewData["response"] = "Employee CTC with id=" + Eid + " is " + CTC;
                return View("~/Views/Shared/Success.cshtml");
            }
            return View();
        }
    }
}

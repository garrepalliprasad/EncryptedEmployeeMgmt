using EmployeeLibrary.Utilities;
using EmployeeMVC.ViewModels;
using EncryptedEmployeeMgmt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Research.SEAL;
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
        private readonly Utilities _utilities;
        public EmployeeController()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("https://localhost:44350/api/admin/");
            _utilities = new Utilities();
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
            MonthlySalaryViewModel monthlySalaryViewModel = new MonthlySalaryViewModel();
            return View(monthlySalaryViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> DepositSalary(MonthlySalaryViewModel monthlySalaryViewModel)
        {
            MonthlySalary monthlySalary = new MonthlySalary() 
            {
                LC=monthlySalaryViewModel.LC,
                Month=monthlySalaryViewModel.Month,
                Employee=new Employee()
                {
                    Id=monthlySalaryViewModel.Eid
                }
            };
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
            var ctcCipher=_utilities.DoubleToCiphertext(employeeViewModel.CTC);
            var ctcString=_utilities.CiphertextToBase64String(ctcCipher);
            var salC=_utilities.BuildCiphertextFromBase64String(ctcString);
            var salD=_utilities.CiphertextToDouble(salC);
            var salCipher = _utilities.DoubleToCiphertext((employeeViewModel.CTC)/12);
            var salString = _utilities.CiphertextToBase64String(salCipher);
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
                Ciphertext resultCipher=_utilities.BuildCiphertextFromBase64String(result);
                double CTC = _utilities.CiphertextToDouble(resultCipher);
                ViewData["response"] = "Employee CTC with id=" + Eid + " is " + CTC;
                return View("~/Views/Shared/Success.cshtml");
            }
            return View();
        }
    }
}

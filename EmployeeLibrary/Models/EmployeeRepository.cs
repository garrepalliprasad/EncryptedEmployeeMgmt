using EmployeeLibrary.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace EncryptedEmployeeMgmt.Models
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private Utilities utilities = new Utilities();

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees.FirstOrDefault(e=> e.Id == id);
        }

        public string GetEmployeeCTC(int id)
        {
            var stringval= _context.Employees.FirstOrDefault(e => e.Id == id).CTC;
            var cipherval= utilities.BuildCiphertextFromBase64String(stringval);
            var val=utilities.CiphertextToDouble(cipherval);
            return stringval;
        }

        public string GetEmployeeSalary(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == id).Salary;
        }

        public Employee CreateEmployee(Employee employee)
        {
            var stringval = employee.CTC;
            var cipherval = utilities.BuildCiphertextFromBase64String(stringval);
            var val = utilities.CiphertextToDouble(cipherval);
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public int GetJoinedMonth(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == id).DOJ.Month;
        }
    }
}

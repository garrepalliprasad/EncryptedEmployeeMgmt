using System.Collections.Generic;
using System.Linq;

namespace EncryptedEmployeeMgmt.Models
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly AppDbContext _context;

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

        public int GetEmployeeCTC(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == id).CTC;
        }

        public int GetEmployeeSalary(int id)
        {
            return (_context.Employees.FirstOrDefault(e => e.Id == id).CTC) / 12; 
        }

        public Employee CreateEmployee(Employee employee)
        {
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

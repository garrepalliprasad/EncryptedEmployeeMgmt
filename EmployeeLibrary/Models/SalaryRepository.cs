using System.Collections.Generic;
using System.Linq;

namespace EncryptedEmployeeMgmt.Models
{
    public class SalaryRepository:ISalaryRepository
    {
        private readonly AppDbContext _context;

        public SalaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public int GetEmployeeMonthSalary(int Eid, Month month)
        {
            MonthlySalary monthlySalary = _context.Salaries.FirstOrDefault(s => s.Employee.Id == Eid && s.Month == month);
            if(monthlySalary == null)
            {
                return 0;
            }
            return monthlySalary.Salary;
        }

        public int GetEmployeeSalarySum(int Eid,Month from, Month to)
        {
            return _context.Salaries.Where(s => s.Month >= from && s.Month <= to && s.Employee.Id==Eid).Sum(s => s.Salary);
        }

        public void AddSalary(MonthlySalary salary)
        {
            
            _context.Salaries.Add(salary);
            _context.SaveChanges();
        }

        public List<MonthlySalary> GetEmployeeSalaryHistory(int Eid)
        {
            return _context.Salaries.Where(s=>s.Employee.Id== Eid).ToList();
        }
    }
}

using System;
using System.Collections.Generic;

namespace EncryptedEmployeeMgmt.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOJ { get; set; }
        public string CTC { get; set; }
        public string Salary { get; set; }
        public ICollection<MonthlySalary> Salaries { get; set; }
    }
}

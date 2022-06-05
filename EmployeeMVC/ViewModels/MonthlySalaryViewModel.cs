using EncryptedEmployeeMgmt.Models;

namespace EmployeeMVC.ViewModels
{
    public class MonthlySalaryViewModel
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public int Salary { get; set; }
        public int LC { get; set; }
        public int Eid { get; set; }    
    }
}

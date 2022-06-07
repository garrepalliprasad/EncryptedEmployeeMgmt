namespace EncryptedEmployeeMgmt.Models
{
    public class MonthlySalary
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public string Salary { get; set; }
        public Employee Employee { get; set; }
    }
}

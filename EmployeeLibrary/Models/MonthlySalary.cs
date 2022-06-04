namespace EncryptedEmployeeMgmt.Models
{
    public class MonthlySalary
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public int Salary { get; set; }
        public int LC { get; set; }
        public Employee Employee { get; set; }
    }
}

using System.Collections.Generic;

namespace EncryptedEmployeeMgmt.Models
{
    public interface ISalaryRepository
    {
        string GetEmployeeMonthSalary(int Eid,Month month);
        List<string> GetEmployeeSalarySum(int Eid,Month from, Month to);
        void AddSalary(MonthlySalary salary);
        List<MonthlySalary> GetEmployeeSalaryHistory(int Eid);
    }
}

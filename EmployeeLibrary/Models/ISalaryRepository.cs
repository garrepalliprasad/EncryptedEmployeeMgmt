using System.Collections.Generic;

namespace EncryptedEmployeeMgmt.Models
{
    public interface ISalaryRepository
    {
        int GetEmployeeMonthSalary(int Eid,Month month);
        int GetEmployeeSalarySum(int Eid,Month from, Month to);
        void AddSalary(MonthlySalary salary);
        List<MonthlySalary> GetEmployeeSalaryHistory(int Eid);
    }
}

using System.Collections.Generic;

namespace EncryptedEmployeeMgmt.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        List<Employee> GetAllEmployees();
        string GetEmployeeCTC(int id);
        string GetEmployeeSalary(int id);
        Employee CreateEmployee(Employee employee);
        int GetJoinedMonth(int id);
    }
}

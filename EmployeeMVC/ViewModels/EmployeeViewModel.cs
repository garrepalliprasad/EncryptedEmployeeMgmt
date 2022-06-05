using System;

namespace EmployeeMVC.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOJ { get; set; }
        public int CTC { get; set; }
    }
}

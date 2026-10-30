using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevcanSDF1
{
    public abstract class EmployeeServiceBase
    {
        public abstract bool AddEmployee(Employee employee);
        public abstract Employee FindEmployee(string code);
        public abstract bool AddPayment(string employeeCode, string title, double amount);
        public abstract List<Payment> GetEmployeePayments(string employeeCode);
        public abstract List<Employee> GetAllEmployees();
    }

    public class EmployeeService : EmployeeServiceBase
    {
        public static List<Employee> Employees = new List<Employee>();

        public override bool AddEmployee(Employee employee)
        {
            if (FindEmployee(employee.Code) != null) return false;
            Employees.Add(employee);
            return true;
        }

        public override Employee FindEmployee(string code)
        {
            return Employees.FirstOrDefault(e => e.Code.Equals(code, System.StringComparison.OrdinalIgnoreCase));
        }

        public override bool AddPayment(string employeeCode, string title, double amount)
        {
            if (amount <= 0) return false;
            var employee = FindEmployee(employeeCode);
            if (employee == null) return false;

            employee.Payments.Add(new Payment { Title = title, Amount = amount });
            return true;
        }

        public override List<Payment> GetEmployeePayments(string employeeCode)
        {
            var employee = FindEmployee(employeeCode);
            return employee?.Payments?.ToList();
        }

        public override List<Employee> GetAllEmployees()
        {
            return Employees;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Api
{
    public class HelperService
    {
        public string GetEmployeeName(int employeeID) {
            var employee = ClarityDB.Instance.Employees
                .Where(e => e.ID == employeeID).FirstOrDefault();

            if (employee != null) {
                return employee.FullName;
            }
            return null;
        }

        public string GetLicensePlate(int truckID)
        {
            var truck = ClarityDB.Instance.Trucks
                .Where(t => t.ID == truckID).FirstOrDefault();

            if (truck != null)
            {
                return truck.LicensePlate;
            }
            return null;
        }
    }
}

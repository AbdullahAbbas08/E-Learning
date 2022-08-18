using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_Domain_Layer.DTO_S.Employee
{
    public class GetEmployeeDto
    {
        public string ID { get; set; }
        public string NationalId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
    }

}

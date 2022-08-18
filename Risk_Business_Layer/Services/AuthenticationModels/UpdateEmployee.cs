using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_Business_Layer.Services.AuthenticationModels
{
    public class UpdateEmployee
    {
        public string Id            { get; set; }
        public string NationalId    { get; set; }
        public string Name          { get; set; }
        public string Mobile        { get; set; }
        public string Address       { get; set; }
        public string UserName      { get; set; }
        public string password      { get; set; }
        public string Role          { get; set; }
    }
}

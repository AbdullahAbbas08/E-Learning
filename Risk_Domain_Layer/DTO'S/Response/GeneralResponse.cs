using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_Domain_Layer.DTOS
{
    public class GeneralResponse<T>
    {
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}

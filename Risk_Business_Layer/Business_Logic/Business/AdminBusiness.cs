using Elasticsearch.Net;
using Risk_Business_Layer.IBusiness_Logic.Interfaces;
using Risk_Data_Access_Layer;
using Risk_Data_Access_Layer.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Risk_Business_Layer.Business_Logic.Business
{
    public class AdminBusiness : IAdminBusiness
    {
        private readonly Risk_Data_Access_Layer.DbContext riskDbContext;

        public AdminBusiness(Risk_Data_Access_Layer.DbContext riskDbContext)
        {
            this.riskDbContext = riskDbContext;
        }

    }
}

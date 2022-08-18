

namespace Risk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region Decalre Variables
        private readonly IAdminBusiness adminBusiness;
        #endregion

        #region Constructor
        public AdminController( IAdminBusiness adminBusiness)
        {
            this.adminBusiness = adminBusiness;
        }
        #endregion


    }
} 
 
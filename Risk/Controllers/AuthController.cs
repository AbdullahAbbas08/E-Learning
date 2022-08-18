using Risk_Business_Layer.Services;
using Risk_Business_Layer.Services.Authentication;

namespace Risk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterEmployee(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<AuthModel>> Login([FromBody] TokenRequestModel model)
        {
            try
            {
                #region Generate Returned Object TypeOf GeneralResponse
                AuthModel GeneralResponse = new AuthModel();
                #endregion

                #region Check ModelState is valid
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                #endregion

                #region Call Login Service
                var result = await _authService.Login(model);
                GeneralResponse = result;
                #endregion

                #region Check If User IsAuthenticated
                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);
                #endregion

                #region return 
                GeneralResponse.Message = "Successffully";
                return Ok(GeneralResponse);
                #endregion
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }


        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

    }
}

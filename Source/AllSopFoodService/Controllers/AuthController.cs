#nullable disable
namespace AllSopFoodService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.ViewModels.UserAuth;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Repositories.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;
        public AuthController(IAuthRepository authRepository) => this.authRepo = authRepository;

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(UserRegisterDto request)
        {
            var userRegistration = new User()
            {
                UserName = request.Username,
                Cart = new ShoppingCart()
                {
                    CartLabel = $"This Shopping Cart is owned by {request.Username}"
                }
            };

            var response = await this.authRepo.RegisterAsync(userRegistration, request.Password).ConfigureAwait(true);

            if (!response.Success)
            {
                return this.BadRequest(response);
            }

            return this.Ok(response);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync(UserLoginDto request)
        {
            var response = await this.authRepo.LoginAsync(request.Username, request.Password).ConfigureAwait(true);

            if (!response.Success)
            {
                return this.BadRequest(response);
            }

            return this.Ok(response);
        }

        [HttpDelete("delete-current-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteUserAndCart(int userId)
        {
            var response = this.authRepo.DeleteUserAccount(userId);
            if (!response.Data)
            {
                return this.BadRequest(response);
            }

            return this.Ok(response);
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsersAccountAsync()
        {
            var response = await this.authRepo.GetAllUsersAsync().ConfigureAwait(true);
            if (response.Data == null)
            {
                return this.NotFound(response);
            }

            return this.Ok(response);
        }
    }
}

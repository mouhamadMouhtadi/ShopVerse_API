using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ShopVerse.API.Errors;
using ShopVerse.API.Extensions;
using ShopVerse.Core.Dtos.Auth;
using ShopVerse.Core.Entities.Identity;
using ShopVerse.Core.Services.Interfaces;
using ShopVerse.Services.Services.Token;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopVerse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController( IUserService userService, UserManager<AppUser> userManager,ITokenService tokenService,IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login (LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if (user is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register (RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(user);
        }
        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var user = await _userManager.FindByEmailAsync(userEmail);
            if(user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindByEmailWithAdsressAsync(User);
            if(user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(_mapper.Map<AddressDto>(user.Address));
        }
        [HttpPut("Address")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateUserAddress(UpdateUserAddressDto addressDto)
        {
            var result = await _userManager.UpdateUserAdddresAsync(User,addressDto);
            if(result is null ) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(result);
        }
        }
}

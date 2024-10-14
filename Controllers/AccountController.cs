using IdentityAndDataProtectionExample.Model;
using IdentityAndDataProtectionExample.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAndDataProtectionExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { massage = "Kayit Basarili" });
            }

            return BadRequest(new { errors = result.Errors.Select(x => x.Description) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) });

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
                return Ok(new { massage = "Giris Basarili" });
            else
                return Unauthorized(new { massage = "Kullanici adi veya sifre yanlis" });

        }

        [HttpPost("creatrole")]
        public async Task<IActionResult> CreatRole([FromBody] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return BadRequest(new { massage = "Rol adi bos olamaz" });

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
                return Ok(new { massage = "Rol olusturuldu" });

            else return BadRequest(new { error = result.Errors.Select(x => x.Description) });

        }


        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost("addtorole")]
        public async Task<IActionResult> AddToRole(AddToRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
                return NotFound(new { massage = "Kullanici bulunamadi" });

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
                return NotFound(new { massage = "Rol bulunamadi" });

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Succeeded)
                return Ok(new { massage = "Rol eklendi" });

            else return BadRequest(new { errors = result.Errors.Select(x => x.Description) });

        }

        [HttpGet("userroles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound(new { massage = "Kulllanici bulunamadi" });

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }
    }
}


using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InventoryControl.Configurations;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class IdentityService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly JwtOptions _jwtOptions;
    

    public IdentityService(SignInManager<User> signInManager, UserManager<User> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<ResponseRequest<SimpleUserResponseRequest>> CreateNewUser(CreateNewUserRequest request)
    {
        var identityUser = new User
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };

        var response = new ResponseRequest<SimpleUserResponseRequest>();

        var result = await _userManager.CreateAsync(identityUser, request.Senha);
        if (result.Succeeded)
        {
            await _userManager.SetLockoutEnabledAsync(identityUser, false);
            response.Success = new SimpleUserResponseRequest(identityUser);
        }

        if (result.Errors.Count() > 0)
        {
            response.SetErros(result.Errors.Select(er => er.Description));
        }

        return response;
    }

    public async Task<ResponseRequest<object>> Login(UserLoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);

        var response = new ResponseRequest<object>();
        if (result.Succeeded)
        {
            var token = await GenerateToken(request.Email);
            response.Success = new { token };
        }

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                response.AddError("Conta bloqueada");
            else if (result.IsNotAllowed)
                response.AddError("Essa conta não tem permissão para essa ação");
            else if (result.RequiresTwoFactor)
                response.AddError("É necessário confirmar o login com o código de 2 fatores");
            else
                response.AddError("E-mail or password invalid");
        }

        return response;
    }

    private async Task<string> GenerateToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var tokenClaims = await GetClaims(user);

        var expDate = DateTime.Now.AddSeconds(_jwtOptions.Expiration);
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: tokenClaims,
            notBefore: DateTime.Now,
            expires: expDate,
            signingCredentials: _jwtOptions.SigningCredentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }

    private async Task<IList<Claim>> GetClaims(User user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        var unixTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, unixTimestamp.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, unixTimestamp.ToString()));

        foreach (var role in roles)
            claims.Add(new Claim("role", role));

        return claims;
    }
    
    public async Task<PaginatedResultResponseRequest<UserListItemResponse>> GetPaginatedUsersAsync(int pageNumber, int pageSize)
    {
        var users = _userManager.Users;

        int totalItems = await users.CountAsync();
        var items = await users
            .OrderBy(u => u.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserListItemResponse
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.UserName
            })
            .ToListAsync();


        return new PaginatedResultResponseRequest<UserListItemResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }

    public async Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        user.UserName = request.Email;
        user.Email = request.Email;
        user.UserName = request.Name;

        // Atualiza dados básicos
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return updateResult;

        // Atualiza a senha
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
        return passwordResult;
    }
    public async Task<IdentityResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        return await _userManager.DeleteAsync(user);
    }

    
}
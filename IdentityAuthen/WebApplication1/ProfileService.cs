using Authen.Data;
using Authen.Repositories;
using Authen.Users.Constants;
using Authen.Users.Models;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Authen
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly RedisUserRepository _redisUserRepository;
        public ProfileService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext, RedisUserRepository redisUserRepository
             )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _redisUserRepository = redisUserRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {

            try
            {
                var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

                var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;
                var clientId = context.Client.ClientId;

                var allowsClientIds = new List<string> { "webapp" };


                var user = await _userManager.FindByIdAsync(subjectId);
                if (user == null)
                    throw new ArgumentException("Invalid subject identifier");

                var claims = new List<Claim>
            {
                new Claim("sub",user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName!.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FirstName!.ToString()),
                new Claim(ClaimTypes.Surname, user.LastName!.ToString())


            };

                if (_userManager.SupportsUserEmail)
                {
                    claims.AddRange(new[]
                    {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
                }

                if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    claims.AddRange(new[]
                    {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
                }

                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    claims.Add(new Claim("avatar", user.AvatarUrl!));

                }

                if (user.UserType == DefaultRoleNames.Administrator && allowsClientIds.Contains(clientId))
                {

                    UserProfileViewModel dataCache = await _redisUserRepository.GetUserProfileAsync(Guid.Parse(subjectId));

                    if (dataCache != null)
                    {
                        claims.Add(new Claim("isDarkMode", dataCache!.IsDarkMode ? "true" : "false", ClaimValueTypes.Boolean));
                        claims.Add(new Claim("isNavOpen", dataCache!.IsNavOpen ? "true" : "false", ClaimValueTypes.Boolean));
                        claims.Add(new Claim("lastPageVisited", dataCache.LastPageVisited));
                    }
                    else
                    {
                        var userProfile = await _applicationDbContext.UserProfiles.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

                        if (userProfile == null)
                        {
                            claims.Add(new Claim("isDarkMode", "false", ClaimValueTypes.Boolean));
                            claims.Add(new Claim("isNavOpen", "true", ClaimValueTypes.Boolean));
                            claims.Add(new Claim("lastPageVisited", "/dashboard"));
                        }
                        else
                        {
                            claims.Add(new Claim("isDarkMode", userProfile.IsDarkMode ? "true" : "false", ClaimValueTypes.Boolean));
                            claims.Add(new Claim("isNavOpen", userProfile.IsNavOpen ? "true" : "false", ClaimValueTypes.Boolean));
                            claims.Add(new Claim("lastPageVisited", userProfile.LastPageVisited));
                        }

                    }

                    claims = await GetClaimsFromUser(user, claims);

                    context.IssuedClaims = claims.ToList();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            var clientId = context.Client.ClientId;
            var userClaims = await _userManager.GetClaimsAsync(user);
            var policies = await _applicationDbContext.ClientClaimPolicies.Where(x => x.ClientId == clientId).ToListAsync();

            foreach (var policy in policies.Where(p => p.IsEnabled))
            {
                var hasClaim = userClaims.Any(c =>
                    c.Type == policy.RequiredClaim &&
                     policy.ClaimValue);

                if (!hasClaim)
                {
                    // Người dùng không đạt yêu cầu
                    context.IsActive = false;
                    return;
                }
            }

            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                        if (db_security_stamp != security_stamp)
                            return;
                    }
                }

                context.IsActive =
                    !user.LockoutEnabled ||
                    !user.LockoutEnd.HasValue ||
                    user.LockoutEnd <= DateTime.UtcNow;
            }
        }

        private async Task<List<Claim>> GetClaimsFromUser(ApplicationUser user, List<Claim> claims)
        {

            var lstRole = await _applicationDbContext.UserRoles.Where(x => x.UserId == user.Id).Include(x => x.Role).Select(x => x.Role).ToListAsync();

            foreach (var item in lstRole)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, item.Name));

                foreach (var item2 in await _roleManager.GetClaimsAsync(item))
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, item2.Value));
                }
            }

            return claims;
        }


    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Helper;
using Vezeeta_Clone.Data.Results;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Service.Abstract;

namespace Vezeeta_Clone.Service.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDoctorRepo _doctorRepo;
        private readonly IPatientRepo _patientRepo;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        public AuthenticationService(UserManager<ApplicationUser> userManager,
                                      IDoctorRepo doctorRepo,
                                      IPatientRepo patientRepo,
                                      JwtSettings jwtSettings,
                                      IRefreshTokenRepo refreshTokenRepo)
        {
            _userManager = userManager;
            _patientRepo = patientRepo;
            _doctorRepo = doctorRepo;
            _jwtSettings = jwtSettings;
            _refreshTokenRepo = refreshTokenRepo;
        }




        public async Task RegisterDoctorAsync(Doctor doctor, ApplicationUser user, string password)
        {

            await using var transaction = _doctorRepo.BeginTransaction();

            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                doctor.AppUserID = user.Id;

                var roleResult = await _userManager.AddToRoleAsync(user, Roles.Doctor);

                if (!roleResult.Succeeded)
                    throw new InvalidOperationException("Failed to assign role (Doctor): " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));

                await _doctorRepo.AddAsync(doctor);
                await _doctorRepo.SaveChangesAsync();
                transaction.Commit();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //log the exception  //LATER
                throw;
            }

        }

        public async Task RegisterPatientAsync(Patient patient, ApplicationUser user, string password)
        {

            await using var transaction = _patientRepo.BeginTransaction();
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                patient.AppUserID = user.Id;
                var roleResult = await _userManager.AddToRoleAsync(user, Roles.Patient);

                if (!roleResult.Succeeded)
                    throw new InvalidOperationException("Failed to assign role (Patient): " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));

                await _patientRepo.AddAsync(patient);
                await _patientRepo.SaveChangesAsync();
                transaction.Commit();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //log the exception  //LATER
                throw;
            }

        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                return true;
            return false;
        }

        public async Task<bool> UserExistsByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
                return true;
            return false;
        }

        public async Task<JwtAuthResult> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var (jwtToken, accessToken) = await GetJwtTokenAsync(user);

            //refresh token
            var refreshToken = GetRefreshToken(user.Id);

            var userTokens = new UserToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiredAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate),
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                JwtId = jwtToken.Id,
                IsUsed = true,
                IsRevoked = false,
            };
            var createTokenResult = await _refreshTokenRepo.AddAsync(userTokens);

            if (createTokenResult == null)
                throw new InvalidOperationException("Failed to save refresh token for user: " + user.Id);

            await _refreshTokenRepo.SaveChangesAsync();

            // return both types of tokens 
            var jwtAuthResult = new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return jwtAuthResult;
        }

        public async Task<bool> ValidateJwtToken(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentNullException("Access token is null or empty");

            var tokenHandler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuer = _jwtSettings.Issuer,

                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidAudience = _jwtSettings.Audience,

                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_jwtSettings.Secret)),

                ValidateLifetime = _jwtSettings.ValidateLifeTime,
                RoleClaimType = ClaimTypes.Role
            };
            //read and validate the token
            try
            {
                var result = tokenHandler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtToken))
                    throw new SecurityTokenException("Invalid JWT token");

                if (jwtToken.Header.Alg != SecurityAlgorithms.HmacSha256Signature)
                    throw new SecurityTokenException("Invalid token algorithm");

                return true;
            }
            catch (Exception ex)
            {
                //log the exception  //LATER
                throw;
            }

        }
        public async Task<JwtAuthResult> GetRefreshTokenAsync(string accessToken, string refreshToken)
        {
            //read access token
            var jwtToken = ReadJwtToken(accessToken);
            //  bool isValid = await ValidateJwtToken(accessToken);

            if (jwtToken.ValidTo > DateTime.UtcNow)
                throw new SecurityTokenException("Access Token has not expired yet");

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == nameof(AppUserClaimModel.Id))?.Value;
            var userTokens = _refreshTokenRepo.GetTableNoTracking().FirstOrDefault(t => t.UserId == userId &&
                                                                                t.AccessToken == accessToken &&
                                                                                t.RefreshToken == refreshToken);
            if (userTokens == null)
            {
                throw new SecurityTokenException("There is no Refresh token for this user");
            }
            if (userTokens.ExpiredAt < DateTime.UtcNow) // i mean refresh token expiry data
            {
                userTokens.IsRevoked = true; // stop using this refresh token
                await _refreshTokenRepo.UpdateAsync(userTokens);
                await _refreshTokenRepo.SaveChangesAsync();
                throw new SecurityTokenException("Refresh Token is expired");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new SecurityTokenException("user is not found");
            }

            var (newJwtToken, newAccesToken) = await GetJwtTokenAsync(user); // new access token

            var refreshTokenObj = new RefreshToken
            {
                ExpiresAt = userTokens.ExpiredAt,
                Token = refreshToken,
                Id = userId
            };
            var jwtAuthResult = new JwtAuthResult
            {
                AccessToken = newAccesToken,
                RefreshToken = refreshTokenObj
            };
            return jwtAuthResult;

        }
        private async Task<(JwtSecurityToken, string)> GetJwtTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            //claims
            var claims = GetUserClaims(user, roles.ToList());

            //access token
            var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                                                                 SecurityAlgorithms.HmacSha256Signature);

            var jwtToken = new JwtSecurityToken(
                            issuer: _jwtSettings.Issuer,
                            audience: _jwtSettings.Audience,
                            claims: claims,
                            signingCredentials: signingCredentials,
                            expires: DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }
        private RefreshToken GetRefreshToken(string userId)
        {
            return new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate),
                Token = GenerateRefreshToken(),
                Id = userId
            };

        }
        private List<Claim> GetUserClaims(ApplicationUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(nameof(AppUserClaimModel.Id), user.Id),
                new Claim(nameof(AppUserClaimModel.Email), user.Email),
                new Claim(nameof(AppUserClaimModel.UserName), user.UserName)

            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private JwtSecurityToken ReadJwtToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            return jwtToken;
        }

    }
}

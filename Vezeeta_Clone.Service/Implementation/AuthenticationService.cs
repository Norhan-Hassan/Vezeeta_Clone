using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vezeeta_Clone.Data.Commons;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Helper;
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
        public AuthenticationService(UserManager<ApplicationUser> userManager,
                                      IDoctorRepo doctorRepo,
                                      IPatientRepo patientRepo,
                                      JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _patientRepo = patientRepo;
            _doctorRepo = doctorRepo;
            _jwtSettings = jwtSettings;
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

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.NameIdentifier, user.Id),
                //new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Name,user.UserName)
                new Claim(nameof(AppUserClaimModel.Id), user.Id),
                new Claim(nameof(AppUserClaimModel.Email), user.Email),
                new Claim(nameof(AppUserClaimModel.UserName), user.UserName)
            };
            var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                                                                 SecurityAlgorithms.HmacSha256Signature);

            var jwtToken = new JwtSecurityToken(
                            issuer: _jwtSettings.Issuer,
                            audience: _jwtSettings.Audience,
                            claims: claims,
                            signingCredentials: signingCredentials,
                            expires: DateTime.UtcNow.AddDays(2));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }
    }
}

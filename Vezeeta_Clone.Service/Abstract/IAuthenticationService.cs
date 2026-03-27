using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Data.Results;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IAuthenticationService
    {
        Task RegisterDoctorAsync(Doctor doctor, ApplicationUser user, string password);
        Task RegisterPatientAsync(Patient patient, ApplicationUser user, string password);
        Task<JwtAuthResult> GenerateJwtTokenAsync(ApplicationUser user);
        Task<JwtAuthResult> GetRefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> ValidateJwtToken(string accessToken);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<bool> UserExistsByUserNameAsync(string userName);
        Task<string> GetEmailConfirmationUrlAsync(ApplicationUser user);
        Task<bool> ConfirmEmailAsync(string? email, string? code);
        Task<string> GetResetPasswordCodeAsync(string email);
        Task<string> CheckResetPasswordCodeAsync(string email, string code);
        Task<bool> ResetPasswordAsync(string email, string password);

    }
}

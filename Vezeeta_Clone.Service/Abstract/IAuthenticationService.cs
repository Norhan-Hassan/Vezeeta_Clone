using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IAuthenticationService
    {
        Task RegisterDoctorAsync(Doctor doctor, ApplicationUser user, string password);
        Task RegisterPatientAsync(Patient patient, ApplicationUser user, string password);
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<bool> UserExistsByUserNameAsync(string userName);
    }
}

namespace Vezeeta_Clone.Service.Abstract
{
    public interface IAutherizationService
    {
        Task<bool> AddRoleAync(string roleName);
        Task<bool> IsRoleExist(string roleName);
        Task<bool> UpdateRoleAync(string id, string roleName);
        Task<bool> DeleteRoleAync(string id);
    }
}

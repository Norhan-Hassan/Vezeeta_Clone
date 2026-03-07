using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class RefreshTokenRepo : GenericRepositoryAsync<UserToken>, IRefreshTokenRepo
    {
        private readonly DbSet<UserToken> _userToken;
        public RefreshTokenRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _userToken = dbContext.Set<UserToken>();
        }
    }
}

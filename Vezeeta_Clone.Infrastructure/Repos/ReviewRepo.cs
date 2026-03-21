using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class ReviewRepo : GenericRepositoryAsync<Review>, IReviewRepo
    {
        private readonly DbSet<Review> _reviewRepo;
        public ReviewRepo(ApplicationDbContext context) : base(context)
        {
            _reviewRepo = context.Set<Review>();
        }
    }
}

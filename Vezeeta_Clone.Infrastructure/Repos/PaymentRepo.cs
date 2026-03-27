using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class PaymentRepo : GenericRepositoryAsync<Payment>, IPaymentRepo
    {
        private DbSet<Payment> _payment;
        public PaymentRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _payment = dbContext.Set<Payment>();
        }
    }
}

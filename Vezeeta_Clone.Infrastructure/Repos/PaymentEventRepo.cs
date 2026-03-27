using Microsoft.EntityFrameworkCore;
using Vezeeta_Clone.Data.Entities;
using Vezeeta_Clone.Infrastructure.Abstract;
using Vezeeta_Clone.Infrastructure.Context;
using Vezeeta_Clone.Infrastructure.InfrastructureBases;

namespace Vezeeta_Clone.Infrastructure.Repos
{
    public class PaymentEventRepo : GenericRepositoryAsync<PaymentEvent>, IPaymentEventRepo
    {
        private DbSet<PaymentEvent> _paymentEvents;
        public PaymentEventRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _paymentEvents = dbContext.Set<PaymentEvent>();
        }
    }
}

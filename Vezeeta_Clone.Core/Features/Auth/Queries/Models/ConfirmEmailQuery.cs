using MediatR;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Auth.Queries.Models
{
    public class ConfirmEmailQuery : IRequest<Response<string>>
    {
        public string userEmail { get; set; }
        public string code { get; set; }
    }
}

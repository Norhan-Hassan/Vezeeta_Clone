using MediatR;
using Vezeeta_Clone.Core.Bases;
using Vezeeta_Clone.Core.Features.Auth.Shared;
using Vezeeta_Clone.Data.Entities.Enums;

namespace Vezeeta_Clone.Core.Features.Auth.Commands.Models
{
    public class RegisterPatientCommand : RegisterUserBase, IRequest<Response<string>>
    {
        public DateTime DateOfBirth { get; set; }
        public BloodType? Blood_Type { get; set; }
    }
}

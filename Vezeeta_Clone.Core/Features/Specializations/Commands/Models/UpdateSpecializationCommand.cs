using MediatR;
using System.Text.Json.Serialization;
using Vezeeta_Clone.Core.Bases;

namespace Vezeeta_Clone.Core.Features.Specializations.Commands.Models
{
    public class UpdateSpecializationCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}

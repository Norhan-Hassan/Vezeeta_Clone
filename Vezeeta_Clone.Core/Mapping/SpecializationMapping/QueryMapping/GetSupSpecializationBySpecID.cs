using Vezeeta_Clone.Core.Features.Specializations.Queries.Results;
using Vezeeta_Clone.Data.Entities;

namespace Vezeeta_Clone.Core.Mapping.SpecializationMapping
{
    public partial class SpecializationProfile
    {
        public void GetSupSpecializationBySpecID()
        {
            CreateMap<SubSpecialization, GetSubSpecializationBySpecIDQueryResult>();
        }
    }
}

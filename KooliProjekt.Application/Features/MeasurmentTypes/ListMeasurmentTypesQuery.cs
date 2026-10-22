using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MeasurmentTypes
{
    [ExcludeFromCodeCoverage]
    public class ListMeasurmentTypesQuery : IRequest<OperationResult<PagedResult<MeasurmentType>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Description { get; set; }
    }
}

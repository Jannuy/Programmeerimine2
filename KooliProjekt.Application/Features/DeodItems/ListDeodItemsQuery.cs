using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DeodItems
{
    [ExcludeFromCodeCoverage]
    public class ListDeodItemsQuery : IRequest<OperationResult<PagedResult<DeodItem>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Name { get; set; }
    }
}

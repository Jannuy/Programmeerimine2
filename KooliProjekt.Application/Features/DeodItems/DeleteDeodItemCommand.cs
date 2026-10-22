using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DeodItems
{
    // DeodItem kustutamise käsk
    [ExcludeFromCodeCoverage]
    public class DeleteDeodItemCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

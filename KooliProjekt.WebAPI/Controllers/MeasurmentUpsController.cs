using System.Threading.Tasks;
using KooliProjekt.Application.Features.MeasurmentUps;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class MeasurmentUpsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public MeasurmentUpsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListMeasurmentUpsQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetMeasurmentUpQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveMeasurmentUpCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteMeasurmentUpCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}

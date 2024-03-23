using AppSec.Domain.Commands.Base;
using AppSec.Domain.Interfaces.ICommands;

namespace AppSec.Domain.Commands;

public class StartDastCommandHandler : IStartDastCommandHandler
{
    public Task<StartDastResponse> Handle(StartDastRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

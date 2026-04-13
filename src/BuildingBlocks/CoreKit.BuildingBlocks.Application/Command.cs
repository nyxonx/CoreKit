using MediatR;

namespace CoreKit.BuildingBlocks.Application;

public interface ICommand<TResponse> : IRequest<OperationResult<TResponse>>
{
}

using MediatR;

namespace CoreKit.BuildingBlocks.Application;

public interface IQuery<TResponse> : IRequest<OperationResult<TResponse>>
{
}

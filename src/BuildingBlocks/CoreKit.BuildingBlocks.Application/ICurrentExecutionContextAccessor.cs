namespace CoreKit.BuildingBlocks.Application;

public interface ICurrentExecutionContextAccessor
{
    CurrentExecutionContext GetCurrent();
}

using System.Reflection;
using CoreKit.BuildingBlocks.Application;
using MediatR;

namespace CoreKit.PlatformAppHost.Server.Rpc;

public sealed class RpcOperationRegistry
{
    private readonly IReadOnlyDictionary<string, RpcOperationDescriptor> operations;

    public RpcOperationRegistry(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        operations = assemblies
            .Distinct()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .Select(
                type => new
                {
                    Type = type,
                    Operation = type.GetCustomAttribute<RpcOperationAttribute>()
                })
            .Where(candidate =>
                candidate.Operation is not null
                && candidate.Type.GetInterfaces().Any(@interface =>
                    @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(IRequest<>)))
            .ToDictionary(
                candidate => candidate.Operation!.Name,
                candidate => new RpcOperationDescriptor(candidate.Operation!.Name, candidate.Type),
                StringComparer.OrdinalIgnoreCase);
    }

    public bool TryGet(string operationName, out RpcOperationDescriptor descriptor)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(operationName);

        return operations.TryGetValue(operationName, out descriptor!);
    }

    public int Count => operations.Count;
}

namespace CoreKit.BuildingBlocks.Application;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RpcOperationAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

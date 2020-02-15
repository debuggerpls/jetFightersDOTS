using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct TimeToLiveData : IComponentData
{
    public float TimeInSeconds;
}

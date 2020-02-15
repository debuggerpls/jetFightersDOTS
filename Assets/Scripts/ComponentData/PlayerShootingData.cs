using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerShootingData : IComponentData
{
    public int IsShooting;
}

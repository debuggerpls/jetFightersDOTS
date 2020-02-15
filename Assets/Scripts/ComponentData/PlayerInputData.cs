using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerInputData : IComponentData
{
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Shoot;
    public KeyCode Restart;
}

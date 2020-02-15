using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PlayerInputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithAll<PlayerData>()
            .ForEach((ref PlayerShootingData shoot, ref PlayerMovementData movement, in PlayerInputData input) =>
            {
                if (Input.GetKey(input.Restart))
                {
                    EntityManager.CreateEntity(typeof(RestartTag));
                }
                else
                {
                    movement.Turn = 0f;
                    movement.Turn += Input.GetKey(input.Left) ? 1 : 0;
                    movement.Turn -= Input.GetKey(input.Right) ? 1 : 0;
                    shoot.IsShooting = Input.GetKeyDown(input.Shoot) ? 1 : 0;
                }
            }).WithStructuralChanges().Run();

        return default;
    }
}

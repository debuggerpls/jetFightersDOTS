using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class BulletSpawnSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        ecbSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer.Concurrent ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();
        Entity bullet0 = GameManager.main.playerBullet0Entity;
        Entity bullet1 = GameManager.main.playerBullet1Entity;
        float bulletTTL = GameManager.main.bulletTTL;
        

        JobHandle myJob = Entities.ForEach(
            (int entityInQueryIndex, in PlayerData player, in PlayerShootingData shoot, in Translation pos,
                in Rotation rot, in LocalToWorld ltw) =>
            {
                if (shoot.IsShooting == 1)
                {
                    // TODO: find a way to instantiate entities without them being place at their prefab location first,
                    // so there is no need to put the prefab out of bounds
                    if (player.PlayerID == 0)
                    {
                        float3 position = pos.Value + (ltw.Up * 1.5f);
                        ecb.SetComponent(entityInQueryIndex, bullet0,
                            new Translation() {Value = position});
                        ecb.SetComponent(entityInQueryIndex, bullet0,
                            new TimeToLiveData() { TimeInSeconds = bulletTTL});
                        ecb.SetComponent(entityInQueryIndex, bullet0, rot);
                        ecb.Instantiate(entityInQueryIndex, bullet0);
                    }
                    else
                    {
                        float3 position = pos.Value + (ltw.Up * 1.5f);
                        ecb.SetComponent(entityInQueryIndex, bullet1,
                            new Translation() {Value = position});
                        ecb.SetComponent(entityInQueryIndex, bullet1,
                            new TimeToLiveData() { TimeInSeconds = bulletTTL});
                        ecb.SetComponent(entityInQueryIndex, bullet1, rot);
                        ecb.Instantiate(entityInQueryIndex, bullet1);
                    }
                }
            }).Schedule(inputDeps);

        ecbSystem.AddJobHandleForProducer(myJob);
        return myJob;
    }
}
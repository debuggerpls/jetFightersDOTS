using Unity.Entities;
using Unity.Jobs;

public class BulletRemovalSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem cmdBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        cmdBufferSystem = World.DefaultGameObjectInjectionWorld
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer.Concurrent ecb = cmdBufferSystem.CreateCommandBuffer().ToConcurrent();
        JobHandle myJob = Entities.ForEach((int entityInQueryIndex, Entity entity, in TimeToLiveData data) =>
        {
            if (data.TimeInSeconds <= 0f)
            {
                ecb.DestroyEntity(entityInQueryIndex, entity);
            }
        }).Schedule(inputDeps);
        
        cmdBufferSystem.AddJobHandleForProducer(myJob);
        
        return myJob;
    }
}

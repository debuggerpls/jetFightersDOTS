using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class DistanceToLiveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        JobHandle myJob = Entities.ForEach((ref TimeToLiveData ttl) =>
            {
                ttl.TimeInSeconds -= deltaTime;
            }).Schedule(inputDeps);

        return myJob;
    }
}

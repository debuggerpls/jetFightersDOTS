using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class BulletMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float xBound = GameManager.main.xBound;
        float yBound = GameManager.main.yBound;
        
        return Entities.ForEach((ref Translation pos, in BulletData data, in LocalToWorld ltw) =>
            {
                pos.Value += ltw.Up * data.Speed * deltaTime;
                if (pos.Value.x > xBound)
                    pos.Value.x -= xBound * 2;
                if (pos.Value.x < -xBound)
                    pos.Value.x += xBound * 2;
                if (pos.Value.y > yBound)
                    pos.Value.y -= yBound * 2;
                if (pos.Value.y < -yBound)
                    pos.Value.y += yBound * 2;
            }).Schedule(inputDeps);
    }
}

using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float xBound = GameManager.main.xBound;
        float yBound = GameManager.main.yBound;
        
        return Entities.ForEach((ref Translation pos, ref Rotation rot, in PlayerMovementData move, in LocalToWorld ltw) =>
        {
            rot.Value = math.mul(
                math.normalize(rot.Value),
                quaternion.AxisAngle(math.forward(rot.Value), move.Turn * move.Speed * deltaTime / math.PI));
            pos.Value += ltw.Up * move.Speed * deltaTime;
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

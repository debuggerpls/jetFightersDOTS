using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class RestartSystem : JobComponentSystem
{
    private EntityQuery playerQuery;
    private EntityQuery bulletQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        playerQuery = GetEntityQuery(typeof(PlayerData));
        bulletQuery = GetEntityQuery(typeof(BulletData));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        int shouldRestart = 0;
        float3 p0pos = GameManager.main.player0StartPosition;
        float3 p1pos = GameManager.main.player1StartPosition;
        Entities
            .WithStructuralChanges()
            .WithAll<RestartTag>()
            .ForEach((Entity entity) =>
            {
                // notify gamemanager
                shouldRestart = 1;
                // move players to their start positions
                NativeArray<Entity> players = playerQuery.ToEntityArray(Allocator.TempJob);
                NativeArray<PlayerData> playerDatas = playerQuery.ToComponentDataArray<PlayerData>(Allocator.TempJob);

                for (int i = 0; i < playerDatas.Length; i++)
                {
                    if (playerDatas[i].PlayerID == 0)
                    {
                        EntityManager.SetComponentData(players[i], new Translation(){Value = p0pos});
                    }
                    else
                    {
                        EntityManager.SetComponentData(players[i], new Translation(){Value = p1pos});
                    }
                }
                players.Dispose();
                playerDatas.Dispose();
                // remove bullets
                EntityManager.DestroyEntity(bulletQuery);
                // delete itself
                EntityManager.DestroyEntity(entity);
                
            }).Run();

        if (shouldRestart == 1)
            GameManager.main.Restart();

        return default;
    }
}

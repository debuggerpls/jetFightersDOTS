using System.ComponentModel;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

public class ScoringSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    
    //[BurstCompile]
    private struct TriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<PlayerData> playerGroup;
        public ComponentDataFromEntity<Bullet0Tag> bullet0Group;
        public ComponentDataFromEntity<Bullet1Tag> bullet1Group;
        public ComponentDataFromEntity<TimeToLiveData> timeGroup;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (playerGroup.HasComponent(triggerEvent.Entities.EntityA))
            {
                int playerId = playerGroup[triggerEvent.Entities.EntityA].PlayerID;
                
                // player0 collided with bullet from player1
                if (playerId == 0 && bullet1Group.HasComponent(triggerEvent.Entities.EntityB))
                {
                    TimeToLiveData time = timeGroup[triggerEvent.Entities.EntityB];
                    time.TimeInSeconds = 0f;
                    timeGroup[triggerEvent.Entities.EntityB] = time;
                    GameManager.main.Scored(1);
                }
                // palyer1 collided with bullet from player0
                if (playerId == 1 && bullet0Group.HasComponent(triggerEvent.Entities.EntityB))
                {
                    TimeToLiveData time = timeGroup[triggerEvent.Entities.EntityB];
                    time.TimeInSeconds = 0f;
                    timeGroup[triggerEvent.Entities.EntityB] = time;
                    GameManager.main.Scored(0);
                }
            }
            
            if (playerGroup.HasComponent(triggerEvent.Entities.EntityB))
            {
                int playerId = playerGroup[triggerEvent.Entities.EntityB].PlayerID;
                
                // player0 collided with bullet from player1
                if (playerId == 0 && bullet1Group.HasComponent(triggerEvent.Entities.EntityA))
                {
                    TimeToLiveData time = timeGroup[triggerEvent.Entities.EntityA];
                    time.TimeInSeconds = 0f;
                    timeGroup[triggerEvent.Entities.EntityA] = time;
                    GameManager.main.Scored(1);
                }
                // palyer1 collided with bullet from player0
                if (playerId == 1 && bullet0Group.HasComponent(triggerEvent.Entities.EntityA))
                {
                    TimeToLiveData time = timeGroup[triggerEvent.Entities.EntityA];
                    time.TimeInSeconds = 0f;
                    timeGroup[triggerEvent.Entities.EntityA] = time;
                    GameManager.main.Scored(0);
                }
            }
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        int scoredP0 = 0;
        int scoredP1 = 0;
        TriggerJob myJob = new TriggerJob
        {
            playerGroup = GetComponentDataFromEntity<PlayerData>(),
            bullet0Group = GetComponentDataFromEntity<Bullet0Tag>(),
            bullet1Group = GetComponentDataFromEntity<Bullet1Tag>(),
            timeGroup = GetComponentDataFromEntity<TimeToLiveData>()
        };
        JobHandle myJobHandle =
            myJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

        return myJobHandle;
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }
}

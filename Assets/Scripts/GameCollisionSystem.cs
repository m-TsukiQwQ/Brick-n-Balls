using Unity.Burst;

using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct GameCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        SimulationSingleton sumulation = SystemAPI.GetSingleton<SimulationSingleton>();

        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var brickHealthLookup = SystemAPI.GetComponentLookup<BrickHealth>(false);
        var brickTagLookup = SystemAPI.GetComponentLookup<BrickTag>(true);
        var ballTagLookup = SystemAPI.GetComponentLookup<BallTag>(true);

        var floorTagLookup = SystemAPI.GetComponentLookup<FloorTag>(true);


        var job = new CollisionJob
        {
            BrickHealthGroup = brickHealthLookup,
            BrickTagGroup = brickTagLookup,
            BallTagGroup = ballTagLookup,
            ECB = ecb,
            FloorTagGroup = floorTagLookup
        };

        state.Dependency = job.Schedule(sumulation, state.Dependency);
    }

    [BurstCompile]
    struct CollisionJob : ICollisionEventsJob
    {
        public ComponentLookup<BrickHealth> BrickHealthGroup;
        [ReadOnly] public ComponentLookup<BrickTag> BrickTagGroup;
        [ReadOnly] public ComponentLookup<FloorTag> FloorTagGroup;
        [ReadOnly] public ComponentLookup<BallTag> BallTagGroup;
        public EntityCommandBuffer ECB;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            CheckBallHitBrick(entityA, entityB);
            CheckBallHitBrick(entityB, entityA);

            CheckBallHitFloor(entityA, entityB);
            CheckBallHitFloor(entityB, entityA);

        }

        private void CheckBallHitBrick(Entity ball, Entity brick)
        {
            if (!BallTagGroup.HasComponent(ball)) return;

            if (!BrickTagGroup.HasComponent(brick)) return;

            var healthComponent = BrickHealthGroup[brick];
            healthComponent.Value -= 1;

            BrickHealthGroup[brick] = healthComponent;

            if (healthComponent.Value <= 0)
            {
                ECB.DestroyEntity(brick);
            }
        }

        private void CheckBallHitFloor(Entity ball, Entity floor)
        {
            if (!BallTagGroup.HasComponent(ball)) return;
            if (!FloorTagGroup.HasComponent(floor)) return;

            ECB.DestroyEntity(ball);
        }

    }
}

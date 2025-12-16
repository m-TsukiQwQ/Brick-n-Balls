using Unity.Entities;
using UnityEngine;

public struct BallSpawnerComponent : IComponentData
{
    public Entity BallPrefab;
}
public class BallSpawnerAuthoring : MonoBehaviour
{
   public GameObject BallPrefab;

    class Baker : Baker<BallSpawnerAuthoring>
    {
        public override void Bake(BallSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BallSpawnerComponent
            {
                BallPrefab = GetEntity(authoring.BallPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}

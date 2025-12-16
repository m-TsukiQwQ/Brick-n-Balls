using Unity.Entities;
using UnityEngine;

public struct BrickTag : IComponentData { }

public struct BrickHealth : IComponentData
{
    public int Value;
}

public struct DestroyTag : IComponentData { }

public class BrickAuthoring : MonoBehaviour
{
    public int Health = 1;
    class Baker : Baker<BrickAuthoring>
    {
        public override void Bake(BrickAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new BrickTag());
            AddComponent(entity, new BrickHealth { Value = authoring.Health });
        }
    }

}

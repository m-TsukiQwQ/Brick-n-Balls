using Unity.Entities;
using UnityEngine;

public class FloorAuthoring : MonoBehaviour
{
    class Baker : Baker<FloorAuthoring>
    {
        public override void Bake(FloorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new FloorTag());
        }
    }
}

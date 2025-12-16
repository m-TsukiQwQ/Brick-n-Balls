using UnityEngine;
using Unity.Entities;

public struct BrickConfigData : IComponentData
{
    public Entity BrickEntityPrefab;
}


public class BrickConfigAuthoring : MonoBehaviour
{
    public GameObject PhysicsBrickPrefab; 

    class Baker : Baker<BrickConfigAuthoring>
    {
        public override void Bake(BrickConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BrickConfigData
            {

                BrickEntityPrefab = GetEntity(authoring.PhysicsBrickPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
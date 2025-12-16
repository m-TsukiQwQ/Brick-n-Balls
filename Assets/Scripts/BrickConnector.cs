using UnityEngine;
using Unity.Entities;

public class BrickConnector : MonoBehaviour
{
    private Entity _targetEntity = Entity.Null;
    private EntityManager _manager;
    private bool _initialized = false;

    public void SetEntity(Entity entity)
    {
        _targetEntity = entity;
        _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _initialized = true;
    }

    void Update()
    {
        if (!_initialized) return;

        if (!_manager.Exists(_targetEntity))
        {
            if (GameManager.Instance != null) 
                GameManager.Instance.AddScore(1);
            

            Destroy(gameObject);
        }
    }

    public Entity GetEntity()
    {
        return _targetEntity; 
    }
}
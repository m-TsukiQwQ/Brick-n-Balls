using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class BallConnector : MonoBehaviour
{
    private EntityManager _entityManager;
    private Entity _targetEntity;


    public void SetTargetEntity(Entity entity)
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _targetEntity = entity;
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetEntity == Entity.Null) return;

        if (_entityManager.Exists(_targetEntity) && _entityManager.HasComponent<LocalTransform>(_targetEntity))
        {
            var transformData = _entityManager.GetComponentData<LocalTransform>(_targetEntity);
            transform.position = transformData.Position;
            transform.rotation = transformData.Rotation;
        }
        else
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBallDestroyed();
            }

            Destroy(gameObject);
        }

    }
}

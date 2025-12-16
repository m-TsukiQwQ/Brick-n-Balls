using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("Settings")]
    public float ShootSpeed = 17f; 
    public GameObject visualBallPrefab;
    public Vector3 SpawnPosition = new Vector3(0, -4, 0);

    private GameInput _input;
    private EntityManager _entityManager;

    private void Awake()
    {
        _input = new GameInput();
        _input.Gameplay.Fire.performed += ctx => Shoot();
    }

    void OnEnable() => _input.Enable();
    void OnDisable() => _input.Disable();

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Shoot()
    {

        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.IsGamePlaying) return;

        if (!GameManager.Instance.TryShoot()) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        float distanceToScreen = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceToScreen));
        mouseWorldPos.z = 0;
        Vector3 direction = (mouseWorldPos - SpawnPosition).normalized;

        var query = _entityManager.CreateEntityQuery(typeof(BallSpawnerComponent));

        if (query.IsEmpty) return;

        var spawnerData = query.GetSingleton<BallSpawnerComponent>();
        var ballPrefab = spawnerData.BallPrefab;

        Entity newBall = _entityManager.Instantiate(ballPrefab);

        _entityManager.SetComponentData(newBall, Unity.Transforms.LocalTransform.FromPosition(0, -4, 0));

        float3 velocity = new float3(direction.x, direction.y, 0) * ShootSpeed;

        _entityManager.SetComponentData(newBall, new PhysicsVelocity
        {
            Linear = velocity,
            Angular = float3.zero
        });

        GameObject visualsObject = Instantiate(visualBallPrefab, new Vector3(0, -4, 0), quaternion.identity);

        var connector = visualsObject.GetComponent<BallConnector>();
        if (connector != null)
        {
            connector.SetTargetEntity(newBall);
        }
    }
}

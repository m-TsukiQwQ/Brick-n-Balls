using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics; 
using Unity.Transforms; 
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject VisualBrickPrefab; 

    [Header("Grid Settings")]
    public int Columns = 8;
    public int Rows = 4;
    public float SpacingX = 1.1f;
    public float SpacingY = 0.6f;
    public Vector3 StartOffset = new Vector3(-4f, 2f, 0f);

    private EntityManager _manager;


    private List<GameObject> _spawnedBricks = new List<GameObject>();
    private Entity _physicsPrefab;


    void Start()
    {
        _manager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }



    private void GenerateBricks()
    {
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                SpawnSingleBrick(x, y);
            }
        }
    }

    private void SpawnSingleBrick(int x, int y)
    {
        float posX = StartOffset.x + (x * SpacingX);
        float posY = StartOffset.y + (y * SpacingY);
        float3 targetPos = new float3(posX, posY, 0);

        int randomHealth = UnityEngine.Random.Range(1, 4); 

        Entity newEntity = _manager.Instantiate(_physicsPrefab);


        _manager.SetComponentData(newEntity, LocalTransform.FromPosition(targetPos));

        _manager.SetComponentData(newEntity, new BrickHealth { Value = randomHealth });

        GameObject newVisual = Instantiate(VisualBrickPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        _spawnedBricks.Add(newVisual);

        var renderer = newVisual.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Color baseColor = Color.white;

            switch (randomHealth)
            {
                case 1:
                    baseColor = Color.green;
                    break;
                case 2:
                    baseColor = Color.yellow;
                    break;
                case 3:
                    baseColor = Color.red;
                    break;
            }

            renderer.material.color = Color.Lerp(baseColor, Color.white, 0.5f);
        }

            var connector = newVisual.GetComponent<BrickConnector>();
        if (connector != null) connector.SetEntity(newEntity);
    }

    public void RestartLevel()
    {

        ClearLevel();
        if (_physicsPrefab == Entity.Null)
        {
            var query = _manager.CreateEntityQuery(typeof(BrickConfigData));
            if (!query.IsEmpty)
            {
                _physicsPrefab = query.GetSingleton<BrickConfigData>().BrickEntityPrefab;
            }
            else
            {
                
                return;
            }
        }

        GenerateBricks();
    }

    private void ClearLevel()
    {
        foreach (var brick in _spawnedBricks)
        {
            if (brick != null)
            {

                var connector = brick.GetComponent<BrickConnector>();
                if (connector != null)
                {
                    Entity e = connector.GetEntity();
                    if (_manager.Exists(e))
                    {
                        _manager.DestroyEntity(e);
                    }
                }
                Destroy(brick);
            }
        }
        _spawnedBricks.Clear();
    }
}
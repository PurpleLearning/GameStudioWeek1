using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject Soldiers;
    public GameObject Trees;
    public float spawnOffset = 1f;
    public bool randomizeYPosition = true;
    public float minY = -5f;
    public float maxY = 5f;
    
    // New variables for collision avoidance
    public float checkRadius = 1.5f; // Radius to check for existing objects
    public int maxSpawnAttempts = 10; // Maximum attempts to find empty spot
    
    // Separate timers for different spawn types
    private float treeSpawnTimer;
    private float soldierSpawnTimer;
    
    // Separate intervals
    private const float TREE_SPAWN_INTERVAL = 2f; // Trees spawn every 2 seconds
    private float soldierSpawnInterval = 3f;
    
    private Camera mainCamera;
    
    // Store references to all spawned objects for checking collisions
    private List<GameObject> spawnedObjects = new List<GameObject>();
    
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            enabled = false;
            return;
        }
        
        // Initialize timers
        treeSpawnTimer = TREE_SPAWN_INTERVAL;
        SetRandomSoldierInterval();
    }
    
    void Update()
    {
        // Handle tree spawning
        treeSpawnTimer -= Time.deltaTime;
        if (treeSpawnTimer <= 0)
        {
            SpawnTree();
            treeSpawnTimer = TREE_SPAWN_INTERVAL;
        }
        
        // Handle soldier spawning
        soldierSpawnTimer -= Time.deltaTime;
        if (soldierSpawnTimer <= 0)
        {
            SpawnSoldier();
            SetRandomSoldierInterval();
        }
        
        // Clean up destroyed objects from the list
        spawnedObjects.RemoveAll(item => item == null);
    }
    
    void SetRandomSoldierInterval()
    {
        soldierSpawnInterval = Random.Range(1f, 3f);
        soldierSpawnTimer = soldierSpawnInterval;
    }
    
    void SpawnTree()
    {
        int randomTreeCount = Random.Range(1, 6); // Spawn 1-5 trees at once
        
        for (int i = 0; i < randomTreeCount; i++)
        {
            Vector3 spawnPosition = GetClearSpawnPosition();
            if (spawnPosition != Vector3.zero) // Only spawn if we found a clear spot
            {
                GameObject newTree = Instantiate(Trees, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(newTree);
            }
        }
    }
    
    void SpawnSoldier()
    {
        Vector3 spawnPosition = GetClearSpawnPosition();
        if (spawnPosition != Vector3.zero) // Only spawn if we found a clear spot
        {
            GameObject newSoldier = Instantiate(Soldiers, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newSoldier);
        }
    }
    
    bool IsPositionClear(Vector3 position)
    {
        // Check against all spawned objects
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                float distance = Vector3.Distance(position, obj.transform.position);
                if (distance < checkRadius)
                {
                    return false; // Position is too close to an existing object
                }
            }
        }
        return true; // Position is clear
    }
    
    Vector3 GetClearSpawnPosition()
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector3 testPosition = GetSpawnPosition();
            
            // Check if this position is clear
            if (IsPositionClear(testPosition))
            {
                return testPosition; // Found a clear spot
            }
        }
        
        // If we couldn't find a clear spot after max attempts, try one more time with a different approach
        // This is a fallback - try to spawn slightly above/below the original position
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector3 testPosition = GetSpawnPosition();
            // Add some vertical offset based on attempt number
            float verticalOffset = (attempt % 2 == 0 ? checkRadius : -checkRadius) * (attempt / 2 + 1);
            testPosition.y += verticalOffset;
            
            // Clamp to valid range
            testPosition.y = Mathf.Clamp(testPosition.y, minY, maxY);
            
            if (IsPositionClear(testPosition))
            {
                return testPosition;
            }
        }
        
        Debug.LogWarning("Could not find clear spawn position after " + maxSpawnAttempts + " attempts");
        return GetSpawnPosition(); // Return original position as last resort
    }
    
    Vector3 GetSpawnPosition()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        
        float rightEdgeX = mainCamera.transform.position.x + (cameraWidth / 2f);
        float spawnX = rightEdgeX + spawnOffset;
        
        float spawnY;
        
        if (randomizeYPosition)
        {
            spawnY = Random.Range(minY, maxY);
        }
        else
        {
            spawnY = mainCamera.transform.position.y;
        }
        
        return new Vector3(spawnX, spawnY, 0);
    }
    
    // Optional: Visualize the check radius in the editor
    void OnDrawGizmosSelected()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        if (mainCamera != null)
        {
            float cameraHeight = 2f * mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            
            Vector3 cameraCenter = mainCamera.transform.position;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(cameraCenter, new Vector3(cameraWidth, cameraHeight, 0));
            
            float rightEdgeX = cameraCenter.x + (cameraWidth / 2f);
            float spawnX = rightEdgeX + spawnOffset;
            
            // Draw the spawn line
            Gizmos.color = Color.red;
            if (randomizeYPosition)
            {
                Gizmos.DrawLine(new Vector3(spawnX, minY, 0), new Vector3(spawnX, maxY, 0));
                
                // Draw sample check radius at several points
                Gizmos.color = new Color(1, 0, 0, 0.3f);
                for (float y = minY; y <= maxY; y += (maxY - minY) / 5)
                {
                    Gizmos.DrawWireSphere(new Vector3(spawnX, y, 0), checkRadius);
                }
            }
            else
            {
                Gizmos.DrawSphere(new Vector3(spawnX, cameraCenter.y, 0), 0.3f);
                Gizmos.color = new Color(1, 0, 0, 0.3f);
                Gizmos.DrawWireSphere(new Vector3(spawnX, cameraCenter.y, 0), checkRadius);
            }
        }
    }
}
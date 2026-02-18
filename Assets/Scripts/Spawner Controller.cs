using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject Soldiers;
    public GameObject Trees;
    public GameObject Hospital;
    
    
    public float spawnOffset = 1f;
    public bool randomizeYPosition = true;
    public float minY = -5f;
    public float maxY = 5f;
    
     
    public float checkRadius = 1.5f;  
    public int maxSpawnAttempts = 10;  
    
    
    private float treeSpawnTimer;
    private float soldierSpawnTimer;
    private float hospitalSpawnTimer;  
    
    // Separate intervals
    private const float treeTimeCounter = 2f;  
    private float soldierTimeCounter = 3f;
    private const float hospitalTimeCounter = 5f;  
    
    private Camera mainCamera;
     
    
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
        treeSpawnTimer = treeTimeCounter;
        hospitalSpawnTimer = hospitalTimeCounter;  
        SetRandomSoldierInterval();
    }
    
    void Update()
    {
        
 
        
        
        treeSpawnTimer -= Time.deltaTime;
        if (treeSpawnTimer <= 0)
        {
            SpawnTree();
            treeSpawnTimer = treeTimeCounter;
        }
        
         
        soldierSpawnTimer -= Time.deltaTime;
        if (soldierSpawnTimer <= 0)
        {
            SpawnSoldier();
            SetRandomSoldierInterval();
        }
        
         
        hospitalSpawnTimer -= Time.deltaTime;
        if (hospitalSpawnTimer <= 0)
        {
            SpawnHospital();
            hospitalSpawnTimer = hospitalTimeCounter;
        }
        
         
        spawnedObjects.RemoveAll(item => item == null);
    }


 

    void SetRandomSoldierInterval()
    {
        soldierTimeCounter = Random.Range(1f, 3f);
        soldierSpawnTimer = soldierTimeCounter;
    }
    
    void SpawnTree()
    {
        int randomTreeCount = Random.Range(1, 6);  
        
        for (int i = 0; i < randomTreeCount; i++)
        {
            Vector3 spawnPosition = GetClearSpawnPosition();
            if (spawnPosition != Vector3.zero)  
            {
                GameObject newTree = Instantiate(Trees, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(newTree);
            }
        }
    }
    
    void SpawnSoldier()
    {
        Vector3 spawnPosition = GetClearSpawnPosition();
        if (spawnPosition != Vector3.zero)  
        {
            GameObject newSoldier = Instantiate(Soldiers, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newSoldier);
        }
    }
    
    void SpawnHospital()
    {
         
        float originalCheckRadius = checkRadius;
        checkRadius *= 2f;  
        
        Vector3 spawnPosition = GetClearSpawnPosition();
        if (spawnPosition != Vector3.zero)  
        {
            GameObject newHospital = Instantiate(Hospital, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newHospital);
            Debug.Log("Hospital spawned!");
        }
        else
        {
            Debug.LogWarning("Could not find clear spot for hospital!");
        }
        
        checkRadius = originalCheckRadius;  
    }
    
    bool IsPositionClear(Vector3 position)
    {
         
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                float distance = Vector3.Distance(position, obj.transform.position);
                if (distance < checkRadius)
                {
                    return false;  
                }
            }
        }
        return true;  
    }
    
    Vector3 GetClearSpawnPosition()
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector3 testPosition = GetSpawnPosition();
            
             
            if (IsPositionClear(testPosition))
            {
                return testPosition;  
            }
        }
        
 
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector3 testPosition = GetSpawnPosition();
             
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
        return GetSpawnPosition();  
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
using UnityEngine;

public class MobSpawner
{
    public MobSpawner()
    {
        int boundries = 5;
        int minX = 0;
        int maxX = 100;
        int minZ = 0;
        int maxZ = 100;
        
        var position = FindValidPointOnMap(x: (minX, maxX), z: (minZ, maxZ), borderSize: boundries);
    }
    public void SpawnEnemies(int day)
    {
        var difficultyMultiplier = 1 + (0.1 * day);
        Debug.Log($"SpawnEnemies: Day {day} x {difficultyMultiplier}");

        var prefab = PrefabPool.Prefabs["Prefabs/Entities/Mob"];

        int boundries = 5;
        int minX = 0;
        int maxX = 100;
        int minZ = 0;
        int maxZ = 100;

        for (int i = 0; i < 5; i++)
        {
            var position = FindValidPointOnMap(x: (minX, maxX), z: (minZ, maxZ), borderSize: boundries);
            Debug.Log($"Spawning Mob at {position}");

            var mob = GameObject.Instantiate(prefab, position, Quaternion.identity);
        }
    }

    private Vector3 FindValidPointOnMap((int min, int max) x, (int min, int max) z, int borderSize)
    {
        // Debug.Log($"X Range is {x.min + borderSize} - {x.max - borderSize + 1}");
        // Debug.Log($"Z Range is {z.min + borderSize} - {z.max - borderSize + 1}");
        var randomX = UnityEngine.Random.Range(x.min + borderSize, x.max - borderSize + 1);
        var randomZ = UnityEngine.Random.Range(z.min + borderSize, z.max - borderSize + 1);

        var position = new Vector3(randomX, 1.5f, randomZ);
        int maxAttempts = 25;
        bool success = false;

        RaycastHit hit = default;
        for(int i = 0; i < maxAttempts; i++)
        {
            success = Physics.Raycast(position, Vector3.down, out hit, 10f, 1 << LayerMask.NameToLayer("Ground"));

            if (!success)
            {
                Debug.Log($"Failed to find location at {position}");
            }
            else return hit.point;
        }
        
        Debug.Log("Failed to find any valid positions after 25 attempts");
        return hit.point;
    }
}
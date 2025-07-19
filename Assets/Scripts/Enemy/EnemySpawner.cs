using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Prefabs de enemigos (uno por visual/animación)
    public Transform[] spawnPoints;
    public Transform[] hidePoints;

    private GameObject[] spawnedEnemies;

    void Awake()
    {
        // Busca hijos llamados "spawnsGroup" y "hidesGroup" y obtiene sus hijos como puntos
        Transform spawnParent = transform.Find("spawnsGroup");
        if (spawnParent != null)
        {
            spawnPoints = new Transform[spawnParent.childCount];
            for (int i = 0; i < spawnParent.childCount; i++)
                spawnPoints[i] = spawnParent.GetChild(i);
        }

        Transform hideParent = transform.Find("hidesGroup");
        if (hideParent != null)
        {
            hidePoints = new Transform[hideParent.childCount];
            for (int i = 0; i < hideParent.childCount; i++)
                hidePoints[i] = hideParent.GetChild(i);
        }
    }

    void Start()
    {
        spawnedEnemies = new GameObject[spawnPoints.Length];
        SpawnAll();
    }

    public void SpawnAll()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnedEnemies[i] == null)
            {
                SpawnAt(i);
            }
        }
    }

    public void SpawnAt(int index)
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        // Elige un prefab aleatorio
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(prefab, spawnPoints[index].position, Quaternion.identity);

        // Asigna un comportamiento aleatorio y los hide points
        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript != null)
        {
            enemyScript.SetRandomBehavior();
            enemyScript.hidePoints = hidePoints;
            enemyScript.spawner = this; // <-- Nueva línea
        }

        spawnedEnemies[index] = enemy;
    }

    public void OnEnemyHidden(EnemyBase enemy)
    {
        // Busca el índice del enemigo y lo elimina del array
        for (int i = 0; i < spawnedEnemies.Length; i++)
        {
            if (spawnedEnemies[i] == enemy.gameObject)
            {
                spawnedEnemies[i] = null;
                break;
            }
        }
        // Spawnea uno nuevo (oleada)
        SpawnAll();
    }
}
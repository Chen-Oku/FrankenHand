using UnityEngine;

public class BookTowerBuilder : MonoBehaviour
{
    public GameObject bookPrefab;
    public int booksInTower = 10;
    public float bookHeight = 0.3f;

    void Start()
    {
        Vector3 startPosition = transform.position;
        for (int i = 0; i < booksInTower; i++)
        {
            Vector3 pos = startPosition + Vector3.up * (i * bookHeight);
            // Puedes agregar un poco de aleatoriedad para inestabilidad
            pos.x += Random.Range(-0.05f, 0.05f);
            pos.z += Random.Range(-0.05f, 0.05f);
            Instantiate(bookPrefab, pos, Quaternion.Euler(0, Random.Range(-5f, 5f), 0));
        }
    }
}

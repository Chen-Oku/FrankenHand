using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int totalPoints = 0;

    public event System.Action<int> OnPointsChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdatePoints(int points)
    {
        totalPoints = points;
        OnPointsChanged?.Invoke(totalPoints);
        // Actualizar UI, guardar progreso, etc.
    }

    public void AddPoints(int amount)
    {
        totalPoints += amount;
        UpdatePoints(totalPoints);
        // Aquí puedes actualizar la UI si tienes una referencia
    }

    public int GetPoints()
    {
        return totalPoints;
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int maxPoints = 999; // Límite máximo de puntos

    private int points;
    public int Points
    {
        get => points;
        set
        {
            int newValue = Mathf.Clamp(value, 0, maxPoints);
            if (points != newValue)
            {
                points = newValue;
                OnPointsChanged?.Invoke(points);
            }
        }
    }

    public event System.Action<int> OnPointsChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdatePoints(int points)
    {
        Points = points;
        // Actualizar UI, guardar progreso, etc.
    }

    public void AddPoints(int amount)
    {
        Points += amount;
        // Aquí puedes actualizar la UI si tienes una referencia
    }

    public int GetPoints()
    {
        return points;
    }
}

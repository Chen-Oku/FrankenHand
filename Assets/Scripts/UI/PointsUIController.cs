using UnityEngine;
using TMPro;

public class PointsUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;

    void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPointsChanged += UpdatePointsUI;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPointsChanged -= UpdatePointsUI;
    }

    void UpdatePointsUI(int points)
    {
        pointsText.text = points + " Puntos";
    }
}
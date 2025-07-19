using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int points = 0;

    public void AddPoints(int amount)
    {
        points += amount;
        GameManager.Instance.UpdatePoints(points);
    }

}

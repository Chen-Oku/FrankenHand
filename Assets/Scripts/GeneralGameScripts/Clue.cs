using UnityEngine;

public class Clue : MonoBehaviour
{
    public string clueText;
    public void Collect(PlayerController player)
    {
        //player.CollectClue(this);
        Destroy(gameObject);
    }
}

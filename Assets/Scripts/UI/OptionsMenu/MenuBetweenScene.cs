using UnityEngine;

public class MenuBetweenScene : MonoBehaviour
{
    private void Awake()
    {
        var noDestroyBetwweenScenes = FindObjectsByType<MenuBetweenScene>(FindObjectsSortMode.None);
        if (noDestroyBetwweenScenes.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
}

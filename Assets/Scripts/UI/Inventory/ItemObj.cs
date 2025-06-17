using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Itemobj : MonoBehaviour
{
    public int ID;
    public string type;
    public string description;
    public Sprite icon;
    public Image iconImage;

    [HideInInspector]
    public bool pickedUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

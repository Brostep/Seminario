using UnityEngine;

public class Spawner : MonoBehaviour
{
    [HideInInspector]
    public bool open;

    private void Start()
    {
        open = true;
    }
}

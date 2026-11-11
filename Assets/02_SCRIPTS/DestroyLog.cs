using UnityEngine;

public class DestroyLog : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 5f);
    }
}

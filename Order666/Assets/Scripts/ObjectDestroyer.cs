using UnityEngine;

public class ObjectDestroyer:MonoBehaviour
{
    [SerializeField] GameObject[] objectsToDestory;

    public void DestoryObjects()
    {
        foreach (GameObject obj in objectsToDestory)
        {
            Destroy(obj.gameObject);
        }

    }
}

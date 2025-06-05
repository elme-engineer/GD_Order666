using UnityEngine;

public class ActivateSecLevel : MonoBehaviour
{

    [SerializeField] GameObject[] objectsToActivate;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateObjects()
    {

        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
    }
}

using UnityEngine;
using Unity.Cinemachine;


public class AimManager : MonoBehaviour
{
    public InputAxis xAxis, yAxis;
    [SerializeField] Transform camFollowPos;

   
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        xAxis.UpdateRecentering(Time.deltaTime, xAxis.TrackValueChange());

        yAxis.UpdateRecentering(Time.deltaTime, yAxis.TrackValueChange());
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}

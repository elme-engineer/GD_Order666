using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerVFXController : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public Volume volume;

    private DepthOfField dof;
    private LensDistortion lens;

    void Start()
    {
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out lens);
    }

    void Update()
    {
        float healthPercent = playerStatus.DreamMeter / playerStatus.MaxDreamMeter;

        if (dof != null)
        {
            if (healthPercent <= 0.1f)
            {
                dof.active = true;
                dof.mode.value = DepthOfFieldMode.Gaussian;
                dof.gaussianStart.value = 10f;
                dof.gaussianEnd.value = 60f;
                dof.gaussianMaxRadius.value = 1f;
            }
            else
            {
                dof.active = false;
            }
        }

        if (lens != null)
        {
            if (healthPercent <= 0.50f)
            {
                lens.active = true;
                lens.intensity.value = Mathf.Sin(Time.time * 3f) * 0.50f;
                lens.scale.value = 1f;
            }
            else
            {
                lens.active = false;
            }
        }
    }
}

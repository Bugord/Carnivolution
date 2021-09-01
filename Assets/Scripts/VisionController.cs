using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VisionController : MonoBehaviour
{
    [SerializeField] private Light2D light;

    public void UpdateVision(float radius)
    {
        if (light != null)
        {
            light.pointLightOuterRadius = radius;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aerodynamic Surface Config", menuName = "Aerodynamic Surface Config")]
public class AeroSurfaceConfig : ScriptableObject
{
    public float liftSlope = 6.28f;
    public float surfaceFriction = 0.02f;
    public float zeroLiftAoA = 0;
    [Range(0, 100)]
    public float stallAngleHigh = 15;
    [Range(-100, 0)]
    public float stallAngleLow = -15;
    public float chord = 1; //bulge of surface
    [Range(0.0f, 0.4f)] 
    public float flapFraction = 0;
    
    public float wingspan = 1;
    public bool autoAspectRatio = true;
    public float aspectRatio = 2;

    private void OnValidate() 
    {
        chord = Mathf.Max(chord, 0.001f);

        if (autoAspectRatio)
            aspectRatio = wingspan / chord;
    }
}

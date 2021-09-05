using UnityEngine;

/// <summary>
/// Applies linear and angular forces to a ship.
/// </summary>
public class PlaneControlTest : MonoBehaviour
{

    //[Header("Thrust")]
    //[Range(0.0f, 1.0f)]
    //[Tooltip("Multiplier for longitudinal thrust when reverse thrust is requested.")]
    //public float reverseMultiplier = 1.0f;

    public float pitchPower, rollPower, yawPower, enginePower;

    private float activeRoll, activePitch, activeYaw;

    public bool throttle => Input.GetKey(KeyCode.LeftShift);

    private void Update()
    {
        if (throttle)
        {
            transform.position += transform.forward * enginePower * Time.deltaTime;  
        }

        activePitch = Input.GetAxisRaw("Vertical") * pitchPower * Time.deltaTime;
        activeRoll = Input.GetAxisRaw("Horizontal") * rollPower * Time.deltaTime * -1;

        transform.Rotate(activePitch * pitchPower * Time.deltaTime,
            activeYaw * yawPower * Time.deltaTime,
            activeRoll * rollPower * Time.deltaTime, Space.Self);
    }

}
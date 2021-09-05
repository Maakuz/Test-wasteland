using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AircraftPhysics : MonoBehaviour
{
    public const float PREDICTION_TIMESTEP_FRACTION = 0.5f;

    [SerializeField] float m_thrust = 0;
    [SerializeField] List<AeroSurface> m_surfaces;

    private Rigidbody m_rb;
    private BiVec3 m_currentForceAndTorque;

    private float m_thrustPercent;
    public float ThrustPercent { get => m_thrustPercent; set => m_thrustPercent = value; }

#if UNITY_EDITOR
    // For gizmos drawing.
    public void CalculateCenterOfLift(out Vector3 center, out Vector3 force, Vector3 displayAirVelocity, float displayAirDensity)
    {
        Vector3 com;
        BiVec3 forceAndTorque;
        if (m_surfaces == null)
        {
            center = Vector3.zero;
            force = Vector3.zero;
            return;
        }

        if (m_rb == null)
        {
            com = GetComponent<Rigidbody>().worldCenterOfMass;
            forceAndTorque = CalculateForces(-displayAirVelocity, Vector3.zero, Vector3.zero, displayAirDensity, com);
        }
        else
        {
            com = m_rb.worldCenterOfMass;
            forceAndTorque = m_currentForceAndTorque;
        }

        force = forceAndTorque.force;
        center = com + Vector3.Cross(forceAndTorque.force, forceAndTorque.torque) / forceAndTorque.force.sqrMagnitude;
    }
#endif

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //TODO: WIND?
        BiVec3 forceAndTorqueNow = CalculateForces(m_rb.velocity, m_rb.angularVelocity, Vector3.zero, 1.2f, m_rb.worldCenterOfMass);

        Vector3 velPrediction = PredictVel(forceAndTorqueNow.force + transform.forward * m_thrust * m_thrustPercent + Physics.gravity * m_rb.mass);
        Vector3 angVelPrediction = PredictAngVel(forceAndTorqueNow.torque);

        BiVec3 forceAndTorquePrediction = CalculateForces(velPrediction, angVelPrediction, Vector3.zero, 1.2f, m_rb.worldCenterOfMass);

        m_currentForceAndTorque = (forceAndTorqueNow + forceAndTorquePrediction) * 0.5f;
        m_rb.AddForce(m_currentForceAndTorque.force);
        m_rb.AddTorque(m_currentForceAndTorque.torque);

        m_rb.AddForce(transform.forward * m_thrust * m_thrustPercent);
    }

    private BiVec3 CalculateForces(Vector3 vel, Vector3 angVel, Vector3 wind, float airDensity, Vector3 centerOfMass)
    {
        BiVec3 forceAndTorque = new BiVec3();

        foreach (AeroSurface surface in m_surfaces)
        {
            Vector3 relativePos = surface.transform.position - centerOfMass;
            forceAndTorque += surface.CalculateForces(-vel + wind - Vector3.Cross(angVel, relativePos), airDensity, relativePos);
        }

        return forceAndTorque;
    }

    private Vector3 PredictVel(Vector3 force)
    {
        return m_rb.velocity + Time.fixedDeltaTime * PREDICTION_TIMESTEP_FRACTION * force / m_rb.mass;
    }

    private Vector3 PredictAngVel(Vector3 torque)
    {
        Quaternion inertiaTensorWorldRotation = m_rb.rotation * m_rb.inertiaTensorRotation;
        Vector3 torqueInDiagonalSpace = Quaternion.Inverse(inertiaTensorWorldRotation) * torque;
        Vector3 angularVelocityChangeInDiagonalSpace;
        angularVelocityChangeInDiagonalSpace.x = torqueInDiagonalSpace.x / m_rb.inertiaTensor.x;
        angularVelocityChangeInDiagonalSpace.y = torqueInDiagonalSpace.y / m_rb.inertiaTensor.y;
        angularVelocityChangeInDiagonalSpace.z = torqueInDiagonalSpace.z / m_rb.inertiaTensor.z;

        return m_rb.angularVelocity + Time.fixedDeltaTime * PREDICTION_TIMESTEP_FRACTION * (inertiaTensorWorldRotation * angularVelocityChangeInDiagonalSpace);
    }
}

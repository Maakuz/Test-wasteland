using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlInputType { Pitch, Yaw, Roll, Flap }

public class AeroSurface : MonoBehaviour
{
#if UNITY_EDITOR
    // For gizmos drawing.
    public AeroSurfaceConfig Config => m_config;
    public float GetFlapAngle() => m_flapAngle;
    public Vector3 CurrentLift { get; private set; }
    public Vector3 CurrentDrag { get; private set; }
    public Vector3 CurrentTorque { get; private set; }
    public bool IsAtStall { get; private set; }
#endif


    [SerializeField] private AeroSurfaceConfig m_config;
    public bool m_controlSurface;
    public ControlInputType m_inputType;
    [Range(-1f, 1f)]
    [SerializeField] private float m_inputMultiplier;

    private float m_flapAngle;

    public float FlapAngle { get => m_flapAngle; set => m_flapAngle = m_inputMultiplier * Mathf.Clamp(value, -Mathf.Deg2Rad * 50, Mathf.Deg2Rad * 50); }

    public BiVec3 CalculateForces(Vector3 worldAirVelocity, float airDensity, Vector3 relativePos)
    {
        BiVec3 forceAndTorque = new BiVec3();
        if (m_config == null) 
            return forceAndTorque;

        float correctedLiftSlope = m_config.liftSlope * m_config.aspectRatio / (m_config.aspectRatio + 2 * (m_config.aspectRatio + 4) / (m_config.aspectRatio + 2));

        //Flap angle calculation
        float angle = Mathf.Acos(2 * m_config.flapFraction - 1);
        float flapPower = 1 - (angle - Mathf.Sin(angle)) / Mathf.PI;
        float deltaLift = correctedLiftSlope * flapPower * FlapPowerCorrection(m_flapAngle) * m_flapAngle;

        //Stalling angle calc
        float zeroLiftAngleBase = m_config.zeroLiftAoA * Mathf.Deg2Rad;
        float stallAngleHighBase = m_config.stallAngleHigh * Mathf.Deg2Rad;
        float stallAngleLowBase = m_config.stallAngleLow * Mathf.Deg2Rad;

        float zeroLiftAngle = zeroLiftAngleBase - deltaLift / correctedLiftSlope;

        float maxHigh = correctedLiftSlope * (stallAngleHighBase - zeroLiftAngleBase) + deltaLift * liftCoefficientMaxFraction(m_config.flapFraction);
        float maxLow = correctedLiftSlope * (stallAngleLowBase - zeroLiftAngleBase) + deltaLift * liftCoefficientMaxFraction(m_config.flapFraction);

        float stallAngleHigh = zeroLiftAngle + maxHigh / correctedLiftSlope;
        float stallAngleLow = zeroLiftAngle + maxLow / correctedLiftSlope;

        //Air influence
        Vector3 airVel = transform.InverseTransformDirection(worldAirVelocity);
        airVel.z = 0;
        Vector3 dragDir = transform.TransformDirection(airVel.normalized);
        Vector3 liftDir = Vector3.Cross(dragDir, transform.forward);

        float area = m_config.chord * m_config.wingspan;
        float pressure = 0.5f * airDensity * airVel.sqrMagnitude;
        float airAngle = Mathf.Atan2(airVel.y, -airVel.x);

        Vector3 aerodynamicCoeff = CalculateCoeff(airAngle, correctedLiftSlope, zeroLiftAngle, stallAngleHigh, stallAngleLow);

        Vector3 lift = liftDir * aerodynamicCoeff.x * pressure * area;
        Vector3 drag = dragDir * aerodynamicCoeff.y * pressure * area;
        Vector3 torque = -transform.forward * aerodynamicCoeff.z * pressure * area * m_config.chord;

        forceAndTorque.force += lift + drag;
        forceAndTorque.torque += Vector3.Cross(relativePos, forceAndTorque.force);
        forceAndTorque.torque += torque;

#if UNITY_EDITOR
        // For gizmos drawing.
        IsAtStall = !(airAngle < stallAngleHigh && airAngle > stallAngleLow);
        CurrentLift = lift;
        CurrentDrag = drag;
        CurrentTorque = torque;
#endif

        return forceAndTorque;
    }

    private Vector3 CalculateCoeff(float angle, float correctedLiftSlope, float zeroLiftAngle, float stallAngleHigh, float stallAngleLow)
    {
        Vector3 coeff;

        float paddingAngleHigh = Mathf.Deg2Rad * Mathf.Lerp(15, 5, (Mathf.Rad2Deg * FlapAngle + 50) / 100);
        float paddingAngleLow = Mathf.Deg2Rad * Mathf.Lerp(15, 5, (-Mathf.Rad2Deg * FlapAngle + 50) / 100);
        float paddedStallAngleHigh = stallAngleHigh + paddingAngleHigh;
        float paddedStallAngleLow = stallAngleLow - paddingAngleLow;

        if (angle < stallAngleHigh && angle > stallAngleLow)
            coeff = CalculateCoeffLowAngle(angle, correctedLiftSlope, zeroLiftAngle);

        else 
        {
            if (angle > paddedStallAngleHigh || angle < paddedStallAngleLow)
            {
                //STALLINGHH
                coeff = CalculateCoeffStalling(angle, correctedLiftSlope, zeroLiftAngle, stallAngleHigh, stallAngleLow);
            }

            else
            {
                Vector3 coeffLow;
                Vector3 coeffStall;
                float lerp;

                if (angle > stallAngleHigh)
                {
                    coeffLow = CalculateCoeffLowAngle(stallAngleHigh, correctedLiftSlope, zeroLiftAngle);
                    coeffStall = CalculateCoeffStalling(paddedStallAngleHigh, correctedLiftSlope, zeroLiftAngle, stallAngleHigh, stallAngleLow);
                    lerp = (angle - stallAngleHigh) / (paddedStallAngleHigh - stallAngleHigh);
                }

                else 
                {
                    coeffLow = CalculateCoeffLowAngle(stallAngleLow, correctedLiftSlope, zeroLiftAngle);
                    coeffStall = CalculateCoeffStalling(paddedStallAngleLow, correctedLiftSlope, zeroLiftAngle, stallAngleHigh, stallAngleLow);
                    lerp = (angle - stallAngleLow) / (paddedStallAngleLow - stallAngleLow);
                }

                coeff = Vector3.Lerp(coeffLow, coeffStall, lerp);
            }
        }

        return coeff;
    }

    private Vector3 CalculateCoeffLowAngle(float angle, float correctedLiftSlope, float zeroLiftAngle)
    {
        float liftCoeff = correctedLiftSlope * (angle - zeroLiftAngle);
        float inducedAngle = liftCoeff / (Mathf.PI * m_config.aspectRatio);
        float effectiveAngle = angle - zeroLiftAngle - inducedAngle;

        float tanCoeff = m_config.surfaceFriction * Mathf.Cos(effectiveAngle);
        float normalCoeff = (liftCoeff + Mathf.Sin(effectiveAngle) * tanCoeff) / Mathf.Cos(effectiveAngle);
        float dragCoeff = normalCoeff * Mathf.Sin(effectiveAngle) + tanCoeff * Mathf.Cos(effectiveAngle);
        float torqueCoeff = -normalCoeff * TorqCoeffProportion(effectiveAngle);

        return new Vector3(liftCoeff, dragCoeff, torqueCoeff);
    }

    private Vector3 CalculateCoeffStalling(float angle, float correctedLiftSlope, float zeroLiftAngle, float stallAngleHigh, float stallAngleLow)
    {
        float liftCoeffLowAngle;
        float lerp;

        if (angle > stallAngleHigh)
        {
            liftCoeffLowAngle = correctedLiftSlope * (stallAngleHigh - zeroLiftAngle);
            lerp = (Mathf.PI / 2 - Mathf.Clamp(angle, -Mathf.PI / 2, Mathf.PI / 2)) / (Mathf.PI / 2 - stallAngleHigh);
        }

        else
        {
            liftCoeffLowAngle = correctedLiftSlope * (stallAngleLow - zeroLiftAngle);
            lerp = (-Mathf.PI / 2 - Mathf.Clamp(angle, -Mathf.PI / 2, Mathf.PI / 2)) / (-Mathf.PI / 2 - stallAngleLow);
        }

        float inducedAngle = liftCoeffLowAngle / (Mathf.PI * m_config.aspectRatio);
        inducedAngle = Mathf.Lerp(0, inducedAngle, lerp);
        float effectiveAngle = angle - zeroLiftAngle - inducedAngle;

        float normalCoeff = FrictionAt90Deg(FlapAngle) * Mathf.Sin(effectiveAngle) * (1 / (0.56f + 0.44f * Mathf.Abs(Mathf.Sin(effectiveAngle))) - 0.41f * (1 - Mathf.Exp(-17 / m_config.aspectRatio)));
        float tanCoeff = 0.5f * m_config.surfaceFriction * Mathf.Cos(effectiveAngle);

        float liftCoeff = normalCoeff * Mathf.Cos(effectiveAngle) - tanCoeff * Mathf.Sin(effectiveAngle);
        float dragCoeff = normalCoeff * Mathf.Sin(effectiveAngle) + tanCoeff * Mathf.Cos(effectiveAngle);
        float torqueCoeff = -normalCoeff * TorqCoeffProportion(effectiveAngle);

        return new Vector3(liftCoeff, dragCoeff, torqueCoeff);
    }

    private float FlapPowerCorrection(float angle)
    {
        return Mathf.Lerp(0.8f, 0.4f, (Mathf.Abs(angle) * Mathf.Rad2Deg - 10) / 50);
    }

    private float liftCoefficientMaxFraction(float fraction)
    {
        return Mathf.Clamp01(1 - 0.5f * (fraction - 0.1f) / 0.3f);
    }

    private float TorqCoeffProportion(float angle)
    {
        return 0.25f - 0.175f * (1 - 2 * Mathf.Abs(angle) / Mathf.PI);
    }

    private float FrictionAt90Deg(float fric)
    {
        return 1.98f - 0.0426f * FlapAngle * FlapAngle + 0.21f * FlapAngle;
    }
}

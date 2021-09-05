using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator m_animator;
    private PlayerScript m_player;
    [SerializeField] private float m_rotateAnimationSpeed;

    private float m_rot;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GetComponent<PlayerScript>();
        m_animator = GetComponent<Animator>();
        m_rotateAnimationSpeed = 4;
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercentage = m_player.ForwardVel / m_player.MaxSpeed;
        float strafePercentage = m_player.StrafeVel / m_player.MaxStrafeSpeed;

        if (Mathf.Abs(m_rot) > Mathf.Abs(strafePercentage))
            strafePercentage = m_rot;

        m_animator.SetFloat("SpeedPercent", speedPercentage, Constants.LOCOMOTION_ANIMATION_SMOOTH_TIME, Time.deltaTime);
        m_animator.SetFloat("StrafePercent", strafePercentage, Constants.LOCOMOTION_ANIMATION_SMOOTH_TIME, Time.deltaTime);


    }

    public void setRotationModifier(float rotation)
    {
        m_rot = rotation / m_rotateAnimationSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public const float LOCOMOTION_ANIMATION_SMOOTH_TIME = 0.1f;


    private Animator m_animator;
    private PlayerScript m_player;
    [SerializeField]
    private Transform m_lookAt;
    [SerializeField]
    private Transform m_lookFrom;
    [SerializeField]
    private Joint m_head;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GetComponent<PlayerScript>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercentage = m_player.XVel / 40f; //this is the theoretical top speed hardcoded.

        m_animator.SetFloat("SpeedPercent", speedPercentage, LOCOMOTION_ANIMATION_SMOOTH_TIME, Time.deltaTime);

        
    }
}

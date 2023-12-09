using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
	public Slider sprint;
	public Image sprintFill;
	public float speedVar = 1f;
	public float sprintAmount = 100f;
	private bool tired = false;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
        
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
		
		
		if(Input.GetKey(KeyCode.LeftShift) && sprintAmount >= 0.1f && tired == false)
		{
			speedVar = 1.8f;
			sprintAmount -= .7f;
			sprint.value = sprintAmount;
		}
		else if(sprintAmount < 100f)
		{
			speedVar = 1f;
			sprintAmount += .3f;
			sprint.value = sprintAmount;
		}
		
		if(sprintAmount <= 0.1f)
		{
			tired = true;
		}
		else if(sprintAmount >= 20 && tired == true)
		{
			tired = false;
		}
		
		if(sprint.value <= 50)
			sprintFill.color = Color.yellow;
		if(sprint.value <= 20)
			sprintFill.color = Color.red;
		if(sprint.value >= 50)
			sprintFill.color = Color.green;

    }

    void OnAnimatorMove ()
    {
			m_Rigidbody.MovePosition (m_Rigidbody.position + (m_Movement * speedVar) * m_Animator.deltaPosition.magnitude);
			m_Rigidbody.MoveRotation (m_Rotation);
    }
}
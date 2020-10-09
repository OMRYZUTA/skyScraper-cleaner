using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class Flying : MonoBehaviour
{
    [SerializeField] float m_IdleSpeed, m_TurnSpeed, m_SwitchSeconds, m_IdleRatio;
    [SerializeField] Vector2 m_AnimSpeedMinMax, m_MoveSpeedMinMax, m_ChangeAnimEveryFromTo, m_ChangeTargetEveryFromTo;
    [SerializeField] Transform m_HomeTarget, m_FlyingTarget;
    [SerializeField] Vector2 m_RadiusMinMax, m_YMinMax;
    [SerializeField] public bool m_ReturnToBase = false;
    [SerializeField] public float m_RandomBaseOffset = 5, m_DelayStart = 0f;

    private Animator m_Animator;
    private Rigidbody m_Body;
    [System.NonSerialized] public float m_ChangeTarget = 0f, m_ChangeAnim = 0f, m_TimeSinceTarget = 0f, 
        m_TimeSinceAnim = 0f, m_PrevAnim, m_CurrAnim = 0f, m_PrevSpeed, m_Speed, m_Zturn, m_Prevz, m_TurnSpeedBackup;
    private Vector3 m_rotateTarget, m_Position, m_Direction, m_RandomBase;
    private Quaternion m_LookRotation;
    [System.NonSerialized] public float m_DistanceFromBase, m_DistanceFromTarget;


    // Start is called before the first frame update
    void Start()
    {
        // initialize
        m_Animator = GetComponent<Animator>();
        m_Body = GetComponent<Rigidbody>();
        m_TurnSpeedBackup = m_TurnSpeed;
        m_Direction = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward);

        if (m_DelayStart < 0f)
        {
            m_Body.velocity = m_IdleSpeed * m_Direction;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_DelayStart > 0f)
        {
            m_DelayStart = Time.fixedDeltaTime;
            return;
        }

        m_DistanceFromBase = Vector3.Magnitude(m_RandomBase - m_Body.position);
        m_DistanceFromTarget = Vector3.Magnitude(m_FlyingTarget.position - m_Body.position);

        if (m_ReturnToBase && m_DistanceFromBase < 10f)
        {
            if (m_TurnSpeed != 300f && m_Body.velocity.magnitude != 0f)
            {
                m_TurnSpeedBackup = m_TurnSpeed;
                m_TurnSpeed = 300f;
            }
            else if (m_DistanceFromBase <= 1f)
            {
                m_Body.velocity = Vector3.zero;
                m_TurnSpeed = m_TurnSpeedBackup;
                return;
            }
        }

        if (m_ChangeAnim < 0f)
        {
            m_PrevAnim = m_CurrAnim;
            m_CurrAnim = ChangeAnim(m_CurrAnim);
            m_ChangeAnim = UnityEngine.Random.Range(m_ChangeAnimEveryFromTo.x, m_ChangeAnimEveryFromTo.y);
            m_TimeSinceAnim = m_RandomBaseOffset;
            m_PrevSpeed = m_Speed;

            if (m_CurrAnim == 0)
            {
                m_Speed = m_IdleSpeed;
            }
            else
            {
                m_Speed = Mathf.Lerp(m_MoveSpeedMinMax.x, m_MoveSpeedMinMax.y, (m_CurrAnim - m_AnimSpeedMinMax.x) / (m_AnimSpeedMinMax.y - m_AnimSpeedMinMax.x));
            }
        }

        if (m_ChangeTarget < 0f)
        {
            m_rotateTarget = changeDirection(m_Body.transform.position);

            if (m_ReturnToBase)
            {
                m_ChangeTarget = 0.2f;
            }
            else 
            {
                m_ChangeTarget = UnityEngine.Random.Range(m_ChangeTargetEveryFromTo.x, m_ChangeTargetEveryFromTo.y);
            }

            m_TimeSinceTarget = 0f;
        }

        if (m_Body.transform.position.y < m_YMinMax.x + 10f || m_Body.transform.position.y > m_YMinMax.y - 10f)
        {
            if (m_Body.transform.position.y < m_YMinMax.x + 10f)
            {
                m_rotateTarget.y = 1f;
            }
            else
            {
                m_rotateTarget.y = -1f;
            }
        }

        m_Zturn = Mathf.Clamp(Vector3.SignedAngle(m_rotateTarget, m_Direction, Vector3.up), -45f, 45f);
        m_ChangeAnim -= Time.fixedDeltaTime;
        m_ChangeTarget -= Time.fixedDeltaTime;
        m_TimeSinceTarget += Time.fixedDeltaTime;
        m_TimeSinceAnim += Time.fixedDeltaTime;

        if (m_rotateTarget != Vector3.zero)
        {
            m_LookRotation = Quaternion.LookRotation(m_rotateTarget, Vector3.up);
        }

        Vector3 rotation = Quaternion.RotateTowards(m_Body.transform.rotation, m_LookRotation, m_TurnSpeed * Time.fixedDeltaTime).eulerAngles;
        m_Body.transform.eulerAngles = rotation;
        float temp = m_Prevz;

        if (m_Prevz < m_Zturn)
        {
            m_Prevz += Mathf.Min(m_TurnSpeed * Time.fixedDeltaTime, m_Zturn - m_Prevz);
        }
        else if (m_Prevz >= m_Zturn)
        {
            m_Prevz -= Mathf.Min(m_TurnSpeed * Time.fixedDeltaTime, m_Zturn - m_Prevz);
        }

        m_Prevz = Mathf.Clamp(m_Prevz, -45f, 45f);
        m_Body.transform.Rotate(0f, 0f, m_Prevz - temp, Space.Self);

        m_Direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        if (m_ReturnToBase && m_DistanceFromBase < m_IdleSpeed)
        {
            m_Body.velocity = Mathf.Min(m_IdleSpeed, m_DistanceFromBase) * m_Direction;
        }

        m_Body.velocity = Mathf.Lerp(m_PrevSpeed, m_Speed, Mathf.Clamp(m_TimeSinceAnim / m_SwitchSeconds, 0f, 1f)) * m_Direction;

        if (m_Body.transform.position.y < m_YMinMax.x || m_Body.transform.position.y > m_YMinMax.y)
        {
            m_Position = m_Body.transform.position;
            m_Position.y = Mathf.Clamp(m_Position.y, m_YMinMax.x, m_YMinMax.y);
            m_Body.transform.position = m_Position;
        }
    }

    private float ChangeAnim(float i_CurrAnim)
    {
        float newState;

        if (UnityEngine.Random.Range(0f, 1f) < m_IdleRatio)
        {
            newState = 0f;
        }
        else
        {
            newState = UnityEngine.Random.Range(m_AnimSpeedMinMax.x, m_AnimSpeedMinMax.y);
        }

        if (newState != i_CurrAnim)
        {
            if (newState == 0)
            {
                m_Animator.speed = 1f;
            }
            else
            {
                m_Animator.speed = newState;
            }
        }

        return newState;
    }

    private Vector3 changeDirection(Vector3 i_CurrPosition)
    {
        Vector3 newDir;

        if (m_ReturnToBase)
        {
            m_RandomBase = m_HomeTarget.position;
            m_RandomBase.y += UnityEngine.Random.Range(-m_RandomBaseOffset, m_RandomBaseOffset); 
            newDir = m_RandomBase - i_CurrPosition;
        }
        else if (m_DistanceFromTarget > m_RadiusMinMax.y)
        {
            newDir = m_FlyingTarget.position - i_CurrPosition;
        }
        else if (m_DistanceFromTarget < m_RadiusMinMax.x)
        {
            newDir = i_CurrPosition - m_FlyingTarget.position;
        }
        else
        {
            float angleXZ = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
            float angleY = UnityEngine.Random.Range(-Mathf.PI / 48f, Mathf.PI / 48f);
            newDir = Mathf.Sin(angleXZ) * Vector3.forward + Mathf.Cos(angleXZ) * Vector3.right + Mathf.Sin(angleY) * Vector3.up;
        }

        return newDir.normalized;
    }
}


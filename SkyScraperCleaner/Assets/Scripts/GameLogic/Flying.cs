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
    [SerializeField] float idleSpeed, turnSpeed, switchSeconds, idleRatio;
    [SerializeField] Vector2 animSpeedMinMax, moveSpeedMinMax, changeAnimEveryFromTo, changeTargetEveryFromTo;
    [SerializeField] Transform homeTarget, flyingTarget;
    [SerializeField] Vector2 radiusMinMax, yMinMax;
    [SerializeField] public bool returnToBase = false;
    [SerializeField] public float randomBaseOffset = 5, delayStart = 0f;

    private Animator animator;
    private Rigidbody body;
    [System.NonSerialized] public float changeTarget = 0f, changeAnim = 0f, timeSinceTarget = 0f, timeSinceAnim = 0f, 
        prevAnim, currAnim = 0f, prevSpeed, speed, zturn, prevz, turnSpeedBackup;
    private Vector3 rotateTarget, position, direction, velocity, randomBase;
    private Quaternion lookRotation;
    [System.NonSerialized] public float distanceFromBase, distanceFromTarget;


    // Start is called before the first frame update
    void Start()
    {
        // initialize
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        turnSpeedBackup = turnSpeed;
        direction = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward);

        if(delayStart < 0f)
        {
            body.velocity = idleSpeed * direction;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(delayStart > 0f)
        {
            delayStart = Time.fixedDeltaTime;
            return;
        }

        distanceFromBase = Vector3.Magnitude(randomBase - body.position);
        distanceFromTarget = Vector3.Magnitude(flyingTarget.position - body.position);

        if(returnToBase && distanceFromBase < 10f)
        {
            if(turnSpeed != 300f && body.velocity.magnitude != 0f)
            {
                turnSpeedBackup = turnSpeed;
                turnSpeed = 300f;
            }
            else if(distanceFromBase <= 1f)
            {
                body.velocity = Vector3.zero;
                turnSpeed = turnSpeedBackup;
                return;
            }
        }

        if(changeAnim < 0f)
        {
            prevAnim = currAnim;
            currAnim = ChangeAnim(currAnim);
            changeAnim = UnityEngine.Random.Range(changeAnimEveryFromTo.x, changeAnimEveryFromTo.y);
            timeSinceAnim = randomBaseOffset;
            prevSpeed = speed;

            if (currAnim == 0)
            {
                speed = idleSpeed;
            }
            else
            {
                speed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, (currAnim - animSpeedMinMax.x) / (animSpeedMinMax.y - animSpeedMinMax.x));
            }
        }

        if (changeTarget < 0f)
        {
            rotateTarget = changeDirection(body.transform.position);
            if (returnToBase)
            {
                changeTarget = 0.2f;
            }
            else 
            {
                changeTarget = UnityEngine.Random.Range(changeTargetEveryFromTo.x, changeTargetEveryFromTo.y);
            }

            timeSinceTarget = 0f;
        }

        if(body.transform.position.y < yMinMax.x + 10f || body.transform.position.y > yMinMax.y - 10f)
        {
            if(body.transform.position.y < yMinMax.x + 10f)
            {
                rotateTarget.y = 1f;
            }
            else
            {
                rotateTarget.y = -1f;
            }
        }

        zturn = Mathf.Clamp(Vector3.SignedAngle(rotateTarget, direction, Vector3.up), -45f, 45f);

        changeAnim -= Time.fixedDeltaTime;
        changeTarget -= Time.fixedDeltaTime;
        timeSinceTarget += Time.fixedDeltaTime;
        timeSinceAnim += Time.fixedDeltaTime;

        if(rotateTarget != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(rotateTarget, Vector3.up);
        }

        Vector3 rotation = Quaternion.RotateTowards(body.transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime).eulerAngles;
        body.transform.eulerAngles = rotation;

        float temp = prevz;
        if(prevz < zturn)
        {
            prevz += Mathf.Min(turnSpeed * Time.fixedDeltaTime, zturn - prevz);
        }
        else if(prevz >= zturn)
        {
            prevz -= Mathf.Min(turnSpeed * Time.fixedDeltaTime, zturn - prevz);
        }

        prevz = Mathf.Clamp(prevz, -45f, 45f);
        body.transform.Rotate(0f, 0f, prevz - temp, Space.Self);

        direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;
        if(returnToBase && distanceFromBase < idleSpeed)
        {
            body.velocity = Mathf.Min(idleSpeed, distanceFromBase) * direction;
        }

        body.velocity = Mathf.Lerp(prevSpeed, speed, Mathf.Clamp(timeSinceAnim / switchSeconds, 0f, 1f)) * direction;

        if(body.transform.position.y < yMinMax.x || body.transform.position.y > yMinMax.y)
        {
            position = body.transform.position;
            position.y = Mathf.Clamp(position.y, yMinMax.x, yMinMax.y);
            body.transform.position = position;
        }
    }

    private float ChangeAnim(float currAnim)
    {
        float newState;

        if(UnityEngine.Random.Range(0f, 1f) < idleRatio)
        {
            newState = 0f;
        }
        else
        {
            newState = UnityEngine.Random.Range(animSpeedMinMax.x, animSpeedMinMax.y);
        }

        if(newState != currAnim)
        {
            animator.SetFloat("flySpeed", newState);
            if (newState == 0)
            {
                animator.speed = 1f;
            }
            else
            {
                animator.speed = newState;
            }
        }

        return newState;
    }

    private Vector3 changeDirection(Vector3 currPosition)
    {
        Vector3 newDir;

        if(returnToBase)
        {
            randomBase = homeTarget.position;
            randomBase.y += UnityEngine.Random.Range(-randomBaseOffset, randomBaseOffset); 
            newDir = randomBase -currPosition;
        }
        else if(distanceFromTarget > radiusMinMax.y)
        {
            newDir = flyingTarget.position - currPosition;
        }
        else if(distanceFromTarget < radiusMinMax.x)
        {
            newDir = currPosition - flyingTarget.position;
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


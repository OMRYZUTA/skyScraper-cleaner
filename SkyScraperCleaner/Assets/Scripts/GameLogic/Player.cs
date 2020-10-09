using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [SerializeField] private GameObject m_LookAtMeWindow;
    [SerializeField] private GameObject m_WaterPistol;
    [SerializeField] private GameObject m_Pistol;
    [SerializeField] private Text m_Timer;
    [SerializeField] private Text m_FpsText;

    private bool m_IsGameOver = false;
    private float m_StartTime, m_Time;
    private int m_FrameCounter = 0;
    private float m_FpsTimer = 0;
    public AudioSource m_RopePull;

    public event Action<GameObject> ReportBuildingHit;
    public event Action<GameObject> ReportWindowHit;
    public event Action<GameObject> ReportBirdHit;

    public bool IsGameOver
    {
        get
        {
            return m_IsGameOver;
        }

        set
        {
            m_IsGameOver = value;
        }
    }

    public float PlayTime
    {
        get
        {
            return m_Time;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        resetPosition();
        m_RopePull = GetComponent<AudioSource>();
        m_StartTime = Time.time;
        m_Pistol.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        input();

        if (!m_IsGameOver)
        {
            m_Time = Time.time - m_StartTime;
            string minutes = ((int)m_Time / 60).ToString();
            string seconds = (m_Time % 60).ToString("f2");

            m_Timer.text = minutes + ":" + seconds;
        }
        else
        {
            Debug.Log("Switching to welcome Scene!");
            SceneManager.LoadScene(0);
        }

        m_FpsTimer += Time.deltaTime;
        m_FrameCounter++;

        if (m_FrameCounter > 7)
        {
            m_FpsText.text = "Fps : " + (1 / (m_FpsTimer / m_FrameCounter));
            m_FpsTimer = 0;
            m_FrameCounter = 0;
        }
    }

    private void input()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(0.9f * Screen.width / 4f, 1.1f * Screen.height / 2f, 0));
        
        if (Physics.Raycast(ray, out hit))
        {

            if (hit.rigidbody != null)
            {
                var selection = hit.transform;
                Debug.Log(selection.tag);

                if (selection.CompareTag("GameBuilding"))
                {
                    moveUp(hit);
                }
                else if (selection.CompareTag("DirtyWindow"))
                {
                    if (m_WaterPistol.activeSelf == false)
                    {
                        m_WaterPistol.SetActive(true);
                        OnReportWindowHit(selection.GetComponent<GameObject>());
                        m_Pistol.SetActive(false);
                    }
                }
                else if (selection.CompareTag("Bird"))
                {
                    if (m_Pistol.activeSelf == false)
                    {
                        m_Pistol.SetActive(true);
                        OnReportBirdHit(selection.GetComponent<GameObject>());
                        m_WaterPistol.SetActive(false);
                    }
                }
            }
        }
    }

    private void moveUp(RaycastHit i_Hit)
    {
        if (Input.anyKeyDown && this.transform.position.y < 46)
        {
            m_Pistol.SetActive(false);
            m_WaterPistol.SetActive(false);
            Vector3 movementChange = i_Hit.transform.position - transform.position;
            movementChange.z = 0;
            movementChange.x = 0;
            movementChange *= 0.1f;
            transform.position -= movementChange;
            m_RopePull.Play();
        }
    }

    private void resetPosition()
    {
        Vector3 lookAt = new Vector3();
        lookAt = m_LookAtMeWindow.transform.position;
        lookAt.x -= 0.3f;
        transform.LookAt(lookAt);
    }

    protected virtual void OnReportWindowHit(GameObject i_Obj)
    {
        ReportWindowHit?.Invoke(i_Obj);
    }

    protected virtual void OnReportBuildingHit(GameObject i_Obj)
    {
        ReportBuildingHit?.Invoke(i_Obj);
    }

    protected virtual void OnReportBirdHit(GameObject i_Obj)
    {
        ReportBirdHit?.Invoke(i_Obj);
    }
}

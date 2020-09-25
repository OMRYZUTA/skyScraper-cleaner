using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject m_LookAtMeWindow;

    private Transform m_selection;
    [SerializeField]
    private GameObject m_WaterPistol;
    [SerializeField]
    private GameObject m_Pistol;
    private float m_Speed = 20;
    private float m_Time;

    public event Action<GameObject> ReportBuildingHit;
                        
    public event Action<GameObject> ReportWindowHit;
                        
    public event Action<GameObject> ReportBirdHit;
    // Start is called before the first frame update
    void Start()
    {
        resetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        input();
    }

    private void input()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out hit))
        {

            if (hit.rigidbody != null)
            {
                var selection = hit.transform;
                var selectionRenderer = selection.GetComponent<Renderer>();
                Debug.Log(selection.tag);
                if (selection.CompareTag("GameBuilding"))
                {
                    moveUp(hit);
                    m_Pistol.SetActive(false);
                    m_WaterPistol.SetActive(false);
                }
                else if (selection.CompareTag("DirtyWindow"))
                {
                    if(Input.anyKeyDown)
                    {
                        if(m_WaterPistol.activeSelf == false)
                        {
                            m_WaterPistol.SetActive(true);
                            OnReportWindowHit(selection.GetComponent<GameObject>());
                            m_Pistol.SetActive(false);
                        }
                    }
                }
                else if (selection.CompareTag("Bird"))
                {
                    if(Input.anyKeyDown)
                    {
                        if(m_Pistol.activeSelf == false)
                        {
                            m_Pistol.SetActive(true);
                            OnReportBirdHit(selection.GetComponent<GameObject>());
                            m_WaterPistol.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    private void moveUp(RaycastHit i_Hit)
    {
        if (Input.anyKeyDown && this.transform.position.y < 43)
        {
            Debug.Log("Moving up!!!");
            Debug.Log("Position: " + transform.position);
            Debug.Log("hit: " + i_Hit.transform.position);
            Vector3 movementChange= i_Hit.transform.position - transform.position;
            movementChange.z = 0;
            movementChange.x = 0;
            movementChange *= 0.05f;
            Debug.Log("to: " + movementChange);
            transform.position -= movementChange;
        }
    }

    private void resetPosition()
    {
        transform.position = new Vector3(-28, 5, -12);
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

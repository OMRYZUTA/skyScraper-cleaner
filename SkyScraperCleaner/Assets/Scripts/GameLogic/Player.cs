using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject m_LookAtMeWindow;

    private Transform m_selection;
    [SerializeField]
    private float m_Speed = 20;
    private float m_Time;
    // Start is called before the first frame update
    void Start()
    {
        resetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput();
    }

    private void moveInput()
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
                    if (Input.anyKeyDown && this.transform.position.y < 43)
                    {
                        Debug.Log("Moving up!!!");
                        Debug.Log("hit: " + hit.transform.position);
                        Vector3 movementChange= (hit.transform.position - transform.position);;
                        movementChange.z = 0;
                        movementChange.x = 0;
                        movementChange *= 0.05f;
                        Debug.Log("to: " + movementChange);
                      transform.position -= movementChange;
                    }
                }
                else if (selection.CompareTag("DirtyWindow"))
                {
                    if (Input.anyKeyDown)
                    {
                        Debug.Log("splash water!");
                    }
                }
            }
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
}

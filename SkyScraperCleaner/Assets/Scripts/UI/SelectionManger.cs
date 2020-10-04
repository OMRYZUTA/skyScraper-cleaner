using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManger : MonoBehaviour
{
    // Start is called before the first frame update
    private bool m_IsButtonHovered = false;
    private Button m_SelectedButton;
    void Start()
    {
        m_SelectedButton = null;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(/*0.275f**/Screen.width / 4f,1.15f*Screen.height / 2f, 0));

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.rigidbody != null)
            {
                m_SelectedButton = hit.transform.GetComponent<Button>();
                if (m_SelectedButton.CompareTag("Button") && !m_IsButtonHovered)
                {
                    m_SelectedButton.OnPointerEnter(new PointerEventData(EventSystem.current));
                    Debug.Log("entered a button");
                    m_IsButtonHovered = true;

                }
                else if (m_IsButtonHovered)
                {
                    if (Input.anyKeyDown)
                    {
                        Debug.Log("button pressed!");
                        m_SelectedButton.OnPointerClick(new PointerEventData(EventSystem.current));
                    }
                }
            }

            else if (m_SelectedButton != null)
            {
                m_IsButtonHovered = false;
                m_SelectedButton.OnPointerExit(new PointerEventData(EventSystem.current));
            }
        }
    }
}

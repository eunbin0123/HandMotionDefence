using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshGUIController : MonoBehaviour
{
    public OVRInput.Controller o_controller;

    // 제스쳐 인식 or 행동 수행 여부를 판단
    public bool m_GUI = false; // false: GUI 비활성화, true: GUI 활성화

    public GameObject m_DefaultLHands;
    public GameObject m_DefaultRHands;
    public GameObject[] m_ItemLHands;
    public GameObject[] m_ItemRHands;

    public Canvas m_Canvas;
    private int m_iGUIInex = 0;
    public Button[] m_Btn; // 0: Axe, 1: Bow, 2: Gun, 3: Sword, 4: Fever, 5: Stop

    IEnumerator GUICotrol()
    {
        while (true)
        {
            if (OVRInput.GetDown(OVRInput.Button.One, o_controller))
            {
                if (!m_GUI)
                {
                    // GUI를 활성화 시킴
                    m_Canvas.enabled = true;

                    m_GUI = true;
                }
                else
                {
                    ChangeBtnColor(m_Btn[m_iGUIInex], Color.white);
                    m_iGUIInex = 0;
                    ChangeBtnColor(m_Btn[m_iGUIInex], Color.red);
                    m_Canvas.enabled = false;
                    if (m_iGUIInex >= 0 && m_iGUIInex <= 3)
                    {
                        // 행동이 진행되는 과정에서 다시 'A'키가 눌렸다면 기본손으로
                        for (int i = 0; i < 4; i++)
                        {
                            // 행동 중인 손을 찾아 active를 false로
                            if (m_ItemLHands[i].activeSelf == true)
                            {
                                m_ItemLHands[i].SetActive(false);
                                m_ItemRHands[i].SetActive(false);
                            }
                        }
                        // 기본 손의 active를 true로
                        m_DefaultLHands.SetActive(true);
                        m_DefaultRHands.SetActive(true);
                    }
                    m_GUI = false;
                }

            }

            yield return null;
        }
    }
    private void Start()
    {
        m_iGUIInex = 0;
        ChangeBtnColor(m_Btn[m_iGUIInex], Color.red);
        m_Canvas.enabled = false;
        StartCoroutine("GUICotrol");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GUI)
        {
            SelectMenu();
        }
    }

    private void SelectMenu()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp, o_controller))
        {
            ChangeBtnColor(m_Btn[m_iGUIInex], Color.white);
            m_iGUIInex--;
            if (m_iGUIInex < 0) m_iGUIInex = 0;
            ChangeBtnColor(m_Btn[m_iGUIInex], Color.red);
        }
        else if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown, o_controller))
        {
            ChangeBtnColor(m_Btn[m_iGUIInex], Color.white);
            m_iGUIInex++;
            if (m_iGUIInex >= 5) m_iGUIInex = 5;
            ChangeBtnColor(m_Btn[m_iGUIInex], Color.red);
        }

        if (OVRInput.GetDown(OVRInput.Button.Two, o_controller))
        {
            if(m_iGUIInex >= 0 && m_iGUIInex <= 3)
            {
                // 기본 손은 active를 false로
                m_DefaultLHands.SetActive(false);
                m_DefaultRHands.SetActive(false);

                // 인식된 제스쳐에 맞는 손을 active true로 
                m_ItemLHands[m_iGUIInex].SetActive(true);
                m_ItemRHands[m_iGUIInex].SetActive(true);
            }
            else
            {
                if(m_iGUIInex == 4) // Fever Time
                {
                    GameObject.Find("TimeManager").GetComponent<cshTimeManager>().isFull_buff = true;

                    GameObject.Find("TimeManager").GetComponent<cshTimeManager>().DoFeverTime();
                }
                else if(m_iGUIInex == 5){ // Stop Motion
                    GameObject.Find("TimeManager").GetComponent<cshTimeManager>().isFull_stop = true;
                    GameObject.Find("TimeManager").GetComponent<cshTimeManager>().DoSlowmotion();
                }

                // GUI초기화
                ChangeBtnColor(m_Btn[m_iGUIInex], Color.white);
                m_iGUIInex = 0;
                ChangeBtnColor(m_Btn[m_iGUIInex], Color.red);
                m_Canvas.enabled = false;
                m_GUI = false;
            }
        }


    }

    private void ChangeBtnColor(Button btn, Color chgColor)
    {
        ColorBlock cb = btn.colors;
        Color pressColor = chgColor;
        cb.normalColor = pressColor;
        btn.colors = cb;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}
public class cshBow : MonoBehaviour
{
    [Header("Assets")]
    public GameObject m_ArrowPrefab = null;

    [Header("Bow")]
    public float m_GrabThreashold = 0.15f;
    public Transform m_Start = null;
    public Transform m_End = null;
    public Transform m_Socket = null;

    private Transform m_PullingHand = null;
    private cshArrow m_CurrentArrow = null;
    private Animator m_Animator =  null;

    private float m_PullValue = 0.0f;

    [SerializeField]Sound[] sound;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateArrow(0.0f));
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_PullingHand || !m_CurrentArrow)
            return;

        m_PullValue = CalculatePull(m_PullingHand);
        m_PullValue = Mathf.Clamp(m_PullValue, 0.0f, 1.0f);

        m_Animator.SetFloat("Blend", m_PullValue);

        m_CurrentArrow.Movement(m_PullValue);
    }

    private float CalculatePull(Transform pullHand)
    {
        Vector3 direction = m_End.position - m_Start.position;
        float magnitude = direction.magnitude;

        direction.Normalize();

        Vector3 difference = pullHand.position - m_Start.position;


        return Vector3.Dot(difference, direction) / magnitude;
    }

    private IEnumerator CreateArrow(float waitTime)
    {
        // Wait
        yield return new WaitForSeconds(waitTime);

        // Create, child
        if(m_Socket.childCount == 0)
        {
            GameObject arrowObject = Instantiate(m_ArrowPrefab, m_Socket);

            // Orient
            // notch가 있는 경우
            arrowObject.transform.localPosition = new Vector3(0, 0, 0);
            //arrowObject.transform.localPosition = new Vector3(0, 0, 0.408f);
            //arrowObject.transform.localEulerAngles = Vector3.zero;

            // Set
            m_CurrentArrow = arrowObject.GetComponent<cshArrow>();
        }
        
    }

    public void Pull(Transform hand)
    {
        float distance = Vector3.Distance(hand.position, m_Start.position);

        if (distance > m_GrabThreashold)
            return;

        m_PullingHand = hand;

        audioSource.clip = sound[0].clip;
        audioSource.Play();
    }

    public void Release()
    {
        if (m_PullValue > 0.25f)
        {
            audioSource.clip = sound[1].clip;
            audioSource.Play();
            FireArrow();
        }
        m_PullingHand = null;
        m_PullValue = 0.0f;
        m_Animator.SetFloat("Blend", 0);

        if (!m_CurrentArrow)
            StartCoroutine(CreateArrow(0.25f));
    }

    private void FireArrow()
    {
        m_CurrentArrow.Fire(m_PullValue);
        m_CurrentArrow = null;
    }
}

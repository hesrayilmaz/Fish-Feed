using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private GameObject m_Target;
    public float m_Speed;

    public bool m_ReverseRotation;

    private void Start()
    {
        m_Target = transform.parent.gameObject;
    }
    void Update()
    {
        if (m_ReverseRotation)
            transform.RotateAround(m_Target.transform.position, Vector3.up, m_Speed * Time.deltaTime);
        else
            transform.RotateAround(m_Target.transform.position, -Vector3.up, m_Speed * Time.deltaTime);
    }
}

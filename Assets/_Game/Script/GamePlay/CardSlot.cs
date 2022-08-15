using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }

    [SerializeField] private GameObject m_Border;
    private Transform m_LastTransformParent;

    public void SetParentTransform(Transform parent)
    {
        Transform.SetParent(parent);
    }
    public void SaveLastParentTransform() 
    {
        m_LastTransformParent = Transform.parent;
    }
    public void SetToLastParentTransform()
    {
        SetParentTransform(m_LastTransformParent);
    }
    public void SetBorder(bool value)
    {
        m_Border.SetActive(value);
    }

}

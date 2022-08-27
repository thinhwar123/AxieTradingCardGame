using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProjectiles : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform
        { 
        get { 
            if(m_Transform == null)
            {
                m_Transform = transform;
            }
            return m_Transform; 
            }
        }
    private SkeletonAnimation m_SkeletonAnimation;
    private void Awake()
    {
        m_SkeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void Setup(Transform target)
    {
        Transform.position = target.position;
        Transform.rotation = target.rotation;
        Transform.localScale = target.localScale;
        m_SkeletonAnimation.ClearState();
    }
}

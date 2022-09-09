using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
    public string m_ProjectileName;
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
    public void Setup(Vector3 startPosition, Vector3 endPosition, float time)
    {
        Transform.SetParent(UI_Game.Instance.CanvasParentTF);
        Transform.localScale = startPosition.x > endPosition.x ? Vector3.one : new Vector3(-1,1,1);
        Transform.position = startPosition;
        Transform.DOMove(endPosition, time).SetEase(Ease.InQuad).OnComplete(SelfDespawn);
    }
    public void SelfDespawn()
    {
        Thinh.SimplePool.Despawn(gameObject);
    }
}

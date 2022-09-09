using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class UIEffect : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }
    private RectTransform m_RectTransform;
    public RectTransform RectTransform { get { return m_RectTransform ??= GetComponent<RectTransform>(); } }
    public SkeletonGraphic m_SkeletonGraphic;
    public string m_EffectName;
    public float m_TimeDuration;
    public void OnSpawn(Transform parent, Vector3 position)
    {
        Transform.SetParent(parent);
        Transform.position = position;
        Transform.localScale = Vector3.one;
        m_SkeletonGraphic.AnimationState.SetAnimation(0, "play", false);
        StartCoroutine(SeflDespawn());
    }
    IEnumerator SeflDespawn()
    {
        yield return new WaitForSeconds(m_SkeletonGraphic.skeletonDataAsset.GetSkeletonData(true).FindAnimation("play").Duration);
        Thinh.SimplePool.Despawn(gameObject);
    }
}

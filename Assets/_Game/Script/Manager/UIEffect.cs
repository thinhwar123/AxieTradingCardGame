using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }
    private RectTransform m_RectTransform;
    public RectTransform RectTransform { get { return m_RectTransform ??= GetComponent<RectTransform>(); } }
    public string m_EffectName;
    public float m_TimeDuration;
    public void OnSpawn(Transform parent, Vector3 position)
    {
        Transform.SetParent(parent);
        Transform.localScale = Vector3.one;
        StartCoroutine(SeflDespawn());
    }
    IEnumerator SeflDespawn()
    {
        yield return new WaitForSeconds(m_TimeDuration);
        Thinh.SimplePool.Despawn(gameObject);
    }
}

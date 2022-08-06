using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinh;
public class Deck : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }

    [SerializeField] private BasicCard m_BasicCard;
    [SerializeField] private DropZone m_DropZone;
    void Start()
    {
        StartCoroutine(StartDraw());
    }
    IEnumerator StartDraw()
    {
        for (int i = 0; i < 6; i++)
        {
            DrawACard();
            yield return new WaitForEndOfFrame();
        }
    }

    public void DrawACard()
    {
        BasicCard card = SimplePool.Spawn(m_BasicCard, Transform.position, Quaternion.identity);
        card.Transform.parent = Transform;
        card.transform.localScale = Vector3.one;
        card.RandomCardConfig();
        card.Setup(false);
        card.InitCard();
        StartCoroutine(card.m_CardController.MoveCardToDropZone(m_DropZone.Transform, () => card.FlipCard(true)));
    }
}

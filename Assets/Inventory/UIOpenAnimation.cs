using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIOpenAnimation : MonoBehaviour
{
    // [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject viewPort;
    [SerializeField] private float fadeDuration;
    void Start()
    {
        InventorySystem.Instance.PopulateInventoryWithLoadedData();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1);
        sequence.AppendCallback(() =>
        {
            Vector2 vector2 = new Vector2(960, 540);
            transform.DOMove(vector2, fadeDuration).SetEase(Ease.OutBounce);
            transform.GetChild(0).GetComponent<Image>().DOFade(1, fadeDuration);
            transform.GetChild(1).GetComponent<Image>().DOFade(1, fadeDuration).OnComplete(() =>
            {
                viewPort.SetActive(true);
            });
        });

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UICloseAnimation : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    public void CloseInventory()
    {
        if(transform.childCount > 0){
            Transform UI = transform.GetChild(0);
            Transform window = UI.GetChild(1);
            Transform inventoryViewPort = window.GetChild(0).GetChild(0);
            Transform DescriptionViewPort = window.GetChild(1).GetChild(0);
            Vector2 vector2 = new Vector2(960,640);
            inventoryViewPort.gameObject.SetActive(false);
            DescriptionViewPort.gameObject.SetActive(false);
            UI.GetChild(0).gameObject.SetActive(false);
            UI.GetChild(2).gameObject.SetActive(false);
            UI.GetChild(3).gameObject.SetActive(false);
            window.DOMove(vector2, fadeDuration);
            window.GetChild(0).GetComponent<Image>().DOFade(0, fadeDuration);
            window.GetChild(1).GetComponent<Image>().DOFade(0, fadeDuration)
            .OnComplete(() => Destroy(UI.gameObject));
        }

    }
    
}

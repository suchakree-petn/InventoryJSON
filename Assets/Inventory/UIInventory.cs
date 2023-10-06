using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public abstract class UIInventory : MonoBehaviour
{
    [Header("Item List")]
    public List<GameObject> UISlot;
    public Item _currentSelectItem;



    [Header("Description Panel")]
    [SerializeField] private TextMeshProUGUI _descriptionName;
    [SerializeField] private Image _descriptionIcon;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;

    [Header("UI Transform")]
    [SerializeField] private Transform _inventoryContentTransform;
    [SerializeField] private Transform _descriptionContentTransform;


    [Header("Prefab")]
    [SerializeField] private GameObject slotPrefab;

    [Header("Animation")]
    [SerializeField] private float spawnScaleDuration;
    [SerializeField] private float spawnScaleBump;
    [SerializeField] private float bumpScaleDuration;
    [SerializeField] private float spawnDelayDuration;
    [SerializeField] private float fadeDuration;
    [SerializeField] private AnimationCurve spawnCurve;
    [SerializeField] private AnimationCurve bumpCurve;

    public delegate void SlotActions(Item item);
    public static SlotActions OnSlotClick;

    public delegate void CategoryActions(Item item);
    public static CategoryActions OnCategoryClick;

    [SerializeField] private List<SlotItem> _slotItem = new List<SlotItem>();

    [Header("For sent price to any class")]
    public static int GetpriceNow;


    private void Start()
    {
        _slotItem = InventorySystem.Instance.itemList;
        if (_slotItem.Count > 0)
        {
            _currentSelectItem = _slotItem[0].item;
            OnSlotClick?.Invoke(_currentSelectItem);
        }
        else
        {
            Debug.Log("No item in list");
        }


    }
    private GameObject CreateUISlot(SlotItem slotItem)
    {
        GameObject slot = Instantiate(slotPrefab, _inventoryContentTransform);
        slot.GetComponent<UISlotData>().item = slotItem.item;
        slot.transform.GetChild(0).GetComponent<Image>().sprite = slotItem.item._icon;
        slot.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = slotItem.stackCount.ToString();
        return slot;
    }

    public void RefreshInventoryData(Item item)
    {
        // Is list empty?
        if (_slotItem == null)
        {
            return;
        }

        // Clear inventory
        foreach (Transform child in _inventoryContentTransform)
        {
            Destroy(child.gameObject);
        }

        // Clear list
        UISlot.Clear();

        // Create new slot
        UISlot = GetItemListByType(item._itemType);
        GetpriceNow = item._price;
        // Debug.Log(item._price);//Price now

        // Enable description
        InitDescriptionUI();

    }

    public List<GameObject> GetItemListByType(ItemType itemType)
    {
        List<GameObject> _UISlot = new();
        SlotItem firstItemInSlot = null;
        if (_slotItem.Count == 0)
        {
            Debug.Log("slot item: 0");
        }
        Sequence mainSequence = DOTween.Sequence();

        for (int i = 0; i < _slotItem.Count; i++)
        {
            int index = i;
            if (_slotItem[index].item._itemType == itemType)
            {
                if (firstItemInSlot == null)
                {
                    firstItemInSlot = _slotItem[index];
                }
                GameObject slot = CreateUISlot(_slotItem[index]);
                _UISlot.Add(slot);
                mainSequence.AppendCallback(() =>
                {
                    Sequence subSequence = DOTween.Sequence();
                    slot.GetComponent<Image>().DOFade(1, fadeDuration);
                    slot.transform.GetChild(0).GetComponent<Image>().DOFade(1, fadeDuration);
                    slot.transform.GetChild(1).GetComponent<Image>().DOFade(1, fadeDuration);
                    subSequence.Append(slot.transform.DOScale(spawnScaleBump, spawnScaleDuration)).SetEase(spawnCurve);
                    subSequence.Append(slot.transform.DOScale(1, bumpScaleDuration)).SetEase(bumpCurve);
                });
                mainSequence.AppendInterval(spawnDelayDuration);

            }
        }

        // Init first select slot
        if (firstItemInSlot == null)
        {
            _currentSelectItem = null;
        }
        else
        {
            _currentSelectItem = firstItemInSlot.item;
        }


        Debug.Log(_UISlot.Count);
        return _UISlot;
    }

    private void InitDescriptionUI()
    {
        if (_slotItem == null || _currentSelectItem == null)
        {
            _inventoryContentTransform.gameObject.SetActive(false);
            _descriptionContentTransform.gameObject.SetActive(false);
        }
        else
        {
            _inventoryContentTransform.gameObject.SetActive(true);
            _descriptionContentTransform.gameObject.SetActive(true);
        }
    }

    private void RefreshUIInventory(Item currentSelectItem)
    {
        RefreshDescriptionName(currentSelectItem);
        RefreshDescriptionIcon(currentSelectItem);
        RefreshDescriptionText(currentSelectItem);
        RefreshPriceText(currentSelectItem);
    }
    private void RefreshDescriptionName(Item item)
    {
        if (item != null)
        {
            _descriptionName.text = item._name;
        }
    }
    private void RefreshDescriptionIcon(Item item)
    {
        if (item != null)
        {
            _descriptionIcon.sprite = item._icon;
        }
    }

    private void RefreshDescriptionText(Item item)
    {
        if (item != null)
        {
            _descriptionText.text = item._description;
        }
    }
    private void RefreshPriceText(Item item)
    {
        if (item != null)
        {
            _priceText.text = "Price: " + item._price.ToString();
        }
    }

    public abstract void ShowInventory(Transform canvasTransform);
    private void OnEnable()
    {
        OnSlotClick += RefreshInventoryData;
        OnSlotClick += RefreshUIInventory;

        OnCategoryClick += RefreshInventoryData;
        OnCategoryClick += RefreshUIInventory;

    }
    private void OnDisable()
    {
        OnSlotClick -= RefreshInventoryData;
        OnSlotClick -= RefreshUIInventory;

        OnCategoryClick -= RefreshInventoryData;
        OnCategoryClick -= RefreshUIInventory;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InventoryUIState { ItemSelection, PartySelection, MoveToForget, Busy }

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] TextMeshProUGUI categoryText;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemDescription;

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    [SerializeField] PartyScreen partyScreen;

    Action<SO_ItemBase> onItemUsed;

    int selectedItem = 0;
    int selectedCategory = 0;

    SO_MoveBase moveToLearn;

    InventoryUIState state;

    const int itemsInViewport = 8;

    List<ItemSlotUI> slotUIList;
    Inventory inventory;
    RectTransform itemListRect;
    private void Awake()
    {
        inventory = Inventory.GetInventory();
        itemListRect = itemList.GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateItemList();

        inventory.OnUpdated += UpdateItemList;
    }

    void UpdateItemList()
    {
        // Clear all the existing items
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);

        slotUIList = new List<ItemSlotUI>();
        foreach (var itemSlot in inventory.GetSlotsByCategory(selectedCategory))
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);

            slotUIList.Add(slotUIObj);
        }

        UpdateItemSelection();
    }

    public void HandleUpdate(Action onBack, Action<SO_ItemBase> onItemUsed = null)
    {
        this.onItemUsed = onItemUsed;

        if (state == InventoryUIState.ItemSelection)
        {
            int prevSelection = selectedItem;
            int prevCategory = selectedCategory;

            if (Input.GetKeyDown(KeyCode.DownArrow))
                ++selectedItem;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                --selectedItem;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                ++selectedCategory;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                --selectedCategory;

            if (selectedCategory > Inventory.ItemCategories.Count - 1)
                selectedCategory = 0;
            else if (selectedCategory < 0)
                selectedCategory = Inventory.ItemCategories.Count - 1;

            selectedItem = Mathf.Clamp(selectedItem, 0, inventory.GetSlotsByCategory(selectedCategory).Count - 1);

            if (prevCategory != selectedCategory)
            {
                ResetSelection();
                categoryText.text = Inventory.ItemCategories[selectedCategory];
                UpdateItemList();
            }
            else if (prevSelection != selectedItem)
            {
                UpdateItemSelection();
            }

            if (Input.GetKeyDown(KeyCode.Z))
                StartCoroutine(ItemSelected());
            else if (Input.GetKeyDown(KeyCode.X))
                onBack?.Invoke();
        }
        else if (state == InventoryUIState.PartySelection)
        {
            Action onSelected = () =>
            {
                StartCoroutine(UseItem());
            };

            Action onBackPartyScreen = () =>
            {
                ClosePartyScreen();
            };

            partyScreen.HandleUpdate(onSelected, onBackPartyScreen);
        }
      
    }

    IEnumerator ItemSelected()
    {
        state = InventoryUIState.Busy;

        var item = inventory.GetItem(selectedItem, selectedCategory);


        if (GameController.Instance.State == GameState.GeneralShop)
        {
            onItemUsed?.Invoke(item);
            state = InventoryUIState.ItemSelection;
            yield break;
        }

        if (GameController.Instance.State == GameState.Battle)
        {
            // In Battle
            if (!item.CanUseInBattle)
            {
                yield return DialogManager.Instance.ShowDialogText($"This item cannot be used in battle");
                state = InventoryUIState.ItemSelection;
                yield break;
            }
        }
        else
        {
            // Outside Battle
            if (!item.CanUseOutsideBattle)
            {
                yield return DialogManager.Instance.ShowDialogText($"This item cannot be used outside battle");
                state = InventoryUIState.ItemSelection;
                yield break;
            }
        }

        if (selectedCategory == (int)ItemCategory.Pokeballs)
        {
            StartCoroutine(UseItem());
        }
        else
        {
            OpenPartyScreen();
        }
    }

    IEnumerator UseItem()
    {
        state = InventoryUIState.Busy;

        var item = inventory.GetItem(selectedItem, selectedCategory);
        var pokemon = partyScreen.SelectedMember;

        var usedItem = inventory.UseItem(selectedItem, partyScreen.SelectedMember, selectedCategory);
        if (usedItem != null)
        {
            if (usedItem is RecoveryItem)
                yield return DialogManager.Instance.ShowDialogText($"The player used {usedItem.Name}");

            onItemUsed?.Invoke(usedItem);
        }
        else
        {
            if (selectedCategory == (int)ItemCategory.Items)
                yield return DialogManager.Instance.ShowDialogText($"It won't have any affect!");
        }

        ClosePartyScreen();
    }


    void UpdateItemSelection()
    {
        var slots = inventory.GetSlotsByCategory(selectedCategory);

        selectedItem = Mathf.Clamp(selectedItem, 0, slots.Count - 1);

        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
                slotUIList[i].NameText.color = GlobalSettings.i.HighlightedColor;
            else
                slotUIList[i].NameText.color = Color.black;
        }

        if (slots.Count > 0)
        {
            var item = slots[selectedItem].Item;
            itemIcon.sprite = item.Icon;
            itemDescription.text = item.Description;
        }

        HandleScrolling();
    }

    void HandleScrolling()
    {
        if (slotUIList.Count <= itemsInViewport) return;

        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewport / 2, 0, selectedItem) * slotUIList[0].Height;
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);

        bool showUpArrow = selectedItem > itemsInViewport / 2;
        upArrow.gameObject.SetActive(showUpArrow);

        bool showDownArrow = selectedItem + itemsInViewport / 2 < slotUIList.Count;
        downArrow.gameObject.SetActive(showDownArrow);
    }

    void ResetSelection()
    {
        selectedItem = 0;

        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);

        itemIcon.sprite = null;
        itemDescription.text = "";
    }

    void OpenPartyScreen()
    {
        state = InventoryUIState.PartySelection;
        partyScreen.gameObject.SetActive(true);
    }

    void ClosePartyScreen()
    {
        state = InventoryUIState.ItemSelection;
        partyScreen.gameObject.SetActive(false);
    }

   
}

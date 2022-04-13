using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum ItemCategory { Items, Pokeballs, Harvested, Weapons, Armor }

public class Inventory : MonoBehaviour, ISavable
{
    [SerializeField] List<ItemSlot> slots;
    [SerializeField] List<ItemSlot> pokeballSlots;
    [SerializeField] List<ItemSlot> harvestedSlot;
    [SerializeField] List<ItemSlot> weaponSlot;
    [SerializeField] List<ItemSlot> armorSlot;
    [SerializeField] List<ItemSlot> keyItemSlot;

    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { slots, pokeballSlots, harvestedSlot, weaponSlot, armorSlot, keyItemSlot };
    }

    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        "ITEMS", "POKEBALLS", "HARVESTED", "WEAPONS", "ARMOR", "KEY ITEMS"
    };

    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
    }

    public SO_ItemBase GetItem(int itemIndex, int categoryIndex)
    {
        var currenSlots = GetSlotsByCategory(categoryIndex);
        return currenSlots[itemIndex].Item;
    }


    public SO_ItemBase UseItem(int itemIndex, Pokemon selectedPokemon, int selectedCategory)
    {
        var currenSlots = GetSlotsByCategory(selectedCategory);

        var item = currenSlots[itemIndex].Item;
        bool itemUsed = item.Use(selectedPokemon);
        if (itemUsed)
        {
            RemoveItem(item);
            return item;
        }

        return null;
    }

    public void AddItem(SO_ItemBase item, int count = 1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slot => slot.Item == item);
        if (itemSlot != null)
        {
            itemSlot.Count += count;
        }
        else
        {
            currentSlots.Add(new ItemSlot()
            {
                Item = item,
                Count = count
            });
        }

        OnUpdated?.Invoke();
    }


    public int GetItemCount(SO_ItemBase item)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slot => slot.Item == item);

        if (itemSlot != null)
            return itemSlot.Count;
        else
            return 0;
    }

    public void RemoveItem(SO_ItemBase item, int countToRemove = 1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.First(slot => slot.Item == item);
        itemSlot.Count -= countToRemove;
        if (itemSlot.Count == 0)
            currentSlots.Remove(itemSlot);

        OnUpdated?.Invoke();
    }

    public bool HasItem(SO_ItemBase item)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        return currentSlots.Exists(slot => slot.Item == item);
    }

    ItemCategory GetCategoryFromItem(SO_ItemBase item)
    {
        if (item is RecoveryItem)
            return ItemCategory.Items;

        else
            return ItemCategory.Pokeballs;
    }

    public static Inventory GetInventory()
    {
        return FindObjectOfType<PlayerController>().GetComponent<Inventory>();
    }

    public object CaptureState()
    {
        var saveData = new InventorySaveData()
        {
            items = slots.Select(i => i.GetSaveData()).ToList(),
            pokeballs = pokeballSlots.Select(i => i.GetSaveData()).ToList(),
        };

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as InventorySaveData;

        slots = saveData.items.Select(i => new ItemSlot(i)).ToList();
        pokeballSlots = saveData.pokeballs.Select(i => new ItemSlot(i)).ToList();

        allSlots = new List<List<ItemSlot>>() { slots, pokeballSlots, harvestedSlot, weaponSlot, armorSlot, keyItemSlot };

        OnUpdated?.Invoke();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] SO_ItemBase item;
    [SerializeField] int count;

    public ItemSlot()
    {

    }

    public ItemSlot(ItemSaveData saveData)
    {
        item = ItemDB.GetObjectByName(saveData.name);
        count = saveData.count;
    }

    public ItemSaveData GetSaveData()
    {
        var saveData = new ItemSaveData()
        {
            name = item.name,
            count = count
        };

        return saveData;
    }

    public SO_ItemBase Item
    {
        get => item;
        set => item = value;
    }
    public int Count
    {
        get => count;
        set => count = value;
    }
}

[Serializable]
public class ItemSaveData
{
    public string name;
    public int count;
}

[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> items;
    public List<ItemSaveData> pokeballs;
}

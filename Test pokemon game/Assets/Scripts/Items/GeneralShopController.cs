using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

public enum ShopState { Menu, Buying, Selling, Busy }

public class GeneralShopController : MonoBehaviour
{
    [SerializeField] Vector2 shopCameraOffset;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] GeneralShopUI generalShopUI;
    [SerializeField] WalletUI walletUI;
    [SerializeField] CountSelectorUI countSelectorUI;

    GeneralMerchant generalMerchant;

    public event Action OnStart;
    public event Action OnFinish;

    ShopState state;

    [SerializeField] AudioSource audioSource;
    [SerializeField] string voiceName;
    bool spoken = false;

    public static GeneralShopController i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    public IEnumerator StartTrading(GeneralMerchant generalMerchant)
    {
        this.generalMerchant = generalMerchant;

        OnStart?.Invoke();
        yield return StartMenuState();
    }

    IEnumerator StartMenuState()
    {
        state = ShopState.Menu;

        int selectedChoice = 0;
        Speaker.Instance.Speak("See anything you like? Buy.", audioSource, Speaker.Instance.VoiceForName(voiceName));
        yield return DialogManager.Instance.ShowDialogText("See anything you like?",
            waitForInput: false,
            choices: new List<string>() { "Buy", "Sell", "Quit" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Buy
            yield return GameController.Instance.MoveCamera(shopCameraOffset);
            walletUI.Show();
            generalShopUI.Show(generalMerchant.AvailableItems, (item) => StartCoroutine(BuyItem(item)),
                () => StartCoroutine(OnBackFromBuying()));

            state = ShopState.Buying;
        }
        else if (selectedChoice == 1)
        {
            // Sell
            state = ShopState.Selling;
            inventoryUI.gameObject.SetActive(true);
        }
        else if (selectedChoice == 2)
        {
            // Quit
            OnFinish?.Invoke();
            yield break;
        }
    }

    public void HandleUpdate()
    {
        if (state == ShopState.Selling)
        {
            inventoryUI.HandleUpdate(OnBackFromSelling, (selectedItem) => StartCoroutine(SellItem(selectedItem)));
        }
        else if (state == ShopState.Buying)
        {
            generalShopUI.HandleUpdate();
        }
    }

    void OnBackFromSelling()
    {
        inventoryUI.gameObject.SetActive(false);
        StartCoroutine(StartMenuState());
    }

    IEnumerator SellItem(SO_ItemBase item)
    {
        state = ShopState.Busy;

        if (!item.IsSellable)
        {
            Speaker.Instance.Speak("You can't sell that!", audioSource, Speaker.Instance.VoiceForName(voiceName));
            yield return DialogManager.Instance.ShowDialogText("You can't sell that!");
            state = ShopState.Selling;
            yield break;
        }

        walletUI.Show();

        float sellingPrice = Mathf.Round(item.Price / 2);
        int countToSell = 1;

        int itemCount = inventory.GetItemCount(item);
        if (itemCount > 1)
        {
            Speaker.Instance.Speak("How many would you like to sell?", audioSource, Speaker.Instance.VoiceForName(voiceName));
            yield return DialogManager.Instance.ShowDialogText("How many would you like to sell?",
                waitForInput: false, autoClose: false);

            yield return countSelectorUI.ShowSelector(itemCount, sellingPrice,
                (selectedCount) => countToSell = selectedCount);

            DialogManager.Instance.CloseDialog();
        }

        sellingPrice = sellingPrice * countToSell;

        int selectedChoice = 0;
        Speaker.Instance.Speak("I can give you " + sellingPrice + " dollars for that. Would you like to sell?", audioSource, Speaker.Instance.VoiceForName(voiceName));
        yield return DialogManager.Instance.ShowDialogText("I can give you " + sellingPrice + " dollars for that. Would you like to sell?",
            waitForInput: false,
            choices: new List<string>() { "Yes", "No" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Yes
            inventory.RemoveItem(item, countToSell);
            Wallet.i.AddMoney(sellingPrice);
            Speaker.Instance.Speak("Turned over " + item.Name + " and received " + sellingPrice + " dollars!", audioSource, Speaker.Instance.VoiceForName(voiceName));
            yield return DialogManager.Instance.ShowDialogText("Turned over " + item.Name + " and received " + sellingPrice + "!");
        }

        walletUI.Close();

        state = ShopState.Selling;
    }

    IEnumerator BuyItem(SO_ItemBase item)
    {
        state = ShopState.Busy;

        Speaker.Instance.Speak("How many would you like to buy?", audioSource, Speaker.Instance.VoiceForName(voiceName));
        yield return DialogManager.Instance.ShowDialogText("How many would you like to buy?",
            waitForInput: false, autoClose: false);

        int countToBuy = 1;
        yield return countSelectorUI.ShowSelector(100, item.Price,
            (selectedCount) => countToBuy = selectedCount);

        DialogManager.Instance.CloseDialog();

        float totalPrice = item.Price * countToBuy;

        if (Wallet.i.HasMoney(totalPrice))
        {
            int selectedChoice = 0;
            Speaker.Instance.Speak("That will be " + totalPrice + " dollars.", audioSource, Speaker.Instance.VoiceForName(voiceName));
            yield return DialogManager.Instance.ShowDialogText("That will be " + totalPrice,
                waitForInput: false,
                choices: new List<string>() { "Yes", "No" },
                onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

            if (selectedChoice == 0)
            {
                // Selected Yes
                inventory.AddItem(item, countToBuy);
                Wallet.i.TakeMoney(totalPrice);
                Speaker.Instance.Speak("Thank you for shopping with us!", audioSource, Speaker.Instance.VoiceForName(voiceName));
                yield return DialogManager.Instance.ShowDialogText("Thank you for shopping with us!");
            }
        }
        else
        {
            Speaker.Instance.Speak("Not enough money for that!", audioSource, Speaker.Instance.VoiceForName(voiceName));
            yield return DialogManager.Instance.ShowDialogText("Not enough money for that!");
        }

        state = ShopState.Buying;
    }

    IEnumerator OnBackFromBuying()
    {
        yield return GameController.Instance.MoveCamera(-shopCameraOffset);
        generalShopUI.Close();
        walletUI.Close();
        StartCoroutine(StartMenuState());
    }
}

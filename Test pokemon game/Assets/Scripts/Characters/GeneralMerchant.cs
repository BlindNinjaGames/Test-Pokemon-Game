using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMerchant : MonoBehaviour
{
    [SerializeField] List<SO_ItemBase> availableItems;

    public IEnumerator Trade()
    {
        yield return GeneralShopController.i.StartTrading(this);
    }

    public List<SO_ItemBase> AvailableItems => availableItems;
}

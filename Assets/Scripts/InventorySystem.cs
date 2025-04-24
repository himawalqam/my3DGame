using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    

    public static InventorySystem Instance { get; set; }

    public GameObject ItemInfoUI;

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>(); 
    public List<string>itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public bool isOpen;

    //pickupPopUp
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
       
        PopulateSlotList();

        Cursor.visible = false;


    }
    private void PopulateSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

            SelectionManager.Instance.DisableSeletion();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);

            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSeletion();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            
            isOpen = false;
        }
    }

    public void AddToInvectory(string itemName)
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);

        whatSlotToEquip = FindNextEmptySlot();
       itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
       itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemToAdd.name = itemName + "(Clone)";

        itemList.Add(itemName);

        Sprite sprite = itemToAdd.GetComponent<Image>().sprite;

        TriggerPickupPop(itemName, sprite);

        ReCalculteList();

        CraftingSystem.Instance.RefreshNeededItems();
    }

    
    //触发拾取弹出窗口
     void TriggerPickupPop(string itemName,Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text= itemName;
        pickupImage.sprite = itemSprite;

        StartCoroutine(AutoClosePickup());
    }

    IEnumerator AutoClosePickup()
    {
        yield return new WaitForSeconds(2f); // 等2秒
        pickupAlert.SetActive(false);
    }



    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }

            
        }

        return new GameObject();
    }

    public bool CheckSlotAvailable(int emptyMeeded) //检查有多少个可用卡槽
    {
        int emptySlot = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 0)
            {
                emptySlot += 1;
            }
        }

        if (emptySlot >= emptyMeeded)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string nameToRemove,int amountToRemove)  
    {
        int counter = amountToRemove;
        for(var i=slotList.Count-1; i>=0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }
    }



   public void ReCalculteList()
    {
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name=slot.transform.GetChild(0).name;
                
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }

    
}
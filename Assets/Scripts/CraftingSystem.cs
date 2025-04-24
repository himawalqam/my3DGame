using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, constructionscreenUI;

    public List<string>inventoryItemList=new List<string>();

    Button toolsBTN, survivalBTN, refineBTN, constructionBTN;
    Button craftAxeBTN, craftPlankBTN;
    Text AxeReq1, AxeReq2, PlankReq1;

    Button craftFoundationBTN, craftWallBTN;
    Text FoundationReq1, FoundationReq2, WallReq1, WallReq2;


    public bool isOpen;

    public Blueprint AxeBLP = new Blueprint("Axe", 1, 2, "Stone", 3, "Stick", 3);
    public Blueprint PlankBLP = new Blueprint("Plank", 2, 1, "Log", 1, "", 0);

    public Blueprint FoundationBLP = new Blueprint("Foundation", 1, 1, "Plank", 4, "", 0);
    public Blueprint WallBLP = new Blueprint("Wall", 1, 1, "Plank", 2, "", 0);



    public static CraftingSystem Instance {  get;  set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen=false;

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });


        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        //Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CrafAnyItem(AxeBLP); });

        //Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CrafAnyItem(PlankBLP); });

        constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBTN.onClick.AddListener(delegate { OpenConstructionCategory(); });

        // Foundation
        FoundationReq1 = constructionscreenUI.transform.Find("Foundation").Find("req1").GetComponent<Text>();
        //FoundationReq2 = constructionscreenUI.transform.Find("Foundation").Find("req2").GetComponent<Text>(); // 可不显示或隐藏
        craftFoundationBTN = constructionscreenUI.transform.Find("Foundation").Find("Button").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate { CrafAnyItem(FoundationBLP); });

        // Wall
        WallReq1 = constructionscreenUI.transform.Find("Wall").Find("req1").GetComponent<Text>();
        //WallReq2 = constructionscreenUI.transform.Find("Wall").Find("req2").GetComponent<Text>(); // 可不显示或隐藏
        craftWallBTN = constructionscreenUI.transform.Find("Wall").Find("Button").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate { CrafAnyItem(WallBLP); });



    }


    void OpenConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        constructionscreenUI.SetActive(true);
    }



    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);
    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);
    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);
    }



    private void CrafAnyItem(Blueprint blueprintToCraft)
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        // 根据蓝图生产物品数量
        StartCoroutine(craftedDelayForSound(blueprintToCraft));


        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            
        }else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }


        StartCoroutine(calculate());

    }

    IEnumerator craftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);
        for (var i = 0; i < blueprintToCraft.numberOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInvectory(blueprintToCraft.itemName);
        }
    }

    public IEnumerator calculate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculteList();
        RefreshNeededItems();
    }
    // Update is called once per frame
    void Update()
    {

        //RefreshNeededItems();
        
        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {

            Debug.Log("i is pressed");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

            SelectionManager.Instance.DisableSeletion();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionscreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;

                Cursor.visible = false;

                SelectionManager.Instance.EnableSeletion();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
           
            isOpen = false;
        }
    }


    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;
        int plank_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;
        foreach(string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case"Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
                case "Log":
                    log_count += 1;
                    break;
                case "Plank":
                        plank_count += 1;
                    break;
            }
        }

        //----Axe----//
        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        

        if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }


        //----Plank *2----//
        PlankReq1.text = "1 Log [" + log_count + "]";

        if (log_count >= 1 && InventorySystem.Instance.CheckSlotAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        }

        // Foundation
        FoundationReq1.text = "4 Plank [" + plank_count + "]";
        //FoundationReq2.text = ""; // 仅作占位，或者可以隐藏这个 UI

        if (plank_count >= 4 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftFoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationBTN.gameObject.SetActive(false);
        }

        WallReq1.text = "2 Plank [" + plank_count + "]";
        //WallReq2.text = ""; // 同上

        if (plank_count >= 2 && InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }
    }
}

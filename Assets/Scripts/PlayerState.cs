using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance {  get;  set; }

    //生命值
    public float currentHealth;
    public float maxHealth;

    //体力值
    public float currentCalories;
    public float maxCalories;
    float distanceTravelled = 0;//走路距离
    Vector3 lastPosition;
    public GameObject playerBody;

    //口渴量
    public float currentHydrationPercent;
    public float maxHydrationPercent;
    public bool isHydrationActive;


    private void Awake()
    {
        if(Instance != null&& Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories= maxCalories;
        currentHydrationPercent= maxHydrationPercent;

        StartCoroutine(decreaseHydration());
    }

     IEnumerator decreaseHydration()
    {
        while (true) 
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }

    public void setHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }

    public void setCalories(float value)
    {
        currentCalories = Mathf.Clamp(value, 0, maxCalories);
    }

    public void setHydration(float value)
    {
        currentHydrationPercent = Mathf.Clamp(value, 0, maxHydrationPercent);
    }

}

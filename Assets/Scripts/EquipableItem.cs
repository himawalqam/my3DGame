using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    private bool canSwing = false; // �Ƿ���ԻӶ���������
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(EnableSwingAfterDelay(0.2f)); // �ȴ�һ�����������
    }

// Update is called once per frame
void Update()
    {

        if (Input.GetMouseButtonDown(0) && //������
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false &&
            SelectionManager.Instance.handIsVisible == false&&
             !ConstructionManager.Instance.inConstructionMode&&
             canSwing
            ) 

        {
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
            
        }
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;
        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.0f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolsSwingSound);
    }
    IEnumerator EnableSwingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSwing = true;
    }
}

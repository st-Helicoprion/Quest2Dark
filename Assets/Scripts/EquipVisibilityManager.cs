using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipVisibilityManager : MonoBehaviour
{
    public Renderer[] itemSkin;
    public KeyItemReporter itemReporter;
    public bool isHideable;

    private void Start()
    {
        itemReporter = GetComponent<KeyItemReporter>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckToolboxVisibility();
    }

    public void CheckToolboxVisibility()
    {
        if (ToolboxManager.itemIDList.Contains(itemReporter.itemID) || GameManager.instance.playerEquipID.Contains(itemReporter.itemID))
        {
            for (int i = 0; i < itemSkin.Length; i++)
            {
                if (itemSkin[i] != null)
                {
                    if (ToolboxManager.instance.isVisible)
                    {
                        itemSkin[i].enabled = true;
                    }
                    else if (ToolboxManager.instance.isHidden&&isHideable)
                    {
                        itemSkin[i].enabled = false;
                    }
                }
                else return;
            }
        }
        else return;
        
        
    }
}

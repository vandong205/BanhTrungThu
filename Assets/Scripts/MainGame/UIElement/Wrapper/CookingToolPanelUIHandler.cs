using TMPro;
using UnityEngine;
using System.Collections.Generic;
public class CookingToolPanelUIHandler : MonoBehaviour
{
    [SerializeField] GameObject Content;
    [SerializeField] TextMeshProUGUI ToolName;
    [SerializeField] TextMeshProUGUI ToolUse;
    [SerializeField] Transform output;

    public void SetProp(string toolname,string tooluse)
    {
        ToolName.text = toolname;
        ToolUse.text = tooluse; 
    }
    public void ClearItem()
    {
        foreach (Transform child in Content.transform) {
            foreach(Transform item in child)
            {
                Destroy(item.gameObject);
            }
        }
    }
    public int[] GetInput()
    {
        int[] result = new int[3];
        int count = 0;
        foreach (Transform child in Content.transform)
        {
            foreach (Transform item in child)
            {
                ObjectInfo iteminfo = item.GetComponent<ObjectInfo>();
                if (iteminfo != null) {
                    result[count] = iteminfo.ID;
                    count++;
                }
            }
        }
        return result;
    }
    public void SetOutput(GameObject obj)
    {
        obj.transform.SetParent(output,false);
    }
}

using TMPro;
using UnityEngine;

public class CookingToolPanelUIHandler : MonoBehaviour
{
    [SerializeField] GameObject Content;
    [SerializeField] TextMeshProUGUI ToolName;
    [SerializeField] TextMeshProUGUI ToolUse;
    [SerializeField] ObjectInfo result;

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
}

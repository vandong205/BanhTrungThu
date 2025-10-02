using System.Collections.Generic;
using UnityEngine;

public class CookingProcessController:MonoBehaviour
{
    [SerializeField] CookingToolPanelUIHandler cookingToolPanelUIHandler;
    private ProcessedItem result = null;
    private List<ProcessedItem> tempitem  =new List<ProcessedItem>();
    bool outputstateok ;
    private int? GetProcesOutputID(KitchenItem tool)
    {
        int[] input = cookingToolPanelUIHandler.GetInput();
        if (!tool.IsValidInput(input))
        {
            Notification.Instance.Display("Bạn đang dùng sai công cụ!", NotificationType.Warning);
            return null;
        }
        return ResourceManager.Instance.recipeBook.FindOutput(input);
    }
    public void ProcessOutput(KitchenItem tool)
    {
        int? outputIdNullable = GetProcesOutputID(tool);
        if (!outputIdNullable.HasValue)
        {
            Notification.Instance.Display("Công thức không đúng hoặc chưa được mở khóa !", NotificationType.Warning);
            outputstateok = false;
            return;
        }
        int outputId = outputIdNullable.Value;


        if (outputId >= 100 && outputId < 200)
        {
            if (ResourceManager.Instance.CakeDict.TryGetValue(outputId, out Cake item))
            {
                ProcessedItem parsecake = new ProcessedItem();
                parsecake.ID  = item.ID;
                parsecake.Name = item.Name;
                parsecake.RoleName = item.RoleName;
                result = parsecake;
            }
        } else if (outputId >= 300 && outputId < 400)
        {
            if (ResourceManager.Instance.CakeFillingDict.TryGetValue(outputId, out ProcessedItem item))
            {
                result = item;
            }
        }
        else if (outputId >= 400 && outputId < 500)
        {
            if (ResourceManager.Instance.ShapedCake.TryGetValue(outputId, out ProcessedItem item))
            {
                result = item;
            }
        }
        else if (outputId >= 500 && outputId < 600)
        {
            if (ResourceManager.Instance.CakeCrust.TryGetValue(outputId, out ProcessedItem item))
            {
                result = item;
            }
        }

        if (result != null)
        {
            
            Debug.Log($"Đã tìm thấy output: {result.Name}");
            if (!(result.ID >= 100 && result.ID < 200)) tempitem.Add(result);
            outputstateok = true;
        }
        else
        {
            outputstateok = false;

        }

    }
    public List<ProcessedItem> GetTempItem()
    {
        return tempitem;
    }
    public ProcessedItem GetOutputInfo()
    {
        return result;
    }
    public bool GetOuputState()
    {
        return outputstateok;

    }



}
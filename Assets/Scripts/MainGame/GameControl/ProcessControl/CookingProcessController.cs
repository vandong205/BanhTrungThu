using System.Collections.Generic;
using UnityEngine;

public class CookingProcessController:MonoBehaviour
{
    [SerializeField] CookingToolPanelUIHandler cookingToolPanelUIHandler;
    private ProcessedItem result = null;
    private List<ProcessedItem> tempitem  =new List<ProcessedItem>();
    bool outputstateok ;
    private int? GetProcesOutputID()
    {
        int[] ouput = cookingToolPanelUIHandler.GetInput();
        return ResourceManager.Instance.recipeBook.FindOutput(ouput);
    }
    public void ProcessOutput()
    {
        outputstateok = true;
        int? outputIdNullable = GetProcesOutputID();
        if (!outputIdNullable.HasValue)
        {
            outputstateok = false;
            return;
        }
        int outputId = outputIdNullable.Value;

       if (outputId >= 300 && outputId < 400)
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
            tempitem.Add(result);
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy output cho ID {outputId}");
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
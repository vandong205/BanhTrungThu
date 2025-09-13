using UnityEngine;

public class CookingProcessController:MonoBehaviour
{
    [SerializeField] CookingToolPanelUIHandler cookingToolPanelUIHandler;
    private int? GetProcesOutput()
    {
        int[] ouput = cookingToolPanelUIHandler.GetInput();
        Debug.Log(ouput.Length);
        return ResourceManager.Instance.recipeBook.FindOutput(ouput);
    }
    public void ShowProcessOutput()
    {
        Debug.Log($"Đã goi show Output");
        int? outputIdNullable = GetProcesOutput();
        if (!outputIdNullable.HasValue) return; 

        int outputId = outputIdNullable.Value;
        object result = null;

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
            if (ResourceManager.Instance.ShapedCake.TryGetValue(outputId, out ProcessedItem item))
            {
                result = item;
            }
        }

        if (result != null)
        {
            Debug.Log($"Đã tìm thấy output: {((ProcessedItem)result).Name}");
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy output cho ID {outputId}");
        }
    }

}
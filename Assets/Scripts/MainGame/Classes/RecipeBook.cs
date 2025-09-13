using System.Linq;
using System.Collections.Generic;

public class RecipeBook
{
    private Dictionary<string, int> recipeMap = new Dictionary<string, int>();

    public RecipeBook(List<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            // chuẩn hóa InputIds thành string key (sắp xếp để không lệch thứ tự)
            string key = string.Join("-", recipe.InputIds.OrderBy(x => x));
            recipeMap[key] = recipe.OutputId;
        }
    }

    public int? FindOutput(int[] inputIds)
    {
        string key = string.Join("-", inputIds.OrderBy(x => x));
        return recipeMap.TryGetValue(key, out int output) ? output : (int?)null;
    }
}

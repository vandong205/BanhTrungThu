using UnityEngine;

public class RecipeTabManager : MonoBehaviour
{
    [SerializeField] Transform RecipeContent;
    public void LoadCakeRecipe()
    {


        foreach (int cakeid in UIGamePlayManager.Instance.player.UnlockedCakes)
        {
            if (ResourceManager.Instance.CakeDict.TryGetValue(cakeid, out Cake cake))
            {
                if (cake == null) continue;
                var recipePrefab = Resources.Load<GameObject>("Prefabs/RecipePrefab");
                if (recipePrefab == null)
                {
                    continue;
                }

                Sprite cakeIcon = null;
                string cakeName = cake.Name;

                if (AssetBundleManager.Instance.GetAssetBundle("banh", out AssetBundle cakeBundle))
                {
                    if (cakeBundle != null)
                    {
                        cakeIcon = cakeBundle.LoadAsset<Sprite>(cake.RoleName);
                    }
                }

                Sprite[] indreIcons = new Sprite[3];
                string[] indreNames = new string[3];

                for (int i = 0; i < cake.Ingredients.Count && i < 3; i++)
                {
                    int indreId = cake.Ingredients[i];
                    if (ResourceManager.Instance.IngredientDict.TryGetValue(indreId, out Ingredient ingredient))
                    {
                        if (ingredient != null)
                        {
                            indreNames[i] = ingredient.Name;

                            if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle indreBundle))
                            {
                                if (indreBundle != null)
                                {
                                    indreIcons[i] = indreBundle.LoadAsset<Sprite>(ingredient.RoleName);
                                }
                            }
                        }
                    }
                }

                var recipeGO = Instantiate(recipePrefab, RecipeContent);

                var controller = recipeGO.GetComponent<RecipeUIController>();
                if (controller != null)
                {
                    controller.SetProp(
                        cakeIcon, cakeName,
                        indreIcons.Length > 0 ? indreIcons[0] : null, indreNames.Length > 0 ? indreNames[0] : "",
                        indreIcons.Length > 1 ? indreIcons[1] : null, indreNames.Length > 1 ? indreNames[1] : "",
                        indreIcons.Length > 2 ? indreIcons[2] : null, indreNames.Length > 2 ? indreNames[2] : ""
                    );
                }
            }
        }
    }
    public void OpenTab()
    {
        gameObject.SetActive(true);
    }
    public void CloseTab()
    {
        gameObject.SetActive(false);
    }
}

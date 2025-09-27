using System.Collections.Generic;
using UnityEngine;

public class PaperBagHolder:MonoBehaviour
{

    public void WrapCakeOnClick()
    {
        if (GamePlayController.Instance._HasDoneCakeForCustumer) return;
        GamePlayController.Instance._HasDoneCakeForCustumer = true;
        StartCoroutine(ReceptionRoomUIManager.Instance.SetActiveDummyBagDelay(true, 0.2f));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTip : MonoBehaviour
{
    [SerializeField]
    private float DeactivateInSeconds = 10f;

    [SerializeField]
    private Text TipText;

    private Coroutine CurrentlyShownTip = null;

    /// <summary>
    /// Show new tip to player
    /// </summary>
    public void SetNewTip(string message)
    {
        if (CurrentlyShownTip != null)
            StopCoroutine(CurrentlyShownTip);

        CurrentlyShownTip = StartCoroutine("Deactivate", message);
    }

    IEnumerator Deactivate(string message)
    {
        TipText.text = message;
        TipText.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(DeactivateInSeconds);
        TipText.transform.parent.gameObject.SetActive(false);
    }
}

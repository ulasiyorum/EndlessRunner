using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class StartPopUpMessage
{
    public static void Message(string message, Color color)
    {
        GameObject go = Object.Instantiate(AssetsHandler.Instance.popUpPrefab,GameHandler.Instance.canvas.transform);
        go.GetComponent<TMP_Text>().text = message;
        go.GetComponent<TMP_Text>().color = color;
    }

    public static void MessageNormal(string message, Color color)
    {
        GameObject go = Object.Instantiate(AssetsHandler.Instance.popUpNormalPrefab, GameHandler.Instance.canvas.transform);
        go.GetComponent<Text>().text = message;
        go.GetComponent<Text>().color = color;
    }

}

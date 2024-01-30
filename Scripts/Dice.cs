using System;
using UnityEngine;
using TMPro;

public class Dice : MonoBehaviour
{
    [Serializable]
    public class DiceFace
    {
        public Transform faceTransform;
        public int faceValue;
        public TextMeshProUGUI character;
        public int characterValue;
    }

    public DiceFace[] diceFaces = new DiceFace[12];

    private void Start()
    {
        UpdateCharacters();
    }

    [ContextMenu("GetTopFace")]
    public int GetValueOnTopFace()
    {
        var topFace = 0;
        var lastYPosition = diceFaces[0].faceTransform.position.y;
        for (int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].faceTransform.position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].faceTransform.position.y;
                topFace = i;
            }
        }

        return diceFaces[topFace].faceValue;
    }

    private void UpdateCharacters()
    {
        foreach (var diceFace in diceFaces)
        {
            diceFace.character.text = diceFace.characterValue.ToString();
        }
    }
}

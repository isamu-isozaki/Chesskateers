// Name: BoardHighlights.cs
// Purpose: Highlight the board where the piece can move
// Version: 1. 
// Date: 2020/6/8
// Author: Isamu Isozaki
// Dependencies: Look at imports below
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    public GameObject highlightPrefab;
    public static BoardHighlights Instance { set; get; }
    private const float TILE_SIZE = 1.5f;
    private const float HIGHTLIGHT_OFFSET = -5.25f;
    private static List<GameObject> highlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject GetHighLightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            DontDestroyOnLoad(go);
            //Vector3 temp = new Vector3(0.0f, 0.0f, 0.0f);
            //go = PhotonNetwork.Instantiate("highlight", temp, Quaternion.identity) as GameObject;
            highlights.Add(go);
        }

        return go;
    }

    public void HighLightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetHighLightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(TILE_SIZE*i + HIGHTLIGHT_OFFSET, 0.0001f, TILE_SIZE*j + HIGHTLIGHT_OFFSET);
                }
            }

        }
    }
    public void HideHighlights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
}
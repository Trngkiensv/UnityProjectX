using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PuzzlePieceSetup
{
    public string displayName = "Piece";
    public Color color = Color.yellow;
    public Sprite blockSprite;

    public Vector2Int[] cells =
    {
        new Vector2Int(0, 0)
    };
}

public class PuzzlePieceSpawner : MonoBehaviour
{
    [SerializeField] private PuzzlePiece piecePrefab;
    [SerializeField] private RectTransform pieceParent;

    [SerializeField]
    private List<PuzzlePieceSetup> pieces =
        new List<PuzzlePieceSetup>();

    private void Start()
    {
        SpawnPieces();
    }

    public void SpawnPieces()
    {
        if (pieceParent == null)
        {
            pieceParent =
                GetComponent<RectTransform>();
        }

        foreach (PuzzlePieceSetup setup in pieces)
        {
            PuzzlePiece piece =
                Instantiate(
                    piecePrefab,
                    pieceParent
                );

            piece.name = setup.displayName;

            piece.Initialize(
                setup.cells,
                setup.color,
                setup.blockSprite,
                pieceParent
            );
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            pieceParent
        );
    }
}
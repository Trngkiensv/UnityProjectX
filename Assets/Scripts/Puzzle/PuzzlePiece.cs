using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PuzzleShape
{
    Single,
    Line3,
    Square2x2,
    L4,
    T4,
    Z4
}

[RequireComponent(typeof(RectTransform))]
public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private PuzzleShape shapeType = PuzzleShape.Line3;

    [Header("Visual")]
    [SerializeField] private float blockSize = 70f;
    [SerializeField] private float spacing = 6f;
    [SerializeField]
    private Color blockColor =
        new Color(1f, 0.65f, 0f, 1f);
    [SerializeField] private Sprite blockSprite;

    public IReadOnlyList<Vector2Int> Cells => cells;
    public Color BlockColor => blockColor;
    public bool IsPlaced { get; private set; }

    private readonly List<Vector2Int> cells =
        new List<Vector2Int>();

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Build();
    }

    private void Build()
    {
        cells.Clear();

        Vector2Int[] shape = GetShape(shapeType);

        foreach (Vector2Int cell in shape)
        {
            cells.Add(cell);
        }

        int maxX = 0;
        int maxY = 0;

        foreach (Vector2Int cell in cells)
        {
            maxX = Mathf.Max(maxX, cell.x);
            maxY = Mathf.Max(maxY, cell.y);
        }

        float step = blockSize + spacing;

        float width =
            (maxX + 1) * blockSize +
            maxX * spacing;

        float height =
            (maxY + 1) * blockSize +
            maxY * spacing;

        rectTransform.sizeDelta =
            new Vector2(width, height);

        foreach (Vector2Int cell in cells)
        {
            CreateBlock(
                cell,
                width,
                height,
                step
            );
        }
    }

    private void CreateBlock(
        Vector2Int cell,
        float width,
        float height,
        float step)
    {
        GameObject blockObject =
            new GameObject(
                "Block",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );

        RectTransform blockRect =
            blockObject.GetComponent<RectTransform>();

        blockRect.SetParent(transform, false);

        blockRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        blockRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        blockRect.pivot =
            new Vector2(0.5f, 0.5f);

        blockRect.sizeDelta =
            new Vector2(blockSize, blockSize);

        float x =
            -width * 0.5f +
            blockSize * 0.5f +
            cell.x * step;

        float y =
            height * 0.5f -
            blockSize * 0.5f -
            cell.y * step;

        blockRect.anchoredPosition =
            new Vector2(x, y);

        Image image =
            blockObject.GetComponent<Image>();

        image.color = blockColor;
        image.sprite = blockSprite;
        image.raycastTarget = true;
    }

    public void MarkPlaced()
    {
        IsPlaced = true;
    }

    private static Vector2Int[] GetShape(
        PuzzleShape shape)
    {
        switch (shape)
        {
            case PuzzleShape.Single:
                return new[]
                {
                    new Vector2Int(0, 0)
                };

            case PuzzleShape.Line3:
                return new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(2, 0)
                };

            case PuzzleShape.Square2x2:
                return new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1)
                };

            case PuzzleShape.L4:
                return new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, 2),
                    new Vector2Int(1, 2)
                };

            case PuzzleShape.T4:
                return new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(2, 0),
                    new Vector2Int(1, 1)
                };

            case PuzzleShape.Z4:
                return new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(1, 1),
                    new Vector2Int(2, 1)
                };

            default:
                return new[]
                {
                    new Vector2Int(0, 0)
                };
        }
    }
}
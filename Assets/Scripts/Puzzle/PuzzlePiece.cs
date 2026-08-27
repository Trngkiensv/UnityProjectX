using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PuzzlePiece : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private float blockSize = 70f;
    [SerializeField] private float spacing = 6f;

    private readonly List<Vector2Int> cells =
        new List<Vector2Int>();

    private RectTransform rectTransform;
    private LayoutElement layoutElement;

    private Color blockColor;
    private Sprite blockSprite;

    private RectTransform homeParent;
    private int homeSiblingIndex;

    private PuzzleBoard currentBoard;
    private int anchorX;
    private int anchorY;

    private PuzzleBoard previousBoard;
    private int previousAnchorX;
    private int previousAnchorY;

    public IReadOnlyList<Vector2Int> Cells => cells;
    public Color BlockColor => blockColor;

    public bool IsPlaced { get; private set; }
    public bool DropSucceededThisDrag { get; private set; }

    public PuzzleBoard CurrentBoard => currentBoard;
    public int AnchorX => anchorX;
    public int AnchorY => anchorY;

    private void Awake()
    {
        rectTransform =
            GetComponent<RectTransform>();

        layoutElement =
            GetComponent<LayoutElement>();

        if (layoutElement == null)
        {
            layoutElement =
                gameObject.AddComponent<LayoutElement>();
        }
    }

    public void Initialize(
        Vector2Int[] shapeCells,
        Color color,
        Sprite sprite,
        RectTransform trayParent)
    {
        blockColor = color;
        blockSprite = sprite;

        homeParent = trayParent;
        homeSiblingIndex =
            transform.GetSiblingIndex();

        SetCells(shapeCells);

        layoutElement.ignoreLayout = false;
    }

    public void SetCells(
        IEnumerable<Vector2Int> newCells)
    {
        cells.Clear();

        foreach (Vector2Int cell in newCells)
        {
            cells.Add(cell);
        }

        NormalizeCells();
        BuildVisual();
    }

    private void NormalizeCells()
    {
        if (cells.Count == 0)
        {
            cells.Add(Vector2Int.zero);
            return;
        }

        int minX = int.MaxValue;
        int minY = int.MaxValue;

        foreach (Vector2Int cell in cells)
        {
            minX = Mathf.Min(minX, cell.x);
            minY = Mathf.Min(minY, cell.y);
        }

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i] =
                new Vector2Int(
                    cells[i].x - minX,
                    cells[i].y - minY
                );
        }
    }

    private void BuildVisual()
    {
        for (
            int i = transform.childCount - 1;
            i >= 0;
            i--)
        {
            GameObject child =
                transform.GetChild(i).gameObject;

            child.SetActive(false);
            Destroy(child);
        }

        int maxX = 0;
        int maxY = 0;

        foreach (Vector2Int cell in cells)
        {
            maxX = Mathf.Max(maxX, cell.x);
            maxY = Mathf.Max(maxY, cell.y);
        }

        float step =
            blockSize + spacing;

        float width =
            blockSize +
            maxX * step;

        float height =
            blockSize +
            maxY * step;

        rectTransform.pivot =
            new Vector2(0f, 1f);

        rectTransform.sizeDelta =
            new Vector2(width, height);

        layoutElement.preferredWidth = width;
        layoutElement.preferredHeight = height;

        foreach (Vector2Int cell in cells)
        {
            CreateBlock(cell, step);
        }
    }

    private void CreateBlock(
        Vector2Int cell,
        float step)
    {
        GameObject block =
            new GameObject(
                "Block",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );

        RectTransform blockRect =
            block.GetComponent<RectTransform>();

        blockRect.SetParent(
            transform,
            false
        );

        blockRect.anchorMin =
            new Vector2(0f, 1f);

        blockRect.anchorMax =
            new Vector2(0f, 1f);

        blockRect.pivot =
            new Vector2(0.5f, 0.5f);

        blockRect.sizeDelta =
            new Vector2(
                blockSize,
                blockSize
            );

        blockRect.anchoredPosition =
            new Vector2(
                blockSize * 0.5f +
                cell.x * step,

                -blockSize * 0.5f -
                cell.y * step
            );

        Image image =
            block.GetComponent<Image>();

        image.color = blockColor;
        image.sprite = blockSprite;
        image.raycastTarget = true;
    }

    public void BeginDrag()
    {
        DropSucceededThisDrag = false;

        previousBoard = null;

        if (
            IsPlaced &&
            currentBoard != null)
        {
            previousBoard =
                currentBoard;

            previousAnchorX =
                anchorX;

            previousAnchorY =
                anchorY;

            currentBoard.RemovePiece(this);

            currentBoard = null;
            IsPlaced = false;
        }

        layoutElement.ignoreLayout = true;
    }

    public void CommitPlacement(
        PuzzleBoard board,
        int x,
        int y,
        RectTransform placedLayer,
        PuzzleCell anchorCell,
        bool markDropSuccess)
    {
        currentBoard = board;

        anchorX = x;
        anchorY = y;

        IsPlaced = true;

        DropSucceededThisDrag =
            markDropSuccess;

        previousBoard = null;

        layoutElement.ignoreLayout = true;

        rectTransform.SetParent(
            placedLayer,
            true
        );

        SnapToCell(anchorCell);
    }

    private void SnapToCell(
        PuzzleCell cell)
    {
        Vector3[] corners =
            new Vector3[4];

        cell.RectTransform.GetWorldCorners(
            corners
        );

        rectTransform.position =
            corners[1];

        rectTransform.localScale =
            Vector3.one;

        rectTransform.localRotation =
            Quaternion.identity;
    }

    public void RestoreAfterFailedDrag()
    {
        if (previousBoard != null)
        {
            PuzzleBoard board =
                previousBoard;

            int x =
                previousAnchorX;

            int y =
                previousAnchorY;

            previousBoard = null;

            if (!board.TryPlacePiece(
                    this,
                    x,
                    y,
                    false))
            {
                ReturnHome();
            }

            return;
        }

        ReturnHome();
    }

    public void ReturnHome()
    {
        currentBoard = null;

        IsPlaced = false;
        DropSucceededThisDrag = false;

        rectTransform.SetParent(
            homeParent,
            false
        );

        if (homeParent != null)
        {
            int sibling =
                Mathf.Clamp(
                    homeSiblingIndex,
                    0,
                    homeParent.childCount - 1
                );

            transform.SetSiblingIndex(
                sibling
            );
        }

        rectTransform.localScale =
            Vector3.one;

        rectTransform.localRotation =
            Quaternion.identity;

        layoutElement.ignoreLayout =
            false;

        if (homeParent != null)
        {
            LayoutRebuilder.MarkLayoutForRebuild(
                homeParent
            );
        }
    }

    public void RotateClockwise()
    {
        if (
            IsPlaced &&
            currentBoard != null)
        {
            currentBoard.TryRotatePiece(
                this
            );

            return;
        }

        ApplyRotationClockwise();
    }

    public void ApplyRotationClockwise()
    {
        int maxY = 0;

        foreach (Vector2Int cell in cells)
        {
            maxY =
                Mathf.Max(
                    maxY,
                    cell.y
                );
        }

        List<Vector2Int> rotated =
            new List<Vector2Int>();

        foreach (Vector2Int cell in cells)
        {
            rotated.Add(
                new Vector2Int(
                    maxY - cell.y,
                    cell.x
                )
            );
        }

        SetCells(rotated);
    }

    public Vector2Int[] GetCellsCopy()
    {
        return cells.ToArray();
    }
}
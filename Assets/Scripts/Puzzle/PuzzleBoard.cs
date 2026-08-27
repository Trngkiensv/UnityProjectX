using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
    [SerializeField] private PuzzleCell cellPrefab;
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    [SerializeField]
    private RectTransform placedPiecesLayer;

    private PuzzleCell[,] cells;
    private PuzzlePiece[,] occupiedBy;

    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        cells =
            new PuzzleCell[width, height];

        occupiedBy =
            new PuzzlePiece[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                PuzzleCell cell =
                    Instantiate(
                        cellPrefab,
                        transform
                    );

                cell.Initialize(
                    this,
                    x,
                    y
                );

                cells[x, y] = cell;
            }
        }
    }

    public bool TryPlacePiece(
        PuzzlePiece piece,
        int anchorX,
        int anchorY,
        bool markDropSuccess = true)
    {
        if (!CanPlacePiece(
                piece,
                anchorX,
                anchorY))
        {
            return false;
        }

        OccupyCells(
            piece,
            anchorX,
            anchorY
        );

        piece.CommitPlacement(
            this,
            anchorX,
            anchorY,
            placedPiecesLayer,
            cells[anchorX, anchorY],
            markDropSuccess
        );

        return true;
    }

    private bool CanPlacePiece(
        PuzzlePiece piece,
        int anchorX,
        int anchorY)
    {
        foreach (
            Vector2Int offset
            in piece.Cells)
        {
            int x =
                anchorX + offset.x;

            int y =
                anchorY + offset.y;

            if (!IsInsideBoard(x, y))
            {
                return false;
            }

            if (occupiedBy[x, y] != null)
            {
                return false;
            }
        }

        return true;
    }

    private void OccupyCells(
        PuzzlePiece piece,
        int anchorX,
        int anchorY)
    {
        foreach (
            Vector2Int offset
            in piece.Cells)
        {
            int x =
                anchorX + offset.x;

            int y =
                anchorY + offset.y;

            occupiedBy[x, y] = piece;
        }
    }

    public void RemovePiece(
        PuzzlePiece piece)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (
                    occupiedBy[x, y] ==
                    piece)
                {
                    occupiedBy[x, y] =
                        null;
                }
            }
        }
    }

    public bool TryRotatePiece(
        PuzzlePiece piece)
    {
        if (
            !piece.IsPlaced ||
            piece.CurrentBoard != this)
        {
            return false;
        }

        int x = piece.AnchorX;
        int y = piece.AnchorY;

        Vector2Int[] oldCells =
            piece.GetCellsCopy();

        RemovePiece(piece);

        piece.ApplyRotationClockwise();

        if (CanPlacePiece(
                piece,
                x,
                y))
        {
            OccupyCells(
                piece,
                x,
                y
            );

            piece.CommitPlacement(
                this,
                x,
                y,
                placedPiecesLayer,
                cells[x, y],
                false
            );

            return true;
        }

        piece.SetCells(oldCells);

        OccupyCells(
            piece,
            x,
            y
        );

        piece.CommitPlacement(
            this,
            x,
            y,
            placedPiecesLayer,
            cells[x, y],
            false
        );

        return false;
    }

    private bool IsInsideBoard(
        int x,
        int y)
    {
        return
            x >= 0 &&
            x < width &&
            y >= 0 &&
            y < height;
    }
}
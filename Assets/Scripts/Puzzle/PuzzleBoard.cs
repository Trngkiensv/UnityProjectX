using UnityEngine;

public class PuzzleBoard : MonoBehaviour
{
    [SerializeField] private PuzzleCell cellPrefab;
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    private PuzzleCell[,] cells;

    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        cells = new PuzzleCell[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                PuzzleCell cell =
                    Instantiate(cellPrefab, transform);

                cell.Initialize(this, x, y);

                cells[x, y] = cell;
            }
        }
    }

    public bool TryPlacePiece(
        PuzzlePiece piece,
        int anchorX,
        int anchorY)
    {
        foreach (Vector2Int offset in piece.Cells)
        {
            int x = anchorX + offset.x;
            int y = anchorY + offset.y;

            if (!IsInsideBoard(x, y))
            {
                return false;
            }

            if (cells[x, y].IsOccupied)
            {
                return false;
            }
        }

        foreach (Vector2Int offset in piece.Cells)
        {
            int x = anchorX + offset.x;
            int y = anchorY + offset.y;

            cells[x, y].Fill(piece.BlockColor);
        }

        piece.MarkPlaced();

        return true;
    }

    private bool IsInsideBoard(int x, int y)
    {
        return
            x >= 0 &&
            x < width &&
            y >= 0 &&
            y < height;
    }
}
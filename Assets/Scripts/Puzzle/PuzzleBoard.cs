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

                cells[x, y] = cell;
            }
        }
    }
}
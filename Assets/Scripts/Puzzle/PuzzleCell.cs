using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleCell :
    MonoBehaviour,
    IDropHandler
{
    [SerializeField] private Image image;

    public bool IsOccupied { get; private set; }

    public int GridX { get; private set; }
    public int GridY { get; private set; }

    private PuzzleBoard board;
    private Color emptyColor;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        emptyColor = image.color;
    }

    public void Initialize(
        PuzzleBoard puzzleBoard,
        int x,
        int y)
    {
        board = puzzleBoard;

        GridX = x;
        GridY = y;
    }

    public void OnDrop(
        PointerEventData eventData)
    {
        PuzzlePiece piece =
            eventData.pointerDrag?
                .GetComponent<PuzzlePiece>();

        if (piece == null || board == null)
        {
            return;
        }

        board.TryPlacePiece(
            piece,
            GridX,
            GridY
        );
    }

    public void Fill(Color color)
    {
        IsOccupied = true;
        image.color = color;
    }

    public void Clear()
    {
        IsOccupied = false;
        image.color = emptyColor;
    }
}
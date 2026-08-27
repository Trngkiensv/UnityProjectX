using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleCell :
    MonoBehaviour,
    IDropHandler
{
    [SerializeField] private Image image;

    public int GridX { get; private set; }
    public int GridY { get; private set; }

    public RectTransform RectTransform
    {
        get;
        private set;
    }

    private PuzzleBoard board;

    private void Awake()
    {
        RectTransform =
            GetComponent<RectTransform>();

        if (image == null)
        {
            image =
                GetComponent<Image>();
        }
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

        if (
            piece == null ||
            board == null)
        {
            return;
        }

        board.TryPlacePiece(
            piece,
            GridX,
            GridY
        );
    }
}
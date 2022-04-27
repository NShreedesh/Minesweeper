using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Tiles Info")]
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private float size;
    public Cell[,] cells;

    [Header("Tile Info")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject buttonTilePrefab;

    [Header("Camera Info")]
    public Camera cam;

    [Header("Tile Sprites Info")]
    [SerializeField] private Sprite tile1;
    [SerializeField] private Sprite tile2;
    [SerializeField] private Sprite tile3;
    [SerializeField] private Sprite tile4;
    [SerializeField] private Sprite tile5;
    [SerializeField] private Sprite tile6;
    [SerializeField] private Sprite tile7;
    [SerializeField] private Sprite tile8;
    [SerializeField] private Sprite tileEmpty;
    [SerializeField] private Sprite tileUnknown;
    [SerializeField] private Sprite tileMine;
    [SerializeField] private Sprite tileExploded;
    [SerializeField] private Sprite tileFlag;

    [Header("Generating Tile Values Info")]
    [SerializeField] private int howManyMines = 12;

    [Header("Button Generation Info")]
    [SerializeField] private Sprite[] buttonSprites = new Sprite[2];
    [SerializeField] private ButtonTile.State[] buttonState = new ButtonTile.State[2];

    private void OnValidate()
    {
        howManyMines = Mathf.Clamp(howManyMines, 0, rows * cols);
    }

    private void Awake()
    {
        buttonSprites[0] = tile1;
        buttonSprites[1] = tileFlag;
    }

    private void Start()
    {
        cells = new Cell[rows, cols];

        GenerateTiles();
        GenerateMines();
        GenerateNumbers();
    }

    private void GenerateTiles()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector2 gridPosition = new(x * size, y * size);

                GameObject tileGameObject = Instantiate(tilePrefab, gridPosition, Quaternion.identity, transform);
                tileGameObject.transform.localScale = new Vector2(size, size);
                tileGameObject.name = $"Tile {x}, {y}";
                SpriteRenderer tileSpriteRendrer = tileGameObject.GetComponent<SpriteRenderer>();

                cells[x, y] = new Cell
                {
                    type = Cell.Type.Empty,
                    position = new Vector2Int(x, y),
                    tileSpriteRenderer = tileSpriteRendrer
                };

                tileSpriteRendrer.sprite = GetTileSprite(cells[x, y]);
            }
        }

        //Button Spawn
        for (int i = -1; i < 1; i++)
        {
            Vector2 position = new Vector3((((rows - 1) * size) / 2) + ((size + 0.1f) * i), (cols * size) + 0.2f);
            GameObject button = Instantiate(buttonTilePrefab, position, Quaternion.identity, transform);
            button.transform.localScale = new Vector2(size, size);
            SpriteRenderer tileSpriteRendrer = button.GetComponent<SpriteRenderer>();
            tileSpriteRendrer.sprite = buttonSprites[i + 1];
            ButtonTile buttonTile = button.GetComponent<ButtonTile>();
            buttonTile.state = buttonState[i + 1];
        }


        Vector3 camInMiddle = new((float)((rows - 1) * size / 2), (float)((cols - 1) * size / 2), -10);
        cam.transform.position = camInMiddle;

        //Camera resizing
        Vector2 lastCell = cells[rows - 1, cols - 1].position;
        lastCell *= size;
        cam.orthographicSize = (lastCell.y + 3) / 2;

        //Camera Fit the horizontal size
        Vector2 leftEdge = cam.ViewportToWorldPoint(Vector3.zero);
        while(leftEdge.x >= cells[0, 0].position.x - 0.5f)
        {
            cam.orthographicSize++;
            leftEdge = cam.ViewportToWorldPoint(Vector3.zero);
        }
    }

    private void GenerateMines()
    {
        for (int i = 0; i < howManyMines; i++)
        {
            int x = Random.Range(0, rows);
            int y = Random.Range(0, cols);

            while (cells[x, y].type == Cell.Type.Mine)
            {
                x++;

                if (x >= rows)
                {
                    x = 0;
                    y++;

                    if (y >= cols)
                    {
                        y = 0;
                    }
                }
            }

            Cell cell = cells[x, y];
            cell.type = Cell.Type.Mine;
        }
    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < rows; x++)
        {
            for (var y = 0; y < cols; y++)
            {
                Cell cell = cells[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    continue;
                }

                cell.number = CountMines(x, y);
                if (cell.number > 0)
                {
                    cell.type = Cell.Type.Number;
                }
            }
        }
    }

    private int CountMines(int cellX, int cellY)
    {
        int count = 0;

        for (int sideX = -1; sideX <= 1; sideX++)
        {
            for (int sideY = -1; sideY <= 1; sideY++)
            {
                if (sideX == 0 && sideY == 0) continue;

                int x = cellX + sideX;
                int y = cellY + sideY;

                if (GetCell(x, y).type == Cell.Type.Mine)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public Sprite GetTileSprite(Cell cell)
    {
        if (cell.revealed)
        {
            return GetRevealedSprite(cell);
        }
        else if (cell.flagged)
        {
            return tileFlag;
        }
        else
        {
            return tileUnknown;
        }
    }

    private Sprite GetRevealedSprite(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty: return tileEmpty;
            case Cell.Type.Mine: return tileMine;
            case Cell.Type.Number: return GetNumberSprite(cell);

            default: return null;
        }
    }

    private Sprite GetNumberSprite(Cell cell)
    {
        switch (cell.number)
        {
            case 1: return tile1;
            case 2: return tile2;
            case 3: return tile3;
            case 4: return tile4;
            case 5: return tile5;
            case 6: return tile6;
            case 7: return tile7;
            case 8: return tile8;

            default: return null;
        }
    }

    public Vector2Int WorldToGridPoint(Vector3 position)
    {
        return new Vector2Int((int)(position.x / size), (int)(position.y / size));
    }

    private Cell GetCell(int x, int y)
    {
        if (IsValid(x, y))
        {
            return cells[x, y];
        }
        else
        {
            return new Cell();
        }
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < rows && y >= 0 && y < cols;
    }

    public void ShowTiles(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Mine:
                Explode(cell);
                SoundManager.Instance.PlayEffect(SoundManager.Instance.wrongAudioClip);
                break;
            case Cell.Type.Empty:
                Flooding(cell);
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clickAudioClip);
                CheckForWin();
                break;
            default:
                cell.revealed = true;
                Sprite tileSprite = GetTileSprite(cell);
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clickAudioClip);
                cell.tileSpriteRenderer.sprite = tileSprite;
                CheckForWin();
                break;
        }
    }

    private void Explode(Cell cell)
    {
        cell.revealed = true;
        cell.exploded = true;
        Sprite tileSprite = GetTileSprite(cell);
        cell.tileSpriteRenderer.sprite = tileSprite;

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                cell = cells[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.revealed = true;
                    cell.exploded = true;
                    cell.tileSpriteRenderer.sprite = tileExploded;
                }
            }
        }

        GameManager.Instance.ChangeState(GameState.GameOver);
    }

    private void Flooding(Cell cell)
    {
        if (cell.revealed) return;
        if (cell.flagged) return;
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) return;

        cell.revealed = true;
        Sprite tileSprite = GetTileSprite(cell);
        cell.tileSpriteRenderer.sprite = tileSprite;

        if (cell.type == Cell.Type.Empty)
        {
            Flooding(GetCell(cell.position.x - 1, cell.position.y));
            Flooding(GetCell(cell.position.x + 1, cell.position.y));
            Flooding(GetCell(cell.position.x, cell.position.y - 1));
            Flooding(GetCell(cell.position.x, cell.position.y + 1));
        }
    }

    public void Flag(Cell cell)
    {
        if (cell.type == Cell.Type.Invalid) return;

        cell.flagged = !cell.flagged;
        Sprite tileSprite = GetTileSprite(cell);
        cell.tileSpriteRenderer.sprite = tileSprite;

        if (cell.flagged)
        {
            SoundManager.Instance.PlayEffect(SoundManager.Instance.flagAudioClip);
        }
        else
        {
            SoundManager.Instance.PlayEffect(SoundManager.Instance.unflagAudioClip);
        }
    }

    private void CheckForWin()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Cell cell = cells[x, y];

                if (cell.type != Cell.Type.Mine && !cell.revealed)
                    return;
            }
        }

        GameManager.instance.ChangeState(GameState.Victory);
    }
}

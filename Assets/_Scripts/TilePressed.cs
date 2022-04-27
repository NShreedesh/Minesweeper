using UnityEngine;

public class TilePressed : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private LayerMask cellButtonLayer;

    private Cell selectedCell;

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Play) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = board.cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInfoForTile = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, 1000, cellLayer);
            RaycastHit2D hitInfoForTileButton = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, 1000, cellButtonLayer);

            if (hitInfoForTile)
            {
                if (selectedCell != null)
                {
                    selectedCell.tileSpriteRenderer.color = Color.white;
                    selectedCell = null;
                }
                Vector2Int tileGridPosition = board.WorldToGridPoint(hitInfoForTile.transform.position);
                selectedCell = board.cells[tileGridPosition.x, tileGridPosition.y];
                selectedCell.tileSpriteRenderer.color = Color.grey;
            }

            else if (hitInfoForTileButton)
            {
                if (selectedCell == null) return;
                if (selectedCell.revealed) return;

                ButtonTile buttonTile = hitInfoForTileButton.transform.GetComponent<ButtonTile>();

                switch (buttonTile.state)
                {
                    case ButtonTile.State.Number:
                        board.ShowTiles(selectedCell);
                        break;
                    case ButtonTile.State.Flag:
                        board.Flag(selectedCell);
                        break;
                }

                selectedCell.tileSpriteRenderer.color = Color.white;
                selectedCell = null;
            }
        }
    }
}

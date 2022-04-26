using System;
using UnityEngine;

public class TilePressed : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private LayerMask cellLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.gameState == GameState.Play)
        {
            Vector3 mouseWorldPosition = board.cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, cellLayer);

            if (hitInfo)
            {
                Vector2Int tileGridPosition = board.WorldToGridPoint(hitInfo.transform.position);
                Cell selectedCell = board.cells[tileGridPosition.x, tileGridPosition.y];

                if(!selectedCell.revealed)
                {
                    board.ShowTiles(selectedCell);
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && GameManager.Instance.gameState == GameState.Play)
        {
            Vector3 mouseWorldPosition = board.cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, cellLayer);

            if (hitInfo)
            {
                Vector2Int tileGridPosition = board.WorldToGridPoint(hitInfo.transform.position);
                Cell selectedCell = board.cells[tileGridPosition.x, tileGridPosition.y];

                if (!selectedCell.revealed)
                {
                    board.Flag(selectedCell);
                }
            }
        }
    }
}

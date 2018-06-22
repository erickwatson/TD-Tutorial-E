using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; set; }

    public bool IsEmpty { get; private set; }

    private Color32 fullColor = new Color32(255, 118, 118, 255);

    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;

    public bool WalkAble { get; set; }

    public bool Debugging { get; set; }

    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        WalkAble = true;
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    private void OnMouseOver()
    {
        //ColorTile(Color.white);
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedButton != null)
        {
            if (IsEmpty && !Debugging)
            {
                ColorTile(emptyColor);
            }
            if (!IsEmpty && !Debugging)
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
        
    }

    private void OnMouseExit()
    {
        if (!Debugging)
        {
            ColorTile(Color.white);
        }

    }

    private void PlaceTower()
    {
        //Creates the tower
        GameObject tower = Instantiate(GameManager.Instance.ClickedButton.TowerPrefab, transform.position, Quaternion.identity);

        //Set the sorting layer order on the tower by the map height
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;

        //Sets the tiles as transform parent to the tower
        tower.transform.SetParent(transform);

        //Makes sure the tile doesn't have something in it already
        IsEmpty = false;

        //Sets the highlight back to white (or normal)
        ColorTile(Color.white);

        //Buys the tower
        GameManager.Instance.BuyTower();

        WalkAble = false;


    }

    // Sets the colour on the tile

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
	
}

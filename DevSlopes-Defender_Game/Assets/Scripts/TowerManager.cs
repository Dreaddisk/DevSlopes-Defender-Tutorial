using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerManager : Singleton<TowerManager>
{
    #region Variables
    public TowerButton towerBtnPressed { get; set; }
    public List<GameObject> TowerList = new List<GameObject>();
    public List<Collider2D> BuildList = new List<Collider2D>();

    private SpriteRenderer spriteRenderer;

    private Collider2D buildTile;

    #endregion

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider.tag == "BuildSite")
            {
                // hit.collider.tag = "BuildSiteFull";
                buildTile = hit.collider;
                buildTile.tag = "BuildSiteFull";
                RegisterBuildSite(buildTile);
                PlaceTower(hit);
            }
        }
        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RenameTagsBuildSites()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "BuildState";
        }
        BuildList.Clear();
    }

    public void RegisterTower(GameObject tower)
    {
        TowerList.Add(tower);
    }


    public void DestroyAllTowers()
    {
        foreach(GameObject tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            RegisterTower(newTower);
            BuyTower(towerBtnPressed.TowerPrice);
            DisableDragSprite();

        }
    }

    public void SelectedTower(TowerButton towerBtn)
    {
        if(towerBtn.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towerBtnPressed = towerBtn;
            EnableDragSprite(towerBtn.DragSprite);
        }
    }

    public void BuyTower(int price)
    {
        GameManager.Instance.SubstractMoney(price);
        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.BuildTower);
    }





    private void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void EnableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;

    }

    public void DisableDragSprite()
    {
        spriteRenderer.enabled = false;
        towerBtnPressed = null;
    }




} // TowerManager class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private GameObject towerObject;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private int towerPrice;
    [SerializeField] private int towerID;
    [SerializeField] private BuildManager buildManager;

    public Sprite DragSprite {get { return dragSprite;}}
    public int TowerPrice {  get { return towerPrice; } }
    public int TowerID { get { return towerID; } set { towerID = value; } }
    public GameObject TowerObject {  get { return towerObject; } }

    /// <summary>
    /// Aray�zde kuleye t�klamas�
    /// Clicking on the tower button in the UI
    /// </summary>
    public void OnMouseDownTrigger()
    {
        Debug.Log("mouseDown");
        buildManager.TowerSelected(this);
    }
    /// <summary>
    /// Aray�zde kuleyi s�r�klemesi
    /// Dragging on the tower button in the UI
    /// </summary>
    public void OnMouseDragTrigger()
    {
        Debug.Log("drag");
        buildManager.TowerDrag(this);

    }

    /// <summary>
    /// Aray�zde s�r�klemenin bitmesi
    /// End of dragging in the ui
    /// </summary>
    public void OnMouseUpTrigger()
    {
        buildManager.TowerDeSelected(this);  
    }
}

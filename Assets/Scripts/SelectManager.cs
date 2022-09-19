using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    public static SelectManager instance{get; private set;}
    [SerializeField] private Unit selectedObject;
    bool isPressed;
    public LayerMask mask;
    RaycastHit hit;
    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if(GameManager.instance.gameStage == GameStage.hazirlik)
        {
            Select();
        }
    }
    public void Select()
    {
        

        if(Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            if(selectedObject == null)
            {
                RaycastHit hit = CastRay(mask);
                if(hit.collider != null)
                {
                    
                    if(!hit.collider.CompareTag("Char"))
                    {
                        return;
                    }
                    selectedObject = hit.collider.gameObject.GetComponent<Unit>();
                }
            }
        }


        if(selectedObject != null && isPressed)
        {
            hit = CastRayChar();
            Vector3 position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.parent.position = new Vector3(worldPosition.x,.5f,worldPosition.z);
        }

        
            if(Input.GetMouseButtonUp(0))
            {   
                if(selectedObject == null)
                {
                    return;
                }
                if(!hit.collider.CompareTag("Ground"))
                {
                    HerosInsertionCancel(selectedObject.whichGrid);
                }
                else
                {
                    Grid hitGridController  = hit.collider.GetComponent<Grid>();

                    // uzerınde car olan grıde car koyarken
                    if(hit.collider.GetComponent<Grid>().heroOnGround != null)
                    {
                        if(hit.collider.GetComponent<Grid>() == selectedObject.whichGrid)
                        {
                            HerosInsertionCancel(selectedObject.whichGrid);
                        }
                       DoluYereHeroKoy(selectedObject.whichGrid,hitGridController);
                    }
                    else
                    {
                        // if(GameManager.instance.gameStage == GameStage.inGame)
                        // {
                        SahaIcıBosYereHeroKoy(hitGridController,selectedObject);
                            // selectedObject.transform.parent.position = selectedObject.GetComponent<Unit>().whichGrid.transform.position;
                            // selectedObject = null;
                            // isPressed = false;
                            // return;
                        // }
                        //sahaya adam koydugumuz yer
                    }
                selectedObject = null;
                isPressed = false;
                }
            }
    }
    public void HerosInsertionCancel(Grid grid)
    {
        selectedObject.transform.parent.position = grid.transform.position;
        selectedObject = null;
        isPressed = false;
        return;
    }
    public void DoluYereHeroKoy(Grid selectedComponent , Grid hitGridController)
    {
        GameObject tempFloor = hit.collider.gameObject;
        Unit tempHero = tempFloor.GetComponent<Grid>().heroOnGround;
        if(selectedObject.unitType == tempHero.GetComponent<Unit>().unitType)
        {
            Merge(tempHero);
            return;
        }
        selectedObject.transform.parent.position = hitGridController.heroOnGround.GetComponent<Unit>().whichGrid.transform.position;
        hitGridController.heroOnGround.transform.parent.position = selectedObject.whichGrid.transform.position;
        hitGridController.heroOnGround.GetComponent<Unit>().whichGrid = selectedObject.whichGrid;
        hitGridController.heroOnGround = selectedObject;
        selectedObject.whichGrid.GetComponent<Grid>().heroOnGround = tempHero; 
        selectedObject.whichGrid = tempFloor.GetComponent<Grid>();
        selectedObject = null;
    }
    public void Merge(Unit otherUnit)
    {
        if(selectedObject.nextLevelUnitPrefab == null)
        {
            HerosInsertionCancel(selectedObject.whichGrid);
        }
        var obj = Instantiate(selectedObject.nextLevelUnitPrefab,otherUnit.whichGrid.transform.position,Quaternion.identity);
        var unit = obj.transform.GetChild(0).GetComponent<Unit>();
        unit.whichGrid = otherUnit.whichGrid;
        selectedObject.whichGrid.heroOnGround = null;
        otherUnit.whichGrid.heroOnGround = unit;
        Destroy(selectedObject.transform.parent.gameObject);
        Destroy(otherUnit.transform.parent.gameObject);
    }
    private RaycastHit CastRay(LayerMask mask)
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear,worldMousePosFar - worldMousePosNear, out hit ,Mathf.Infinity,mask);

        return hit;
    }
    private RaycastHit CastRayChar()
    {
        Vector3 charPos = selectedObject.transform.parent.position;
        Vector3 dir = Vector3.down;
        RaycastHit hit;
        Physics.Raycast(charPos,dir, out hit ,Mathf.Infinity);

        return hit;
    }
    public void SahaIcıBosYereHeroKoy(Grid hitGrid , Unit selectedObject)
    {
        if(selectedObject.whichGrid != null)
            selectedObject.whichGrid.heroOnGround = null;  
        selectedObject.transform.parent.position = hitGrid.gameObject.transform.position;
        selectedObject.whichGrid = hitGrid;
        hitGrid.heroOnGround = selectedObject;
        return;
    }
}

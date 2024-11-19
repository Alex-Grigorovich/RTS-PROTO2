using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    public LayerMask clickable;
    
    public LayerMask ground;
    public GameObject groundMaker;
    private Camera cam;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // if we are hitting a clickable object;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else // if we are NOT hitting a clickable object;
            {
                if (Input.GetKey(KeyCode.LeftShift) == false)
                {
                    DeselectAll();
                }
                
                
                
            }
        }


        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // if we are hitting a clickable object;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMaker.transform.position = hit.point;
                groundMaker.SetActive(false);
                groundMaker.SetActive(true);
            }
        }

    }

    private void MultiSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            EnableUnitMovement(unit, false);
            SelectUnit(unit, false);
        }
    }

    public void DeselectAll()
    {
        
        foreach (var unit in unitsSelected)
        {
            SelectUnit(unit, false);
        }
        groundMaker.SetActive(false);
        unitsSelected.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        
        unitsSelected.Add(unit);
        SelectUnit(unit, true);
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
           unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }


    public void DragSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        
        TriggerSelectionIndicator(unit, isSelected);
        EnableUnitMovement(unit, isSelected);
        
    }
    
    
}

//using UnityEditor;

using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class InGameGUI : MonoBehaviour
{
    // GUI button Tween things
    public TweenPosition buildPanelTweener;
    private bool buildPanelOpen = false;
    public TweenRotation buildPanelArrowTweener;

    // Color changes for buttons
    public Color onColor;
    public Color offColor;
    public UISlicedSprite[] buildBtnGraphics;

    //Plane Materials
    private Material originalMat;
    public Material hoverMat;

    //turrets
    public GameObject[] structures;
    private int structureIndex;


    int layerMask = 1 << 8;
    private GameObject lastHitObj;

	void Start ()
	{
	    structureIndex = 0;
        updateGUI();
	    buildPanelOpen = false;
	}
	

	void Update () 
    {
        if (buildPanelOpen == true)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (lastHitObj)
                {
                    lastHitObj.renderer.material = originalMat;
                }

                lastHitObj = hit.collider.gameObject;
                originalMat = lastHitObj.renderer.material;
                lastHitObj.renderer.material = hoverMat;
            }
        }
        else
        {
            if (lastHitObj)
            {
                lastHitObj.renderer.material = originalMat;
                lastHitObj = null;
            }
        }

	    if (Input.GetMouseButtonDown(0) && lastHitObj)
	    {
	        if (lastHitObj.tag == "PlacementPlane_Open")
	        {
	            int rotationY = Random.Range(0, 360);

	            GameObject newStructure = Instantiate(structures[structureIndex], lastHitObj.transform.position, lastHitObj.transform.rotation) as GameObject;
                newStructure.transform.eulerAngles = new Vector3(0, rotationY, 0);
	            lastHitObj.tag = "PlacementPlane_Taken";
	        }
	    }
	}


    void toggleBuildPanel()
    {
        if (buildPanelOpen == false)
        {
            buildPanelTweener.Play(true);
            buildPanelArrowTweener.Play(true);
            buildPanelOpen = true;
        }
        else
        {
            buildPanelTweener.Play(false);
            buildPanelArrowTweener.Play(false);
            buildPanelOpen = false;
        }
    }

    void SetBuildChoice(GameObject btnObj)
    {
        string btnName = btnObj.name;

        if (btnName == "Btn_Cannon")
            structureIndex = 0;
        if (btnName == "Btn_Missile")
            structureIndex = 1;
        if (btnName == "Btn_Mine")
            structureIndex = 2;

        updateGUI();
    }



    void updateGUI()
    {
        foreach (var btn in buildBtnGraphics)
        {
            btn.color = offColor;
        }
        
        buildBtnGraphics[structureIndex].color = onColor;
        //Debug.Log(onColor);
       // Debug.Log(buildBtnGraphics[structureIndex].color);
    }
}



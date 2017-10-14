using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Map map;
    public Tile tile;
    public int size = 9;
    public int tilesize = 120;
    public GameObject frame;
    public GameObject frameN;

    Tile selected;
    int selectionCount;
    List<Tile> selectedNeigh = new List<Tile>();


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ///for touch screen
        //if(Input.touchCount > 0)
        //{
        //    Vector2 touchPos = Input.GetTouch(0).position;
        //    RaycastHit2D selected = Physics2D.Raycast(touchPos, Vector2.zero);
        //}


        /// for mouse control

		map.MapStateUpdate ();
		if (Input.GetMouseButtonUp(0) & map.State ==0)
        {
			play ();
        }           
    }

	void play(){

		//Vector3 worldClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Vector2 pos = new Vector2(worldClick.x, worldClick.y);
        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit hit;
        bool hitTrue = Physics.Raycast(ray, Vector3.forward, out hit);
		Tile t;
		if (hitTrue) {t = hit.collider.GetComponent<Tile>();
			if (selectionCount < 1)
			{
				selected = t;
				GameObject fr = Instantiate(frame, t.transform.position, Quaternion.identity);
				selectedNeigh = map.TileFindAllNeigh(t);
				selectionCount = 1;
			}
			else
			{
				if (selectedNeigh.Contains(t))
				{
					map.Elimination(selected, t);			
					selectedNeigh.Clear();
					selectionCount = 0;
					GameObject fr = GameObject.FindGameObjectWithTag("Frame");
					Destroy(fr);
				}
				else
				{
					GameObject fr = GameObject.FindGameObjectWithTag("Frame");
					Destroy(fr);
					selected = t;
					Instantiate(frame, t.transform.position, Quaternion.identity);
					selectedNeigh = map.TileFindAllNeigh(selected);
					selectionCount = 1;
				}
			}
		}
		else { print("not tile Selected"); }
	}
}




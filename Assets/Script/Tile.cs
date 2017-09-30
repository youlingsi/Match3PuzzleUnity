using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public GameManager GM;
    public Map map;
    public Sprite[] sprites;
    public int hp = 1;
    public int val;
    public int tCoIndex;
    public int tTiIndex;



	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}


    // randomly assign the sprite
    public void RandTile(List<int> pool)
    {
        int indx = Random.Range(0, pool.Count);
        this.val = pool[indx];
        SpUpdate();
    }

    // update the sprite based on the value
    public void SpUpdate(){
        this.GetComponent<SpriteRenderer>().sprite = sprites[val];
    }

    // destroy this game object
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }



    public void HpDeduct(){
        hp -= 1;
        if (hp == 0){
            SelfDestroy();
        }
    }






}

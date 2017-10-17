using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public GameManager GM;
    public Map map;
    public Material[] mate;
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

    // update the material based on the value
    public void SpUpdate(){
        //this.GetComponent<SpriteRenderer>().sprite = sprites[val];
        this.GetComponent<MeshRenderer>().material = mate[val];
    }

    // destroy this game object

    public void HpDeduct() {
        hp -= 1;
        if (hp == 0) {
            StartCoroutine(SelfDestroy());
        }
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        try
        {
            Vector3 c = new Vector3(0, 0, -200);
            this.transform.position = c;
            Destroy(gameObject);
        }
        catch (System.NullReferenceException) { }
    }



}

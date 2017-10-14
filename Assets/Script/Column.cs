using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour {

	public GameManager GM;
    public Map map;
	public int coIndex;
	public List<Tile> TileList;
    public int TileNum;
	public int SpawnerState; // 0-idle; 1 = spawning

	// Use this for initialization
	void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
	{
		int count = TileList.Count;
		if (count < TileNum) {
			SpawnerState = 1;
			float yTopBottom = this.transform.position.y - 50;
			float yTileTop = TileList [TileList.Count - 1].transform.position.y + 60;
			if (yTileTop < yTopBottom) {
				Spawn ();
			}
		} else {
			SpawnerState = 0;
		}
	}

    // spawn a tile and add it to the column list
	void Spawn()
	{
        Tile t = Instantiate(map.tile,this.transform);
		TileList.Add(t);
		List<int> pool = new List<int>();
		for (int i = 1; i < map.tile.mate.Length; i++)
		{
			pool.Add(i);
		}
		t.RandTile(pool);
		t.tCoIndex = coIndex;
		UpdateIndex();
		if(TileList.Count == TileNum){
  //          StartCoroutine(MatchElim());
			map.MapMatchElim ();
		}
	}
	
    // Update the TiIndex of each tile in the tilelist
    void UpdateIndex()
    {
        foreach(Tile t in TileList)
        {
            t.tTiIndex = TileList.IndexOf(t);
        }
    }

}

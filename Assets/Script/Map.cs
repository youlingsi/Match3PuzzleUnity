using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Map : MonoBehaviour
{
    public GameManager GM;
    public Tile tile;
    public Column[] top;
	public int State;



    // Use this for initialization
    void Start()
    {
        tile = GM.tile;
        MapInitiate();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void TileSwap(Tile T1, Tile T2)
    {
        int val1 = T1.val;
        T1.val = T2.val;
        T2.val = val1;
        T1.SpUpdate();
        T2.SpUpdate();     
    }

    // check if the two given tile are the same
    private bool TileCheck(Tile T1, Tile T2) {
        try {
            bool result = (T1.val == T2.val);
            return result;
        }
        catch (System.NullReferenceException) {
            return false;
        }
    }
    
    private Tile TileFindNei(Tile c, int[] tCod)
    {
        int[] cod = { c.tCoIndex + tCod[0], c.tTiIndex + tCod[1] };
        return TileFindTile(cod);
    }

    private Tile TileFindTile(int[] tcod)
    {
        if (tcod[0] >= 0 & tcod[1] >= 0)
        {
            try
            {
                return top[tcod[0]].TileList[tcod[1]];
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //print("invaild tile coordinate");
                return null;
            }
            catch (System.IndexOutOfRangeException)
            {
                //print("invaild tile coordinate");
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    private void TileKill(Tile t)
    {

        try {
			Vector3 c = new Vector3(0,0,-20);
            int tCoIndex = t.tCoIndex;
            top[tCoIndex].TileList.Remove(t);
			t.transform.position = c;
            t.HpDeduct();
        }
        catch (System.NullReferenceException) { }
    }

    // create initial map Alternat
    private void MapInitiate()
    {
        int[][] xCheck = { new int[] { -1, 0 }, new int[] { -2, 0 } };
        int[][] yCheck = { new int[] { 0, -1 }, new int[] { 0, -2 } };
        for (int i = 0; i < top.Count(); i++)
        {
            top[i].coIndex = i;
            for (int j = 0; j < GM.size; j++)
            {
                Vector3 po = top[i].transform.position;
                po.y = transform.position.y + GM.tilesize + j * GM.tilesize;
                Tile c = Instantiate(tile, po, Quaternion.identity);
                c.transform.parent = top[i].transform;
                c.tCoIndex = i; c.tTiIndex = j;
                top[i].TileList.Add(c);
                List<int> vElim = new List<int>();
                List<int> pool = new List<int>();
                Tile check = TileFindNei(c, xCheck[0]);
                bool xMatch = TileCheck(check, TileFindNei(c, xCheck[1]));
                if (xMatch) { vElim.Add(check.val); }
                check = TileFindNei(c, yCheck[0]);
                bool yMatch = TileCheck(TileFindNei(c, yCheck[0]), TileFindNei(c, yCheck[1]));
                if (yMatch) { vElim.Add(check.val); }
                for (int n = 1; n < c.sprites.Length; n++) {
                    if (!vElim.Contains(n)) { pool.Add(n); }
                }
                c.RandTile(pool);
            }
        }
		if (!MapCheck()) {
			MapInitiate ();
		}
    }

    //Check if there are any match available in the map. True = Match available, False = Match not available
    private bool MapCheck()
    {
        int[][] smpArr = {
            new int[] { 0, 1 },
            new int[] { 0, 2 },
            new int[] { 1, 0 },
            new int[] { 1, 1 },
            new int[] { 1, 2 },
        };
        for (int i = 0; i < top.Length; i++) {
            for (int j = 0; j < top[i].TileList.Count(); j++) {
                List<Tile> smp = new List<Tile> { top[i].TileList[j] };
                int[] tSort = new int[top[i].TileList[j].sprites.Length];
                try
                {
                    foreach (int[] arr in smpArr)
                    {
                        smp.Add(top[i + arr[0]].TileList[j + arr[1]]);
                    }
                }
                catch (System.ArgumentOutOfRangeException) { continue; }
                foreach (Tile t in smp) {
                    tSort[t.val]++;
                }
                foreach (int n in tSort) {
                    if (n > 2) { return true; }
                }
            }
        }
        return false;
    }

    private void MapClear() {
        for (int i = 0; i < top.Length; i++) {
            for (int j = 0; j < top[i].TileList.Count(); j++) {
                Destroy(top[i].TileList[j].gameObject);
            }
            top[i].TileList.Clear();
        }
    }




    //return a list of eliminatable tiles around  the tile T in X or Y direction. exclude tile T;
    private List<Tile> ElimList(Tile T, string dir)
    {
        int[][] posArr = new int[2][];
        if (dir == "x" | dir == "X") { posArr = new int[][] { new int[] { -1, 0 }, new int[] { 1, 0 } }; }
        if (dir == "y" | dir == "Y") { posArr = new int[][] { new int[] { 0, 1 }, new int[] { 0, -1 } }; }

        List<Tile> tList = new List<Tile>();
        foreach (int[] pos in posArr)
        {
            List<Tile> t = MatchSearch(T, pos);
            try
            {
                tList.AddRange(t.GetRange(1, t.Count - 1));
            }
            catch (System.ArgumentException) { }
        }
        if (tList.Count < 2) { tList.Clear(); }

        return tList;
    }

    //Check if there are any match available towards one direction of the tile, including the tile itself.
    private List<Tile> MatchSearch(Tile T, int[] pos)
    {
        List<Tile> tList = new List<Tile>() { T };
        Tile tNei = TileFindNei(T, pos);
        try
        {
            if (tNei.val == T.val)
            {
                tList.AddRange(MatchSearch(tNei, pos));
                return tList;
            }
            else
            {
                return tList;
            }
        }
        catch (System.NullReferenceException)
        {
            return tList;
        }
    }

    private List<Tile> ElimListAll(Tile T)
    {
        List<Tile> temp = new List<Tile>();
        List<Tile> tList = new List<Tile>();
        temp.AddRange(ElimList(T, "x"));
        temp.AddRange(ElimList(T, "y"));
        if (temp.Count > 0) { tList.AddRange(temp); tList.Add(T); temp.Clear(); }
        return tList;

    }
    //kill the tiles in the eilmination list
	public void Elimination(Tile T1, Tile T2) {
        List<Tile> tList = new List<Tile>();
        TileSwap(T1, T2);
        try
        {
            tList.AddRange(ElimListAll(T1));
            tList.AddRange(ElimListAll(T2));
        }
        catch (System.NullReferenceException) { print(tList.Count); }
        if(tList.Count != 0)
        {
            foreach (Tile t in tList)
            {
                TileKill(t);
            }
        }
        else
        {
            TileSwap(T1, T2);
        }
     
    }

	private void MapElimination()
	{
		if (MapCheck()) {
			print ("checkTrue");
			List<Tile> tList = new List<Tile> ();
			for (int i = 0; i < top.Length; i++) {
				for (int j = 0; j < top [i].TileNum; j++) {
					Tile t = top [i].TileList [j];
					if (!tList.Contains (t)) {
						try {
							tList.AddRange (ElimListAll (t));
						} catch (System.NullReferenceException) {
							continue;
						}
					} else {
						continue;
					}
				}
			}
			foreach (Tile t in tList) {
				TileKill (t);
			}
		} 
		else {
			MapClear ();
			MapInitiate ();

		}

	}

	public List<Tile> TileFindAllNeigh(Tile T)
    {
		
        int[][] posArr = { new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 } };
        List<Tile> tList = new List<Tile>();
        foreach (int[] pos in posArr)
        {
			try{
                tList.Add(TileFindNei(T, pos));
            }
            catch (System.NullReferenceException) { }
        }
        return tList;
    }

	public void MapMatchElim(){
		MapStateUpdate ();
		if (State == 0) {
			MapElimination ();
		} else {
			print ("False");
		}
				
	}

	public void MapStateUpdate(){
		State = 0;
		foreach (Column co in top) {
			if(co.TileList.Count < co.TileNum){State++;}
		}
	}
		

}

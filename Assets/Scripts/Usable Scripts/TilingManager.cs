/* ""Mandeep Yadav""
 * 
 * This script is used to make a tile floor on which we can define how the player will be moving and
 * also we can define the distance and placement of the environment
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lean;

// lean is the reference to the leanpool tool which is used to pooling the object


public class TilingManager : Singleton<TilingManager>
{
	public List<TileSprite> TileSprites;
	public Vector2 MapSize;
	public Sprite DefaultImage;
	public GameObject TileContainerPrefab;
	public GameObject TilePrefab;
	public Vector2 CurrentPosition;
	public Vector2 ViewPortSize;

	[HideInInspector]
	public GameObject[,] _mapObjects;
	private TileSprite[,] _map;
//	private GameObject controller;
	private GameObject _tilecontainer;
	//[HideInInspector]
	public List<GameObject> _tiles = new List<GameObject> ();

	void Awake ()
	{
		this.Reload ();
	}


	void Start ()
	{
//		controller = GameObject.Find ("Controller");
		_map = new TileSprite[(int)MapSize.x, (int)MapSize.y];
		_mapObjects = new GameObject[(int)MapSize.x, (int)MapSize.y];

		DefaultTiles ();
		SetTiles ();
		AddTilesToWorld ();
		SetRank ();

	}

	//set the default tiles in the view
	void DefaultTiles ()
	{
		for (int y = 0; y < MapSize.y; y++) {
			for (int x = 0; x < MapSize.x; x++) {
				_map [x, y] = new TileSprite ("unset", DefaultImage, Tiles.Unset);
			}
		}
	}


	// set the tiles properties

	void SetTiles ()
	{
		var index = 0;
		for (int y = 0; y < MapSize.y; y++) {
			for (int x = 0; x < MapSize.x; x++) {
				_map [x, y] = new TileSprite (TileSprites [index].Name, TileSprites [index].TileImage, TileSprites [index].TileType);
				index++;
				if (index > TileSprites.Count - 1)
					index = 0;
				
			}
		}
	}

	void SetRank ()
	{
		var index = 0;
		for (int y = 0; y < MapSize.y; y++) {
			for (int x = 0; x < MapSize.x; x++) {
				_mapObjects [x, y] = _tiles [index];
				_mapObjects [x, y].GetComponent<WayPoint> ().row_number = x;
				_mapObjects [x, y].GetComponent<WayPoint> ().column_number = y;
				index++;
			}
		}
	}



	//adding the tiles to the world also managing with the camera view
	void AddTilesToWorld ()
	{
		foreach (GameObject obj in _tiles) {
			LeanPool.Despawn (obj);
		}

		_tiles.Clear ();

		LeanPool.Despawn (_tilecontainer);
		_tilecontainer = LeanPool.Spawn (TileContainerPrefab);
		_tilecontainer.name = "grid";
		_tilecontainer.transform.localScale = new Vector3 (MapSize.x, MapSize.y, 1);
		_tilecontainer.transform.eulerAngles = new Vector3 (0, 0, 45);
		var tilesize = 1f;
		var viewoffsetX = ViewPortSize.x;
		var viewoffsetY = ViewPortSize.y;


		for (var y = 0; y < viewoffsetY; y++) {
			for (var x = 0; x < viewoffsetX; x++) {
				var tX = x * tilesize;
				var tY = y * tilesize;
		
				var iX = x + CurrentPosition.x;
				var iY = y + CurrentPosition.y;

				if (iX < 0)
					continue;
				if (iY < 0)
					continue;


				var t = LeanPool.Spawn (TilePrefab);
				t.transform.SetParent (_tilecontainer.transform, true);
				t.transform.position = new Vector3 (tX, tY, 0);
				var renderer = t.GetComponent<SpriteRenderer> ();
				renderer.sprite = _map [(int)x + (int)CurrentPosition.x, (int)y + (int)CurrentPosition.y].TileImage;
				_tiles.Add (t);

			}
		}

		_tilecontainer.transform.position = new Vector3 (-MapSize.x / 2, -MapSize.y / 2, 0);

			
	}


	//for finding a certain type of tiles
	private TileSprite FindTile (Tiles tile)
	{
		foreach (TileSprite tilesprite in TileSprites) {
			if (tilesprite.TileType == tile)
				return tilesprite;
		}
		return null;

	}

}

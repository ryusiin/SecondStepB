using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Holder_Map : MonoBehaviour
{
    // :: Find
    public Dictionary<Vector3, GameObject> DictTiles { get; private set; }
    public Dictionary<Vector3, GameObject> DictTiles_Red { get; private set; }
    public Dictionary<Vector3, GameObject> DictTiles_Blue { get; private set; }
    public Dictionary<Vector3, GameObject> DictAllTiles { get; private set; }

    // :: Initialise
    public void Init()
    {
        // :: for Use
        this.DictTiles = new Dictionary<Vector3, GameObject>();
        this.DictAllTiles = new Dictionary<Vector3, GameObject>();
        this.DictTiles_Red = new Dictionary<Vector3, GameObject>();
        this.DictTiles_Blue = new Dictionary<Vector3, GameObject>();

        // :: Get
        this.FindTiles();

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // :: Find Tiles
    private void FindTiles()
    {
        foreach(Transform child in this.transform)
        {
            // :: Get
            string[] coordinateString = child.gameObject.name.Replace("Tile", "").Replace("[", "").Replace("]", " ").Split(' ');
            int x = int.Parse(coordinateString[0]);
            int y = int.Parse(coordinateString[1]);
            int z = int.Parse(coordinateString[2]);
            Vector3 coordinate = new Vector3(x, y, z);

            // :: set
            this.DictAllTiles.Add(coordinate, child.gameObject);
            if(child.gameObject.activeSelf == true)
            {
                this.DictTiles.Add(coordinate, child.gameObject);

                // :: Divide
                if (child.GetComponent<InHeist_Leader_Tile>().eTeam == EnumAll.eTeam.RED)
                    this.DictTiles_Red.Add(coordinate, child.gameObject);
                else if (child.GetComponent<InHeist_Leader_Tile>().eTeam == EnumAll.eTeam.BLUE)
                    this.DictTiles_Blue.Add(coordinate, child.gameObject);
            }

            // :: Init
            child.GetComponent<InHeist_Leader_Tile>().Init(coordinate);
        }
    }
}

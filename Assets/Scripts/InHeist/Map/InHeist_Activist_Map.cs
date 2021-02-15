using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Activist_Map : MonoBehaviour
{
    // :: Holder
    private InHeist_Holder_Map MAPHolder;

    // :: Initialise
    public void Init()
    {
        // :: Holder
        this.MAPHolder = this.GetComponent<InHeist_Holder_Map>();
        this.MAPHolder.Init();

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // :: Get
    public List<InHeist_Leader_Character> GetTeam(EnumAll.eTeam eTeam)
    {
        // :: for Use
        List<InHeist_Leader_Character> listCharacters = new List<InHeist_Leader_Character>();

        // :: Get
        foreach (var itm in this.MAPHolder.DictTiles.Values)
        {
            var tile = itm.GetComponent<InHeist_Leader_Tile>();
            
            if(tile.GetStatus_isEmpty() == false)
            {
                var character = tile.GetCharacter();
                if(character.GetTeamColor() == eTeam)
                {
                    listCharacters.Add(character);
                }
            }
        }
        // :: Return
        return listCharacters;
    }
    public List<InHeist_Leader_Tile> GetEmptyTile()
    {
        // :: for Use
        List<InHeist_Leader_Tile> listTiles = new List<InHeist_Leader_Tile>();

        // :: Get
        foreach (var itm in this.MAPHolder.DictTiles.Values)
        {
            var tile = itm.GetComponent<InHeist_Leader_Tile>();

            if(tile.GetStatus_isEmpty() == true)
            {
                listTiles.Add(tile);
            }
        }

        // :: Return
        return listTiles;
    }
    public List<InHeist_Leader_Tile> GetAllTile()
    {
        // :: for Use
        List<InHeist_Leader_Tile> listTiles = new List<InHeist_Leader_Tile>();

        // :: Get
        foreach (var itm in this.MAPHolder.DictAllTiles.Values)
        {
            var tile = itm.GetComponent<InHeist_Leader_Tile>();
            listTiles.Add(tile);
        }

        // :: Return
        return listTiles;
    }
    public InHeist_Leader_Tile GetTile(Vector3 tileKey)
    {
        if (this.MAPHolder.DictAllTiles.ContainsKey(tileKey) == false)
            return null;

        return this.MAPHolder.DictAllTiles[tileKey].GetComponent<InHeist_Leader_Tile>();
    }

    // : Show
    public void Show_AllHexImage(bool check, EnumAll.eTeam eTeam = EnumAll.eTeam.YELLOW)
    {
        switch(eTeam)
        {
            case EnumAll.eTeam.RED:
                {
                    foreach (var tile in this.MAPHolder.DictTiles_Red)
                        tile.Value.GetComponent<InHeist_Leader_Tile>().Show_HexImage(check);
                }
                break;
            case EnumAll.eTeam.BLUE:
                {
                    foreach (var tile in this.MAPHolder.DictTiles_Blue)
                        tile.Value.GetComponent<InHeist_Leader_Tile>().Show_HexImage(check);
                }
                break;
            default:
                {
                    foreach (var tile in this.MAPHolder.DictAllTiles)
                        tile.Value.GetComponent<InHeist_Leader_Tile>().Show_HexImage(check);
                }break;
        }
    }
}

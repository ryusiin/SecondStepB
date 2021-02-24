using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InHeist_UIChief : MonoBehaviour
{
    // :: Holder
    private InHeist_UIHolder UIHolder;

    // :: Initialise
    public void Init()
    {
        // :: Holder
        this.UIHolder = this.GetComponent<InHeist_UIHolder>();
        this.UIHolder.Init();

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // : Add Button Listener
    public void AddButtonListener_StartBattle(System.Action action)
    {
        this.UIHolder.BUTTNON_startBattle.GetComponent<Button>().onClick.AddListener(() => { action?.Invoke(); });
    }

    // : Show
    public void ShowButton_StartBattle(bool check)
    {
        this.UIHolder.BUTTNON_startBattle.SetActive(check);
    }
    public void ShowImage_CostAll(bool check)
    {
        this.GetImage_Cost(EnumAll.eTeam.RED).gameObject.SetActive(check);
        this.GetImage_Cost(EnumAll.eTeam.BLUE).gameObject.SetActive(check);
    }

    // : Get
    public Image GetImage_Dim()
    {
        return this.UIHolder.IMAGE_dim.GetComponent<Image>();
    }
    public Image GetImage_Cost(EnumAll.eTeam eTeam)
    {
        if (eTeam == EnumAll.eTeam.RED)
            return this.UIHolder.IMAGE_costRed.GetComponent<Image>();

        if (eTeam == EnumAll.eTeam.BLUE)
            return this.UIHolder.IMAGE_costBlue.GetComponent<Image>();

        return null;
    }

    // : Set
    public void SetText_RemainingTime(string text)
    {
        this.UIHolder.TEXT_remainingTime.GetComponent<Text>().text = text;
    }
    public void SetTeam(EnumAll.eTeam eTeam, string nickname)
    {
        // :: Cost
        this.ShowImage_CostAll(false);
        this.GetImage_Cost(eTeam).gameObject.SetActive(true);

        // :: Team and Nickname
        this.UIHolder.IMAGE_team.GetComponent<Image>().color = Dictator.GetColor(eTeam);
        this.UIHolder.TEXT_nickname.GetComponent<Text>().color = Dictator.GetColor(eTeam);
        this.UIHolder.TEXT_nickname.GetComponent<Text>().text = nickname;
    }
    public void SetText_Cost(EnumAll.eTeam eTeam, int curCost, int maxCost)
    {
        string cost = string.Format("{0}/{1}", curCost, maxCost);
        if (eTeam == EnumAll.eTeam.BLUE)
            this.UIHolder.TEXT_costBlue.GetComponent<Text>().text = cost;
        else if (eTeam == EnumAll.eTeam.RED)
            this.UIHolder.TEXT_costRed.GetComponent<Text>().text = cost;
    }
}

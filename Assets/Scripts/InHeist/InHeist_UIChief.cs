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

    // : Get
    public Image GetImage_Dim()
    {
        return this.UIHolder.IMAGE_dim.GetComponent<Image>();
    }

    // : Set
    public void SetText_RemainingTime(string text)
    {
        this.UIHolder.TEXT_remainingTime.GetComponent<Text>().text = text;
    }
    public void SetTeam(EnumAll.eTeam eTeam, string nickname)
    {
        this.UIHolder.IMAGE_team.GetComponent<Image>().color = Dictator.GetColor(eTeam);
        this.UIHolder.TEXT_nickname.GetComponent<Text>().color = Dictator.GetColor(eTeam);
        this.UIHolder.TEXT_nickname.GetComponent<Text>().text = nickname;
    }
}

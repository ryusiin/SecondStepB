using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InHeist_Lobby_UIChief : MonoBehaviour
{
    // : Holder
    private InHeist_Lobby_UIHolder UIHolder;

    // : Initialise
    public void Init()
    {
        // :: Holder
        this.UIHolder = this.GetComponent<InHeist_Lobby_UIHolder>();
        this.UIHolder.Init();

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // : Change
    public void ChangeColor_Team(EnumAll.eTeam eTeam)
    {
        this.UIHolder.Image_team.GetComponent<Image>().color = Dictator.GetColor(eTeam);
        this.UIHolder.Text_nickname.GetComponent<Text>().color = Dictator.GetColor(eTeam);
    }
    public void ChangeText_Nickname(string nickname)
    {
        this.UIHolder.Text_nickname.GetComponent<Text>().text = nickname;
    }

    // : Show
    public void Show_Flag(EnumAll.eTeam eTeam, bool check)
    {
        if(eTeam == EnumAll.eTeam.RED)
            this.UIHolder.Image_flagRed.SetActive(check);
        else if(eTeam == EnumAll.eTeam.BLUE)
            this.UIHolder.Image_flagBlue.SetActive(check);
    }

    // : Able
    public void AbleButton_Heist(bool check)
    {
        this.UIHolder.Button_heist.GetComponent<Button>().interactable = check;
    }

    // : Button Listener
    public void AddButtonListener_Heist(System.Action action)
    {
        this.UIHolder.Button_heist.GetComponent<Button>().onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }

    // : Get
    public Image GetImage_Dim()
    {
        return this.UIHolder.IMAGE_dim.GetComponent<Image>();
    }
}

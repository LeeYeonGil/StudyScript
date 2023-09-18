using DTT.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    public static PlayerUI _playerUi;
    public RPGPlayer player;
    public CharacterStat charStat;

    public Slider myHpBar;
    public Slider myMpBar;
    public Slider myExpBar;

    public TMP_Text HPtext;
    public TMP_Text MPtext;
    public TMP_Text EXPtext;
    public TMP_Text totalHPtext;
    public TMP_Text totalMPtext;
    public TMP_Text totalEXPtext;

    public TMP_Text AP;
    public TMP_Text DP;
    public TMP_Text AS;
    public TMP_Text MS;

    public TMP_Text Lv;

    public Inventory inventory;
    public Equipment equipment;
    public item_ToolTip tooltip;

    public Image EXP;
    public TMP_Text EXP_Text;

    public TMP_Text Gold_Text;

    // Start is called before the first frame update
    void Start()
    {
        Exp_hide();
        EquipSet();
        Stat_Set();
    }

    // Update is called once per frame
    void Update()
    {
        Stat_Set(); // 이벤트 발생시에 발동
    }

    public void Gold_Set() // 골드 변동 있을 경우 사용
    {
        UIManager.Instance.Gold = GameManager.Instance.Gold;
        int Gold = UIManager.Instance.Gold;
        string gold_text = Gold.ToString();

        int Gold_Length = gold_text.Length;
        int i = Gold_Length % 3; // 2
        int j = Gold_Length / 3; // 2
        for (int k = 0; k < j; k++) // 0~1
        {
            if ((k * 3 + i) != 0)
            {
                gold_text = gold_text.Insert(k * 3 + i++, ",");
            }
        }
        
        Gold_Text.text = gold_text; 
    }

    public void EquipSet()
    {
        equipment.Equipment_Set();
    }

    public void Exp_show()
    {
        EXP_Text.gameObject.transform.parent.gameObject.SetActive(true);
        EXP_Text.gameObject.transform.SetAsLastSibling();
        EXP_Text.text = $"{charStat.Exp} / {charStat.MaxExp}";
    }
    public void Exp_hide()
    {
        EXP_Text.gameObject.transform.parent.gameObject.SetActive(false);
    }


    public void Stat_Set()
    {
        charStat = player.myStat;
        HPtext.text = charStat.HP.ToString("0");
        MPtext.text = charStat.MP.ToString("0");
        EXPtext.text = charStat.Exp.ToString("0");
        totalHPtext.text = charStat.MaxHP.ToString("0");
        totalMPtext.text = charStat.MaxMP.ToString("0");
        /*totalEXPtext.text = charStat.MaxExp.ToString("0");*/

        myHpBar.value = charStat.HP / charStat.MaxHP;
        myMpBar.value = charStat.MP / charStat.MaxMP;
        /*myExpBar.value = charStat.Exp / charStat.MaxExp;*/
        EXP_Text.text = $"{charStat.Exp} / {charStat.MaxExp}";
        EXP.fillAmount = charStat.Exp / charStat.MaxExp;

        Lv.text = charStat.Lv.ToString();

        AP.text = charStat.AP.ToString("0.0");
        DP.text = charStat.DP.ToString("0.0");
        AS.text = charStat.AttackSpeed.ToString("0.0");
        MS.text = charStat.MoveSpeed.ToString("0.0");
    }
}

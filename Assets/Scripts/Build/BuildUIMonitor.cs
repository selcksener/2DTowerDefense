using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuildClickType
{
    None,Buildable,Upgradable
}
public class BuildUIMonitor : MonoBehaviour
{
    public static Action<BuildClickType,GameObject> BuildState;// �n�a sisteminin durumu // Build status

    [SerializeField] private GameObject buildablePanel;// �n�a aray�z� // Build ui
    [SerializeField] private GameObject upgradablePanel;// Y�kseltme aray�z� // upgrade ui
    [SerializeField] private Tower selectedUpgradeTower; // Se�ilen kule bilgisi // selected tower info

    [Space(10)]
    [Header("UpgradeUIPanel")]
    [SerializeField] private Image upgradeTowerImage;//Y�kseltme aray�z�nde se�ilen kule g�rseli // image in the upgrade ui
    [SerializeField] private GameObject upgradeUIButton;// Y�kseltme butonu // upgrade button
    [SerializeField] private GameObject upgradeUIMaxButton; // Y�kseltme aray�z�nde maksimum y�kseltme butonu // max upgrade button in the uprgrade ui
    [SerializeField] private Text upgradeTowerCurrentLevelText; // Y�kseltme aray�z�nde g�ncel kule level bilgisi // current tower level in the upgrade ui
    [SerializeField] private Text upgradePriceText;// Y�kseltme fiyat� bilgisi // upgrade price
    [SerializeField] private List<GameObject> upgradeUIDamage= new List<GameObject>();// Y�kseltme hasar ui'�
    [SerializeField] private List<GameObject> upgradeUIRange= new List<GameObject>();// Y�kseltme mensiz ui'�
    [SerializeField] private List<GameObject> upgradeUITime= new List<GameObject>();// Y�kseltme at�� aral��� ui'�

    [SerializeField] private UpgradeBaseData towerUpgradeData ;// y�kseltme bilgilerinin sakland��� de�i�ken // upgrade info
    public UpgradeBaseData TowerUpgradeData { get { return towerUpgradeData; } }


    private void Awake()
    {
        BuildState -= BuildStateEventListener;
        BuildState += BuildStateEventListener;
    }
    /// <summary>
    /// Build status listener
    /// </summary>
    /// <param name="state">�n�a durumu State</param>
    /// <param name="tower">�n�a objesi tower</param>
    private void BuildStateEventListener(BuildClickType state,GameObject tower)
    {
        switch(state)
        {
            case BuildClickType.None:// Bo� alana t�klad���nda in�a ile ilgili k�s�mlar kapat�l�yor // when click on the empty area, close ui and towers range
                buildablePanel.SetActive(false);
                upgradablePanel.SetActive(false);
                if (selectedUpgradeTower != null)
                    selectedUpgradeTower.rangeObject.GetComponent<SpriteRenderer>().enabled = false;
                if(tower != null && tower.TryGetComponent<Tower>(out var Tower))
                {
                    Tower.rangeObject.GetComponent<SpriteRenderer>().enabled = false;
                }
                break;
            case BuildClickType.Buildable:// �n�a edilebilir alana t�klan�ld���nda in�a aray�z� a��l�yor when click on the buildable are, show buildable ui

                ShowBuildablePanel();
                break;
            case BuildClickType.Upgradable:// Kuleye t�klad��nda y�kseltme aray�z� a��l�yor // when click on the tower, show upgrade ui
                if (selectedUpgradeTower != null)
                    selectedUpgradeTower.rangeObject.GetComponent<SpriteRenderer>().enabled = false;
                selectedUpgradeTower = tower.GetComponent<Tower>();
                ShowUpgradablePanel();
                break;
        }
      
    }
    /// <summary>
    /// Buildable ui
    /// </summary>
    private void ShowBuildablePanel()
    {
        buildablePanel.SetActive(true);
        upgradablePanel.SetActive(false);
    }
    /// <summary>
    /// Upgrade ui
    /// </summary>
    private void ShowUpgradablePanel()
    {
        upgradeTowerImage.sprite = towerUpgradeData.towerLevelData[selectedUpgradeTower.TowerID - 1].towerSprite;
        
        upgradeTowerCurrentLevelText.text = selectedUpgradeTower.TowerLevel.ToString();

        if(selectedUpgradeTower.TowerLevel>=3)//Y�kseltme maksimum ise y�kseltme butonu kapat�l�yor // if tower is max level , close upgrade button
        {
            upgradeUIMaxButton.SetActive(true);
            upgradeUIButton.SetActive(false);
        }
        else
        {
            upgradeUIButton.SetActive(true);
            upgradeUIMaxButton.SetActive(false);
            upgradePriceText.text = towerUpgradeData.towerLevelData[selectedUpgradeTower.TowerID - 1].towerLevelUpgradeInfo[selectedUpgradeTower.TowerLevel].towerUpgradePrice.ToString();
        }
        
        TowerLevelUpgradeUIData  upgradeLevel = towerUpgradeData.towerLevelData[selectedUpgradeTower.TowerID-1].uiTowerLevelUpgradeInfo[selectedUpgradeTower.TowerLevel-1];
        // y�kseltme aray�z�ndeki �zelliklerin g�ncellenmesini sa�l�yor
        // update upgrade ui
        for (int i = 0; i < 5; i++)
        {
            if (i < upgradeLevel.uiTowerAttackDamage)
            {
                upgradeUIDamage[i].SetActive(true);
            }
            else
            {
                upgradeUIDamage[i].SetActive(false);
            }
            if (i < upgradeLevel.uiTowerAttackRange)
            {
                upgradeUIRange[i].SetActive(true);
            }
            else
            {
                upgradeUIRange[i].SetActive(false);
            }
            if (i < upgradeLevel.uiTowerTimeBetweenAttack)
            {
                upgradeUITime[i].SetActive(true);
            }
            else
            {
                upgradeUITime[i].SetActive(false);
            }
        }
        
        upgradablePanel.SetActive(false);
        upgradablePanel.SetActive(true);
    }

    /// <summary>
    /// Upgrade tower
    /// </summary>
    public void UpgradeTower()
    {
        int price = towerUpgradeData.towerLevelData[selectedUpgradeTower.TowerID - 1].towerLevelUpgradeInfo[selectedUpgradeTower.TowerLevel].towerUpgradePrice;
        if (GameManager.Instance.buildManager.Gold>=price)
        {
            GameManager.Instance.buildManager.Gold -= price;
            selectedUpgradeTower.TowerLevel++;
            ShowUpgradablePanel();
            selectedUpgradeTower.UpgradeTower();
        }
    }

}


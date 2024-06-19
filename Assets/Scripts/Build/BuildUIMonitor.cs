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
    public static Action<BuildClickType,GameObject> BuildState;// Ýnþa sisteminin durumu // Build status

    [SerializeField] private GameObject buildablePanel;// Ýnþa arayüzü // Build ui
    [SerializeField] private GameObject upgradablePanel;// Yükseltme arayüzü // upgrade ui
    [SerializeField] private Tower selectedUpgradeTower; // Seçilen kule bilgisi // selected tower info

    [Space(10)]
    [Header("UpgradeUIPanel")]
    [SerializeField] private Image upgradeTowerImage;//Yükseltme arayüzünde seçilen kule görseli // image in the upgrade ui
    [SerializeField] private GameObject upgradeUIButton;// Yükseltme butonu // upgrade button
    [SerializeField] private GameObject upgradeUIMaxButton; // Yükseltme arayüzünde maksimum yükseltme butonu // max upgrade button in the uprgrade ui
    [SerializeField] private Text upgradeTowerCurrentLevelText; // Yükseltme arayüzünde güncel kule level bilgisi // current tower level in the upgrade ui
    [SerializeField] private Text upgradePriceText;// Yükseltme fiyatý bilgisi // upgrade price
    [SerializeField] private List<GameObject> upgradeUIDamage= new List<GameObject>();// Yükseltme hasar ui'ý
    [SerializeField] private List<GameObject> upgradeUIRange= new List<GameObject>();// Yükseltme mensiz ui'ý
    [SerializeField] private List<GameObject> upgradeUITime= new List<GameObject>();// Yükseltme atýþ aralýðý ui'ý

    [SerializeField] private UpgradeBaseData towerUpgradeData ;// yükseltme bilgilerinin saklandýðý deðiþken // upgrade info
    public UpgradeBaseData TowerUpgradeData { get { return towerUpgradeData; } }


    private void Awake()
    {
        BuildState -= BuildStateEventListener;
        BuildState += BuildStateEventListener;
    }
    /// <summary>
    /// Build status listener
    /// </summary>
    /// <param name="state">Ýnþa durumu State</param>
    /// <param name="tower">Ýnþa objesi tower</param>
    private void BuildStateEventListener(BuildClickType state,GameObject tower)
    {
        switch(state)
        {
            case BuildClickType.None:// Boþ alana týkladýðýnda inþa ile ilgili kýsýmlar kapatýlýyor // when click on the empty area, close ui and towers range
                buildablePanel.SetActive(false);
                upgradablePanel.SetActive(false);
                if (selectedUpgradeTower != null)
                    selectedUpgradeTower.rangeObject.GetComponent<SpriteRenderer>().enabled = false;
                if(tower != null && tower.TryGetComponent<Tower>(out var Tower))
                {
                    Tower.rangeObject.GetComponent<SpriteRenderer>().enabled = false;
                }
                break;
            case BuildClickType.Buildable:// Ýnþa edilebilir alana týklanýldýðýnda inþa arayüzü açýlýyor when click on the buildable are, show buildable ui

                ShowBuildablePanel();
                break;
            case BuildClickType.Upgradable:// Kuleye týkladðýnda yükseltme arayüzü açýlýyor // when click on the tower, show upgrade ui
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

        if(selectedUpgradeTower.TowerLevel>=3)//Yükseltme maksimum ise yükseltme butonu kapatýlýyor // if tower is max level , close upgrade button
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
        // yükseltme arayüzündeki özelliklerin güncellenmesini saðlýyor
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


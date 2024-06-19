using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Tower Upgrade Data ", menuName = "Tower Defense")]
public class UpgradeBaseData : ScriptableObject
{

    public List<TowerBaseData> towerLevelData = new List<TowerBaseData>();//Kule bilgileri // Tower info
}

[System.Serializable]
public class TowerBaseData
{
    public int towerID;//Kule ID'si
    public Sprite towerSprite; // Kule görseli
    public List<TowerLevelUpgradeData> towerLevelUpgradeInfo = new List<TowerLevelUpgradeData>();// Kule yükseltme bilgisi // tower upgrade info
    public List<TowerLevelUpgradeUIData> uiTowerLevelUpgradeInfo = new List<TowerLevelUpgradeUIData>();// Kule yükseltme ui bilgisi // tower upgrade ui info
}

[System.Serializable]
public class TowerLevelUpgradeData
{
    public int towerUpgradePrice;// Kule yükseltme ücreti // upgrade price
    public int towerLevelID;// Kule yükseltme leveli // tower level
    public int towerAttackDamage;// Kule level hasarý
    public float towerAttackRange;// Kule level menzili
    public float towerTimeBetweenAttack;// Kule atýþ hýzý
}

[System.Serializable]
public class TowerLevelUpgradeUIData
{
    // Kule yükseltme ui'ýndaki barlarýn sayýlarýný level bilgisine göre saklýyor
    public int uiTowerLevelID;
    public int uiTowerAttackDamage;
    public int uiTowerAttackRange;
    public int uiTowerTimeBetweenAttack;
}
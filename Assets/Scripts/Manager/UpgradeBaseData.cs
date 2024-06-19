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
    public Sprite towerSprite; // Kule g�rseli
    public List<TowerLevelUpgradeData> towerLevelUpgradeInfo = new List<TowerLevelUpgradeData>();// Kule y�kseltme bilgisi // tower upgrade info
    public List<TowerLevelUpgradeUIData> uiTowerLevelUpgradeInfo = new List<TowerLevelUpgradeUIData>();// Kule y�kseltme ui bilgisi // tower upgrade ui info
}

[System.Serializable]
public class TowerLevelUpgradeData
{
    public int towerUpgradePrice;// Kule y�kseltme �creti // upgrade price
    public int towerLevelID;// Kule y�kseltme leveli // tower level
    public int towerAttackDamage;// Kule level hasar�
    public float towerAttackRange;// Kule level menzili
    public float towerTimeBetweenAttack;// Kule at�� h�z�
}

[System.Serializable]
public class TowerLevelUpgradeUIData
{
    // Kule y�kseltme ui'�ndaki barlar�n say�lar�n� level bilgisine g�re sakl�yor
    public int uiTowerLevelID;
    public int uiTowerAttackDamage;
    public int uiTowerAttackRange;
    public int uiTowerTimeBetweenAttack;
}
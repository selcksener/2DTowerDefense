using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



/// <summary>
/// Dictionary serialize kısmı
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [HideInInspector]
    private List<TKey> keyData = new List<TKey>();
    [HideInInspector]
    private List<TValue> valueData = new List<TValue>();
    public void OnAfterDeserialize()
    {
        this.Clear();
        for(int i =0;i<this.keyData.Count && i<this.valueData.Count;i++)
        {
            this[this.keyData[i]] = this.valueData[i];
        }
    }

    public void OnBeforeSerialize()
    {
        this.keyData.Clear();
        this.valueData.Clear();
        List<int> l = new List<int>();
        foreach (var item in this)
        {
            this.keyData.Add(item.Key);
            this.valueData.Add(item.Value);
        }
       
    }
}

public static class Extensions
{
    /// <summary>
    /// Bir değeri yeni aralık arasına dönüştüren işlem
    /// </summary>
    /// <param name="stageStartRange">eski  başlangıç aralığı</param>
    /// <param name="stageFinisgRange">eski bitiş aralığı</param>
    /// <param name="newStartRange">yeni başlangıç aralığı</param>
    /// <param name="newFinishRange">yeni bitiş aralığı</param>
    /// <param name="floatingValue">sayı</param>
    /// <returns></returns>
    public static float UnitIntervalRange(float stageStartRange,float stageFinisgRange,float newStartRange,float newFinishRange,float floatingValue)
    {
        float oldRange = (stageFinisgRange - stageStartRange);
        float newRange = (newFinishRange - newStartRange);
        float newValue = (((floatingValue - stageStartRange) * newRange) / oldRange) + newStartRange;
        return newValue;
    }
    /// <summary>
    /// Listeyi rasgele karışması işlemini yapan kısım
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_list"></param>
    /// <returns></returns>
    public static List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }

   
}
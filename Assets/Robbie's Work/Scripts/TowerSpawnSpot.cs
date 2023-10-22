using System.Collections;
using System.Collections.Generic;
using GameJam.Towers;
using UnityEngine;

public class TowerSpawnSpot : MonoBehaviour
{
   public Transform spawnPos;
   public bool isEmpty = true;
   [HideInInspector] public GenericTowerBehaviour tower;
   public GenericTowerBehaviour Tower
   {
      get {return tower;}
      set
      {
         if(value == null)
         {
            isEmpty = true;
            tower.OnDestroyTower -= RemoveCurrentTower;
            tower = null;
         }
         else
         {
            tower = value;
            isEmpty = false;
            tower.OnDestroyTower += RemoveCurrentTower;
         }
      }
   }

   [SerializeField] private GenericTowerBehaviour prefabTower;
   [SerializeField] private TowerInfo towerInfo;

   protected void Awake()
   {
      if(prefabTower != null)
      {
         isEmpty = false;
         GenericTowerBehaviour newTower = Instantiate(prefabTower, spawnPos).GetComponent<GenericTowerBehaviour>();
         Tower = newTower;
      }
   }

   private void RemoveCurrentTower(GenericTowerBehaviour destroyedTower)
   {
      Tower = null;
      Debug.Log(isEmpty);
   }
}

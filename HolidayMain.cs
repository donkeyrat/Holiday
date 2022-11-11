using Landfall.TABS;
using UnityEngine;
using Landfall.TABS.UnitEditor;
using Landfall.TABS.Workshop;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection;

namespace Holiday
{
	public class HolidayMain
    {
        public HolidayMain()
        {
            var db = LandfallUnitDatabase.GetDatabase();
            List<Faction> factions = (List<Faction>)typeof(LandfallUnitDatabase).GetField("Factions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
            foreach (var fac in holiday.LoadAllAssets<Faction>())
            {
                var theNew = new List<UnitBlueprint>(fac.Units);
                var veryNewUnits = (
                    from UnitBlueprint unit
                    in fac.Units
                    orderby unit.GetUnitCost()
                    select unit).ToList();
                fac.Units = veryNewUnits.ToArray();
                foreach (var vFac in factions)
                {
                    if (fac.Entity.Name == vFac.Entity.Name + "_NEW")
                    {
                        var vFacUnits = new List<UnitBlueprint>(vFac.Units);
                        vFacUnits.AddRange(fac.Units);
                        var newUnits = (
                            from UnitBlueprint unit
                            in vFacUnits
                            orderby unit.GetUnitCost()
                            select unit).ToList();
                        vFac.Units = newUnits.ToArray();
                        Object.DestroyImmediate(fac);
                    }
                }
            }
            foreach (var sb in holiday.LoadAllAssets<SoundBank>())
            {
                if (sb.name.Contains("Sound"))
                {
                    var vsb = ServiceLocator.GetService<SoundPlayer>().soundBank;
                    var cat = vsb.Categories.ToList();
                    cat.AddRange(sb.Categories);
                    vsb.Categories = cat.ToArray();
                }
            }
            foreach (var fac in holiday.LoadAllAssets<Faction>())
            {
                db.FactionList.AddItem(fac);
                db.AddFactionWithID(fac);
                foreach (var unit in fac.Units)
                {
                    if (!db.UnitList.Contains(unit))
                    {
                        db.UnitList.AddItem(unit);
                        db.AddUnitWithID(unit);
                    }
                }
            }
            foreach (var unit in holiday.LoadAllAssets<UnitBlueprint>())
            {
                if (!db.UnitList.Contains(unit))
                {
                    db.UnitList.AddItem(unit);
                    db.AddUnitWithID(unit);
                }
                foreach (var b in db.UnitBaseList)
                {
                    if (unit.UnitBase != null)
                    {
                        if (b.name == unit.UnitBase.name)
                        {
                            unit.UnitBase = b;
                        }
                    }
                }
                foreach (var b in db.WeaponList)
                {  
                    if (unit.RightWeapon != null && b.name == unit.RightWeapon.name) unit.RightWeapon = b;
                    if (unit.LeftWeapon != null && b.name == unit.LeftWeapon.name) unit.LeftWeapon = b;
                }
            }
            foreach (var objecting in holiday.LoadAllAssets<GameObject>())
            {
                if (objecting != null)
                {
                    if (objecting.GetComponent<Unit>())
                    {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("UnitBases", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("UnitBases", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                    else if (objecting.GetComponent<WeaponItem>())
                    {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("Weapons", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("Weapons", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                    else if (objecting.GetComponent<ProjectileEntity>())
                    {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("Projectiles", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("Projectiles", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                    else if (objecting.GetComponent<SpecialAbility>())
                    {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("CombatMoves", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("CombatMoves", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                    else if (objecting.GetComponent<PropItem>())
                    {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("CharacterProps", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("CharacterProps", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                }
            }
            new GameObject()
            {
                name = "SnowyBullshit",
                hideFlags = HideFlags.HideAndDontSave
            }.AddComponent<HolidaySecretManager>();
            ServiceLocator.GetService<CustomContentLoaderModIO>().QuickRefresh(WorkshopContentType.Unit, null);
        }

        public static UnitBlueprint GetUnit(string name)
        {
            List<UnitBlueprint> unit = (List<UnitBlueprint>)typeof(LandfallUnitDatabase).GetField("Units", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(LandfallUnitDatabase.GetDatabase());
            return unit.Find(x => x.name == name);
        }

        public static Faction GetFaction(string name)
        {
            List<Faction> fac = (List<Faction>)typeof(LandfallUnitDatabase).GetField("Factions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(LandfallUnitDatabase.GetDatabase());
            return fac.Find(x => x.name == name);
        }

        public static AssetBundle holiday = AssetBundle.LoadFromMemory(Properties.Resources.holiday);

        public static Material wet;
    }
}

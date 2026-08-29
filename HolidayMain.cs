using System;
using Landfall.TABS;
using UnityEngine;
using Landfall.TABS.UnitEditor;
using Landfall.TABS.Workshop;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Bootstrap;
using Landfall.TABS.GameMode;
using TGCore;
using Object = UnityEngine.Object;

namespace Holiday
{
	public class HolidayMain
    {
        public HolidayMain()
        {
            foreach (var mat in holiday.LoadAllAssets<Material>()) if (Shader.Find(mat.shader.name)) mat.shader = Shader.Find(mat.shader.name);
            
            foreach (var unit in holiday.LoadAllAssets<UnitBlueprint>().Where(x => x.UnitBase != null))
            {
                foreach (var unitBase in TGMain.landfallDb.GetUnitBases().ToList().Where(unitBase => unitBase.name == unit.UnitBase.name))
                {
                    unit.UnitBase = unitBase;
                }

                foreach (var weapon in TGMain.landfallDb.GetWeapons().ToList())
                {
                    if (unit.RightWeapon && weapon.name == unit.RightWeapon.name) unit.RightWeapon = weapon;
                    if (unit.LeftWeapon && weapon.name == unit.LeftWeapon.name) unit.LeftWeapon = weapon;
                }
            }
            
            foreach (var faction in holiday.LoadAllAssets<Faction>())
            {
                var moddedUnitList = faction.Units.Where(x => x).OrderBy(x => x.GetUnitCost()).ToArray();
                faction.Units = moddedUnitList.ToArray();
                foreach (var vanillaFaction in TGMain.landfallDb.GetFactions().ToList())
                {
                    if (faction.Entity.Name == vanillaFaction.Entity.Name + "_NEW") 
                    {
                        var vanillaUnitList = new List<UnitBlueprint>(vanillaFaction.Units);
                        vanillaUnitList.AddRange(faction.Units);
                        vanillaFaction.Units = vanillaUnitList.Where(x => x).OrderBy(x => x.GetUnitCost()).ToArray();

                        Object.DestroyImmediate(faction);
                    }
                }
            }
            
            foreach (var lvl in holiday.LoadAllAssets<TABSCampaignLevelAsset>())
            {
                var holidayFaction = HolidayMain.holiday.LoadAllAssets<Faction>().ToList().Find(x => x.name.Contains("Holiday"));
                var secretFaction = TGMain.landfallDb.GetFactions().ToList().Find(x => x.name.Contains("Secret"));
                
                var allowedU = new List<UnitBlueprint>();
                var allowed = new List<Faction>();
                
                if (lvl.name.Contains("HolidayLevel"))
                { 
                    lvl.MapAsset = TGMain.landfallDb.GetMapAssetsOrdered().ToList().Find(x => x.name.Contains("Farmer_Snow"));
                    
                    allowed.AddRange(TGMain.landfallDb.GetFactions().ToList().Where(x => x.m_displayFaction));
                    allowed.Remove(secretFaction);
                }
                else if (lvl.name.Contains("HolidaySpookyLevel"))
                {
                    allowed.Add(holidayFaction);
                    allowed.Add(secretFaction);
                    
                    allowedU.AddRange(holidayFaction.Units);
                    
                    allowedU.Add(secretFaction.Units.ToList().Find(x => x.name.Contains("SnowCannon")));
                    allowedU.Add(secretFaction.Units.ToList().Find(x => x.name.Contains("ToyRobot")));
                    allowedU.Add(secretFaction.Units.ToList().Find(x => x.name.Contains("SmoreKnight")));
                    allowedU.Add(secretFaction.Units.ToList().Find(x => x.name.Contains("Snowman")));
                }
                
                if (lvl.name.Contains("MapEquals"))
                {
                    var find = TGMain.landfallDb.GetMapAssetsOrdered().ToList().Find(x => x.name.Contains(lvl.name.Split(new[] { "MapEquals_" }, StringSplitOptions.RemoveEmptyEntries).Last()));
                    if (find) lvl.MapAsset = find;
                }

                var unitsToSearch = new List<TABSCampaignLevelAsset.TABSLayoutUnit>();
                unitsToSearch.AddRange(lvl.BlueUnits);
                unitsToSearch.AddRange(lvl.RedUnits);
                foreach (var unit in unitsToSearch)
                {
                    if (unit.m_unitBlueprint.name.Contains("_VANILLA"))
                    {
                        var vanillaVersion = TGMain.landfallDb.GetUnitBlueprints().ToList().Find(x => x.name == unit.m_unitBlueprint.name.Replace("_VANILLA", ""));
                        if (vanillaVersion) unit.m_unitBlueprint = vanillaVersion;
                    }
                }
                
                lvl.AllowedFactions = allowed.ToArray();
                lvl.AllowedUnits = allowedU.ToArray();
            }
            
            
            var allGameObjects = holiday.LoadAllAssets<GameObject>();
            TGAddons.AddItems(holiday.LoadAllAssets<UnitBlueprint>(), holiday.LoadAllAssets<Faction>(),
                holiday.LoadAllAssets<TABSCampaignAsset>(), holiday.LoadAllAssets<TABSCampaignLevelAsset>(),
                holiday.LoadAllAssets<VoiceBundle>(), holiday.LoadAllAssets<FactionIcon>(),
                allGameObjects.Select(x => x.GetComponent<Unit>()), allGameObjects.Select(x => x.GetComponent<PropItem>()),
                allGameObjects.Select(x => x.GetComponent<SpecialAbility>()), allGameObjects.Select(x => x.GetComponent<WeaponItem>()),
                allGameObjects.Select(x => x.GetComponent<ProjectileEntity>()));
            TGMain.newSounds.AddRange(holiday.LoadAllAssets<SoundBank>());
        }

        public static AssetBundle holiday = AssetBundle.LoadFromMemory(Properties.Resources.holiday);
        
        public static bool HasWesternMap = Chainloader.PluginInfos.ContainsKey("org.bepinex.plugins.WesternHoliday");
    }
}

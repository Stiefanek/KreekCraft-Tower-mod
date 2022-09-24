using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Main.Scenes;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Powers;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using Assets.Scripts.Unity.UI_New.Upgrade;
using Assets.Scripts.Utils;
using Harmony;
using Il2CppSystem.Collections.Generic;
using MelonLoader;

using UnhollowerBaseLib;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using System.Net;
using Assets.Scripts.Unity.UI_New.Popups;
using TMPro;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.TowerFilters;
using Assets.Scripts.Unity.UI_New.Main.MonkeySelect;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Towers;

namespace KreekCraft
{

    class Main : BloonsMod
    {
        //https://github.com/gurrenm3/BloonsTD6-Mod-Helper/releases

        public class KreekCraft : ModTower
        {
            public override string Name => "KreekCraft";
            public override string DisplayName => "KreekCraft";
            public override string Description => "AHH Dude That's Overpowered i hate boogie Bombs";
            public override string BaseTower => "BombShooter";
            public override int Cost => 900;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 1;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                //balance stuff
                //towerModel.display = "06bf915dea753ad43b772045caf1d906";
                towerModel.display = new PrefabReference() { guidRef = "06bf915dea753ad43b772045caf1d906" };
                //towerModel.GetBehavior<DisplayModel>().display = "06bf915dea753ad43b772045caf1d906";
                towerModel.GetBehavior<DisplayModel>().display = new PrefabReference() { guidRef = "06bf915dea753ad43b772045caf1d906" };
                var attackModel = towerModel.GetBehavior<AttackModel>();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().CapDamage(9999);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().maxDamage = 9999;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.maxPierce = 99999;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.CapPierce(99999);
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 99;
                //attackModel.weapons[0].projectile.display = "62e990209b10d374d89f70c6f578def0";
                attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = "62e990209b10d374d89f70c6f578def0" };

                //pierce and damage
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 25;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().damage = 2;

                //change radius to 75% of 100 mortar
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius = 28 * 0.75f;



                //how many seconds until it shoots
                attackModel.weapons[0].Rate = 2.5f;
            }
            public override string Icon => "KreekCraft_Icon";
            public override string Portrait => "KreekCraft_Portrait";
        }
        public class QuickdrawSight : ModUpgrade<KreekCraft>
        {
            public override string Name => "Slay";
            public override string DisplayName => "Boss Slayer";
            public override string Description => "uh oh why?";
            public override int Cost => 1;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                var bananaGunProj = attackModel.weapons[0].projectile;
                bananaGunProj.AddBehavior(new DamageModel("DamageModel_", 99999999, 99999999, true, true, true, BloonProperties.None, BloonProperties.None));
                towerModel.range = 99999999;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().damage = 99999999;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 99999999;
                attackModel.weapons[0].Rate = 0;
            }
            public override string Icon => "hm";
            public override string Portrait => "hm";
        }
 



        [HarmonyPatch(typeof(InGame), "Update")]
        public class Update_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (!(InGame.instance != null && InGame.instance.bridge != null)) return;
                try
                {
                    foreach (var tts in InGame.Bridge.GetAllTowers())
                    {

                        if (!tts.namedMonkeyKey.ToLower().Contains("KreekCraft")) continue;
                        if (tts?.tower?.Node?.graphic?.transform != null)
                        {
                            tts.tower.Node.graphic.transform.localScale = new UnityEngine.Vector3(1.3f, 1.3f, 1.3f);

                        }

                    }
                }
                catch
                {

                }


            }
        }


    }
}
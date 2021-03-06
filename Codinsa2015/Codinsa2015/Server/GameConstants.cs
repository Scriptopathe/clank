﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Codinsa2015.Server.Balancing;
using BS = Codinsa2015.Server.Balancing.IBalanceSystem<float>;
using LinearBS = Codinsa2015.Server.Balancing.LinearBalanceSystem<float>;
using ArrayBS = Codinsa2015.Server.Balancing.ArrayBalanceSystem<float>;
namespace Codinsa2015.Server
{
    /// <summary>
    /// Représente les constantes de vision du jeu.
    /// </summary>
    public class VisionConstants
    {
        public float WardRange;
        public float WardPutRange;
        
        public float WardDuration;
        public float WardRevealDuration;
        public int MaxWardsPerHero;
        public VisionConstants()
        {
            WardRange = 5.0f;
            WardDuration = 30.0f;
            MaxWardsPerHero = 5;
            WardRevealDuration = 10;
            WardPutRange = 5.0f;
        }
    }

    /// <summary>
    /// Constantes des passifs uniques.
    /// </summary>
    public class UniquePassiveConstants
    {
        #region Hunter
        public float HunterMonsterArmorDebuff = 10;
        public float HunterMonsterMRDebuff = 10;
        /// <summary>
        /// Bonus de regen du hunter en présence de monstres neutres (lvl 1).
        /// </summary>
        public float HunterBonusRegen = 2;
        /// <summary>
        /// Bonus du hunter lorsqu'il tue un monstre neutre. (lvl 2)
        /// </summary>
        public float HunterBonusGold = 10;
        /// <summary>
        /// Bonus de Armure du Hunter en présence de monstre (lvl 3)
        /// </summary>
        public float HunterBonusArmor = 50;
        /// <summary>
        /// Bonus de MR du Hunter en présence de monstre (lvl 3)
        /// </summary>
        public float HunterBonusMR = 50;
        /// <summary>
        /// Range à partir de laquelle les bonus du hunter sont actifs.
        /// </summary>
        public float HunterActivationRange = 3;
        #endregion

        #region Rugged
        // Niveau 1
        public float RuggedADBonusScaling = 0.10f;
        public float RuggedAPBonusScaling = 0.10f;
        public float RuggedASBonusFlat = 0.10f;
        public float RuggedCDRBonusFlat = 0.10f;
        // Niveau 2
        public float RuggedMSBonus = 0.20f;

        // Niveau 3
        public float RuggedKillReward = 50;

        public float RuggedActivationRange = 3;
        #endregion

        #region Unshakable
        public float UnshakableMaxHpFlatBonus = 40;
        /// <summary>
        /// Coefficient multiplicateur de la durée des slows.
        /// </summary>
        public float UnshakableSlowResistance = 0.5f;
        #endregion

        #region Strategist
        public float StrategistEnnemyStructureArmorDebuff = -0.25f; // % de l'armure
        public float StrategistEnnemyStructureRMDebuff = -0.25f; // % de la RM
        public float StrategistAllyVirusMSBuff = 1f; // flat
        public float StrategistAllyStructureArmorBuff = 0.5f; // % armure
        public float StrategistAllyStructureRMBuff = 0.5f; // % armure 
        public float StrategistActivationRange = 4f;
        #endregion

        #region Soldier
        // Lvl 1 : armure + 25%, RM + 25%
        // Lvl 2 : regen +100% quand ennemy proche
        // Lvl 3 : max HP + 10%
        public float SoldierArmorBuff = 0.25f; // %
        public float SoldierMRBuff = 0.25f; // %
        public float SoldierRegenBuff = 1.0f; // % de la regen actuelle
        public float SoldierMaxHPBuff = 0.10f; // % des HP max
        public float SoldierRegenActivationRange = 3;
        #endregion

        #region Altruist
        public float AltruistAllyRegenBonus = 2; // flat
        public float AltruistHealBonus = 0.5f; // % des soins donnés
        public float AltruistAllyMRBonus = 0.25f; // % de la mr
        public float AltruistAllyArmorBonus = 0.25f; // % de l'armor
        public float AltruistBonusCDR = 0.2f;
        public float AltruistActivationRange = 3;
        #endregion
    }
    /// <summary>
    /// Constantes de récompenses.
    /// </summary>
    public class RewardConstants
    {
        public float KillReward = 100;
        public float AssistReward = 100;
        public float PAPerSecond = 1;

        public float TankPAPerHPLost = 0.1f;
        public float TankPAPerHPLostRange = 8;
        public float TankAssistBonus = 50;
        public float TankTowerDestructionBonus = 50;
        public float TankTowerDestructionBonusRange = 5f;

        public float MageAssistBonus = 50;
        public float MagePAPerHPHealed = 0.2f;
        public float MagePAPerDamageDealt = 0.2f;
        public float MagePAPerShieldHPConsumed = 0.1f;

        public float FighterPAPerDamageDealt = 0.2f;

        public float VirusDeathRewardRange = 5f;
        public float VirusDeathReward = 4f;
    }

    /// <summary>
    /// Constantes pour les camps de monstres.
    /// </summary>
    public class MonsterCampEventConstants
    {
        /// <summary>
        /// Temps de respawn en secondes du camp.
        /// </summary>
        public float RespawnTimer;
        /// <summary>
        /// Intervalle en secondes entre 2 spawn de Virus par le camp
        /// lorsqu'il est possédé.
        /// </summary>
        public float ViruspawnInterval;
        /// <summary>
        /// Montant en PA de la récompense attribuée au tueur du camp.
        /// </summary>
        public float Reward;
        public MonsterCampEventConstants()
        {
            RespawnTimer = 10;
            ViruspawnInterval = 5;
            Reward = 30;
        }
    }

    /// <summary>
    /// Constantes concernant les routeur.
    /// </summary>
    public class RouteresEventConstants
    {
        /// <summary>
        /// Range de la vision accordée à l'équipe tuant le routeur.
        /// </summary>
        public float VisionRange;
        /// <summary>
        /// Temps de respawn en secondes du routeur après sa mort.
        /// </summary>
        public float RespawnTimer;
        /// <summary>
        /// Montant en PA de la récompense attribuée au tueur du camp.
        /// </summary>
        public float Reward;
        public RouteresEventConstants()
        {
            VisionRange = 8;
            RespawnTimer = 5;
            Reward = 30;
        }
    }
    /// <summary>
    /// Constantes concernant le big boss.
    /// </summary>
    public class BigBossEventConstants
    {
        /// <summary>
        /// Temps de respawn en secondes du boss après sa mort.
        /// </summary>
        public float RespawnTimer;
        /// <summary>
        /// Montant en PA de la récompense attribuée au tueur du camp.
        /// </summary>
        public float Reward;
        /// <summary>
        /// Durée pendant laquelle le buff sur les Virus est actif.
        /// </summary>
        public float BuffDuration;
        public BigBossEventConstants()
        {
            RespawnTimer = 30;
            Reward = 50;
            BuffDuration = 25;
        }
    }
    /// <summary>
    /// Constantes concernant les évènements.
    /// </summary>
    public class EventConstants
    {
        public MonsterCampEventConstants MonsterCamp;
        public RouteresEventConstants RouterCamp;
        public BigBossEventConstants BigBossCamp;
        public EventConstants()
        {
            MonsterCamp = new MonsterCampEventConstants();
            RouterCamp = new RouteresEventConstants();
            BigBossCamp = new BigBossEventConstants();
        }
    }

    /// <summary>
    /// Constantes pour les tours.
    /// </summary>
    public class TowerConstants : EntityConstants
    {
        public float BuffedArmor;
        public float BuffedMagicResist;
        public TowerConstants()
        {
            HP = 100;
            BuffedArmor = 300;
            BuffedMagicResist = 300;
            Armor = 100;
            MagicResist = 200;
            AttackSpeed = 1.5f;
            AttackDamage = 250;
            VisionRange = 10;
        }
    }

    /// <summary>
    /// Constantes pour les inhibs.
    /// </summary>
    public class InhibitorConstants : EntityConstants
    {
        public InhibitorConstants()
        {
            HP = 5000;
        }

    }

    /// <summary>
    /// Constante pour les spawners.
    /// </summary>
    public class SpawnerConstants : EntityConstants
    {
        public int VirusPerWave; // nombre de Virus par vague
        public float ViruspawnDelay; // délai entre le spawn de 2 Virus.
        public float WavesInterval; // délai entre l'apparition de 2 vagues
        public int Rows;          // nombre de lignes de sbires.
        public float RespawnTimer;
        public SpawnerConstants() : base()
        {
            VirusPerWave = 16;
            WavesInterval = 45.0f;
            ViruspawnDelay = 0.1f;
            Rows = 4;
            RespawnTimer = 60.0f;
        }
    }

    /// <summary>
    /// Contient les constantes concernant les échoppes.
    /// </summary>
    public class ShopConstants
    {
        public float DefaultBuyRange;
        public float SellingPriceFactor;
        public ShopConstants()
        {
            DefaultBuyRange = 6.0f;
            SellingPriceFactor = 0.9f;
        }
    }

    /// <summary>
    /// Contient les constantes concernant les structures.
    /// </summary>
    public class StructureConstants
    {
        public TowerConstants Towers;
        public InhibitorConstants Inhibs;
        public SpawnerConstants Spawners;
        public ShopConstants Shops;
        public StructureConstants()
        {
            Towers = new TowerConstants();
            Inhibs = new InhibitorConstants();
            Spawners = new SpawnerConstants();
            Shops = new ShopConstants();
        }
    }

    /// <summary>
    /// Constantes concernant les héros.
    /// </summary>
    public class HeroConstants : EntityConstants
    {
        public float StartPA;
        public HeroConstants() : base()
        {
            StartPA = 50000;
            MoveSpeed *= 2;
        }
    }
    /// <summary>
    /// Constantes concernant toutes les entités
    /// </summary>
    public class EntityConstants
    {
        public float HP;
        public float HPRegen;
        public float AttackDamage;
        public float AbilityPower;
        public float Armor;
        public float MagicResist;
        public float AttackSpeed;
        public float MoveSpeed;
        public float VisionRange;
        public float CooldownReduction;
        public EntityConstants()
        {
            HPRegen = 0.1f;
            VisionRange = 8;
            MoveSpeed = 4;
            HP = 10000;
            Armor = 25;
            MagicResist = 10;
            CooldownReduction = 0;
            AttackDamage = 4;
            AttackSpeed = 1.0f;
            AbilityPower = 15;
        }
    }

    /// <summary>
    /// Constantes des Virus.
    /// </summary>
    public class VirusConstants : EntityConstants
    {
        public float AttackRange;

        public VirusConstants() : base()
        {
            HP = 100;
            VisionRange = 4.0f;
            MoveSpeed = 4.0f;
            AttackSpeed = 1.0f;
            AttackDamage = 5;
            AttackRange = 5f;
            Armor = 10f;
        }
    }

    public class CampMonsterConstants : EntityConstants
    {
        /// <summary>
        /// Distance maximale à laquelle cette unité peut s'éloigner de GuardPosition.
        /// </summary>
        public float MaxMoveDistance;
        public float AttackRange;
        public CampMonsterConstants() 
        {
            MoveSpeed = 4;
            MaxMoveDistance = 5;
            AttackSpeed = 0.75f;
            AttackDamage = 30;
            VisionRange = 9;
            AttackRange = 4;
            HP = 100;
        }

    }

    public class RouterConstants : EntityConstants
    {        
        /// <summary>
        /// Distance maximale à laquelle cette unité peut s'éloigner de GuardPosition.
        /// </summary>
        public float MaxMoveDistance;
        public float AttackRange;
        public RouterConstants()
        {
            MaxMoveDistance = 5;
            AttackSpeed = 0.75f;
            AttackDamage = 30;
            VisionRange = 8;
            AttackRange = 4;
            HP = 100;
        }
    }

    public class RoleConstants
    {
        // Fighter
        public float FighterAttackSpeedMultiplier = 1.3f;
        public float FighterAttackDamageMultiplier = 1.3f;
        public float FighterTrueDamageMultiplier = 1.3f;
        public float FighterMagicDamageMultiplier = 1.3f;
        public float FighterStealthDurationMultiplier = 1.3f;
        
        // Mage
        public float MageHealValueMultiplier = 1.3f;
        public float MageShieldValueMultiplier = 1.3f;
        public float MageSilenceDurationMultiplier = 1.3f;
        public float MageSightDurationMultiplier = 1.3f;
        public float MageRootDurationMultiplier = 1.3f;

        // Tank
        public float TankMoveSpeedBonusMultiplier = 1.3f;
        public float TankArmorBonusMultiplier = 1.3f;
        public float TankRMBonusMultiplier = 1.3f;
        public float TankStunDurationMultiplier = 1.3f;

        public RoleConstants()
        {

        }
    }

    /// <summary>
    /// Constantes utilisées pour les spells actifs.
    /// </summary>
    public class ActiveSpellsConstants
    {
        public ArrayBS Ranges = new float[] { 2, 4, 6, 8 };
        public ArrayBS Aoes   = new float[] { 0.4f, 0.8f, 1.5f, 2.5f};
        public ArrayBS HealApRatio = new float[] { 0.1f, 0.2f, 0.4f, 0.8f };
        public ArrayBS ShieldRatios = new float[] { 0.2f, 0.5f, 1.0f, 2.0f };
        public ArrayBS ShieldDuration = new float[] { 1, 2, 3, 5 };
        public ArrayBS MoveSpeedAlterations = new float[] { 0.1f, 0.2f, 0.4f, 0.7f };
        public ArrayBS MoveSpeedDurations = new float[] { 1, 2, 3, 5 };
        public ArrayBS AoeDurations = new float[] { 0.2f, 2, 3, 5 };
        public ArrayBS ApDamageFlat = new float[] { 5, 10, 15, 20 };
        public ArrayBS AdDamageFlat = new float[] { 5, 10, 15, 20 };
        public ArrayBS ApDamageRatios = new float[] { 0.2f, 0.5f, 1, 2 };
        public ArrayBS AdDamageRatios = new float[] { 0.2f, 0.5f, 1, 2 };
        public ArrayBS MrAlterations = new float[] { 10, 20, 50, 100 };
        public ArrayBS ArmorAlterations = new float[] { 10, 20, 50, 100 };
        public ArrayBS ResistAlterationDuration = new float[] { 1, 2, 3, 5 };
        public ArrayBS StunDurations = new float[] { 0.3f, 0.6f, 1, 1.5f };
        public ArrayBS RootDurations = new float[] { 0.3f, 0.6f, 1, 1.5f };
        public ArrayBS SilenceDurations = new float[] { 0.5f, 0.1f, 1.5f, 2.5f };
        public ArrayBS BlindDurations = new float[] { 0.5f, 0.1f, 1.5f, 2.5f };
        public ArrayBS DashLengths = new float[] { 2, 4, 6, 8 };
        public ArrayBS CDs = new float[] { 0.5f, 0.5f, 0.5f, 0.5f }; // new float[] { 1, 3, 5, 7 };
        public ArrayBS AttackSpeedBonuses = new float[] { 0.20f, 0.40f, 0.60f, 0.80f };
        public ArrayBS AttackSpeedBonusesDurations = new float[] { 1, 2, 3, 5 };
        public ArrayBS FlatADAPBonuses = new float[] { 4, 8, 12, 20 };
        public ArrayBS ADAPBonusesDurations = new float[] { 1, 2, 3, 5 };
        public ArrayBS ScalingADAPBonuses = new float[] { 0.05f, 0.10f, 0.15f, 0.20f };
        public ArrayBS ProjectileSpeed = new float[] { 0.5f, 1, 1.5f, 2.0f };
        public LinearBS SpellUpgradeCost = new LinearBS(100, 100);
        public ActiveSpellsConstants()
        {

        }
    }

    /// <summary>
    /// Regroupe les constantes relatives aux équipements.
    /// </summary>
    public class EquipConstants
    {
        public float UpgradeCost1 = 300.0f;
        public float UpgradeCost2 = 300.0f;

        #region 
        public LinearBS EquipAdFlatOnHit = new LinearBS(2, 2);
        public LinearBS EquipAdRatioOnHit = new LinearBS(0.8f, 0.2f);
        public LinearBS EquipBonusAdFlat = new LinearBS(2f, 1.5f);
        public LinearBS EquipBonusSourcePercentAd = new LinearBS(0.8f, 0.2f);
        public LinearBS EquipBonusApFlat = new LinearBS(2f, 1.5f);
        public LinearBS EquipBonusAttackSpeed = new LinearBS(0.20f, 0.2f);
        public LinearBS EquipCost = new LinearBS(100, 50);
        public LinearBS EquipRange = new LinearBS(2, 2);
        public LinearBS EquipBonusArmor = new LinearBS(10, 10);
        public LinearBS EquipBonusMR = new LinearBS(10, 10);
        public LinearBS EquipBonusMaxHP = new LinearBS(10, 10);
        public LinearBS EquipBonusMoveSpeed = new LinearBS(0.1f, 0.1f);
        public LinearBS EquipBonusRegen = new LinearBS(1, 0.5f);
        public LinearBS EquipBonusCDR = new LinearBS(0.05f, 0.025f);

        public LinearBS EquipOnHitEffectFlatHeal = new LinearBS(4, 1);
        public LinearBS EnchantOnHitEffectsDuration = new LinearBS(0.50f, 0.25f);
        #endregion


        #region Weapon
        public ArrayBS UpgradeAdBonus = new float[] { 10, 20, 30, 40 };
        public ArrayBS UpgradeAsBonus = new float[] { 0.1f, 0.15f, 0.2f, 0.25f };
        #endregion

        #region Armor
        public float UpgradeCostBonusIfExpensive = 100.0f;
        public ArrayBS ArmorBonusArmor = new float[] { 10, 20, 30, 40, 50 };
        public float ArmorBonusArmorStep = 10;
        public ArrayBS ArmorBonusRM = new float[] { 10, 20, 30, 40, 50 };
        public float ArmorBonusRMStep = 10;
        public ArrayBS ArmorBonusHP = new float[] { 10, 20, 30, 40, 50 };
        public float ArmorBonusHPStep = 10;
        public ArrayBS ArmorBonusRegen = new float[] { 10, 20, 30, 40, 50 };
        public float ArmorBonusRegenStep = 10;
        public ArrayBS ArmorBonusSpeed = new float[] { 10, 20, 30, 40, 50 };
        public float ArmorBonusSpeedStep = 10;
        public ArrayBS ArmorCost = new float[] { 100, 200, 300, 400, 500 };
        #endregion

        public EquipConstants() { }
    }

    /// <summary>
    /// Constantes concernant l'Datacenter;
    /// </summary>
    public class DatacenterConstants : EntityConstants
    {

        public DatacenterConstants()
        {
            Armor = 80;
            AttackDamage = 900;
            MagicResist = 40;
            HP = 400;
        }
    }

    /// <summary>
    /// Constantes du mining farm (big boss).
    /// </summary>
    public class MiningfarmConstants : EntityConstants
    {

        public MiningfarmConstants()
        {
            HP = 1000;
        }
    }


    /// <summary>
    /// Représente toutes les constantes du jeu.
    /// Elles sont hierarchisées dans d'autres objets, afin que le fichier
    /// xml sérialisé soit plus clair à lire / écrire à la main.
    /// </summary>
    public class GameConstants
    {
        public VisionConstants Vision;
        public StructureConstants Structures;
        public VirusConstants Virus;
        public VirusConstants BuffedVirus;
        public RoleConstants Roles;
        public RewardConstants Rewards;
        public CampMonsterConstants CampMonsters;
        public RouterConstants Routers;
        public EventConstants Events;
        public UniquePassiveConstants UniquePassives;
        public ActiveSpellsConstants ActiveSpells;
        public HeroConstants Heroes;
        public TowerConstants Towers;
        public EquipConstants Equip;
        public DatacenterConstants DatacenterConstants;
        public MiningfarmConstants MiningFarms;
        /// <summary>
        /// Crée une nouvelle instance de GameConstants avec des constantes par défaut.
        /// </summary>
        public GameConstants()
        {
            Vision = new VisionConstants();
            Structures = new StructureConstants();
            Virus = new VirusConstants();
            Roles = new RoleConstants();
            Rewards = new RewardConstants();
            Events = new EventConstants();
            CampMonsters = new CampMonsterConstants();
            Routers = new RouterConstants();
            BuffedVirus = new VirusConstants();
            UniquePassives = new UniquePassiveConstants();
            ActiveSpells = new ActiveSpellsConstants();
            Equip = new EquipConstants();
            Heroes = new HeroConstants();
            Towers = new TowerConstants();
            DatacenterConstants = new DatacenterConstants();
            MiningFarms = new MiningfarmConstants();
        }

        /// <summary>
        /// Sauvegarde les constantes dans le fichier donné.
        /// </summary>
        public void Save(string path)
        {
            System.IO.File.WriteAllText(path, Tools.Serializer.Serialize<GameConstants>(this));
        }

        /// <summary>
        /// Charge les constantes depuis le fichier donné.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GameConstants LoadFromFile(string path)
        {
            return Tools.Serializer.Deserialize<GameConstants>(System.IO.File.ReadAllText(path));
        }
    }
}

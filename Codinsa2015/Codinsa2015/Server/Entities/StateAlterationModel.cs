﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server.Entities
{
    [Clank.ViewCreator.Enum("Représente un type d'altération d'état.")]
    public enum StateAlterationType
    {
        None            = 0x0000,
        // Crowd control
        Root            = 0x0001,
        Silence         = 0x0002,
        Interruption    = 0x0004,
        Blind           = 0x00400000,
        Stun            = Root | Silence | Interruption | Blind,

        // Stats
        CDR             = 0x0008,
        MoveSpeed       = 0x0010,
        ArmorBuff       = 0x0020,
        Regen           = 0x0040,
        AttackDamageBuff= 0x0080,
        MaxHP           = 0x0100,
        MagicDamageBuff = 0x0200,
        MagicResistBuff = 0x0400,
        AttackSpeed     = 0x0800,

        // Autres
        Dash            = 0x1000,
        AttackDamage    = 0x2000,     // indique que le sort inflige des dégâts ou soigne
        MagicDamage     = 0x4000,
        TrueDamage      = 0x8000,
        Heal            = 0x00010000,
        Stealth         = 0x00020000,
        Shield          = 0x00080000,
        Sight           = 0x00100000,
        WardSight       = 0x00100000, // révèle les wards.
        TrueSight       = 0x00200000, // révèle les unités stealth.
        DamageImmune    = 0x01000000,
        ControlImmune   = 0x02000000,
        Cleanse         = 0x04000000,

        // Macros
        AllCC           = Stun,
        AllDamage       = AttackDamage | MagicDamage | TrueDamage,
        All             = 0x0FFFFFFF,
    }

    public enum ScalingRatios
    {
        SrcAd           = 0x0001,
        SrcArmor        = 0x0002,
        SrcHP           = 0x0004,
        SrcMaxHP        = 0x0008,
        SrcAP           = 0x0010,
        SrcMR           = 0x0020,

        DstAd           = 0x0100,
        DstArmor        = 0x0200,
        DstHP           = 0x0400,
        DstMaxHP        = 0x0800,
        DstAP           = 0x1000,
        DstMr           = 0x2000,
        All             = 0xFFFF,
        None            = 0x0000
    }

    /// <summary>
    /// Représente une direction de dash.
    /// </summary>
    [Clank.ViewCreator.Enum("Représente une direction de dash.")]
    public enum DashDirectionType
    {
        TowardsEntity,
        Direction,
        TowardsSpellPosition,
        BackwardsCaster,
    }
    /// <summary>
    /// Représente un modèle pour une altération d'état.
    /// 
    /// L'altération en elle-même est gérée par la classe StateAlteration.
    /// </summary>
    public class StateAlterationModel
    {
        /// <summary>
        /// Type de l'altération d'état.
        /// </summary>
        [Clank.ViewCreator.Export("StateAlterationType", "Représente le type de l'altération d'état.")]
        public StateAlterationType Type { get; set; }
        /// <summary>
        /// Durée de base de l'altération d'état en secondes.
        /// 
        /// Cette durée peut être modifiée par les multiplicateurs 
        /// 
        /// Cette durée représente :
        ///     - la durée d'un dash
        ///     - la durée d'une altération d'état (buff etc...)
        /// 
        /// Elle doit être nulle pour les sorts infligeant des dégâts.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Durée de base de l'altération d'état en secondes")]
        public float BaseDuration { get; set; }
        /// <summary>
        /// Si Type contient Dash : vitesse du dash.
        /// 
        /// La vitesse du dash et la durée du sort permettent d'obtenir la distance
        /// parcourue lors du dash.
        /// 
        /// Note : DashSpeed est un alias de FlatValue utilisé pour la lisibilité et la 
        /// rétro-compatibilité.
        /// </summary>
        public float DashSpeed { get { return FlatValue; } set { FlatValue = value; } }
        /// <summary>
        /// Si Type contient Dash :
        /// Obtient ou définit une valeur indiquant si le dash permet traverser les murs.
        /// </summary>

        [Clank.ViewCreator.Export("bool", @"Si Type contient Dash : Obtient ou définit une valeur indiquant si le dash permet traverser les murs.")]
        public bool DashGoThroughWall { get; set; }
        /// <summary>
        /// Si Type contient Dash : type direction du dash.
        /// </summary>
        [Clank.ViewCreator.Export("DashDirectionType", @"Si Type contient Dash : type direction du dash.")]
        public DashDirectionType DashDirType { get; set; }


        /// <summary>
        /// Si Type contient Dash : obtient direction du dash.
        /// </summary>
        /// <param name="caster">Entité ayant lancé le sort qui provoque le dash.</param>
        /// <param name="source">Entité subissant le dash.</param>
        /// <param name="target">Si le dash est targetté, entité vers laquelle le dash doit se diriger</param>
        public Vector2 GetDashDirection(EntityBase caster, EntityBase source, StateAlterationParameters parameters)
        {
            Vector2 direction;
            switch(DashDirType)
            {
                case Entities.DashDirectionType.TowardsEntity:
                    if (parameters.DashTargetEntity == null)
                    {
#if DEBUG
                        GameServer.GetBattleLog().AddError("StateAlterationModel.GetDashDirection : target null & DashDirectionType == TowardsEntity");
                        // throw new Exception("StateAlterationModel.GetDashDirection : target null & DashDirectionType == TowardsEntity");
#endif
                        return Vector2.Zero;
                    }

                    direction = parameters.DashTargetEntity.Position - source.Position;
                    direction = normalize(direction);
                    return direction;
                case DashDirectionType.TowardsSpellPosition:
                    direction = parameters.DashTargetPosition - source.Position;
                    direction = normalize(direction);
                    return direction;
                case Entities.DashDirectionType.Direction:

                    direction = parameters.DashTargetDirection; 
                    direction = normalize(direction);
                    return direction;
                case Entities.DashDirectionType.BackwardsCaster:
                    direction = source.Position - caster.Position;
                    direction = normalize(direction);
                    return direction;
            }

            return Vector2.Zero;
        }

        /// <summary>
        /// Normalise une direction, en prenant en compte le cas (0, 0).
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        Vector2 normalize(Vector2 direction)
        {
            if (direction.X == 0 && direction.Y == 0)
                return Vector2.Zero;
            else
            {
                direction.Normalize();
                return direction;
            }
        }
        /// <summary>
        /// Valeur flat du buff / debuff (valeur positive : buff, valeur négative : debuff).
        /// La nature du buff dépend de Type.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Valeur flat du buff / debuff (valeur positive : buff, valeur négative : debuff). La nature du buff dépend de Type.")]
        public float FlatValue { get; set; }
        /// <summary>
        /// Même que FlatValue, mais en pourcentage de dégâts d'attaque actuels de la source.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue, mais en pourcentage de dégâts d'attaque actuels de la source.")]
        public float SourcePercentADValue { get; set; }
        /// <summary>
        /// Même que FlatValue, mais en pourcentage des HP actuels de la source.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue, mais en pourcentage des HP actuels de la source.")]
        public float SourcePercentHPValue { get; set; }
        /// <summary>
        /// Même que FlatValue, mais en pourcentage des HP max de la source.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue, mais en pourcentage des HP max de la source.")]
        public float SourcePercentMaxHPValue { get; set; }
        /// <summary>
        /// Même que FlatValue mais en pourcentage de l'armure actuelle de la source.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue mais en pourcentage de l'armure actuelle de la source.")]
        public float SourcePercentArmorValue { get; set; }
        /// <summary>
        /// Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité source.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité source.")]
        public float SourcePercentAPValue { get; set; }
        /// <summary>
        /// Même que FlatValue mais en pourcentage de la RM actuelle de l'entité source.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue mais en pourcentage de la RM actuelle de l'entité source.")]
        public float SourcePercentRMValue { get; set; }


        /// <summary>
        /// Même que FlatValue, mais en pourcentage de dégâts d'attaque actuels de l'entité de destination.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue, mais en pourcentage de dégâts d'attaque actuels de l'entité de destination.")]
        public float DestPercentADValue { get; set; }
        /// <summary>
        /// Même que FlatValue, mais en pourcentage des HP actuels de l'entité de destination.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue, mais en pourcentage des HP actuels de l'entité de destination.")]
        public float DestPercentHPValue { get; set; }
        /// <summary>
        /// Même que FlatValue, mais en pourcentage des HP max de l'entité de destination.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue, mais en pourcentage des HP max de l'entité de destination.")]
        public float DestPercentMaxHPValue { get; set; }
        /// <summary>
        /// Même que FlatValue mais en pourcentage de l'armure actuelle de l'entité de destination.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue mais en pourcentage de l'armure actuelle de l'entité de destination.")]
        public float DestPercentArmorValue { get; set; }
        /// <summary>
        /// Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité de destination.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité de destination.")]
        public float DestPercentAPValue { get; set; }
        /// <summary>
        /// Même que FlatValue mais en pourcentage de la RM actuelle de l'entité de destination.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Même que FlatValue mais en pourcentage de la RM actuelle de l'entité de destination.")]
        public float DestPercentRMValue { get; set; }

        /// <summary>
        /// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée à une structure.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient le multiplicateur de dégâts lorsque l'altération est appliquée à une structure.")]
        public float StructureBonus { get; set; }
        /// <summary>
        /// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un monstre neute.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un monstre neute.")]
        public float MonsterBonus { get; set; }
        /// <summary>
        /// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un Virus.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un Virus.")]
        public float VirusBonus { get; set; }


        /// <summary>
        /// Obtient un string permettant de visualiser cette altération.
        /// </summary>
        public string Debug_Stats
        {
            get
            {
                StringBuilder b = new StringBuilder();
                b.Append(Type + "{ ");

                b.Append("FlatValue = " + FlatValue);
                switch(Type)
                {
                    case StateAlterationType.AttackDamageBuff:
                    case StateAlterationType.AttackDamage:
                        b.Append(", SourcePercentADValue = " + SourcePercentADValue);
                        break;
                    case StateAlterationType.MagicDamage:
                    case StateAlterationType.MagicDamageBuff:
                        b.Append(", SourcePercentAPValue = " + SourcePercentAPValue);
                        break;
                    case StateAlterationType.MaxHP:
                        b.Append(", SourcePercentMaxHP = " + SourcePercentMaxHPValue);
                        break;
                    case StateAlterationType.ArmorBuff:
                        b.Append(", SourcePercentArmor = " + SourcePercentArmorValue);
                        break;
                    case StateAlterationType.MagicResistBuff:
                        b.Append(", SourcePercentMagicResist = " + SourcePercentRMValue);
                        break;

                }
                b.Append(" }");

                return b.ToString();
            }
        }
        /// <summary>
        /// Calcule la valeur totale de l'altération d'état en fonction des différents scalings passés en 
        /// paramètres au moyen de "ratios".
        /// 
        /// Cette fonction garantit que les ratios ne seront calculés que pour les scalings demandés.
        /// 
        /// Par exemple, si scaling ratios ne contient pas SrcAd, source.GetAttackDamage() ne sera pas appelé.
        /// </summary>
        public float GetValue(EntityBase source, EntityBase destination, ScalingRatios ratios)
        {
            float totalValue = FlatValue;
            if ((ratios & ScalingRatios.SrcAd) == ScalingRatios.SrcAd && SourcePercentADValue != 0)
                totalValue += SourcePercentADValue * source.GetAttackDamage();
            if ((ratios & ScalingRatios.SrcArmor) == ScalingRatios.SrcArmor && SourcePercentArmorValue != 0)
                totalValue += SourcePercentArmorValue * source.GetArmor();
            if ((ratios & ScalingRatios.SrcHP) == ScalingRatios.SrcHP && SourcePercentHPValue != 0)
                totalValue += SourcePercentHPValue * source.GetHP();
            if ((ratios & ScalingRatios.SrcMaxHP) == ScalingRatios.SrcMaxHP && SourcePercentMaxHPValue != 0)
                totalValue += SourcePercentMaxHPValue * source.GetMaxHP();
            if ((ratios & ScalingRatios.SrcAP) == ScalingRatios.SrcAP && SourcePercentAPValue != 0)
                totalValue += SourcePercentAPValue * source.GetAbilityPower();
            if ((ratios & ScalingRatios.SrcMR) == ScalingRatios.SrcMR && SourcePercentRMValue != 0)
                totalValue += SourcePercentRMValue * source.GetMagicResist();


            /*
            if ((ratios & ScalingRatios.DstAd) == ScalingRatios.DstAd)
                totalValue += DestPercentADValue * destination.GetAttackDamage();
            if ((ratios & ScalingRatios.DstArmor) == ScalingRatios.DstArmor)
                totalValue += DestPercentArmorValue * destination.GetArmor();
            if ((ratios & ScalingRatios.DstHP) == ScalingRatios.DstHP)
                totalValue += DestPercentHPValue * destination.GetHP();
            if ((ratios & ScalingRatios.DstMaxHP) == ScalingRatios.DstMaxHP)
                totalValue += DestPercentMaxHPValue * destination.GetMaxHP();
            if ((ratios & ScalingRatios.DstAP) == ScalingRatios.DstAP)
                totalValue += DestPercentAPValue * destination.GetAbilityPower();
            if ((ratios & ScalingRatios.SrcMR) == ScalingRatios.SrcMR)
                totalValue += DestPercentRMValue * destination.GetMagicResist();*/

            // Application des bonus.
            if(destination.Type.HasFlag(EntityType.Structure))
                totalValue *= StructureBonus;
            if(destination.Type.HasFlag(EntityType.Monster))
                totalValue *= MonsterBonus;
            if(destination.Type.HasFlag(EntityType.Virus))
                totalValue *= VirusBonus;

            // Application des bonus de rôles.
            if(source is EntityHero)
            {
                RoleConstants constants = GameServer.GetScene().Constants.Roles;
                EntityHero hero = (EntityHero)source;
                switch(hero.Role)
                {
                    case EntityHeroRole.Fighter:
                        if (Type.HasFlag(StateAlterationType.AttackSpeed))
                            totalValue *= constants.FighterAttackSpeedMultiplier;
                        else if (Type.HasFlag(StateAlterationType.AttackDamage))
                            totalValue *= constants.FighterAttackDamageMultiplier;
                        else if (Type.HasFlag(StateAlterationType.MagicDamageBuff))
                            totalValue *= constants.FighterMagicDamageMultiplier;
                        else if (Type.HasFlag(StateAlterationType.TrueDamage))
                            totalValue *= constants.FighterTrueDamageMultiplier;

                        break;
                    case EntityHeroRole.Mage:
                        if (Type.HasFlag(StateAlterationType.Heal))
                            totalValue *= constants.MageHealValueMultiplier;
                        else if (Type.HasFlag(StateAlterationType.Shield))
                            totalValue *= constants.MageShieldValueMultiplier;

                        break;
                    case EntityHeroRole.Tank:
                        if (Type.HasFlag(StateAlterationType.MoveSpeed))
                            totalValue *= constants.TankMoveSpeedBonusMultiplier;
                        else if(Type.HasFlag(StateAlterationType.ArmorBuff))
                            totalValue *= constants.TankArmorBonusMultiplier;
                        else if(Type.HasFlag(StateAlterationType.MagicResistBuff))
                            totalValue *= constants.TankRMBonusMultiplier;

                        break;
                }

                // Application du bonus des passifs uniques
                switch(hero.UniquePassive)
                {
                    case EntityUniquePassives.Altruistic:
                        // Soins augmentés
                        if(Type.HasFlag(StateAlterationType.Heal))
                        {
                            totalValue *= 1 + GameServer.GetScene().Constants.UniquePassives.AltruistHealBonus;
                        }
                        break;
                }
            }


            return totalValue;
            // Note : ceci n'est pas équivalent au code ci dessous.
            // Dans cette version les fonctions source.GetAttackDamage(), etc... sont toujours appelées.
            /*
            return FlatValue +
                   SourcePercentADValue * source.GetAttackDamage() * (int)(ratios & ScalingRatios.SrcAd) +
                   SourcePercentArmorValue * source.GetArmor() * (int)(ratios & ScalingRatios.SrcArmor)+
                   SourcePercentHPValue * source.HP * (int)(ratios & ScalingRatios.SrcHP) +
                   SourcePercentMaxHPValue * source.MaxHP * (int)(ratios & ScalingRatios.SrcMaxHP) +

                   DestPercentADValue * destination.GetAttackDamage() * (int)(ratios & ScalingRatios.DstAd) +
                   DestPercentArmorValue * destination.GetArmor() * (int)(ratios & ScalingRatios.DstArmor) +
                   DestPercentHPValue * destination.HP * (int)(ratios & ScalingRatios.DstHP) +
                   DestPercentMaxHPValue * destination.MaxHP * (int)(ratios & ScalingRatios.DstMaxHP);*/
        }

        /// <summary>
        /// Retourne la durée effective de l'altération d'état en prenant en compte les différents
        /// bonus des altérations.
        /// </summary>
        public float GetDuration(EntityBase source)
        {
            float duration = BaseDuration;
            // Application des bonus de rôles.
            if (source is EntityHero)
            {
                RoleConstants constants = GameServer.GetScene().Constants.Roles;
                EntityHero hero = (EntityHero)source;
                switch (hero.Role)
                {
                    case EntityHeroRole.Fighter:
                        if (Type.HasFlag(StateAlterationType.Stealth))
                            duration *= constants.FighterStealthDurationMultiplier;
                        break;
                    case EntityHeroRole.Mage:
                        if (Type.HasFlag(StateAlterationType.Silence))
                            duration *= constants.MageSilenceDurationMultiplier;
                        else if (Type.HasFlag(StateAlterationType.Root))
                            duration *= constants.MageRootDurationMultiplier;
                        else if (Type.HasFlag(StateAlterationType.Sight) | Type.HasFlag(StateAlterationType.TrueSight))
                            duration *= constants.MageSightDurationMultiplier;
                        break;
                    case EntityHeroRole.Tank:
                        if (Type.HasFlag(StateAlterationType.Stun))
                            duration *= constants.TankStunDurationMultiplier;
                        break;
                }
            }
            return duration;
        }

        public StateAlterationModel()
        {
            VirusBonus = 1.0f;
            MonsterBonus = 1.0f;
            StructureBonus = 1.0f;
        }

        /// <summary>
        /// Retourne une vue de cette instance de StateAlterationModel.
        /// </summary>
        /// <returns></returns>
        public Views.StateAlterationModelView ToView()
        {
            Views.StateAlterationModelView view = new Views.StateAlterationModelView();
            view.BaseDuration = BaseDuration;
            view.VirusBonus = VirusBonus;
            view.DashDirType = (Views.DashDirectionType)DashDirType;
            view.DashGoThroughWall = DashGoThroughWall;
            view.DestPercentADValue = DestPercentADValue;
            view.DestPercentAPValue = DestPercentAPValue;
            view.DestPercentArmorValue = DestPercentArmorValue;
            view.DestPercentHPValue = DestPercentHPValue;
            view.DestPercentMaxHPValue = DestPercentMaxHPValue;
            view.DestPercentRMValue = DestPercentRMValue;
            view.FlatValue = FlatValue;
            view.MonsterBonus = MonsterBonus;
            view.SourcePercentADValue = SourcePercentADValue;
            view.SourcePercentAPValue = SourcePercentAPValue;
            view.SourcePercentArmorValue = SourcePercentArmorValue;
            view.SourcePercentHPValue = SourcePercentHPValue;
            view.SourcePercentMaxHPValue = SourcePercentMaxHPValue;
            view.SourcePercentRMValue = SourcePercentRMValue;
            view.StructureBonus = StructureBonus;
            view.Type = (Views.StateAlterationType)Type;
            return view;
        }

        /// <summary>
        /// Obtient une copie de cet objet.
        /// </summary>
        /// <returns></returns>
        public StateAlterationModel Copy()
        {
            StateAlterationModel view = new StateAlterationModel();
            view.BaseDuration = BaseDuration;
            view.VirusBonus = VirusBonus;
            view.DashDirType = DashDirType;
            view.DashGoThroughWall = DashGoThroughWall;
            view.DestPercentADValue = DestPercentADValue;
            view.DestPercentAPValue = DestPercentAPValue;
            view.DestPercentArmorValue = DestPercentArmorValue;
            view.DestPercentHPValue = DestPercentHPValue;
            view.DestPercentMaxHPValue = DestPercentMaxHPValue;
            view.DestPercentRMValue = DestPercentRMValue;
            view.FlatValue = FlatValue;
            view.MonsterBonus = MonsterBonus;
            view.SourcePercentADValue = SourcePercentADValue;
            view.SourcePercentAPValue = SourcePercentAPValue;
            view.SourcePercentArmorValue = SourcePercentArmorValue;
            view.SourcePercentHPValue = SourcePercentHPValue;
            view.SourcePercentMaxHPValue = SourcePercentMaxHPValue;
            view.SourcePercentRMValue = SourcePercentRMValue;
            view.StructureBonus = StructureBonus;
            view.Type = Type;
            
            return view;
        }
        #region Constructors
        public static StateAlterationModel None() { return new StateAlterationModel(); }
        public static StateAlterationModel Root(float duration)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.Root,
                BaseDuration = duration,
            };
        }
        public static StateAlterationModel Silence(float duration)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.Silence,
                BaseDuration = duration,
            };
        }
        public static StateAlterationModel Stun(float duration)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.Stun,
                BaseDuration = duration,
            };
        }
        public static StateAlterationModel DamageFlatBuff(float duration, float buff)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.AttackDamageBuff,
                BaseDuration = duration,
                FlatValue = buff
            };
        }
        public static StateAlterationModel DamagePercentBuff(float duration, float buff)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.AttackDamageBuff,
                BaseDuration = duration,
                SourcePercentADValue = buff
            };
        }

        public static StateAlterationModel RegenFlatBuff(float duration, float buff)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.Regen,
                BaseDuration = duration,
                FlatValue = buff
            };
        }
        public static StateAlterationModel RegenPercentBuff(float duration, float buff)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.Regen,
                BaseDuration = duration,
                SourcePercentADValue = buff
            };
        }

        public static StateAlterationModel ArmorFlatBuff(float duration, float buff)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.ArmorBuff,
                BaseDuration = duration,
                FlatValue = buff
            };
        }
        public static StateAlterationModel ArmorPercentBuff(float duration, float buff)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.ArmorBuff,
                BaseDuration = duration,
                SourcePercentADValue = buff
            };
        }
        public static StateAlterationModel MoveSpeedFlatBuff(float duration, float buff)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.MoveSpeed,
                BaseDuration = duration,
                FlatValue = buff
            };
        }
        public static StateAlterationModel MoveSpeedPercentBuff(float duration, float buff)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.MoveSpeed,
                BaseDuration = duration,
                SourcePercentADValue = buff
            };
        }

        public static StateAlterationModel AttackDamageFlat(float amount)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.AttackDamage,
                BaseDuration = 0.0f,
                FlatValue = Math.Abs(amount)
            };
        }
        public static StateAlterationModel HealHPFlat(float amount)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.Heal,
                BaseDuration = 0.0f,
                SourcePercentADValue = Math.Abs(amount)
            };
        }
        public static StateAlterationModel AttackDamagePercent(float amount)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.AttackDamage,
                BaseDuration = 0.0f,
                SourcePercentADValue = Math.Abs(amount)
            };
        }
        public static StateAlterationModel HealHPPercent(float amount)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.Heal,
                BaseDuration = 0.0f,
                SourcePercentADValue = Math.Abs(amount)
            };
        }

        public static StateAlterationModel TrueDamageFlat(float amount)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.TrueDamage,
                BaseDuration = 0.0f,
                FlatValue = Math.Abs(amount)
            };
        }

        public static StateAlterationModel TrueDamagePercent(float amount)
        {
            return new StateAlterationModel()
            {
                Type = StateAlterationType.TrueDamage,
                BaseDuration = 0.0f,
                FlatValue = Math.Abs(amount)
            };
        }
        #endregion
    }
}

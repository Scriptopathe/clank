﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
using Codinsa2015.Server;
namespace Codinsa2015.Server.Spells
{
    /// <summary>
    /// Représente les différents résultats possibles d'utilisation d'un sort.
    /// </summary>
    [Clank.ViewCreator.Enum("Enumère les différents codes de retour possibles lors de d'utilisation d'un sort.")]
    public enum SpellUseResult
    {
        Success,
        InvalidTarget,
        InvalidTargettingType,
        OnCooldown,
        Silenced,
        Blind,
        OutOfRange,
        InvalidOperation
    }

    [Clank.ViewCreator.Enum("Enumère les différents codes de retour possibles lors de l'upgrade d'un sort.")]
    public enum SpellUpgradeResult
    {
        Success,
        AlreadyMaxLevel,
        NotEnoughPA,
        InvalidOperation
    }

    /// <summary>
    /// Classe de base pour représenter les sorts.
    /// 
    /// Des classes héritant de Spell doivent être crées pour représenter
    /// les différents sorts.
    /// 
    /// Un spell peut contenir plusieurs SpellDescription (correspondant à plusieurs niveaux du spell?)
    /// et est rattaché à un héros.
    /// 
    /// Les spells sont utilisés sur des SpellCastTargetInfo, représentant l'endroit ou le spell 
    /// doit être lancé, ou sa cible.
    /// 
    /// Le spell, une fois utilisé, envoie un SpellCast dans le jeu, qui, s'il atteint sa cible
    /// (dès fois automatique lorsque ciblage targetté), applique les effets décrits dans SpellDescription.
    /// </summary>
    public abstract class Spell
    {
        #region Properties

        /// <summary>
        /// Référence vers la description du spell au niveau actuel du spell.
        /// </summary>
        public SpellLevelDescription Description
        {
            get { return Levels[Level]; }
        }

        /// <summary>
        /// Cooldown actuel de ce sort, en secondes.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Cooldown actuel du sort, en secondes")]
        public float CurrentCooldown
        {
            get;
            protected set;
        }

        /// <summary>
        /// Entité possédant le sort.
        /// </summary>
        [Clank.ViewCreator.Export("int", "Id de l'entité possédant le sort.")]
        public Entities.EntityBase SourceCaster
        {
            get;
            set;
        }

        /// <summary>
        /// Représente les descriptions du spell pour les différents niveaux.
        /// </summary>
        [Clank.ViewCreator.Export("int", "Représente l'id du modèle du spell. Ce modèle décrit les différents effets du spell pour chacun de ses niveaux")]
        public SpellModel Model
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient la description des différents niveaux du spell.
        /// (Raccourci pour Model.Levels)
        /// </summary>
        public List<SpellLevelDescription> Levels
        {
            get { return Model.Levels; }
            set { Model.Levels = value;}
        }

        /// <summary>
        /// Obtient le actuel du spell.
        /// </summary>
        [Clank.ViewCreator.Export("int", "Niveau actuel du spell.")]
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient le niveau maximum du spell.
        /// </summary>
        public int MaxLevel
        {
            get { return Levels.Count; }
        }

        /// <summary>
        /// Obtient une valeur indiquant si ce spell peut être amélioré.
        /// </summary>
        public bool CanUpgrade
        {
            get { return Level < Levels.Count - 1; }
        }
        #endregion

        #region Indicateurs textuels
        /// <summary>
        /// Obtient le nom du spell.
        /// </summary>
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtient le temps de récupération de ce spell après l'utilisation d'un
        /// sort. Ce temps est calculé à partir du cooldown de base, et de la réduction
        /// de cooldown du héros.
        /// </summary>
        /// <returns></returns>
        protected virtual float GetUseCooldown()
        {
            return Description.BaseCooldown;
        }

        /// <summary>
        /// Tente une upgrade du spell.
        /// </summary>
        public SpellUpgradeResult Upgrade()
        {
            if(!CanUpgrade)
                return SpellUpgradeResult.AlreadyMaxLevel;

            EntityHero hero = SourceCaster as EntityHero;
            if(hero != null)
            {
                float price = GameServer.GetScene().Constants.ActiveSpells.SpellUpgradeCost[Level];;
                bool priceOK = hero.PA >= Level * price;
                if (!priceOK)
                    return SpellUpgradeResult.NotEnoughPA;

                hero.PA -= price;
                Level++;
                return SpellUpgradeResult.Success;

            }

            return SpellUpgradeResult.AlreadyMaxLevel;
        }
        /// <summary>
        /// Indique si oui ou non ce sort a un effet sur l'entité donné.
        /// </summary>
        public bool HasEffectOn(EntityBase entity, SpellCastTargetInfo info)
        {
            // Vérifie que le sort peut toucher cette entité.
            EntityTypeRelative flag = EntityTypeConverter.ToRelative(entity.Type, SourceCaster.Type & (EntityType.Team1 | EntityType.Team2));
            if (!(Description.TargetType.AllowedTargetTypes.HasFlag(flag) ||
                 (Description.TargetType.AllowedTargetTypes.HasFlag(EntityTypeRelative.Me) && entity.ID == SourceCaster.ID)))
                return false;
            
            // Vérifie que si le sort est targetté, il on est bien sur la bonne cible.
            if (Description.TargetType.Type == Spells.TargettingType.Targetted &&
                entity.ID != info.TargetId)
                return false; 

            // Si le caster et le receveur, on vérifie que le flag "me" est présent.
            if ((!Description.TargetType.AllowedTargetTypes.HasFlag(EntityTypeRelative.Me)) && entity.ID == SourceCaster.ID)
                return false;

            return true;
        }


        /// <summary>
        /// Utilise ce spell, si il n'est pas en cooldown et que la cible spécifiée est valide.
        /// </summary>
        /// <returns>Retourne une valeur indiquant le résultat du cast du sort. Le sort n'est pas casté si : la
        /// cible subit un silence, tente de cibler une entité invalide, le sort est en cooldown, 
        /// le sort est ciblé sur une entité et l'entité n'est pas en range.</returns>
        public SpellUseResult Use(SpellCastTargetInfo target, bool isWeaponAttack=false)
        {
            // Vérifie que le type de ciblage est le bon.
            if (((target.Type & Description.TargetType.Type) != Description.TargetType.Type))
                return SpellUseResult.InvalidTargettingType;

            // Vérifie que le sort n'est pas en cooldown.
            if (CurrentCooldown > 0)
                return SpellUseResult.OnCooldown;

            // Vérifie que la cible ne subit pas un silence
            if (!isWeaponAttack && SourceCaster.IsSilenced)
                return SpellUseResult.Silenced;

            // Vérifie que la cible est dans le bon range.
            if ((target.Type & TargettingType.Targetted) == TargettingType.Targetted)
            {
                EntityBase entity = GameServer.GetMap().GetEntityById(target.TargetId);
                if (entity == null)
                    return SpellUseResult.InvalidTarget;

                if (!this.HasEffectOn(entity, target))
                    return SpellUseResult.InvalidTarget;

                Vector2 entityPosition = entity.Position;
                if (Vector2.Distance(entityPosition, SourceCaster.Position) > Description.TargetType.Range)
                    return SpellUseResult.OutOfRange;
            }
            else if((target.Type & TargettingType.Position) == TargettingType.Position)
            {
                if (Vector2.Distance(target.TargetPosition, SourceCaster.Position) > Description.TargetType.Range)
                    return SpellUseResult.OutOfRange;
            }
            // Préparation des paramètres
            SetupParameters(target);

            // Applique les effets du casting time.
            string altId = "spell-casting-time-" + this.Name;
            SourceCaster.StateAlterations.EndAlterations(altId);
            foreach(var alterationModel in Description.CastingTimeAlterations)
            {
                SourceCaster.AddAlteration(new Entities.StateAlteration(altId,
                    SourceCaster,
                    alterationModel, 
                    target.AlterationParameters,
                    isWeaponAttack ? Entities.StateAlterationSource.Weapon : Entities.StateAlterationSource.SpellActive));
            }
            

            // Appelle la fonction qui va lancer le spell avec un délai correspondant au casting time.
            GameServer.GetScene().EventSheduler.Schedule(new Scheduler.ActionDelegate(() => {
                DoUseSpell(target);
            }), Description.CastingTime);


            // Met le spell en cooldown.
            CurrentCooldown = GetUseCooldown() * (1 - Math.Min(0.40f, SourceCaster.GetCooldownReduction()));
            return SpellUseResult.Success;
        }

        /// <summary>
        /// Fonction à réécrire pour chaque sous-classe de Spell, qui contient le comportement du sort.
        /// </summary>
        /// <param name="target"></param>
        protected virtual void DoUseSpell(SpellCastTargetInfo target)
        {

        }

        /// <summary>
        /// Applique les effets passifs du sort.
        /// Les effets passifs sont reset à chaque frame.
        /// </summary>
        public virtual void ApplyPassives()
        {

        }

        /// <summary>
        /// Effectue un setup des paramètres du sort à partir des infos de targetting.
        /// </summary>
        /// <param name="cast"></param>
        public virtual void SetupParameters(SpellCastTargetInfo target)
        {

        }
        /// <summary>
        /// Mets à jour le cooldown de ce sort.
        /// </summary>
        public void UpdateCooldown(float elapsedSeconds)
        {
            CurrentCooldown = Math.Max(0.0f, CurrentCooldown - elapsedSeconds);
        }

        /// <summary>
        /// Crée une nouvelle instance de Spell et initialise son ID.
        /// </summary>
        public Spell()
        {
        }
        #endregion

        /// <summary>
        /// Retourne une vue représentant cette instance.
        /// </summary>
        /// <returns></returns>
        public Views.SpellView ToView()
        {
            Views.SpellView view = new Views.SpellView();
            view.CurrentCooldown = CurrentCooldown;
            view.Level = Level;
            view.Model = Model.ID;
            view.SourceCaster = SourceCaster.ID;
            return view;
        }
    }
}

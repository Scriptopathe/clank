﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Clank.View.Engine.Spells
{
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
        /// Description du spell.
        /// </summary>
        public SpellDescription Description
        {
            get { return Levels[Level]; }
        }

        /// <summary>
        /// Cooldown actuel de ce sort, en secondes.
        /// </summary>
        public float CurrentCooldown
        {
            get;
            protected set;
        }

        /// <summary>
        /// Entité possédant le sort.
        /// </summary>
        public Entities.EntityBase SourceCaster
        {
            get;
            set;
        }

        /// <summary>
        /// Représente les descriptions du spell pour les différents niveaux.
        /// </summary>
        public List<SpellDescription> Levels
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient le actuel du spell.
        /// </summary>
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
        /// Utilise ce spell, si il n'est pas en cooldown et que la cible spécifiée est valide.
        /// </summary>
        /// <returns>Retourne true si le sort a pu être casté, false sinon. Le sort n'est pas casté si : la
        /// cible subit un silence, tente de cibler une entité invalide, le sort est en cooldown, 
        /// le sort est ciblé sur une entité et l'entité n'est pas en range.</returns>
        public bool Use(SpellCastTargetInfo target)
        {
            // Vérifie que le type de ciblage est le bon.
            if (((target.Type & Description.TargetType.Type) != Description.TargetType.Type))
                return false;

            // Vérifie que le sort n'est pas en cooldown.
            if (CurrentCooldown > 0)
                return false;

            // Vérifie que la cible ne subit pas un silence
            if (SourceCaster.IsSilenced)
                return false;

            // Vérifie que la cible est dans le bon range.
            if ((target.Type & TargettingType.Targetted) == TargettingType.Targetted)
            {
                Vector2 entityPosition = Mobattack.GetMap().GetEntityById(target.TargetId).Position;
                if (Vector2.Distance(entityPosition, SourceCaster.Position) > Description.TargetType.Range)
                    return false;
                
            }

            // Applique les effets du casting time.
            SourceCaster.AddAlteration(new Entities.StateAlteration(SourceCaster, Description.CastingTimeAlteration, target.AlterationParameters, Entities.StateAlterationSource.Spell));

            // Appelle la fonction qui va lancer le spell avec un délai correspondant au casting time.
            Mobattack.GetScene().EventSheduler.Schedule(new Scheduler.ActionDelegate(() => {
                DoUseSpell(target);
            }), Description.CastingTime);


            // Met le spell en cooldown.
            CurrentCooldown = GetUseCooldown() * (1 - Math.Min(0.40f, SourceCaster.GetCooldownReduction()));
            return true;
        }

        /// <summary>
        /// Fonction à réécrire pour chaque sous-classe de Spell, qui contient le comportement du sort.
        /// </summary>
        /// <param name="target"></param>
        protected virtual void DoUseSpell(SpellCastTargetInfo target)
        {
            
        }

        /// <summary>
        /// Mets à jour le cooldown de ce sort.
        /// </summary>
        public void UpdateCooldown(float elapsedSeconds)
        {
            CurrentCooldown = Math.Max(0.0f, CurrentCooldown - elapsedSeconds);
        }
        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spells
{
    /// <summary>
    /// Représente un coup de tour.
    /// </summary>
    public class TargettedTowerSpell : Spell
    {
        /// <summary>
        /// Utilise le spell
        /// </summary>
        /// <param name="target"></param>
        protected override void DoUseSpell(SpellCastTargetInfo target)
        {
            base.DoUseSpell(target);

            // TODO ici : vérification de range etc...

            Spellcasts.SpellcastBase fireball = new Spellcasts.SpellcastBase(this, target);
            GameServer.GetMap().AddSpellcast(fireball);
        }

        /// <summary>
        /// Retourne le cooldown du sort.
        /// </summary>
        /// <returns></returns>
        protected override float GetUseCooldown()
        {
            return base.GetUseCooldown();
        }

        /// <summary>
        /// Crée une nouvelle instance de ce spell.
        /// </summary>
        /// <param name="caster"></param>
        public TargettedTowerSpell(EntityBase caster)
        {
            SourceCaster = caster;
            Model = new SpellModel(new List<SpellLevelDescription>() { new SpellLevelDescription()
            {
                
                TargetType = new SpellTargetInfo()
                {
                    AllowedTargetTypes = EntityTypeRelative.AllEnnemy | EntityTypeRelative.AllTargettableNeutral,
                    AoeRadius = 0.3f,
                    Range = 8f,
                    Duration = 2f,
                    DieOnCollision = true,
                    Type = TargettingType.Targetted
                },
                BaseCooldown = 0.5f,
                CastingTime = 0.01f,
                CastingTimeAlterations = new List<StateAlterationModel>()
                {
                    new StateAlterationModel() 
                    {
                        Type = StateAlterationType.Root,
                        BaseDuration = 0.01f,
                    }
                },
                OnHitEffects = new List<StateAlterationModel>() { 
                    new StateAlterationModel()
                    {
                        Type = StateAlterationType.AttackDamage,
                        BaseDuration = 0.0f,
                        FlatValue = 0.0f,
                        SourcePercentADValue = 1.0f,
                    },
                }
            }}, "TowerSpell");
            CurrentCooldown = 0.0f;
        }
    }
}

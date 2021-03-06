﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Spells
{

    /// <summary>
    /// Contient des informations de ciblage d'un sort qui a été lancé.
    /// 
    /// ---
    /// 
    /// Différence avec SpellTargetInfo : 
    /// 
    /// SpellCastTargetInfo ne sert juste qu'à indiquer la direction / position / cible
    /// que prend le spell une fois lancé.
    /// 
    /// SpellTargetInfo, lui, est une propriété des sorts qui détermine la manière dont il est lancé
    /// (si c'est un sort ciblé, sa range, sa zone d'effet).
    /// 
    /// -----
    /// 
    /// Différence avec SpellDescription :
    /// SpellDescription décrit la totalité du sort, et encapsule un objet de type SpellTargetInfo.
    /// 
    /// </summary>
    public class SpellCastTargetInfo
    {
        int m_targetId;
        Vector2 m_targetPosition;
        Vector2 m_targetDirection;

        /// <summary>
        /// Retourne le type de ciblage de cet objet TargetInfo.
        /// 
        /// Il existe 3 types de ciblages : Targetted (sur une cible précise), Position (à une
        /// position donnée), Direction (vers une direction donnée).
        /// </summary>
        [Clank.ViewCreator.Export("TargettingType", "Type de ciblage de cet objet TargetInfo.")]
        public TargettingType Type
        {
            get;
            set;
        }
        /// <summary>
        /// Retourne la position de la cible, si le type de ciblage (Type) est TargettingType.Position.
        /// </summary>
        [Clank.ViewCreator.Export("Vector2", "Retourne la position de la cible, si le type de ciblage (Type) est TargettingType.Position.")]
        public Vector2 TargetPosition
        {
            get
            {
                return m_targetPosition;
            }
            set
            {
                m_targetPosition = value;
            }
        }
        /// <summary>
        /// Retourne la direction de la cible, si le type de ciblage (Type) est TargettingType.Direction.
        /// Ce vecteur est transformé automatiquement en vecteur unitaire.
        /// </summary>
        [Clank.ViewCreator.Export("Vector2", "Retourne la direction de la cible, si le type de ciblage (Type) est TargettingType.Direction. Ce vecteur est transformé automatiquement en vecteur unitaire.")]
        public Vector2 TargetDirection
        {
            get
            {
                return m_targetDirection;
            }
            set
            {
                m_targetDirection = value;
                m_targetDirection.Normalize();
            }
        }
        /// <summary>
        /// Retourne l'id de la cible, si le type de cibale (Type) est TargettingType.Targetted.
        /// </summary>
        [Clank.ViewCreator.Export("int", "Retourne l'id de la cible, si le type de cibale (Type) est TargettingType.Targetted.")]
        public int TargetId
        {
            get
            {
                return m_targetId;
            }
            set
            {
                m_targetId = value;
            }
        }

        /// <summary>
        /// Obtient ou définit les paramètres de l'altération d'état qui sera associée
        /// à la cible.
        /// 
        /// A affecter dans Spell.DoUseSpell() (et ses sous-classes).
        /// </summary>
        public StateAlterationParameters AlterationParameters { get; set; }

        public SpellCastTargetInfo()
        {
            AlterationParameters = new StateAlterationParameters();
        }
    }
}

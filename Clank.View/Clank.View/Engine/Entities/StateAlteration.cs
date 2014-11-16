﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Clank.View.Engine.Entities
{
    /// <summary>
    /// Représente les paramètres d'une altération d'état.
    /// </summary>
    public class StateAlterationParameters
    {
        /// <summary>
        /// Position finale que le dash doit atteindre.
        /// (si le targetting est Position)
        /// </summary>
        public Vector2 DashTargetPosition { get; set; }

        /// <summary>
        /// Entité vers laquelle le dash doit se diriger.
        /// (si le targetting du dash est Entity).
        /// </summary>
        public EntityBase DashTargetEntity { get; set; }
    }
    /// <summary>
    /// Représente une altération d'état en cours.
    /// </summary>
    public class StateAlteration
    {
        /// <summary>
        /// Représente la source de l'altération d'état.
        /// </summary>
        public EntityBase Source { get; set; }
        /// <summary>
        /// Représente le modèle d'altération d'état appliquée sur une entité.
        /// </summary>
        public StateAlterationModel Model { get; set; }

        /// <summary>
        /// Représente les paramètres de l'altération d'état.
        /// </summary>
        public StateAlterationParameters Parameters { get; set; }

        /// <summary>
        /// Temps restant en secondes pour l'altération d'état.
        /// </summary>
        public float RemainingTime { get; set; }

        /// <summary>
        /// Retourne une valeur indiquant si l'intéraction est terminée.
        /// </summary>
        public bool HasEnded(EntityBase dstEntity, GameTime time)
        {
            if(!Model.Type.HasFlag(StateAlterationType.Dash))
            {
                return RemainingTime <= 0;
            }
            else
            {
                Vector2 dstPosition = Vector2.Zero;
                if(Model.DashDirectionType == DashDirectionType.Position)
                {
                    dstPosition = Parameters.DashTargetPosition;
                }
                else if(Model.DashDirectionType == DashDirectionType.TowardsEntity)
                {
                    dstPosition = Parameters.DashTargetEntity.Position;
                }

                return Vector2.Distance(dstPosition, dstEntity.Position) <= Model.DashSpeed * (float)(time.ElapsedGameTime.TotalSeconds);
            }
        }
        /// <summary>
        /// Crée une nouvelle altération d'état à partir du modèle donné.
        /// La durée restante de l'altération d'état est déterminée à partir
        /// de la durée contenue dans le modèle d'altération d'état donné.
        /// </summary>
        public StateAlteration(EntityBase source, StateAlterationModel model, StateAlterationParameters parameters)
        {
            Parameters = parameters;
            Source = source;
            Model = model;
            RemainingTime = model.Duration;

        }

        /// <summary>
        /// Mets à jour l'altération d'état (réduit la durée du temps écoulé depuis
        /// le dernier appel à Update()).
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            RemainingTime -= (float)time.ElapsedGameTime.TotalSeconds;
        }
    }
}

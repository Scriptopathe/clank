﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.CodeGeneration
{
    /// <summary>
    /// Classe aggrégeant toutes les données statiques du jeu, qui pourront être
    /// fournies aux candidats en une seule fois.
    /// </summary>
    public class GameStaticData
    {
        [Clank.ViewCreator.Export("List<WeaponModelView>", "Obtient une liste de tous les modèles d'armes du jeu")]
        public List<Equip.WeaponModel> Weapons;
        [Clank.ViewCreator.Export("List<PassiveEquipmentModelView>", "Obtient une liste de tous les modèles d'armures du jeu")]
        public List<Equip.PassiveEquipmentModel> Armors;
        [Clank.ViewCreator.Export("List<PassiveEquipmentModelView>", "Obtient une liste de tous les modèles de bottes du jeu")]
        public List<Equip.PassiveEquipmentModel> Boots;
        [Clank.ViewCreator.Export("List<WeaponEnchantModelView>", "Obtient une liste de tous les modèles d'enchantements d'arme du jeu")]
        public List<Equip.WeaponEnchantModel> Enchants;
        [Clank.ViewCreator.Export("List<SpellModelView>", "Obtient une liste de tous les modèles de sorts du jeu.")]
        public List<Spells.SpellModel> Spells;

        [Clank.ViewCreator.Export("List<EntityBaseView>", "Obtient la liste des structures présentes sur la carte. Attention : cette liste n'est pas tenue à jour (statistiques / PV).")]
        public List<Views.EntityBaseView> Structures;

        [Clank.ViewCreator.Export("List<Vector2>", "Obtient la position de tous les camps")]
        public List<Views.Vector2> CampsPositions { get; set; }

        [Clank.ViewCreator.Export("List<Vector2>", "Obtient la position de tous les routeurs.")]
        public List<Views.Vector2> RouterPositions { get; set; }

        [Clank.ViewCreator.Export("List<EntityBaseView>", "")]
        public List<Views.EntityBaseView> VirusCheckpoints { get; set; }

        [Clank.ViewCreator.Export("MapView", "Obtient une vue sur les données statiques de la carte (telles que sa table de passabilité).")]
        public Map Map;

    }
}

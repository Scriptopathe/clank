﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Server.Controlers
{
    public enum ControlerPermissions
    {
        // Donne le droit d'appeler les fonctions prévues pour les IA
        Player      = 0x0001,
        // Donnes tous les droits.
        Admin       = 0x0002 | Player
    }
    /// <summary>
    /// Classe abstraite de contrôleur.
    /// 
    /// Un contrôleur permet de contrôler un seul héros.
    /// </summary>
    public abstract class ControlerBase
    {
        /// <summary>
        /// Obtient le nom affiché du héros (nom de l'école / pseudo).
        /// </summary>
        public string HeroName { get; set; }
        /// <summary>
        /// Obtient ou définit le héros contrôlé par ce contrôleur.
        /// </summary>
        public abstract Entities.EntityHero Hero { get; set; }
        /// <summary>
        /// Crée un nouveau contrôleur ayant le contrôle sur le héros donné.
        /// </summary>
        /// <param name="hero"></param>
        public ControlerBase(Entities.EntityHero hero) { }

        /// <summary>
        /// Mets à jour l'état de ce contrôleur, et lui permet d'envoyer des commandes au héros.
        /// </summary>
        /// <param name="time"></param>
        public abstract void Update(GameTime time);

        /// <summary>
        /// Dessine les éléments graphique du contrôleur à l'écran.
        /// </summary>
        public abstract void Draw(SpriteBatch batch, GameTime time);

        /// <summary>
        /// Charge les ressources graphiques et autres dont a besoin ce contrôleur.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Obtient les permissions de ce contrôleur.
        /// </summary>
        /// <returns></returns>
        public abstract ControlerPermissions GetPermissions();
        /// <summary>
        /// Obtient une référence vers le gestionnaire de GUI du contrôleur.
        /// </summary>
        public EnhancedGui.GuiManager EnhancedGuiManager { get; set; }
        /* ------------------------------------------------------------------
         * IFACE
         * Contient les fonctions auxquelles vont faire appel les IAs.
         * ----------------------------------------------------------------*/
        #region misc

        /// <summary>
        /// Retourne la map de jeu.
        /// </summary>
        /// <returns></returns>
        public Map GetMap()
        {
            return GameServer.GetMap();
        }
        #endregion


        const string controlerAccessStr = "Codinsa2015.Server.GameServer.GetScene().GetControler(clientId)";

        #region  Picks IFACE
        [Clank.ViewCreator.Enum("Enumère les différentes actions que le héros peut effectuer durant la phase de picks.")]
        public enum PickAction
        {
            Wait,
            PickActive,
            PickPassive,
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Lors de la phase de picks, retourne l'action actuellement attendue de la part de ce héros.")]
        public Views.PickAction Picks_NextAction()
        {
            if(GameServer.GetScene().Mode != SceneMode.Pick)
                return Views.PickAction.Wait;

            bool myTurn = GameServer.GetScene().PickControler.IsMyTurn(Hero.ID);
            if (!myTurn)
                return Views.PickAction.Wait;
            else
            {
                if (GameServer.GetScene().PickControler.IsPickingActive())
                    return Views.PickAction.PickActive;
                else
                    return Views.PickAction.PickPassive;
            }
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells actifs disponibles.")]
        public List<int> Picks_GetActiveSpells()
        {
            if (GameServer.GetScene().Mode != SceneMode.Pick)
                return new List<int>();
            return GameServer.GetScene().PickControler.GetActiveSpells();
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Lors de la phase de picks, permet à l'IA d'obtenir la liste des ID des spells passifs disponibles.")]
        public List<Views.EntityUniquePassives> Picks_GetPassiveSpells()
        {
            if (GameServer.GetScene().Mode != SceneMode.Pick)
                return new List<Views.EntityUniquePassives>();
            var lst = GameServer.GetScene().PickControler.GetPassiveSpells();
            List<Views.EntityUniquePassives> view = new List<Views.EntityUniquePassives>();
            foreach (var l in lst) { view.Add((Views.EntityUniquePassives)l); }
            return view;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Lors de la phase de picks, permet à l'IA de pick un passif donné (si c'est son tour).")]
        public Views.PickResult Picks_PickPassive(Views.EntityUniquePassives passive)
        {
            if (GameServer.GetScene().Mode != SceneMode.Pick)
                return Views.PickResult.NotYourTurn;
            bool myTurn = GameServer.GetScene().PickControler.IsMyTurn(Hero.ID);
            bool isPickingActive = GameServer.GetScene().PickControler.IsPickingActive();
            Entities.EntityUniquePassives p = (Entities.EntityUniquePassives)passive;
            if (!myTurn || isPickingActive)
                return Views.PickResult.NotYourTurn;

            return (Views.PickResult)GameServer.GetScene().PickControler.PickPassiveSpell(this.Hero.ID, p);
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Lors de la phase de picks, permet à l'IA de pick un spell actif dont l'id est donné (si c'est son tour).")]
        public Views.PickResult Picks_PickActive(int spell)
        {
            if (GameServer.GetScene().Mode != SceneMode.Pick)
                return Views.PickResult.NotYourTurn;
            bool myTurn = GameServer.GetScene().PickControler.IsMyTurn(Hero.ID);
            bool isPickingActive = GameServer.GetScene().PickControler.IsPickingActive();
            
            if (!myTurn || !isPickingActive)
                return Views.PickResult.NotYourTurn;

            return (Views.PickResult)GameServer.GetScene().PickControler.PickActiveSpell(this.Hero.ID, spell);
        }

        #endregion

        #region  Game IFACE
        /* --------------------------------------------------------------------------------------
         * Signals
         * ------------------------------------------------------------------------------------*/
        #region Signals
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste de tous les signaux émis par les coéquipiers de ce héros.")]
        public List<Views.SignalView> GetSignals()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.SignalView>();

            List<Views.SignalView> view = new List<Views.SignalView>();
            foreach(var v in GameServer.GetMap().GetSignals(Hero))
            {
                view.Add(v.ToView());
            }

            return view;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Envoie le signal passé en paramètre aux coéquipiers. (le champ SourceEntity est automatiquement rempli par la fonction)")]
        public bool SendSignal(Views.SignalView signal)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            GameServer.GetMap().AddSignal(new Signals.Signal() 
                { SourceEntity = Hero.ID, DestinationPosition = ViewToV2(signal.DestinationPosition), DestinationEntity = signal.DestinationEntity });

            return true;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du héros contrôlé.")]
        public int GetMyId()
        {
            return Hero.ID;
        }

        #endregion
        /* --------------------------------------------------------------------------------------
         * Shop
         * ------------------------------------------------------------------------------------*/
        #region Shop
        /* --------------------------------------------------------------------------------------
         * Purchase / sell
         * ------------------------------------------------------------------------------------*/
        #region Shop Purchase / sell
        Entities.EntityShop GetShop()
        {
            var entities = GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop | (Hero.Type & Entities.EntityType.Teams));
            Entities.EntityShop shopEntity = (Entities.EntityShop)entities.First().Value;
            return shopEntity;
        }
        /// <summary>
        /// Achète un objet d'id donné au shop.
        /// Les ids peuvent être obtenus via ShopGetWeapons(), ShopGetArmors(), ShopGetBoots() etc...
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Achète et équipe un objet d'id donné au shop. Les ids peuvent être obtenus via ShopGetWeapons()," + 
        "ShopGetArmors(), ShopGetBoots() etc...")]
        public Views.ShopTransactionResult ShopPurchaseItem(int equipId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.ShopTransactionResult.UnavailableItem;
            Entities.EntityShop shopEntity = GetShop();

            return (Views.ShopTransactionResult)shopEntity.Shop.Purchase(this.Hero, equipId);
        }

        /// <summary>
        /// Achète un consommable pour le héros donné, et le place dans le slot donné.
        /// </summary>
        // [Clank.ViewCreator.Access(controlerAccessStr, "Achète un consommable d'id donné, et le place dans le slot donné.")]
        public Views.ShopTransactionResult ShopPurchaseConsummable(int consummableId, int slot)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.ShopTransactionResult.UnavailableItem;
            Entities.EntityShop shopEntity = GetShop();

            return (Views.ShopTransactionResult)shopEntity.Shop.PurchaseConsummable(this.Hero, consummableId, slot);
        }

        /// <summary>
        /// Vend l'équipement passé en paramètre au shop.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Vend l'équipement du type passé en paramètre. (vends l'arme si Weapon, l'armure si Armor etc...)")]
        public Views.ShopTransactionResult ShopSell(Views.EquipmentType equipType)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.ShopTransactionResult.UnavailableItem;
            Entities.EntityShop shopEntity = GetShop();

            return (Views.ShopTransactionResult)shopEntity.Shop.Sell(this.Hero, (Equip.EquipmentType)equipType);
        }

        /// <summary>
        /// Vends un consommable situé dans le slot donné.
        /// </summary>
        // [Clank.ViewCreator.Access(controlerAccessStr, "Vends un consommable situé dans le slot donné.")]
        public Views.ShopTransactionResult ShopSellConsummable(int slot)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.ShopTransactionResult.UnavailableItem;
            Entities.EntityShop shopEntity = GetShop();

            return (Views.ShopTransactionResult)shopEntity.Shop.SellConsummable(this.Hero, slot);
        }

        /// <summary>
        /// Effectue une upgrade d'un équipement indiqué en paramètre.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Effectue une upgrade d'un équipement indiqué en paramètre.")]
        public Views.ShopTransactionResult ShopUpgrade(Views.EquipmentType equipType)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.ShopTransactionResult.UnavailableItem;

            Entities.EntityShop shopEntity = GetShop();

            return (Views.ShopTransactionResult)shopEntity.Shop.UpgradeEquip(Hero, (Equip.EquipmentType)equipType);
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des modèles d'armes disponibles au shop.")]
        public List<Views.WeaponModelView> ShopGetWeapons()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.WeaponModelView>();

            var shopEntity = GetShop();
            
            List<Equip.EquipmentModel> items = shopEntity.Shop.GetWeapons(Hero);
            List<Views.WeaponModelView> views = new List<Views.WeaponModelView>();
            foreach(var item in items)
            {
                views.Add(((Equip.WeaponModel)item).ToView());
            }
            return views;
        }
        #endregion

        /* --------------------------------------------------------------------------------------
         * Get lists
         * ------------------------------------------------------------------------------------*/
        #region Shop Get lists
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des id des modèles d'armures disponibles au shop.")]
        public List<int> ShopGetArmors()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<int>();

            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;


            List<Equip.EquipmentModel> items = shopEntity.Shop.GetArmors(Hero);
            List<int> views = new List<int>();
            foreach (var item in items)
            {
                views.Add(((Equip.PassiveEquipmentModel)item).ID);
            }
            return views;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des id des modèles de bottes disponibles au shop.")]
        public List<int> ShopGetBoots()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<int>();

            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;


            List<Equip.EquipmentModel> items = shopEntity.Shop.GetBoots(Hero);
            List<int> views = new List<int>();
            foreach (var item in items)
            {
                views.Add(((Equip.PassiveEquipmentModel)item).ID);
            }
            return views;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la liste des id des enchantements disponibles au shop.")]
        public List<int> ShopGetEnchants()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<int>();

            Entities.EntityShop shopEntity = (Entities.EntityShop)GameServer.GetMap().Entities.GetEntitiesByType(
                Entities.EntityType.Shop & (Hero.Type & Entities.EntityType.Teams)).First().Value;


            List<Equip.EquipmentModel> items = shopEntity.Shop.GetEnchants(Hero);
            List<int> views = new List<int>();
            foreach (var item in items)
            {
                views.Add(((Equip.WeaponEnchantModel)item).ID);
            }
            return views;
        }
        #endregion

        /* --------------------------------------------------------------------------------------
         * Hero equip
         * ------------------------------------------------------------------------------------*/
        #region Hero equip
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le nombre de Point d'améliorations du héros.")]
        public float GetMyPA()
        {
            return Hero.PA;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du modèle d'arme équipé par le héros. (-1 si aucun)")]
        public int GetMyWeaponId()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;
            if (Hero.Weapon == null)
                return -1;
            return Hero.Weapon.Model.ID;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le niveau du modèle d'arme équipé par le héros. (-1 si aucune arme équipée)")]
        public int GetMyWeaponLevel()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;
            if (Hero.Weapon == null)
                return -1;
            return Hero.Weapon.Level;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du modèle d'armure équipé par le héros. (-1 si aucun)")]
        public int GetMyArmorId()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;
            if (Hero.Armor == null)
                return -1;
            return Hero.Armor.Model.ID;
        }


        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le niveau du modèle d'armure équipé par le héros. (-1 si aucune armure équipée)")]
        public int GetMyArmorLevel()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;
            if (Hero.Armor == null)
                return -1;
            return Hero.Armor.Level;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du modèle de bottes équipé par le héros. (-1 si aucun)")]
        public int GetMyBootsId()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;
            if (Hero.Boots == null)
                return -1;
            return Hero.Boots.Model.ID;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le niveau du modèle de bottes équipé par le héros. (-1 si aucune paire équipée)")]
        public int GetMyBootsLevel()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;
            if (Hero.Boots == null)
                return -1;
            return Hero.Boots.Level;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'id du modèle d'enchantement d'arme équipé par le héros. (-1 si aucun)")]
        public int GetMyWeaponEnchantId()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;
            if (Hero.Weapon == null || Hero.Weapon.Enchant == null)
                return -1;
            return Hero.Weapon.Enchant.ID;
        }
        #endregion

        #endregion
        /* --------------------------------------------------------------------------------------
         * Hero Attributes
         * ------------------------------------------------------------------------------------*/
        #region Hero Attributes
        /// <summary>
        /// Retourne une vue vers le héros contrôlé par ce contrôleur.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Retourne une vue vers le héros contrôlé par ce contrôleur.")]
        public Views.EntityBaseView GetMyHero()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.EntityBaseView();

            return GetEntityById(Hero.ID);
        }
        
        /// <summary>
        /// Retourne la position du héros.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Retourne la position du héros.")]
        public Views.Vector2 GetMyPosition()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.Vector2(-1, -1);

            return V2ToView(Hero.Position);
        }

        #endregion

        /* --------------------------------------------------------------------------------------
         * Moving
         * ------------------------------------------------------------------------------------*/
        #region Moving
        /// <summary>
        /// Déplace le joueur vers la position donnée en utilisant l'A*.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Déplace le joueur vers la position donnée en utilisant l'A*.")]
        public bool StartMoveTo(Views.Vector2 position)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            return Hero.StartMoveTo(ViewToV2(position));
        }

        /// <summary>
        /// Indique si le joueur est entrain de se déplacer en utilisant son A*.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Indique si le joueur est entrain de se déplacer en utilisant son A*.")]
        public bool IsAutoMoving()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            return Hero.IsAutoMoving();
        }

        /// <summary>
        /// Arrête le déplacement automatique (A*) du joueur.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Arrête le déplacement automatique (A*) du joueur.")]
        public bool EndMoveTo()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            Hero.EndMoveTo();
            return true;
        }
        #endregion

        #region Entities / Sight

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient une valeur indiquant si votre équipe possède la vision à la position donnée.")]
        public bool HasSightAt(Views.Vector2 position)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            int w = GameServer.GetMap().Passability.GetLength(0);
            int h = GameServer.GetMap().Passability.GetLength(1);

            if (position.X < 0 || position.Y < 0 || position.X >= w - 1 || position.Y >= h - 1)
                return false;

            return GameServer.GetMap().Vision.HasVision(Hero.Type, ViewToV2(position));
        }


        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient une liste des héros morts.")]
        public List<Views.EntityBaseView> GetDeadHeroes()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.EntityBaseView>();

            List<Views.EntityBaseView> view = new List<Views.EntityBaseView>();
            foreach(var hero in GameServer.GetMap().Heroes)
            {
                if (hero.IsDead)
                    view.Add(EToView(hero));
            }

            return view;
        }



        /// <summary>
        /// Retourne la liste des entités en vue.
        /// </summary>
        /// <returns></returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Retourne la liste des entités en vue")]
        public List<Views.EntityBaseView> GetEntitiesInSight()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.EntityBaseView>();

            List<Views.EntityBaseView> views = new List<Views.EntityBaseView>();
            foreach(var kvp in GetMap().Entities.GetEntitiesInSight(Hero.Type))
            {
                views.Add(GetEntityById(kvp.Key));
            }
            return views;
        }

        /// <summary>
        /// Obtient une vue sur l'entité dont l'id est passé en paramètre.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient une vue sur l'entité dont l'id est passé en paramètre. (si l'id retourné est -1 : accès refusé)")]
        public Views.EntityBaseView GetEntityById(int entityId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.EntityBaseView() { ID = -1 };

            Entities.EntityBase entity =  GetMap().GetEntityById(entityId);
            if (entity == null)
                return new Views.EntityBaseView() { ID = -1 };

            if(Hero.HasSightOn(entity))
            {
                Views.EntityBaseView view = EToView(entity);
                return view;
            }
            else
            {
                return new Views.EntityBaseView() { ID = -1 };
            }
        }

        /// <summary>
        /// Transforme l'entité en vue.
        /// </summary>
        public Views.EntityBaseView EToView(Entities.EntityBase entity)
        {
            Views.EntityBaseView view = new Views.EntityBaseView();
            view.BaseHPRegen = entity.BaseHPRegen;
            view.BaseAbilityPower = entity.BaseAbilityPower;
            view.BaseArmor = entity.BaseArmor;
            view.BaseAttackDamage = entity.BaseAttackDamage;
            view.BaseAttackSpeed = entity.BaseAttackSpeed;
            view.BaseCooldownReduction = entity.BaseCooldownReduction;
            view.BaseMagicResist = entity.BaseMagicResist;
            view.BaseMaxHP = entity.BaseMaxHP;
            view.BaseMoveSpeed = entity.BaseMoveSpeed;
            view.Direction = V2ToView(entity.Direction);
            view.GetAbilityPower = entity.GetAbilityPower();
            view.GetArmor = entity.GetArmor();
            view.GetAttackDamage = entity.GetAttackDamage();
            view.GetCooldownReduction = entity.GetCooldownReduction();
            view.GetHP = entity.GetHP();
            view.GetMagicResist = entity.GetMagicResist();
            view.GetMaxHP = entity.GetMaxHP();
            view.GetMoveSpeed = entity.GetMoveSpeed();
            view.GetHPRegen = entity.GetHPRegen();
            view.UniquePassive = (Views.EntityUniquePassives)entity.UniquePassive;
            view.UniquePassiveLevel = entity.UniquePassiveLevel;
            view.HasTrueVision = entity.HasTrueVision;
            view.HasWardVision = entity.HasWardVision;
            view.HP = entity.HP;
            view.ID = entity.ID;
            view.IsDead = entity.IsDead;
            view.IsRooted = entity.IsRooted;
            view.IsSilenced = entity.IsSilenced;
            view.IsStealthed = entity.IsStealthed;
            view.IsStuned = entity.IsStuned;
            view.IsDamageImmune = entity.IsDamageImmune;
            view.IsControlImmune = entity.IsControlImmune;
            view.Role = (Views.EntityHeroRole)entity.Role;
            view.Position = V2ToView(entity.Position);
            view.ShieldPoints = entity.ShieldPoints;
            view.Type = (Views.EntityTypeRelative)(Entities.EntityTypeConverter.ToRelative(entity.Type, Hero.Type & Entities.EntityType.Teams));
            view.VisionRange = entity.VisionRange;
            return view;
        }
        #endregion

        #region Spell

        [Clank.ViewCreator.Access(controlerAccessStr, "Utilise l'arme du héros sur l'entité dont l'id est donné.")]
        public Views.SpellUseResult UseMyWeapon(int entityId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.SpellUseResult.InvalidOperation;

            if (Hero.Weapon == null)
                return Views.SpellUseResult.InvalidOperation;

            Entities.EntityBase entity = GameServer.GetMap().GetEntityById(entityId);
            if(entity == null)
                return Views.SpellUseResult.InvalidTarget;

            return (Views.SpellUseResult)Hero.Weapon.Use(this.Hero, entity);
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient l'attack range de l'arme du héros au niveau actuel.")]
        public float GetMyAttackRange()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return 0;
            if (Hero.Weapon == null)
                return 0;

            return Hero.Weapon.Model.Upgrades[Hero.Weapon.Level].Description.TargetType.Range;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient les points de la trajectoire du héros;")]
        public List<Views.Vector2> GetMyTrajectory()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.Vector2>();
            if(Hero.Path == null)
                return new List<Views.Vector2>();
            List<Views.Vector2> vectors = new List<Views.Vector2>();
            
            foreach(var vect in Hero.Path.TrajectoryUnits)
            {
                vectors.Add(V2ToView(vect));
            }

            return vectors;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Déplace le héros selon la direction donnée.")]
        public bool MoveTowards(Views.Vector2 direction)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return false;

            Hero.Direction = ViewToV2(direction);
            Hero.MoveForward(GameServer.GetTime());
            return true;
        }

        /// <summary>
        /// Obtient les spells du héros.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient les id des spells possédés par le héros.")]
        public List<int> GetMySpells()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<int>();

            List<int> spells = new List<int>();
            foreach(var spell in Hero.Spells)
            {
                spells.Add(spell.Model.ID);
            }
            return spells;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Effectue une upgrade du spell d'id donné (0 ou 1).")]
        public Views.SpellUpgradeResult UpgradeMyActiveSpell(int spellId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.SpellUpgradeResult.InvalidOperation;

            if (spellId >= Hero.Spells.Count)
                return Views.SpellUpgradeResult.InvalidOperation;

            return (Views.SpellUpgradeResult)Hero.Spells[spellId].Upgrade();
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le niveau actuel du spell d'id donné (numéro du spell : 0 ou 1). -1 si le spell n'existe pas.")]
        public int GetMyActiveSpellLevel(int spellId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;

            if (spellId >= Hero.Spells.Count)
                return -1;

            return Hero.Spells[spellId].Level;
        }
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient le niveau actuel du spell passif. -1 si erreur.")]
        public int GetMyPassiveSpellLevel()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return -1;


            return Hero.UniquePassiveLevel;
        }

        [Clank.ViewCreator.Access(controlerAccessStr, "Effectue une upgrade du spell passif du héros.")]
        public Views.SpellUpgradeResult UpgradeMyPassiveSpell()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.SpellUpgradeResult.InvalidOperation;

            if (Hero.UniquePassiveLevel >= 3) // désolé pour cette constante
                return Views.SpellUpgradeResult.AlreadyMaxLevel;

            Hero.UniquePassiveLevel++;
            float price = GameServer.GetScene().Constants.ActiveSpells.SpellUpgradeCost[Hero.UniquePassiveLevel];

            if (price > Hero.PA)
                return Views.SpellUpgradeResult.NotEnoughPA;

            Hero.PA -= price;

            return Views.SpellUpgradeResult.Success;
        }

        /// <summary>
        /// Utilise le sort d'id donné.
        /// </summary>
        /// <returns>Retourne true si l'action a été effectuée.</returns>
        [Clank.ViewCreator.Access(controlerAccessStr, "Utilise le sort d'id donné. Retourne true si l'action a été effectuée.")]
        public Views.SpellUseResult UseMySpell(int spellId, Views.SpellCastTargetInfoView target)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return Views.SpellUseResult.InvalidOperation;

            if(Hero.Spells.Count <= spellId)
            {
                return Views.SpellUseResult.InvalidOperation;
            }

            return (Views.SpellUseResult)Hero.Spells[spellId].Use(new Spells.SpellCastTargetInfo()
            {
                TargetDirection = ViewToV2(target.TargetDirection),
                TargetPosition = ViewToV2(target.TargetPosition),
                TargetId = target.TargetId,
                Type = (Codinsa2015.Server.Spells.TargettingType)target.Type,
                AlterationParameters = new Entities.StateAlterationParameters()
            }); 
        }
        /// <summary>
        /// Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient une vue sur le spell du héros contrôlé dont l'id est passé en paramètre. (soit 0 soit 1)")]
        public Views.SpellView GetMySpell(int spellId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.SpellView();

            // Check de l'id du spell.
            if (Hero.Spells.Count <= spellId)
            {
                return new Views.SpellView();
            }

            return Hero.Spells[spellId].ToView();
        }


        #endregion
        /// <summary>
        /// Obtient le mode de scène actuel.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la phase actuelle du jeu : Pick (=> phase de picks) ou Game (phase de jeu).")]
        public Views.SceneMode GetMode()
        {
            var mode = GameServer.GetScene().Mode;
            var view = (Views.SceneMode)mode;
            return view;
        }

        /// <summary>
        /// Obtient la description du spell dont l'id est donné en paramètre.
        /// </summary>
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient la description du spell dont l'id est donné en paramètre.")]
        public Views.SpellLevelDescriptionView GetMySpellCurrentLevelDescription(int spellId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.SpellLevelDescriptionView();

            // Check de l'id du spell.
            if (Hero.Spells.Count <= spellId)
            {
                return new Views.SpellLevelDescriptionView();
            }

            return Hero.Spells[spellId].Description.ToView();
        }



        /// <summary>
        /// (déprécié)
        /// Obtient les spells possédés par le héros dont l'id est passé en paramètre.
        /// </summary>
        /// [Clank.ViewCreator.Access(controlerAccessStr, "Obtient les spells possédés par le héros dont l'id est passé en paramètre.")]
        public List<Views.SpellView> GetHeroSpells(int entityId)
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new List<Views.SpellView>();

            // Vérifie que l'entité existe.
            if(!GetMap().Entities.ContainsKey(entityId))
                return new List<Views.SpellView>();

            Entities.EntityHero hero = GetMap().Entities[entityId] as Entities.EntityHero;

            // Vérifie que l'entité est bien un héros.
            if (hero == null)
                return new List<Views.SpellView>();

            List<Views.SpellView> spells = new List<Views.SpellView>();
            foreach (var spell in hero.Spells)
                spells.Add(spell.ToView());

            return spells;
        }
        #endregion

        #region Static data
        List<Views.PassiveEquipmentModelView> GetDBArmors()
        {
            List<Equip.PassiveEquipmentModel> items = GameServer.GetScene().ShopDB.Armors;
            List<Views.PassiveEquipmentModelView> view = new List<Views.PassiveEquipmentModelView>();
            foreach (var item in items) { view.Add(item.ToView()); }
            return view;
        }
        List<Views.PassiveEquipmentModelView> GetDBBoots()
        {
            List<Equip.PassiveEquipmentModel> items = GameServer.GetScene().ShopDB.Boots;
            List<Views.PassiveEquipmentModelView> view = new List<Views.PassiveEquipmentModelView>();
            foreach (var item in items) { view.Add(item.ToView()); }
            return view;
        }
        List<Views.WeaponModelView> GetDBWeapons()
        {
            List<Equip.WeaponModel> items = GameServer.GetScene().ShopDB.Weapons;
            List<Views.WeaponModelView> view = new List<Views.WeaponModelView>();
            foreach (var item in items) { view.Add(item.ToView()); }
            return view;
        }
        List<Views.WeaponEnchantModelView> GetDBEnchants()
        {
            List<Equip.WeaponEnchantModel> items = GameServer.GetScene().ShopDB.Enchants;
            List<Views.WeaponEnchantModelView> view = new List<Views.WeaponEnchantModelView>();
            foreach (var item in items) { view.Add(item.ToView()); }
            return view;
        }
        public List<Views.SpellModelView> GetDBSpells()
        {
            List<Spells.SpellModel> models = GameServer.GetScene().ShopDB.Spells;
            List<Views.SpellModelView> view = new List<Views.SpellModelView>();
            foreach (var item in models) { view.Add(item.ToView()); }
            return view;
        }

        /// <summary>
        /// Retourne les informations concernant la map actuelle.
        /// </summary>
        /// <returns></returns>
        //[Clank.ViewCreator.Access(controlerAccessStr, "Retourne les informations concernant la map actuelle")]
        public Views.MapView GetMapView()
        {
            if (GameServer.GetScene().Mode != SceneMode.Game)
                return new Views.MapView();

            Views.MapView view = new Views.MapView();
            List<List<bool>> passability = new List<List<bool>>();
            bool[,] mappass = GetMap().Passability;
            for (int x = 0; x < mappass.GetLength(0); x++)
            {
                passability.Add(new List<bool>());
                for (int y = 0; y < mappass.GetLength(1); y++)
                {
                    passability[x].Add(mappass[x, y]);
                }
            }
            view.Passability = passability;
            return view;
        }
        [Clank.ViewCreator.Access(controlerAccessStr, "Obtient toutes les données du jeu qui ne vont pas varier lors de son déroulement. A appeler une fois en PickPhase (pour récup les sorts) et une fois en GamePhase (pour récup les données de la map)")]
        public Views.GameStaticDataView GetStaticData()
        {
            Views.GameStaticDataView view = new Views.GameStaticDataView();
            view.Armors = GetDBArmors();
            view.Weapons = GetDBWeapons();
            view.Boots = GetDBBoots();
            view.Enchants = GetDBEnchants();
            view.Spells = GetDBSpells();

            if (GameServer.GetScene().Mode == SceneMode.Game)
            {
                view.Map = GetMapView();

                // Structures
                view.Structures = new List<Views.EntityBaseView>();
                var structures = GameServer.GetMap().Entities.GetEntitiesByType(Entities.EntityType.Structure);
                foreach (var structure in structures)
                {
                    view.Structures.Add(EToView(structure.Value));
                }

                // Routeurs
                view.RouterPositions = new List<Views.Vector2>();
                var entities = GameServer.GetMap().Entities;
                var routers = GameServer.GetMap().Entities.GetEntitiesByType(Entities.EntityType.Router);
                foreach (var router in routers)
                {
                    view.RouterPositions.Add(V2ToView(router.Value.Position));
                }

                // Camps
                var events = GameServer.GetMap().Events;
                List<Events.EventId> ids = new List<Events.EventId>() { Events.EventId.Camp1, Events.EventId.Camp2, 
                Events.EventId.Camp3, Events.EventId.Camp4, Events.EventId.Camp5, Events.EventId.Camp6, Events.EventId.Camp7,Events.EventId.Camp1, Events.EventId.Camp8 };
                view.CampsPositions = new List<Views.Vector2>();
                foreach (var id in ids)
                {
                    if (events.ContainsKey(id))
                        view.CampsPositions.Add(V2ToView(events[id].Position));
                }

                // Virus checkpoints
                var checkpoints = GameServer.GetMap().Entities.GetEntitiesByType(Entities.EntityType.Checkpoint);
                view.VirusCheckpoints = new List<Views.EntityBaseView>();
                foreach (var checkpoint in checkpoints)
                {
                    view.VirusCheckpoints.Add(EToView(checkpoint.Value));
                }
            }
            
            return view;
        }
        #endregion

        #region XNA macros
        float cf(float f)
        {
            if (float.IsInfinity(f) || float.IsNaN(f))
                return 1;
            return f;
        }
        Views.Vector2 V2ToView(Vector2 xnaVector)
        {
            return new Views.Vector2() { X = cf(xnaVector.X), Y = cf(xnaVector.Y) };
        }
        Vector2 ViewToV2(Views.Vector2 viewVector)
        {
            return new Vector2(viewVector.X, viewVector.Y);
        }
        #endregion
    }

    
}

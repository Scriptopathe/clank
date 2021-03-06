﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codinsa2015.Server;
using Codinsa2015.Server.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.DebugHumanControler.Components;
using Codinsa2015.EnhancedGui;
using Codinsa2015.Rendering;
using System.Reflection;
using ZLayer = Codinsa2015.Rendering.GraphicsHelpers.Z;
namespace Codinsa2015.DebugHumanControler
{
    /// <summary>
    /// Attribue qu'utilise la fonction d'aide pour afficher les fonctions
    /// du contrôleur.
    /// </summary>
    public class ScriptFuncAttribute : Attribute
    {
        public string Tip { get; set; }
        public ScriptFuncAttribute(string tip) { Tip = tip; }
    }
    /// <summary>
    /// Contrôleur de l'éditeur de map.
    /// </summary>
    public class MapEditorControler
    {
        
        #region Variables
        bool m_terraFormingMode = true;
        int m_rowId = 0;
        int m_checkpointId = 0;
        private GuiButton m_modeButton;
        int m_brushSize = 2;
        HumanControler m_baseControler;
        #region Graphics
        Minimap m_minimap;
        DeveloperConsole m_console;
        #endregion
        #endregion

        #region Events
        public delegate void MapLoadedDelegate(MapFile map);
        public event MapLoadedDelegate OnMapLoaded;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la map en cours.
        /// </summary>
        public Codinsa2015.Server.Map CurrentMap
        {
            get
            {
                return m_baseControler.MapRdr.Map;
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si ce contrôleur est actuellement activé.
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de MapEditorControler associé à la map donnée.
        /// </summary>
        public MapEditorControler(HumanControler baseControler)
        {
            m_baseControler = baseControler;
            m_minimap = new Minimap(baseControler.MapRdr);
            m_console = new DeveloperConsole(baseControler.EnhancedGuiManager);
            
        }


        /// <summary>
        /// Charge les ressources graphiques dont a besoin ce contrôleur.
        /// </summary>
        public void LoadContent()
        {
            m_minimap.LoadContent();
            m_console.LoadContent();
            CreateGui();

            Server.GameServer.GetScene().GameInterpreter.MainContext.LocalVariables.Add("ctrl", new PonyCarpetExtractor.ExpressionTree.Mutable(this));
            Server.GameServer.GetBattleLog().OnEvent += delegate(Tools.EventLog.Entry entry)
            {
                m_console.Output.AppendLine("[Battle] " + entry.Message);
            };
        }

 

        /// <summary>
        /// Initialise les composants de l'interface graphique.
        /// </summary>
        void CreateGui()
        {
            m_modeButton = new GuiButton(m_baseControler.EnhancedGuiManager)
            {
                Location = new Point(0, 0),
                Title = "Land",
                Size = new Point(150, 25)
            };
            m_modeButton.Clicked += OnChangeMode;

        }

        /// <summary>
        /// Change le mode du contrôleur.
        /// </summary>
        void OnChangeMode()
        {
            m_terraFormingMode = !m_terraFormingMode;
            m_modeButton.Title = m_terraFormingMode ? "Land" : "Entities";
        }

        /// <summary>
        /// Mets à jour le contrôleur en prenant en compte les entrées utilisateurs.
        /// </summary>
        public void Update(GameTime time)
        {
            if (!IsEnabled)
                return;

            bool processGameInput = m_baseControler.GameWindow.HasFocus();

            if (m_console.HasFocus)
                return;

            // Affichage de la minimap.
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.M) && processGameInput)
                m_minimap.Visible = !m_minimap.Visible;

            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Tab))
                OnChangeMode();

            // Zoom
            Vector2 mousePosPx = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            Vector2 mousePosUnits = GetMousePosUnits();

            if(processGameInput)
            {
                // Modif de la taille du brush.
                if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Add))
                {
                    m_baseControler.MapRdr.UnitSize *= 2;
                    m_baseControler.MapRdr.ScrollingVector2 *= 2;
                }
                else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Subtract))
                {
                    m_baseControler.MapRdr.UnitSize /= 2;
                    m_baseControler.MapRdr.ScrollingVector2 /= 2;
                }

                // Terraforming
                if (m_terraFormingMode && mousePosPx.Y > 25)
                {
                    // Ajout de matière
                    if (Input.IsLeftClickPressed())
                    {
                        for (int i = 0; i < m_brushSize; i++)
                            for (int j = 0; j < m_brushSize; j++)
                                CurrentMap.SetPassabilityAt(mousePosUnits + new Vector2(i, j), false);
                    }
                    else if (Input.IsRightClickPressed())
                    {
                        for (int i = 0; i < m_brushSize; i++)
                            for (int j = 0; j < m_brushSize; j++)
                                CurrentMap.SetPassabilityAt(mousePosUnits + new Vector2(i, j), true);
                    }
                    else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.O))
                        m_brushSize++;
                    else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.I))
                        m_brushSize--;
                }

                // Edition d'entités.
                else if (!m_terraFormingMode)
                {
                    if (Input.IsRightClickTrigger())
                    {

                        Server.Entities.EntityCollection entitiesInRange = CurrentMap.Entities.GetAliveEntitiesInRange(mousePosUnits, 1f);
                        if (entitiesInRange.Count != 0)
                        {
                            GuiMenu menu = new GuiMenu(m_baseControler.EnhancedGuiManager);
                            menu.Location = new Point((int)mousePosPx.X, (int)mousePosPx.Y);
                            menu.Title = "Menu";
                            foreach (EntityBase entity in entitiesInRange.Values)
                            {
                                GuiMenu.GuiMenuItem item = new GuiMenu.GuiMenuItem("Remove " + entity.Type.ToString() + " (" + entity.ID + ")");
                                item.ItemSelected += new GuiMenu.ItemSelectedDelegate(() =>
                                {
                                    CurrentMap.Entities.Remove(entity.ID);
                                    entity.Dispose();
                                });
                                item.IsEnabled = true;
                                menu.AddItem(item);
                            }
                            menu.Focus();
                        }
                    }

                    // Ajout d'éléments de jeu.
                    Server.Entities.EntityType team = Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.Q) ? Server.Entities.EntityType.Team2 : Server.Entities.EntityType.Team1;
                    if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.T))
                    {
                        AddTower(team, mousePosUnits);
                    }
                    else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.R))
                    {
                        AddSpawner(team, mousePosUnits);
                    }
                    else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.Y))
                    {
                        AddCheckpoint(team, mousePosUnits);
                    }
                    else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.U))
                    {
                        AddWardPlace(mousePosUnits);
                    }
                    else if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.W))
                    {
                        PutWard(team, mousePosUnits);

                    }
                    if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1))
                    {
                        m_checkpointId = 0;
                        m_rowId = 0;
                    }
                    else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2))
                    {
                        m_checkpointId = 0;
                        m_rowId = 1;
                    }
                    else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3))
                    {
                        m_checkpointId = 0;
                        m_rowId = 2;
                    }
                    else if (Input.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4))
                    {
                        m_checkpointId = 0;
                        m_rowId = 3;
                    }
                }
            }
            
            

            // Sauvegarde
            /*if( Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.M))
            {
                Save();
            }
            */
            // Chargement
            if (Input.IsTrigger(Microsoft.Xna.Framework.Input.Keys.L))
            {
                Load();
            }
        }

        /// <summary>
        /// Dessine les éléments graphiques du contrôleur.
        /// </summary>
        public void Draw(SpriteBatch batch)
        {
            int ts = m_baseControler.MapRdr.UnitSize;
            // Récupère la position de la souris
            Vector2 position = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            // Dessine le cursor
            batch.Draw(Ressources.Cursor, new Rectangle((int)position.X, (int)position.Y, 32, 32), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, ZLayer.Front);
            
            if (!IsEnabled)
            {
                m_modeButton.IsVisible = false;
                m_console.Visible = false;
                m_console.HasFocus = false;
            }
            else
            {
                m_modeButton.IsVisible = true;
                m_console.Visible = true;
            }
            
            // Dessine la case survolée
            var rawpos = GetMousePosUnits();
            rawpos.X -= rawpos.X % 1;
            rawpos.Y -= rawpos.Y % 1;
            var pos = m_baseControler.MapRdr.ToScreenSpace(rawpos);
            batch.Draw(Ressources.HighlightMark, new Rectangle((int)pos.X, (int)pos.Y, ts, ts), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, ZLayer.GUI);
            

            // Dessine le bandeau supérieur
            batch.Draw(Ressources.DummyTexture, new Rectangle(0, 0, (int)Ressources.ScreenSize.X, 25),
                null, new Color(0, 0, 0, 200), 0.0f, Vector2.Zero, SpriteEffects.None, ZLayer.GUI + 5 * ZLayer.BackStep);
            
            // Dessine la minimap
            m_minimap.Position = new Vector2(Ressources.ScreenSize.X - 200, Ressources.ScreenSize.Y - 100);
            m_minimap.Size = new Vector2(200, 100);
            m_minimap.Z = ZLayer.GUI + 2 * ZLayer.BackStep;
            m_minimap.Draw(batch);
            
            // Dessine des infos de debug.
            batch.DrawString(Ressources.Font, "RowId = " + m_rowId + " | CheckpointId = " + m_checkpointId + " | Mouse=" + rawpos.ToString(),
                new Vector2(150, 0), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, ZLayer.GUI + 4 * ZLayer.BackStep);
        }

        #region Script

        #region Help
        /// <summary>
        /// Affiche un message d'aide.
        /// </summary>
        public void help()
        {
            puts("+ print_shortcuts() ");
            puts("+ print_commands() ");
        }

        /// <summary>
        /// Affiche les raccourcis claviers de l'éditeur.
        /// </summary>
        public void print_shortcuts()
        {
            puts("> T = tower, R = spawner, Y = checkpoint, U = ward place, W = ward");
            puts("> Numpad 1, 2, 3, 4 : set checkpoint id");
            puts("> M = save, L = load");
        }

        /// <summary>
        /// Affiche les commandes disponibles.
        /// </summary>
        public void print_commands()
        {
            MethodInfo[] methods = this.GetType().GetMethods();
            foreach(MethodInfo method in methods)
            {
                object[] attrs = method.GetCustomAttributes(false);
                foreach(object attr in attrs)
                {
                    ScriptFuncAttribute func = attr as ScriptFuncAttribute;
                    if(func != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        puts(" " + func.Tip);
                        builder.Append("+ " + method.Name + "(");
                        // Arguments
                        ParameterInfo[] parameters = method.GetParameters();
                        foreach(var par in parameters)
                        {
                            builder.Append(par.ParameterType.Name + " " + par.Name + ",");
                        }

                        puts(builder.ToString().TrimEnd(',') + ")");
                    }
                }
            }
        }

        /// <summary>
        /// Ecrit une ligne dans la console.
        /// </summary>
        /// <param name="msg"></param>
        [ScriptFunc("Ecrit dans la console.")]
        public void puts(string msg)
        {
            m_console.Output.AppendLine(msg);
            m_console.Output.ScrollDown();
        }

        /// <summary>
        /// Ajoute un visualiseur de stats pour le héros donné.
        /// </summary>
        [ScriptFunc("Ajoute un visualiseur de stats pour le héros donné.")]
        public Components.StatsViewer add_hviewer(int heroId)
        {
            if (heroId >= GameServer.GetMap().Heroes.Count || heroId < 0)
                puts("Le héros " + heroId + " n'existe pas.");

            EntityBase entity = GameServer.GetMap().Heroes[heroId];
            return add_eviewer(entity);
        }
        /// <summary>
        /// Ajoute un visualiseur de stats pour l'entité d'id donné.
        /// </summary>
        [ScriptFunc("Ajoute un visualiseur de stats pour l'entité d'id donné.")]
        public Components.StatsViewer add_viewer(int entityId)
        {
            if (!GameServer.GetMap().Entities.ContainsKey(entityId))
                puts("Le l'entité " + entityId + " n'existe pas.");

            EntityBase entity = GameServer.GetMap().Entities[entityId];
            return add_eviewer(entity);
        }
        /// <summary>
        /// Ajoute l'interface de shop.
        /// </summary>
        [ScriptFunc("Ajoute l'interface de shop. pour le héros d'id donné.")]
        public Components.ShopInterface add_shop(int heroId)
        {
            if (heroId >= GameServer.GetMap().Heroes.Count || heroId < 0)
                puts("Le héros " + heroId + " n'existe pas.");

            // Héros
            EntityHero entity = GameServer.GetMap().Heroes[heroId];

            // Shop
            var shops = GameServer.GetMap().Entities.GetEntitiesByType(EntityType.Shop | (entity.Type & EntityType.Teams));
            if(shops.Count <= 0)
                puts("Aucun shop n'existe pour la " + (entity.Type & EntityType.Teams) + "...");

            return new ShopInterface(m_console, m_baseControler.EnhancedGuiManager, entity, ((EntityShop)shops.First().Value).Shop);
        }
        /// <summary>
        /// Ajoute un visualiseur de stats pour l'entité donnée.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Components.StatsViewer add_eviewer(EntityBase entity)
        {
            StatsViewer v = new StatsViewer(m_baseControler.EnhancedGuiManager, entity);
            v.Size = new Point(150, 220);
            v.Location = new Point(100, 100);

            return v;
        }
        #endregion


        #region API
        /// <summary>
        /// Retourne la position de la souris sur la map en unités métriques.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMousePosUnits()
        {
            Vector2 mousePosPx = new Vector2(Input.GetMouseState().X, Input.GetMouseState().Y);
            Vector2 mousePosUnits = ((mousePosPx + m_baseControler.MapRdr.ScrollingVector2) - new Vector2(m_baseControler.MapRdr.Viewport.X, m_baseControler.MapRdr.Viewport.Y)) / m_baseControler.MapRdr.UnitSize;
            return mousePosUnits;
        }


        [ScriptFunc("Change l'id du prochain checkpoint créé dans l'éditeur.")]
        public void SetCheckpointId(int id) { m_checkpointId = id; }
        [ScriptFunc("Change la row du prochain checkpoint créé dans l'éditeur.")]
        public void SetRowId(int id) { m_rowId = id; m_checkpointId = 0; }
        /// <summary>
        /// Pose une ward sur l'emplacement à proximité de la placementPosition.
        /// </summary>
        /// <param name="team"></param>

        [ScriptFunc("Pose une ward à l'emplacement donné.")]
        public void PutWard(EntityType team, Vector2 placementPosition)
        {
            EntityWardPlacement p = m_baseControler.MapRdr.Map.Entities.GetEntitiesByType(EntityType.WardPlacement).
                                    GetAliveEntitiesInRange(placementPosition, 1).
                                    NearestFrom(placementPosition) as EntityWardPlacement;
            if (p != null)
            {
                p.PutWard(new EntityHero() { Type = team });
            }
            else
                m_console.Output.AppendLine("Pas d'emplacement où poser la ward !");
        }

        /// <summary>
        /// Ajoute un tour pour la team donnée à la position donnée.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="position"></param>

        [ScriptFunc("Ajoute une tour à l'emplacement donné.")]
        public void AddTower(EntityType team, Vector2 position)
        {
            Server.Entities.EntityBase entity = new Server.Entities.EntityTower()
            {
                Position = position,
                Type = Server.Entities.EntityType.Tower | team,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
        }
        /// <summary>
        /// Ajoute un emplacement de ward à la position donnée.
        /// </summary>
        /// <param name="position"></param>
        [ScriptFunc("Ajoute un emplacement de ward à l'emplacement donné.")]
        public void AddWardPlace(Vector2 position)
        {
            Server.Entities.EntityBase entity = new EntityWardPlacement()
            {
                Position = position,
                Type = EntityType.WardPlacement,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
        }

        /// <summary>
        /// Ajoute un checkpoint à la position donnée et incrémente le compteur de checkpoint.
        /// </summary>
        [ScriptFunc("Ajoute un checkpoint tour à l'emplacement donné.")]
        public void AddCheckpoint(EntityType team, Vector2 position)
        {
            Server.Entities.EntityBase entity = new Server.Entities.EntityCheckpoint()
            {
                Position = position,
                Type = Server.Entities.EntityType.Checkpoint | team,
                CheckpointID = m_checkpointId,
                CheckpointRow = m_rowId,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
            m_checkpointId++;
        }

        /// <summary>
        /// Ajoute un spawner pour la team donnée à la position donnée.
        /// </summary>
        [ScriptFunc("Ajoute un spawner à l'emplacement donné.")]
        public void AddSpawner(EntityType team, Vector2 position)
        {
            Server.Entities.EntityBase entity = new Server.Entities.EntitySpawner()
            {
                Position = position,
                SpawnPosition = position,
                Type = Server.Entities.EntityType.Spawner | team,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
        }

        /// <summary>
        /// Ajoute une zone de spawn de héros à la position donnée.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="position"></param>
        [ScriptFunc("Ajoute un spawner de héros à l'emplacement donné.")]
        public void AddHeroSpawner(EntityType team, Vector2 position)
        {
            Server.Entities.EntityBase entity = new EntityHeroSpawner()
            {
                Position = position,
                Type = EntityType.HeroSpawner | team,
            };
            CurrentMap.Entities.Add(entity.ID, entity);
        }
        /// <summary>
        /// Sauvegarde la map.
        /// </summary>
        public void Save(float scale=1.0f)
        {
            MapFile.Save(CurrentMap, Ressources.MapFilename, scale);
            m_console.Output.AppendLine("Map sauvegardée avec succès.");
        }

        /// <summary>
        /// Charge la map.
        /// </summary>
        public void Load()
        {
            if (System.IO.File.Exists(Ressources.MapFilename))
            {
                try
                {
                    MapFile loaded = MapFile.FromFile(Ressources.MapFilename);
                    if (OnMapLoaded != null)
                        OnMapLoaded(loaded);
                    m_console.Output.AppendLine("Map chargée avec succès");
                }
                /*catch { }*/
                finally { }
            }
        }
        #endregion

        #endregion
        #endregion
    }
}

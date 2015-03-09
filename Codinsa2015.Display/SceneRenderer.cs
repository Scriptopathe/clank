﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Codinsa2015.Views;
namespace Codinsa2015.Rendering
{    
    public enum DataMode
    {
        // Indique que le renderer doit faire un rendu d'un objet correspondant
        // exactement à l'objet détenu par le serveur.
        Direct,
        // Indique que le renderer doit faire un rendu d'une vue de l'objet détenu
        // par le serveur.
        Remote,
    }
    /// <summary>
    /// Représente une vue paramétrable de la scène du jeu.
    /// Cette scène doit posséder une instance de l'état du jeu à jour,
    /// et l'appel à draw dessine sur une texture renvoyée en sortie.
    /// </summary>
    public class SceneRenderer
    {
        #region Variables
        bool __dirty = true;
        /// <summary>
        /// Viewport de rendu de la scène.
        /// </summary>
        Rectangle m_viewport;

        /// <summary>
        /// Mode dans lequel se trouve la scène.
        /// </summary>
        Codinsa2015.Views.SceneMode m_sceneMode;
        
        /// <summary>
        /// Renderer de la map.
        /// </summary>
        MapRenderer m_mapRenderer;

        /// <summary>
        /// Renderer du lobby
        /// </summary>
        LobbyRenderer m_lobbyRenderer;

        /// <summary>
        /// Renderer de la phase de picks.
        /// </summary>
        PickPhaseRenderer m_pickPhaseRenderer;

        /// <summary>
        /// Render target principal de la scène.
        /// </summary>
        RenderTarget2D m_mainRenderTarget;

        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la résolution de la texture sur laquelle va être rendue la scène.
        /// </summary>
        public Rectangle Viewport
        {
            get { return m_viewport; }
            set { m_viewport = value; SetupRenderTarget(); }
        }
        /// <summary>
        /// Obtient une donnée représentant la manière dont vont être récupérées les informations à dessiner
        /// (à distance, ou directement sur le moteur).
        /// </summary>
        public DataMode Mode { get; set; }


        /// <summary>
        /// Obtient le temps de jeu sur le serveur local ou distant.
        /// </summary>
        /// <returns></returns>
        public GameTime GetTime()
        {
            switch(Mode)
            {
                case DataMode.Remote:
                    throw new NotImplementedException();
                case DataMode.Direct:
                    return GameServer.GetSrvTime();
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Obtient le mode actuel de la scène sur le serveur local ou distant.
        /// </summary>
        /// <returns></returns>
        public Views.SceneMode GetSceneMode()
        {
            switch (Mode)
            {
                case DataMode.Remote:
                    throw new NotImplementedException();
                case DataMode.Direct:
                    return (Views.SceneMode)GameServer.GetSrvScene().Mode;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region State
        /// <summary>
        /// Si en mode Direct, instance du serveur qui tourne actuellement.
        /// </summary>
        public Server.GameServer GameServer { get; set; }
        /// <summary>
        /// Si en mode Remote, instance du serveur distant.
        /// </summary>
        public Views.Client.State ServerRemote { get; set; }
        /// <summary>
        /// Obtient le render target principal sur lequel doit être dessiné la scène.
        /// </summary>
        public RenderTarget2D MainRenderTarget { get { return m_mainRenderTarget; } }
        /// <summary>
        /// Obtient une référence vers le map renderer de la scène.
        /// </summary>
        public MapRenderer MapRdr { get { return m_mapRenderer; } }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de Scene Renderer avec les paramètres donnés.
        /// </summary>
        /// <param name="mode"></param>
        public SceneRenderer(DataMode mode)
        {
            m_mapRenderer = new MapRenderer(this);
            m_lobbyRenderer = new LobbyRenderer(this);
            m_pickPhaseRenderer = new PickPhaseRenderer(this);
        }

        /// <summary>
        /// Mets à jour l'état de la scène à partir de l'objet state communiquant avec
        /// le serveur.
        /// </summary>
        /// <param name="state"></param>
        public void UpdateRemoteState(Codinsa2015.Views.Client.State state)
        {
            __dirty = false;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Crée le render target nécessaire au dessin.
        /// </summary>
        public void SetupRenderTarget()
        {
            m_mainRenderTarget = new RenderTarget2D(Ressources.Device,
                m_viewport.Width, m_viewport.Height,
                false, SurfaceFormat.Color, DepthFormat.None,
                0, RenderTargetUsage.PreserveContents);
        }

        /// <summary>
        /// Charge le contenu nécessaire pour le fonctionnement de la scène.
        /// </summary>
        public void LoadContent()
        {

            // Setup du map renderer
            MapRdr.UnitSize = 32;
            MapRdr.VisionDisplayed = EntityType.Team1;
            MapRdr.Viewport = new Rectangle(0, 25, Viewport.Width, Viewport.Height - 125);
            MapRdr.LoadContent();

            // Setup du lobby renderer
            m_lobbyRenderer.LoadContent();

            // Setup du Pick Phase renderer
            m_pickPhaseRenderer.LoadContent();
        }

        /// <summary>
        /// Dessine cette scène.
        /// </summary>
        public void Draw(SpriteBatch batch, GameTime time)
        {
            if (Mode == DataMode.Remote && __dirty)
                throw new Exception("Appel à UpdateRemoteState manquant pour un renderer en mode Remote.");

            if (Mode == DataMode.Direct)
                m_sceneMode = (Views.SceneMode)GameServer.GetSrvScene().Mode; // TODO adapter auto au serveur.
            else
                throw new NotImplementedException();

            switch (m_sceneMode)
            {
                case SceneMode.Game:
                    m_mapRenderer.Draw(batch, time, m_mainRenderTarget);
                    break;
                case SceneMode.Lobby:
                    m_lobbyRenderer.Draw(batch, time, m_mainRenderTarget);
                    break;
                case SceneMode.Pick:
                    m_pickPhaseRenderer.Draw(batch, time, m_mainRenderTarget);
                    break;
            }

            __dirty = true;
        }

        #region Draw


        #endregion
        #endregion
    }
}
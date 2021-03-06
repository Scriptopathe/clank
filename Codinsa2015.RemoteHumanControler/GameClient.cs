﻿using System;
using System.Collections.Generic;
using System.Linq;
using Codinsa2015.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Codinsa2015.Rendering;
using Codinsa2015.Server.Entities;
using Codinsa2015.Views.Client;
namespace Codinsa2015.RemoteHumanControler
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameClient : Microsoft.Xna.Framework.Game
    {
        public const int __DEBUG_PORT = 5000;

        #region Variables
        public static GameClient Instance;
        GraphicsDeviceManager m_graphics;
        SpriteBatch m_batch;
        State m_server;
        SceneRenderer m_renderer;
        HumanControler m_controler;
        #endregion

        #region Properties
        public State Server { get { return m_server; } }
        public SceneRenderer Renderer { get { return m_renderer; } }
        public HumanControler Controler { get { return m_controler; } }
        public ServerStateSnapshot Snapshot { get { return m_renderer.ServerRemote; } }
        #endregion

        #region Server communication

        #endregion


        #region Methods
        /// <summary>
        /// Obtient la taille de l'écran.
        /// </summary>
        /// <returns></returns>
        static Vector2 GetScreenSize()
        {
            return new Vector2(1366, 768 + 100);//new Vector2(800, 600 + 100);//new Vector2(1366, 768 + 100);
        }

        public GameClient()
        {
            Instance = this;
            m_graphics = new GraphicsDeviceManager(this);
            m_server = new State();
            m_renderer = new SceneRenderer(DataMode.Remote);
            Content.RootDirectory = "Content";
            m_graphics.PreferredBackBufferWidth = (int)GameClient.GetScreenSize().X;
            m_graphics.PreferredBackBufferHeight = (int)GameClient.GetScreenSize().Y;
            m_graphics.SynchronizeWithVerticalRetrace = false;
            m_graphics.IsFullScreen = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            SetupControler();
            TCPHelper.Initialize(__DEBUG_PORT, "127.0.0.1", "Joueur 1 lol mdr");
            m_graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            base.Initialize();
        }

        /// <summary>
        /// Initialise le contrôleur.
        /// </summary>
        void SetupControler()
        {
            m_controler = new HumanControler(this, null);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";

            // Charge les ressources (doit être fait après l'appel à BindGraphicsClients()).$
            Ressources.LoadRessources(GraphicsDevice, Content);
            Ressources.ScreenSize = GetScreenSize();

            m_batch = new SpriteBatch(Ressources.Device);
            m_renderer.ServerRemote = new ServerStateSnapshot(m_server);
            m_renderer.Viewport = new Rectangle(0, 0, (int)GameClient.GetScreenSize().X, (int)GameClient.GetScreenSize().Y);
            m_renderer.LoadContent();
            m_controler.LoadContent();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            m_controler.Update(gameTime);
            m_renderer.UpdateRemoteState();

            // Mise à jour des entrées claviers.
            Input.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Dessine le contenu du serveur.
            base.Draw(gameTime);

            // Dessin de la map
            Renderer.Draw(m_batch, gameTime);

            // Dessin des éléments du contrôleur.
            //m_controler.Draw(m_batch, gameTime);


            // Dessine le render target principal sur le back buffer.
            m_batch.GraphicsDevice.SetRenderTarget(null);
            m_batch.Begin();
            m_batch.Draw(Renderer.MainRenderTarget, Vector2.Zero, Color.White);
            m_batch.End();
        }
        #endregion
    }
}

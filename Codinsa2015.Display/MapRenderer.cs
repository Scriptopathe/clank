﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Rendering
{
    /// <summary>
    /// Classe contenant toutes les vues nécessaires pour que le renderer puisse effectuer un
    /// render de la map en mode Remote.
    /// </summary>
    public class MapStateView
    {
        public Views.MapView Map { get; set; }
        public List<Views.EntityBaseView> Entities { get; set; }
        public Views.VisionMapView VisionMap { get; set; }
        public List<Views.SpellcastBaseView> SpellCasts { get; set; }
    }

    /// <summary>
    /// Représente une table de passabilité qui peut avoir 2 représentations internes mais qui n'expose qu'une interface.
    /// </summary>
    public class PassabilityTable
    {
        public DataMode Mode { get; set; }
        public bool[,] DirectData { get; set; }
        public List<List<bool>> RemoteData { get; set; }

        public static implicit operator bool[,](PassabilityTable table)
        {
            return table.DirectData;
        }

        public static implicit operator List<List<bool>>(PassabilityTable table)
        {
            return table.RemoteData;
        }

        public bool this[int x, int y]
        {
            get
            {
                switch (Mode)
                {
                    case DataMode.Direct: return DirectData[x, y]; break;
                    case DataMode.Remote: return RemoteData[x][y]; break;
                }
                throw new NotImplementedException();
            }
            set
            {
                switch (Mode)
                {
                    case DataMode.Direct: DirectData[x, y] = value; break;
                    case DataMode.Remote: RemoteData[x][y] = value; break;
                }
                throw new NotImplementedException();
            }
        }

        public int GetLength(int dimension)
        {
            switch(Mode)
            {
                case DataMode.Direct:
                    return DirectData.GetLength(dimension);
                case DataMode.Remote:
                    if (dimension == 0)
                        return RemoteData.Count;
                    else
                        return RemoteData[0].Count;
            }
            throw new NotImplementedException();
        }

        public PassabilityTable(bool[,] directData)
        {
            Mode = DataMode.Direct;
            DirectData = directData;
        }

        public PassabilityTable(List<List<bool>> remoteData)
        {
            Mode = DataMode.Remote;
            RemoteData = remoteData;
        }
    }
    /// <summary>
    /// Classe gérant le rendu d'une MapView.
    /// </summary>
    public class MapRenderer
    {
        #region Constants
        public bool SMOOTH_LIGHT = true;
        #endregion

        #region Variables
        /// <summary>
        /// Rectangle sur lequel va être dessinée la map.
        /// </summary>
        Rectangle m_viewport;
        /// <summary>
        /// Référence vers le renderer chargé de dessiner les entités.
        /// </summary>
        EntityRenderer m_entityRenderer;
        /// <summary>
        /// Référence vers le renderer chargé de dessiner les spell casts.
        /// </summary>
        SpellcastRenderer m_spellCastRenderer;
        /// <summary>
        /// Référence vers le renderer de la scène.
        /// </summary>
        SceneRenderer m_sceneRenderer;
        /// <summary>
        /// Render Target des tiles
        /// </summary>
        RenderTarget2D m_tilesRenderTarget;
        /// <summary>
        /// Render target des entities.
        /// </summary>
        RenderTarget2D m_entitiesRenderTarget;
        RenderTarget2D m_tmpRenderTarget;
        RenderTarget2D m_tmpRenderTarget2;
        /// <summary>
        /// Effet de flou gaussien.
        /// </summary>
        GraphicsHelpers.GaussianBlur m_blur;
        /// <summary>
        /// Scrolling de la map (px).
        /// </summary>
        Point m_scrolling;
        #endregion

        #region properties
        /// <summary>
        /// Obtient une référence vers le Scene Renderer parent de ce Map Renderer.
        /// </summary>
        public SceneRenderer SceneRenderer { get { return m_sceneRenderer; } }
        /// <summary>
        /// Indique les équipes desquelles la vision doit être affichée.
        /// </summary>
        public Views.EntityType VisionDisplayed { get; set; }
        /// <summary>
        /// Taille d'une unité métrique en pixels.
        /// </summary>
        public int UnitSize { get; set; }
        /// <summary>
        /// Obtient le scrolling de la map (px).
        /// </summary>
        public Point Scrolling
        {
            get { return m_scrolling; }
            set
            {
                m_scrolling = value;
                m_scrolling.X = Math.Max(0, Math.Min((Passability.GetLength(0) * UnitSize) - m_viewport.Width, m_scrolling.X));
                m_scrolling.Y = Math.Max(0, Math.Min((Passability.GetLength(1) * UnitSize) - m_viewport.Height, m_scrolling.Y));
            }
        }
        /// <summary>
        /// Valeur divisant la taille du render target sur lesquels seront dessinés les tiles, en vue
        /// d'un filtrage bilinéaire matériel lors de la remise à l'échelle.
        /// Une valeur + grande => tiles plus arrondis (et meilleures perfs)
        /// Valeur + petite     => tiles plus nets / carrés
        /// </summary>
        const int TILE_SCALE = 8;
        #endregion

        #region State
        /// <summary>
        /// Si en mode Remote => vue de la map qui sera dessinée.
        /// </summary>
        public MapStateView MapView { get; set; }
        /// <summary>
        /// Si le en mode Direct => map qui sera dessinée.
        /// </summary>
        public Codinsa2015.Server.Map Map { get { return m_sceneRenderer.GameServer.GetSrvScene().Map; } }

        /// <summary>
        /// Obtient la passabilité de la map.
        /// </summary>
        PassabilityTable Passability { 
            get
            {
                switch(m_sceneRenderer.Mode)
                {
                    case DataMode.Direct:
                        return new PassabilityTable(Map.Passability);
                    case DataMode.Remote:
                        return new PassabilityTable(MapView.Map.Passability);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la/les teams données ont la vision au point donné.
        /// </summary>
        /// <returns></returns>
        public bool HasVision(Views.EntityType teams, Vector2 position)
        {
            switch(m_sceneRenderer.Mode)
            {
                case DataMode.Direct:
                    return Map.Vision.HasVision((Server.Entities.EntityType)teams, position);
                case DataMode.Remote:
                    teams &= (Views.EntityType.Team1 | Views.EntityType.Team2);
                    return (MapView.VisionMap.Vision[(int)position.X][(int)position.Y] & (Views.VisionFlags)teams) != 0;
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Obtient ou définit le rectangle sur lequel va être dessiné la map.
        /// </summary>
        public Rectangle Viewport
        {
            get { return m_viewport; }
            set { m_viewport = value; }
        }

        public Vector2 ScrollingVector2 
        { 
            get { return new Vector2(Scrolling.X, Scrolling.Y); } 
            set { Scrolling = new Point((int)value.X, (int)value.Y); } 
        }
        #endregion
        
        #region Methods

        /// <summary>
        /// Crée une nouvelle instance du map renderer.
        /// </summary>
        public MapRenderer(SceneRenderer sceneRenderer)
        {
            m_sceneRenderer = sceneRenderer;
            m_entityRenderer = new EntityRenderer(this);
            m_spellCastRenderer = new SpellcastRenderer(this);
        }

        #region Setup
        /// <summary>
        /// Charge les ressources nécessaires pour ce map renderer.
        /// </summary>
        public void LoadContent()
        {
            m_blur = new GraphicsHelpers.GaussianBlur(Ressources.Content);
            m_blur.ComputeKernel(1, 1);
        }

        /// <summary>
        /// Crée les render targets.
        /// </summary>
        void SetupRenderTargets()
        {
            if (m_entitiesRenderTarget != null)
            {
                // Si on a pas besoin de recreer le render target => on return.
                if (m_entitiesRenderTarget.Width == Viewport.Width && m_entitiesRenderTarget.Height == Viewport.Height)
                    return;

                // Sinon on supprime tout
                m_entitiesRenderTarget.Dispose();
                m_tilesRenderTarget.Dispose();
            }
            m_entitiesRenderTarget = new RenderTarget2D(Ressources.Device, Viewport.Width, Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            m_tmpRenderTarget = new RenderTarget2D(Ressources.Device, Viewport.Width, Viewport.Height);
            m_tmpRenderTarget2 = new RenderTarget2D(Ressources.Device, Viewport.Width, Viewport.Height);
            SetupTileRenderTarget();
            m_blur.ComputeOffsets(Viewport.Width, Viewport.Height);
        }

        Dictionary<Point, RenderTarget2D> m_tilesRenderTargets = new Dictionary<Point, RenderTarget2D>();
        /// <summary>
        /// Crée le render target des tiles (s'adapted à la taille des cases).
        /// </summary>
        void SetupTileRenderTarget()
        {
            Point resolution = new Point(Viewport.Width / TILE_SCALE, Viewport.Height / TILE_SCALE);
            if (m_tilesRenderTargets.ContainsKey(resolution))
                m_tilesRenderTarget = m_tilesRenderTargets[resolution];
            else
            {
                m_tilesRenderTarget = new RenderTarget2D(Ressources.Device, resolution.X, resolution.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                m_tilesRenderTargets.Add(resolution, m_tilesRenderTarget);
            }
        }
        #endregion


        Vector2 __oldScroll;
        float __oldUnitSize;
        float __xShaderTime;
        public void Draw(SpriteBatch batch, GameTime time, RenderTarget2D output)
        {
            // Effectue une setup des render target
            SetupRenderTargets();

            batch.GraphicsDevice.SetRenderTarget(m_tilesRenderTarget);
            // batch.GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            
            // Dessin de debug de la map.
            int beginX = Math.Max(0, Scrolling.X / UnitSize);
            int beginY = Math.Max(0, Scrolling.Y / UnitSize);
            int endX = Math.Min(beginX + (Viewport.Width / UnitSize + 1), Passability.GetLength(0));
            int endY = Math.Min(beginY + (Viewport.Height / UnitSize + 1), Passability.GetLength(1));

            int smoothAlpha = (__oldScroll == ScrollingVector2 && UnitSize == __oldUnitSize) ? 25 : 255;
            bool[,] passability = Passability;
            for (int x = beginX; x < endX; x++)
            {
                for (int y = beginY; y < endY; y++)
                {
                    Point drawPos = new Point(x * UnitSize - Scrolling.X, y * UnitSize - Scrolling.Y);
                    int r = passability[x, y] ? 0 : 255;
                    int b = HasVision(VisionDisplayed, new Vector2(x, y)) ? 255 : 0;
                    int a = SMOOTH_LIGHT ? smoothAlpha : 255;
                    batch.Draw(Ressources.DummyTexture, new Rectangle(drawPos.X / TILE_SCALE, drawPos.Y / TILE_SCALE, UnitSize / TILE_SCALE, UnitSize / TILE_SCALE), new Color(r, 0, b, a));

                }
            }
            batch.End();

            // Dessin des entités
            batch.GraphicsDevice.SetRenderTarget(m_entitiesRenderTarget);
            batch.GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            switch(m_sceneRenderer.Mode)
            {
                case DataMode.Direct:
                    foreach (var kvp in Map.Entities) 
                    {
                        m_entityRenderer.Draw(batch, time, kvp.Value);
                    }
                    foreach (var cast in Map.Spellcasts) 
                    {
                        float radius = 0.0f;
                        Vector2 size = Vector2.Zero;
                        Codinsa2015.Server.Shapes.CircleShape circ = cast.GetShape() as Codinsa2015.Server.Shapes.CircleShape;
                        Codinsa2015.Server.Shapes.RectangleShape rect = cast.GetShape() as Codinsa2015.Server.Shapes.RectangleShape;
                        if (circ != null) radius = circ.Radius;
                        if (rect != null) size = new Vector2(rect.Width, rect.Height);

                        m_spellCastRenderer.Draw(
                            batch,
                            new Views.GenericShapeView()
                            {
                                Position = cast.GetShape().Position,
                                Radius = radius,
                                Size = size,
                                ShapeType = (circ == null) ? Views.GenericShapeType.Rectangle : Views.GenericShapeType.Circle
                            },
                            ((Server.Spellcasts.SpellcastBase)cast).Name);
                    }
                    break;
                case DataMode.Remote:
                    foreach(var entity in MapView.Entities)
                    {
                        // m_entityRenderer.Draw(batch, time, entity.Position, entity.Type, entity.Role);
                    }
                    foreach(var cast in MapView.SpellCasts)
                    {
                        m_spellCastRenderer.Draw(batch, cast.Shape, cast.Name);
                    }
                    break;
            }
            batch.End();


            // Blur
            /*for (int i = 0; i < BLUR_PASSES; i++)
            {
                m_blur.PerformGaussianBlur(m_tilesRenderTarget, m_tmpRenderTarget, m_tmpRenderTarget2, batch);
                m_blur.PerformGaussianBlur(m_tmpRenderTarget2, m_tmpRenderTarget, m_tilesRenderTarget, batch);
            }*/

            // Dessin du tout
            batch.GraphicsDevice.SetRenderTarget(output);
            batch.GraphicsDevice.Clear(Color.DarkGray);
            Ressources.MapEffect.Parameters["xSourceTexture"].SetValue(m_tilesRenderTarget);
            Ressources.MapEffect.Parameters["scrolling"].SetValue(new Vector2(Scrolling.X / (float)Viewport.Width, Scrolling.Y / (float)Viewport.Height));
            Ressources.MapEffect.Parameters["xPixelSize"].SetValue(new Vector2(1.0f / Viewport.Width, 1.0f / Viewport.Height));
            Ressources.MapEffect.Parameters["xUnitSize"].SetValue(UnitSize);
            __xShaderTime += 0.0005f;
            Ressources.MapEffect.Parameters["xTime"].SetValue(__xShaderTime);

            // Dessine les tiles avec le shader
            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, Ressources.MapEffect);
            batch.Draw(m_tilesRenderTarget, Viewport, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.Map);
            batch.End();

            // Dessine les entités.
            batch.Begin();
            batch.Draw(m_entitiesRenderTarget, Viewport, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, GraphicsHelpers.Z.Entities);
            batch.End();



            __oldScroll = ScrollingVector2;
            __oldUnitSize = UnitSize;
        }

        #endregion

        #region Utils
        /// <summary>
        /// Obtient la position dans l'espace de l'écran à partir d'une position
        /// en unités métriques. 
        /// (prends en compte viewport, scrolling, scaling).
        /// </summary>
        /// <returns></returns>
        public Vector2 ToScreenSpace(Vector2 mapPos)
        {
            return mapPos * UnitSize - ScrollingVector2 + new Vector2(Viewport.X, Viewport.Y);
        }

        /// <summary>
        /// Obtient la position dans l'espace de la map (unités métriques) à
        /// partir d'une position à l'écran.
        /// </summary>
        /// <param name="screenSpacePos"></param>
        /// <returns></returns>
        public Vector2 ToMapSpace(Vector2 screenSpacePos)
        {
            return (screenSpacePos + ScrollingVector2 - new Vector2(Viewport.X, Viewport.Y)) / UnitSize;
        }
        #endregion
    }
}

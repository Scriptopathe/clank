﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Codinsa2015
{

    /// <summary>
    /// Classe ayant la responsabilité de s'occuper des ressources du jeu.
    /// </summary>
    public static class Ressources
    {
        public static ContentManager Content;
        public static GraphicsDevice Device;
        public static Texture2D DummyTexture;
        public static Vector2 ScreenSize { get; set; }
        public static string MapFilename = "Content/map.txt";

        #region ByName
        static Dictionary<string, Texture2D> s_textureCache = new Dictionary<string, Texture2D>();
        public static Texture2D GetSpellTexture(string spellname)
        {
            Texture2D tex;
            try
            {
                if (s_textureCache.ContainsKey(spellname))
                    return s_textureCache[spellname];
                else
                {
                    tex = Content.Load<Texture2D>("textures/spells/" + spellname);
                    s_textureCache.Add(spellname, tex);
                }
            }
            catch
            {
                tex = DummyTexture;
                s_textureCache.Add(spellname, tex);
            }

            return tex;
        }
        #endregion

        #region Effects
        public static Effect MapEffect { get; set; }
        public static Texture2D WallTexture { get; set; }
        public static Texture2D WallBorderTexture { get; set; }
        public static Texture2D GrassTexture { get; set; }
        public static Texture2D LavaTexture { get; set; }

        /// <summary>
        /// Matrice de transformation pour les dessins 2D.
        /// </summary>
        public static Matrix PlaneTransform2D { get; set; }
        #endregion
        #region Imported
        /// <summary>
        /// Obtient une police de caractère utilisable pour l'affichage de texte.
        /// </summary>
        public static SpriteFont Font
        {
            get;
            private set;
        }

        public static SpriteFont CourrierFont
        {
            get;
            private set;
        }

        public static SpriteFont NumbersFont
        {
            get;
            private set;
        }

        public static Texture2D LifebarEmpty
        {
            get;
            set;
        }

        public static Texture2D LifebarFull
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient une texture représentant une marque de sélection.
        /// </summary>
        public static Texture2D SelectMark
        {
            get;
            private set;
        }

        public static Texture2D HighlightMark
        {
            get;
            private set;
        }

        public static Texture2D CanMoveMark
        {
            get;
            private set;
        }
        public static Texture2D MenuItem
        {
            get;
            private set;
        }

        public static Texture2D MenuItemHover
        {
            get;
            private set;
        }

        public static Texture2D TextBox
        {
            get;
            set;
        }

        public static Texture2D Menu
        {
            get;
            private set;
        }

        public static Texture2D Cursor
        {
            get;
            private set;
        }
        #endregion

        public static Texture2D IconMage { get; set; }
        public static Texture2D IconFighter { get; set; }
        public static Texture2D IconTank { get; set; }
        public static Texture2D BlueTower { get; set; }
        public static Texture2D RedTower { get; set; }
        public static Texture2D BlueFighter { get; set; }
        public static Texture2D BlueMage { get; set; }
        public static Texture2D BlueTank { get; set; }
        public static Texture2D RedFighter { get; set; }
        public static Texture2D RedMage { get; set; }
        public static Texture2D RedTank { get; set; }
        public static Texture2D RedVirus { get; set; }
        public static Texture2D BlueVirus { get; set; }
        public static Texture2D Fireball { get; set; }
        public static Texture2D SpellZone { get; set; }
        public static Texture2D BlindIcon { get; set; }
        public static Texture2D SilenceIcon { get; set; }
        public static Texture2D Team1Wins { get; set; }
        public static Texture2D Team2Wins { get; set; }
        public static Texture2D BlueDatacenter { get; set; }
        public static Texture2D RedDatacenter { get; set; }
        public static Texture2D BlueSpawner { get; set; }
        public static Texture2D RedSpawner { get; set; }
        public static Texture2D CampMonster { get; set; }
        public static Texture2D Router { get; set; }
        public static Texture2D BlueWard { get; set; }
        public static Texture2D RedWard { get; set; }
        /// <summary>
        /// Charge les ressources à partir du content manager passé en paramètre.
        /// </summary>
        /// <param name="content"></param>
        public static void LoadRessources(GraphicsDevice device, ContentManager content)
        {
            Content = content;
            Device = device;
            Router = content.Load<Texture2D>("textures/charsets/router");
            BlueWard = content.Load<Texture2D>("textures/charsets/ward_bleu");
            RedWard = content.Load<Texture2D>("textures/charsets/ward_rouge");
            BlueSpawner = content.Load<Texture2D>("textures/charsets/spawner_bleu");
            RedSpawner = content.Load<Texture2D>("textures/charsets/spawner_rouge");
            CampMonster = content.Load<Texture2D>("textures/charsets/camp_monster");
            BlueDatacenter = content.Load<Texture2D>("textures/charsets/nexus_bleu"); 
            RedDatacenter = content.Load<Texture2D>("textures/charsets/nexus_rouge");
            Team1Wins = content.Load<Texture2D>("textures/team1wins");
            Team2Wins = content.Load<Texture2D>("textures/team2wins");
            BlindIcon = content.Load<Texture2D>("textures/effects/blind");
            SilenceIcon = content.Load<Texture2D>("textures/effects/silence");
            SpellZone = content.Load<Texture2D>("textures/effects/spellzone");
            Fireball = content.Load<Texture2D>("textures/effects/fireball");
            BlueVirus = content.Load<Texture2D>("textures/charsets/Virus_bleu");
            RedVirus = content.Load<Texture2D>("textures/charsets/Virus_rouge");
            BlueFighter = content.Load<Texture2D>("textures/charsets/combattant_bleu");
            BlueMage = content.Load<Texture2D>("textures/charsets/mage_bleu");
            BlueTank = content.Load<Texture2D>("textures/charsets/tank_bleu");
            RedFighter = content.Load<Texture2D>("textures/charsets/combattant_rouge");
            RedMage = content.Load<Texture2D>("textures/charsets/mage_rouge");
            RedTank = content.Load<Texture2D>("textures/charsets/tank_rouge");
            BlueTower = content.Load<Texture2D>("textures/charsets/tour_bleue");
            RedTower = content.Load<Texture2D>("textures/charsets/tour_rouge");
            IconMage = content.Load<Texture2D>( "textures/icons/mage");
            IconFighter = content.Load<Texture2D>( "textures/icons/fighter");
            IconTank = content.Load<Texture2D>( "textures/icons/tank");
            DummyTexture = content.Load<Texture2D>( "textures/dummy");
            Font = content.Load<SpriteFont>( "textfont");
            NumbersFont = content.Load<SpriteFont>( "numbers_font");
            CourrierFont = content.Load<SpriteFont>( "courrier-16pt");
            SelectMark = content.Load<Texture2D>( "textures/select_mark");
            MenuItem = content.Load<Texture2D>( "textures/gui/menu_item");
            MenuItemHover = content.Load<Texture2D>( "textures/gui/menu_item_hover");
            Menu = content.Load<Texture2D>( "textures/gui/menu");
            Cursor = content.Load<Texture2D>( "textures/gui/cursor");
            HighlightMark = content.Load<Texture2D>( "textures/highlight_mark");
            CanMoveMark = content.Load<Texture2D>( "textures/canmove_mark");
            TextBox = content.Load<Texture2D>( "textures/gui/textbox");
            LifebarEmpty = content.Load<Texture2D>( "textures/gui/lifebar_empty");
            LifebarFull = content.Load<Texture2D>( "textures/gui/lifebar_full");
            LavaTexture = content.Load<Texture2D>( "textures/lava");
            // Effet de la map
            WallTexture = content.Load<Texture2D>( "textures/wall");
            WallBorderTexture = content.Load<Texture2D>( "textures/border");
            GrassTexture = content.Load<Texture2D>( "textures/grass");
            MapEffect = content.Load<Effect>("shaders/mapshader");
            MapEffect.Parameters["xBorderTexture"].SetValue(WallBorderTexture);
            MapEffect.Parameters["xWallTexture"].SetValue(WallTexture);
            MapEffect.Parameters["xGrassTexture"].SetValue(GrassTexture);
            MapEffect.Parameters["xLavaTexture"].SetValue(LavaTexture);

            /* Matrice
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            PlaneTransform2D = halfPixelOffset * projection;*/
        }

    }
    

}

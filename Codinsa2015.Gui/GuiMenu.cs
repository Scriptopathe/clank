﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Codinsa2015.EnhancedGui
{
    /// <summary>
    /// Représente un menu dans la GUI.
    /// </summary>
    public class GuiMenu : GuiWidget
    {
        #region Delegate / Events / Classes
        public delegate void ItemSelectedDelegate();

 
        /// <summary>
        /// Représente un item du menu.
        /// </summary>
        public class GuiMenuItem
        {
            /// <summary>
            /// Texte affiché par cet item.
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// Icône affiché sur cet item.
            /// </summary>
            public Texture2D Icon { get; set; }
            /// <summary>
            /// Id de l'event déclenché par cet item.
            /// </summary>
            public event ItemSelectedDelegate ItemSelected;
            /// <summary>
            /// Retourne vrai si l'item est activé (cliquable).
            /// </summary>
            public bool IsEnabled { get; set; }
            /// <summary>
            /// Dispatche l'event ItemSelected.
            /// </summary>
            public void DispatchItemSelectedEvent()
            {
                if (ItemSelected != null && IsEnabled)
                    ItemSelected();
            }

            public GuiMenuItem()
            {
                Text = "";
                Icon = null;
                IsEnabled = true;
            }

            public GuiMenuItem(string text)
            {
                Text = text;
                Icon = null;
                IsEnabled = true;
            }

            public GuiMenuItem(string text, Texture2D icon)
            {
                Text = text;
                Icon = icon;
                IsEnabled = true;
            }

            public GuiMenuItem(string text, Texture2D icon, bool isEnabled)
            {
                Text = text;
                Icon = icon;
                IsEnabled = isEnabled;
            }

            public GuiMenuItem(string text, Texture2D icon, bool isEnabled, ItemSelectedDelegate onItemSelected)
            {
                Text = text;
                Icon = icon;
                ItemSelected += onItemSelected;
                IsEnabled = isEnabled;
            }
        }
        #endregion

        #region Variables
        /// <summary>
        /// Id de l'item sur lequel est positionnée la souris.
        /// -1 pour aucun.
        /// </summary>
        protected int m_hoverItemId;
        bool firstFrame = true;
        /// <summary>
        /// Taille de la barre de titre.
        /// </summary>
        int m_titleBarSize = 35;
        /// <summary>
        /// Taille de la marge globale du menu.
        /// </summary>
        int m_mainMarginSize = 8;
        /// <summary>
        /// Taille des marges des items en pixels.
        /// </summary>
        int m_itemMarginSize = 3;
        /// <summary>
        /// Largeur d'un item en pixels.
        /// </summary>
        int m_itemWidth;
        /// <summary>
        /// Hauteur d'un item en pixels.
        /// </summary>
        int m_itemHeight;
        /// <summary>
        /// Taille des icones en pixels.
        /// Les icones sont des textures carrées.
        /// </summary>
        int m_iconSize = 16;

        /// <summary>
        /// Obtient ou définit la texture utilisée pour dessiner la boite principale du menu.
        /// </summary>
        public Texture2D MenuBoxTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit la texture utilisée pour dessiner les items du menu.
        /// </summary>
        public Texture2D ItemBoxTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit la texture utilisée pour dessiner les items du menu, lorsque la souris est dessus.
        /// </summary>
        public Texture2D ItemHoverBoxTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Couleur du texte pour les items désactivés.
        /// </summary>
        public Color DisabledTextColor
        {
            get;
            set;
        }
        /// <summary>
        /// Couleur du texte pour les items.
        /// </summary>
        public Color EnabledTextColor
        {
            get;
            set;
        }
        /// <summary>
        /// Couleur du texte pour les items en surbrillance.
        /// </summary>
        public Color HoverTextColor
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le menu doit se fermer si on clique dessus ou à côté.
        /// </summary>
        public bool CloseOnClick
        {
            get;
            set;
        }
        /// <summary>
        /// Liste des items du menu.
        /// </summary>
        List<GuiMenuItem> m_items;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la taille de la marge globale du menu.
        /// </summary>
        public int MainMarginSize
        {
            get { return m_mainMarginSize; }
            set { m_mainMarginSize = value; }
        }
        /// <summary>
        /// Obtient ou définit la taille de la barre des titres.
        /// </summary>
        public int TitleBarSize
        {
            get { return m_titleBarSize; }
            set { m_titleBarSize = value; }
        }
        /// <summary>
        /// Obtient ou définit la hauteur d'un item.
        /// </summary>
        public int ItemHeight
        {
            get { return m_itemHeight; }
            set { m_itemHeight = value; }
        }
        /// <summary>
        /// Définit la liste des items de ce menu.
        /// </summary>
        public List<GuiMenuItem> Items
        {
            get { return m_items; }
            set { m_items = value; ComputeItemsSize(); }
        }

        /// <summary>
        /// Obtient ou définit la largeur d'un item.
        /// </summary>
        public int ItemWidth
        {
            get { return m_itemWidth; }
            set { m_itemWidth = value; }
        }

        /// <summary>
        /// Obtient ou définit la taille des marges des items.
        /// </summary>
        public int ItemMarginSize
        {
            get { return m_itemMarginSize; }
            set { m_itemMarginSize = value; }
        }
        /// <summary>
        /// Ajoute l'item donné à la liste des items.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(GuiMenuItem item)
        {
            m_items.Add(item);
            ComputeItemsSize();
        }

        /// <summary>
        /// Obtient ou définit le titre du menu.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient la largeur totale du menu.
        /// </summary>
        public int TotalWidth
        {
            get { return m_itemWidth + m_mainMarginSize; }
        }

        /// <summary>
        /// Obtient la hauteur totale du menu.
        /// </summary>
        public int TotalHeight
        {
            get { return m_titleBarSize + m_mainMarginSize * 3 + m_itemHeight * m_items.Count; }
        }

        /// <summary>
        /// Obtient une valeur indiquant si ce menu est visible.
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }
        #endregion 

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de GuiMenu.
        /// </summary>
        public GuiMenu(GuiManager manager) : base(manager)
        {
            manager.AddWidget(this);
            Items = new List<GuiMenuItem>();
            m_hoverItemId = -1;
            Title = "Sans titre";
            MenuBoxTexture = Ressources.Menu;
            ItemBoxTexture = Ressources.MenuItem;
            ItemHoverBoxTexture = Ressources.MenuItemHover;
            EnabledTextColor = Color.White;
            DisabledTextColor = Color.Gray;
            HoverTextColor = Color.White;
            CloseOnClick = true;
            Visible = true;
        }

        /// <summary>
        /// Calcule la taille des items du menu.
        /// </summary>
        void ComputeItemsSize()
        {
            // Détermine la largeur du menu en fonction
            // de l'élément à la plus grande taille.
            int maxStringWidth = 120;
            int maxStringHeight = 20;
            int maxIconWidth = 0;
            const int iconWidth = 32;

            for (int y = 0; y < Items.Count; y++)
            {
                SpriteFont font = Ressources.Font;
                Vector2 size = font.MeasureString(Items[y].Text);
                
                if(Items[y].Icon != null)
                {
                    int w = iconWidth;
                    if (w > maxIconWidth)
                        maxIconWidth = w;
                }

                if (size.X > maxStringWidth)
                {
                    maxStringWidth = (int)size.X;
                }

                if (size.Y > maxStringHeight)
                {
                    maxStringHeight = (int)size.Y;
                }
            }

            
            m_itemWidth = maxStringWidth + m_itemMarginSize*2 + m_iconSize + m_itemMarginSize;
            m_itemHeight = maxStringHeight + m_itemMarginSize*2;
        }
        /// <summary>
        /// Mets à jour le menu.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (!Visible)
                return;
            // Retourne si première frame : évite certains artifacts de clic.
            if(firstFrame)
            {
                firstFrame = false;
                return;
            }

            bool hover = IsHover();
            // Hover
            if(hover)
            {
                m_hoverItemId = -1;
                for (int y = 0; y < Items.Count; y++)
                {
                    Point pos = new Point(0, y * m_itemHeight);
                    Rectangle pxRect = new Rectangle((int)pos.X, (int)pos.Y, m_itemWidth, m_itemHeight);
                    Point mousePos = GetMousePos();
                    if (pxRect.Contains(mousePos))
                    {
                        m_hoverItemId = y;
                    }
                }
            }

            if (hover && Input.IsLeftClickTrigger())
            {
                // Envoie un signal indiquant que la sélection a changé si le menu est cliqué.
                if (m_hoverItemId != -1)
                {
                    Items[m_hoverItemId].DispatchItemSelectedEvent();
                }

                if (CloseOnClick)
                {
                    // Supprime le menu.
                    Dispose();
                }
            }

            // Mise à jour de la taille en fonction des items.
            Size = new Point(TotalWidth, TotalHeight);
        }

        /// <summary>
        /// Dessine les items de ce menu.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            if (!Visible)
                return;

            Vector2 pos = new Vector2(-m_mainMarginSize, -m_titleBarSize);
            Rectangle menuBox = new Rectangle((int)pos.X, (int)pos.Y, m_itemWidth + m_mainMarginSize * 2, m_itemHeight * Items.Count + m_titleBarSize + m_mainMarginSize);
            
            // Dessine la boite principale du menu.
            DrawRectBox(batch, MenuBoxTexture,
                menuBox,
                Color.White,
                1);

            Vector2 tSize = Ressources.Font.MeasureString(Title);
            pos = (new Vector2(menuBox.X, menuBox.Y) 
                + new Vector2(menuBox.Width, m_titleBarSize) / 2
                - new Vector2(tSize.X/2, tSize.Y/2));

            // Dessine le titre du menu.
            DrawString(batch, Ressources.Font, Title, 
                pos,
                EnabledTextColor,
                0.0f,
                Vector2.Zero, 
                1.0f,
                2);

            for (int y = 0; y < Items.Count; y++)
            {
                bool isHover = m_hoverItemId == y;
                pos = new Vector2(0, y * m_itemHeight);
                tSize = Ressources.Font.MeasureString(Items[y].Text);

                // Dessin de la box
                Rectangle pxRect = new Rectangle((int)pos.X, (int)pos.Y, m_itemWidth, m_itemHeight);
                Texture2D t = isHover && Items[y].IsEnabled ? ItemHoverBoxTexture : ItemBoxTexture;
                DrawRectBox(batch, t, pxRect, Color.White, 4);

                // Dessin de l'icone
                if(Items[y].Icon != null)
                {
                    Rectangle dstRect = new Rectangle(pxRect.X + m_itemMarginSize, pxRect.Y + (pxRect.Height - m_iconSize) / 2, m_iconSize, m_iconSize);
                    Color color = Items[y].IsEnabled ? Color.White : new Color(125, 125, 125, 125);
                    Draw(batch, Items[y].Icon, dstRect, null, color, 0.0f, Vector2.Zero, 6);
                }

                // Dessin du texte
                pos = new Vector2(pxRect.X + 2*m_itemMarginSize + m_iconSize, pxRect.Y + pxRect.Height/2 - tSize.Y/2);
                Color textColor = Items[y].IsEnabled ? (isHover ? HoverTextColor : EnabledTextColor ) : DisabledTextColor;
                DrawString(batch, Ressources.Font, Items[y].Text, pos, textColor, 0.0f, Vector2.Zero, 1.0f, 8);
            }
        }

        public override void OnFocusLost()
        {
            Dispose();
        }
        #endregion
    }
}

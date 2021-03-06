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
    /// Représente un contrôle d'entrée de texte dans la gui.
    /// </summary>
    public class GuiTextInput : GuiWidget
    {
        #region Delegate / Events / Classes
        /// <summary>
        /// Delegate de fonction callback de validation de texte.
        /// <param name="sender">TextInput source du message.</param>
        /// </summary>
        public delegate void TextValidatedDelegate(GuiTextInput sender);
        /// <summary>
        /// Evènement déclenché lorsque l'utilisateur appuie sur entrée dans le contrôle de texte.
        /// </summary>
        public event TextValidatedDelegate TextValidated;
        /// <summary>
        /// Evènement déclenché lorsqu'une touche est pressée par l'utilisateur alors que le contrôle a le focus.
        /// </summary>
        public event System.Windows.Forms.KeyPressEventHandler KeyPressed;
        #endregion

        #region Variables
        /// <summary>
        /// Contient le texte en cours d'édition.
        /// </summary>
        StringBuilder m_textBuilder;
        /// <summary>
        /// Représente les entrées de textes précédemment validées.
        /// </summary>
        Stack<string> m_olderEntries;
        /// <summary>
        /// Représente les entrées de textes arrivées après celle en cours d'édition.
        /// </summary>
        Stack<string> m_newerEntries;
        /// <summary>
        /// Retourne la position actuelle du cursor.
        /// </summary>
        int m_cursorPosition;
        /// <summary>
        /// Retourne le nombre de frames écoulées depuis le dernier blink du cursor.
        /// </summary>
        int m_cursorBlinkTime;
        const int CursorVisibleTime = 40;
        const int CursorInvisibleTime = 20;
        #endregion

        #region Properties
        
        /// <summary>
        /// Représente le texte affiché dans ce contrôle.
        /// </summary>
        public string Text
        {
            get { return m_textBuilder.ToString(); }
            set { m_textBuilder.Clear(); m_textBuilder.Append(value); ClampCursorPosition(); }
        }

        /// <summary>
        /// Obtient ou définit l'opacité de ce contrôle.
        /// </summary>
        public int Opacity
        {
            get;
            set;
        }
        #endregion 

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de GuiMenu.
        /// </summary>
        public GuiTextInput(GuiManager manager) : base(manager)
        {
            manager.AddWidget(this);
            Opacity = 255;
            Size = new Point(640, 20);
            IsVisible = true;
            m_textBuilder = new StringBuilder();
            m_newerEntries = new Stack<string>();
            m_olderEntries = new Stack<string>();
        }

        /// <summary>
        /// Mets à jour le menu.
        /// </summary>
        /// <param name="time"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (HasFocus() && IsVisible)
            {
                HandleKeyPress();
            }

            // Curseur
            m_cursorBlinkTime++;
            if (m_cursorBlinkTime > CursorInvisibleTime + CursorVisibleTime)
                m_cursorBlinkTime = 0;
        }
        
        /// <summary>
        /// Effectue le traitement de l'appui de touche.
        /// </summary>
        void HandleKeyPress()
        {
            List<Keys> trigger = Input.GetTriggerKeys();
            if (trigger.Count >= 1)
            {
                // On traite l'appui de touche.
                Keys key = trigger.First();
                bool majUp = (Input.IsPressed(Keys.RightShift) | Input.IsPressed(Keys.LeftShift)) ^ Input.IsPressed(Keys.CapsLock);
                bool isLeftControl = Input.IsPressed(Keys.LeftControl);
                bool isText = true;
                #region Big Switch
                switch (key)
                {
                    #region Letters / Numbers
                    case Keys.A:
                    case Keys.B:
                    case Keys.C:
                    case Keys.D:
                    case Keys.E:
                    case Keys.F:
                    case Keys.G:
                    case Keys.H:
                    case Keys.I:
                    case Keys.J:
                    case Keys.K:
                    case Keys.L:
                    case Keys.M:
                    case Keys.N:
                    case Keys.O:
                    case Keys.P:
                    case Keys.Q:
                    case Keys.R:
                    case Keys.S:
                    case Keys.T:
                    case Keys.U:
                    case Keys.V:
                    case Keys.W:
                    case Keys.Y:
                    case Keys.Z:
                        InsertToCursor(majUp ? key.ToString().ToUpper() : key.ToString().ToLower());
                        break;
                    case Keys.X:
                        InsertToCursor("x");
                        break;
                    case Keys.NumPad0:
                    case Keys.NumPad1:
                    case Keys.NumPad2:
                    case Keys.NumPad3:
                    case Keys.NumPad4:
                    case Keys.NumPad5:
                    case Keys.NumPad6:
                    case Keys.NumPad7:
                    case Keys.NumPad8:
                    #endregion
                    case Keys.NumPad9:
                        InsertToCursor(key.ToString().Remove(0, 6));
                        break;
                    case Keys.OemQuestion:
                        InsertToCursor(majUp ? "/" : ":");
                        break;
                    case Keys.Oem8:
                        InsertToCursor(majUp ? "§" : "!");
                        break;
                    case Keys.OemBackslash:
                        InsertToCursor(majUp ? ">" : "<");
                        break;
                    case Keys.D0:
                        InsertToCursor(isLeftControl ? "@" : "à");
                        break;
                    case Keys.D1:
                        InsertToCursor("&");
                        break;
                    case Keys.D2:
                        InsertToCursor(isLeftControl ? "~" : "é");
                        break;
                    case Keys.D3:
                        InsertToCursor(isLeftControl ? "#" : "\"");
                        break;
                    case Keys.D4:
                        InsertToCursor(isLeftControl ? "{" : "'");
                        break;
                    case Keys.D5:
                        InsertToCursor(isLeftControl ? "[" : "(");
                        break;
                    case Keys.D6:
                        InsertToCursor(isLeftControl ? "|" : "-");
                        break;
                    case Keys.D7:
                        InsertToCursor(isLeftControl ? "`" : "è");
                        break;
                    case Keys.D8:
                        InsertToCursor(isLeftControl ? "\\" : "_");
                        break;
                    case Keys.D9:
                        InsertToCursor(isLeftControl ? "^" : "ç");
                        break;
                    case Keys.OemComma:
                        InsertToCursor(majUp ? "?" : ",");
                        break;
                    case Keys.OemPeriod:
                        InsertToCursor(majUp ? "." : ";");
                        break;
                    case Keys.OemOpenBrackets:
                        InsertToCursor(isLeftControl ? "]" : (majUp ? "°" : ")"));
                        break;
                        
                    case Keys.Space:
                        InsertToCursor(" ");
                        break;
                    case Keys.Subtract:
                        InsertToCursor("-");
                        break;
                    case Keys.Add:
                        InsertToCursor("+");
                        break;
                    case Keys.Multiply:
                        InsertToCursor("*");
                        break;
                    case Keys.Divide:
                        InsertToCursor("/");
                        break;
                    case Keys.OemPlus:
                        InsertToCursor(isLeftControl ? "}" : (majUp ? "+" : "="));
                        break;
                    case Keys.Back:
                        RemoveAtCursor();
                        break;
                    case Keys.Delete:
                        RemoveAfterCursor();
                        break;

                    case Keys.Enter:
                        m_olderEntries.Push(Text);
                        if (TextValidated != null)
                            TextValidated(this);
                        isText = false;
                        break;
                    case Keys.Up:
                        if (m_olderEntries.Count != 0)
                        {
                            m_newerEntries.Push(Text);
                            string str = m_olderEntries.Pop();
                            Text = str;
                            m_cursorPosition = int.MaxValue;
                            ClampCursorPosition();
                        }
                        break;
                    case Keys.Down:
                        if (m_newerEntries.Count != 0)
                        {
                            m_olderEntries.Push(Text);
                            Text = m_newerEntries.Pop();
                            m_cursorPosition = int.MaxValue;
                            ClampCursorPosition();
                        }
                        break;
                    case Keys.Left:
                        m_cursorPosition--;
                        ClampCursorPosition();
                        break;
                    case Keys.Right:
                        m_cursorPosition++;
                        ClampCursorPosition();
                        break;
                }
                #endregion

                // Si le caractère est interprété comme texte, envoie un KeyPress event
                if (isText && KeyPressed != null)
                {
                    // TODO KeyPressed(this, new System.Windows.Forms.KeyPressEventArgs())
                }
            }
        }
        /// <summary>
        /// Insère la chaine passée en paramètre à la position pointée par le curseur.
        /// </summary>
        /// <param name="str"></param>
        void InsertToCursor(string str)
        {
            m_textBuilder.Insert(m_cursorPosition, str);
            m_cursorPosition += str.Length;
        }
        /// <summary>
        /// Supprime le caractère à l'emplacement indiqué par le curseur.
        /// </summary>
        void RemoveAtCursor()
        {
            if (Text.Length >= 1 && m_cursorPosition > 0)
            {
                m_textBuilder.Remove(m_cursorPosition-1, 1);
                m_cursorPosition--;
                ClampCursorPosition();
            }
        }

        /// <summary>
        /// Supprime le caractère à l'emplacement indiqué par le curseur.
        /// </summary>
        void RemoveAfterCursor()
        {
            if (Text.Length >= 1 && m_cursorPosition < Text.Length)
            {
                m_textBuilder.Remove(m_cursorPosition, 1);
                ClampCursorPosition();
            }
        }
        /// <summary>
        /// Positionne le curseur dans un intervalle correct.
        /// </summary>
        void ClampCursorPosition()
        {
            m_cursorPosition = Math.Min(Text.Length, Math.Max(0, m_cursorPosition));
        }

        /// <summary>
        /// Dessine les items de ce menu.
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            if (!IsVisible)
                return;

            const int Margins = 2; // marges en pixels.
            var font = Ressources.CourrierFont;
            Rectangle menuBox = new Rectangle(0, 0, Size.X, Size.Y);
            
            // Dessine la boite principale du menu.
            DrawRectBox(batch, Ressources.TextBox,
                menuBox,
                new Color(255, 255, 255, Opacity),
                1);

            // Dessine le titre du menu.
            DrawString(batch, font, Text, 
                new Vector2(Margins, Margins),
                new Color(0, 0, 0, Opacity),
                0.0f,
                Vector2.Zero, 
                1.0f,
                4);

            // Taille du texte avant le curseur
            Vector2 size = font.MeasureString(Text.Substring(0, m_cursorPosition));
            // Dessine le titre du menu.
            bool isCursorVisible = m_cursorBlinkTime < CursorVisibleTime;
            if (isCursorVisible)
            {
                DrawString(batch, font, "|",
                    new Vector2(Margins + size.X, Margins),
                    new Color(0, 0, 0, Opacity),
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    4);
            }
        }
        #endregion
    }
}

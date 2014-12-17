﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente un appel à SpriteBatch.DrawString().
    /// </summary>
    class CommandSpriteBatchDrawString : Command
    {
        public RemoteSpriteBatch Batch { get; set; }
        public RemoteSpriteFont Font { get; set; }
        public string String { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float LayerDepth { get; set; }
        public CommandSpriteBatchDrawString(
            RemoteSpriteBatch batch, 
            RemoteSpriteFont font,
            string str,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            float layerDepth)
        {
            Batch = batch;
            String = str;
            Font = font;
            Position = position;
            Color = color;
            Rotation = rotation;
            Origin = origin;
            Scale = scale;
            LayerDepth = layerDepth;
        }
    }
}

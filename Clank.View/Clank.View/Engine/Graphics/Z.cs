﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics
{
    /// <summary>
    /// Contient des informations sur le z-layer à utiliser pour différents types d'objets graphiques.
    /// </summary>
    public static class Z
    {
        #region Steps
        public const float Front = 0.0f;
        public const float Back = 1.0f;
        public const float FrontStep = (Front - Back) * 0.001f;
        public const float BackStep = (Back - Front) * 0.001f;
        #endregion

        #region Entities
        public const float Entities     = 0.9f;
        public const float Background   = 1.0f;
        public const float Map          = 1.0f;
        public const float HeroControler = 0.15f;
        public const float GUI          = 0.1f;
        public const float Particles    = 0.2f;
        #endregion

    }
}

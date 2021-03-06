﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Graphics.Client
{
    /// <summary>
    /// Représente un client graphique.
    /// </summary>
    public abstract class GraphicsClient
    {
        /// <summary>
        /// Procède à l'exécution de la commande donnée.
        /// </summary>
        public abstract void ProcessCommand(Server.Command command);
    }
}

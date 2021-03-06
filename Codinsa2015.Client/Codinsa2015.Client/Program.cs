﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Codinsa2015.Views.Client;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Client
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            /*
            using (Game1 game = new Game1())
            {
                game.Run();
            }*/
            string str = Console.ReadLine();
            TCPHelper.Initialize(Codinsa2015.Server.GameServer.__DEBUG_PORT, "127.0.0.1", str);
            State state = new State();
            Console.WriteLine("Client started...");

            // On récupère des données concernant la map.
            Views.GameStaticDataView data = state.GetStaticData();
            Console.WriteLine("Got static data");

            // On récupère les id de nos spells.
            List<int> mySpells = state.GetMySpells();
           
            Console.WriteLine("Got spells");
            int spellId = 0;
            while (true)
            {
                // On récupère les entités en vue.
                var entities = state.GetEntitiesInSight();
                Console.WriteLine("Entities in sight : " + entities.Count);
                if(entities.Count != 0)
                {
                    var entity = entities[0];
                    // Si le déplacement auto est terminé, on démarre un autre déplacement.
                    if(!state.IsAutoMoving())
                    {
                        Console.WriteLine("Moving to entity " + entity.ID + ", position = " + entity.Position);
                        state.StartMoveTo(entity.Position);
                    }

                    spellId++; spellId %= mySpells.Count;
                    // On récupère la description de notre sort : elle indique comment l'utiliser ainsi que ses
                    // caractéristiques.
                    Views.SpellLevelDescriptionView spell = state.GetMySpellCurrentLevelDescription(spellId);

                    Views.EntityBaseView e = state.GetEntityById(1);
                    var positionView = state.GetMyPosition();

                    // Transformation des vector2 en vecteurs XNA.
                    Vector2 position = new Vector2(positionView.X, positionView.Y);
                    Vector2 entityPosition = new Vector2(entity.Position.X, entity.Position.Y);

                    // On utilise le spell.
                    Console.WriteLine("Using spell n° " + spellId + ", targetting type = " + spell.TargetType.Type);
                    switch(spell.TargetType.Type)
                    {
                        case Views.TargettingType.Direction:
                            Vector2 dir = entityPosition - position;
                            dir.Normalize();
                            if (entityPosition == position)
                                dir = new Vector2(1, 0);

                            Views.Vector2 dirView = new Views.Vector2(dir.X, dir.Y);
                            state.UseMySpell(spellId, new Views.SpellCastTargetInfoView() { TargetDirection = dirView, Type = Views.TargettingType.Direction });
                            break;

                        case Views.TargettingType.Position:
                            state.UseMySpell(spellId, new Views.SpellCastTargetInfoView() { TargetPosition = entity.Position, Type = Views.TargettingType.Position });
                            break;

                        case Views.TargettingType.Targetted:
                            state.UseMySpell(spellId, new Views.SpellCastTargetInfoView() { TargetId = entity.ID, Type = Views.TargettingType.Targetted });
                            break;
                    }

                }
            }
        }
    }
#endif
}


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{

    public class Game
    {
        const string SaveFile = @".\game.sav";
        Dictionary<LocalKeyInfo, ICommand> commands;

        public Map Map { get; set; }
        public bool PlayerIsDead { get; set; }

        public const short MapStartLeft = 0;
        public const short MapStartTop = 0;

        public Game()
        {
            commands = new Dictionary<LocalKeyInfo, ICommand>();
            this.PlayerIsDead = false;
        }

        public void SetCommand(LocalKeyInfo keyInfo, ICommand command)
        {
            commands.Add(keyInfo, command);
        }

        public bool CommandActivated(LocalKeyInfo keyInfo)
        {
            bool nonTurnAction = true;

            try
            {
                nonTurnAction = commands[keyInfo].execute(keyInfo);
            }
                //TODO add logging...
            catch (Exception e)
            {
                Status.WriteToStatusLine(keyInfo.Key + " key is not bound to a command.  Press any key to continue");
                Console.ReadKey(true);
                Status.ClearInfo();
                Status.WriteToStatusLine(string.Empty);
            }

            return nonTurnAction;
        }


        public void InitConsole()
        {
            Console.TreatControlCAsInput = true;

            Console.Clear();
            Console.SetWindowSize(80, 25);
            Console.BufferWidth = 80;
            Console.BufferHeight = 25;

            Console.CursorVisible = false;

            this.WriteConsole();

            Status.Message = "Height: " + Console.WindowHeight + " Width: " + Console.WindowWidth;
        }


        //TODO eventually optimize how and when things are drawn.  don't necessarily need to redraw all the time.
        //TODO maybe have use a second array that tracks what should be drawn on the grid.
        public void WriteConsole()
        {
            if (Map.Outdated)
            {
                this.GenerateMap();  //TODO only do this when really needed
            }

            //   Status.Message = "Map Left: " + character.LeftMapPosition + " :MapTop: " + character.TopMapPosition;

            Status.WriteToStatus();
        }


        public void GenerateMap()
        {
            foreach (Tile tile in this.Map.Tiles)
            {
                Console.SetCursorPosition(tile.Location.Left + Game.MapStartLeft, tile.Location.Top + Game.MapStartTop);

                if (tile.Mobile != null)
                {
                    Console.Write(tile.Mobile.DisplayCharacter);
                    continue;
                }

                if (tile.Item != null)
                {
                    Console.Write(tile.Item.DisplayCharacter);
                    continue;
                }

                Console.Write(Map.availableTiles[tile.TypeId].DisplayCharacter);
            }

            Map.Outdated = false;
        }

        public void PerformNonPlayerCharacterAction()
        {
            List<Tile> nonPlayerCharacterTiles = Map.GetNonPlayerTiles().Where(t => t.Mobile.CanAct()).ToList();

            Mobile mobile;

            while (!this.PlayerIsDead && nonPlayerCharacterTiles.Count > 0)
            {
                foreach (Tile nonPlayerCharacterTile in nonPlayerCharacterTiles)
                {
                    mobile = nonPlayerCharacterTile.Mobile;
                    ((INonPlayerCharacter)mobile).UpdateState(this.Map, nonPlayerCharacterTile);
                    this.PlayerIsDead = this.Map.GetPlayer().IsDead();

                    if (this.PlayerIsDead)
                    {
                        break;
                    }

                    mobile.TimeBucket = 0;  //TODO -- probably move into NPC or specific commands
                }
                nonPlayerCharacterTiles = Map.GetNonPlayerTiles().Where(t => t.Mobile.CanAct()).ToList();
            }
        }

        public void AdvanceTime()
        {
            List<Mobile> mobiles = this.Map.GetMobiles();

            foreach (var mobile in mobiles)
            {
                mobile.AdvanceTime();
            }
        }

        public bool PlayerCanAct()
        {
            return this.Map.GetPlayer().CanAct();
        }

    }

}
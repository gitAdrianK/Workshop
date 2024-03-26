﻿using EntityComponent;
using JumpKing.GameManager;
using JumpKing.Util;
using JumpKing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using JumpKing.SaveThread;
using JumpKing.Controller;
using Microsoft.Xna.Framework.Input;
using JumpKing.PauseMenu;
using BehaviorTree;
using JumpKing.PauseMenu.BT;
using JumpKing.Workshop;
using JumpKing.Util.Tags;
using EntityComponent.BT;
using JumpKingMultiplayer.Menu;

namespace JumpKingMultiplayer.Models
{
    class LeaderBoard
    {
        public static GuiFormat leader_format = new GuiFormat
        {
            all_padding = 16,
            all_margin = 16,
            element_margin = 6,
            anchor_bounds = new Rectangle(0, 0, JumpGame.GAME_RECT.Width, JumpGame.GAME_RECT.Height),
            anchor = new Vector2(0.5f, 0.25f),
        };

        static GuiFormat list = new GuiFormat
        {
            element_margin = 0,
            anchor_bounds = new Rectangle(0, 0, JumpGame.GAME_RECT.Width, JumpGame.GAME_RECT.Height)
        };

        static LeaderboardDisplayFrame leader_frame;

        public static Rectangle Bounds
        {
            get => leader_frame.Bounds;
        }

        public LeaderBoard()
        {
            leader_frame = new LeaderboardDisplayFrame(leader_format, BTresult.Running, 0.5f);
            
            var title = new TextInfoAligned("Leaderboard", Color.Gold, TextAlignment.Center, Bounds);
            leader_frame.PropertyChanged += (_, __) => title.bounds = leader_frame.Bounds;
            leader_frame.AddChild(title);
            
            leader_frame.AddChild(new PlayerList(list));
            //leader_frame.AddChild(new InvisiblePlayerList(leader_format, list));
            leader_frame.Initialize();
        }

        internal void Run(BehaviorTree.TickData p_data)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                leader_frame.Run(p_data);
            }
        }

        internal void DrawBoard()
        {
            if (leader_frame.IsRunning() && Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                leader_frame.Draw();
            }
        }
    }
}
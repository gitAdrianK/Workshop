﻿using JumpKing.PauseMenu.BT.Actions;
using System;

namespace JumpKingLastJumpValue.Menu
{
    public class LastJumpValueOption : IOptions
    {
        public LastJumpValueOption()
            : base(Enum.GetNames(typeof(ELastJumpDisplayType)).Length, (int)ELastJumpDisplayType.Percentage, EdgeMode.Wrap)
        {
            CurrentOption = (int)JumpKingLastJumpValue.Preferences.DisplayType;
        }

        protected override bool CanChange() => JumpKingLastJumpValue.Preferences.IsEnabled;

        protected override string CurrentOptionName() => ((ELastJumpDisplayType)CurrentOption).ToString();

        protected override void OnOptionChange(int option)
        {
            JumpKingLastJumpValue.Preferences.DisplayType = (ELastJumpDisplayType)option;
        }
    }
}

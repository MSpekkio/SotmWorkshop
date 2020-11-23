using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class NewMoonRisingCardController : MoonwolfCardController
    {
        public NewMoonRisingCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            Func<int> numberOfHitPointsToRestoreTo = () => base.PullOfTheMoon.CurrentValue;
            base.AddWhenHPDropsToZeroOrBelowRestoreHPTriggers(
                () => base.CharacterCard,
                numberOfHitPointsToRestoreTo,
                true,
                ga => base.GameController.RemoveTokensFromPool(base.PullOfTheMoon, base.PullOfTheMoon.CurrentValue, cardSource: base.GetCardSource())
            );
        }
    }
}
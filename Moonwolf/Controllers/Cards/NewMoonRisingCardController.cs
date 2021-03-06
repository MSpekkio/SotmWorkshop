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
        public NewMoonRisingCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
            SpecialStringMaker.ShowTokenPool(PullOfTheMoon);
        }

        public override void AddTriggers()
        {
            AddWhenHPDropsToZeroOrBelowRestoreHPTriggers(
                () => CharacterCard,
                () => PullOfTheMoon.CurrentValue,
                true,
                ga => GameController.RemoveTokensFromPool(PullOfTheMoon, PullOfTheMoon.CurrentValue, gameAction: ga, cardSource: GetCardSource())
            );
        }
    }
}
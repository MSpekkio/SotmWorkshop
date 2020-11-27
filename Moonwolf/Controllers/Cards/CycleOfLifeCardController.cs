using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class CycleOfLifeCardController : MoonwolfCardController
    {
        public CycleOfLifeCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddIncreaseDamageTrigger(dda => dda.Target != null && dda.Target.IsEnvironmentTarget, 2);
            AddMakeDamageIrreducibleTrigger(dda => dda.Target != null && dda.Target.IsEnvironmentTarget);
            AddStartOfTurnTrigger(tt => tt == TurnTaker, p => GameController.GainHP(CharacterCard, 1, cardSource: GetCardSource()), TriggerType.GainHP);
        }
    }
}
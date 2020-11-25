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
        public CycleOfLifeCardController(Card card, TurnTakerController turnTakerController)
         : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            AddTrigger<DealDamageAction>(dealDamage => dealDamage.Target.IsEnvironmentTarget, DealDamageResponse, new[] { TriggerType.IncreaseDamage, TriggerType.MakeDamageIrreducible }, TriggerTiming.Before);

            AddStartOfTurnTrigger(tt => tt == TurnTaker, p => GameController.GainHP(CharacterCard, 1), TriggerType.GainHP);
        }

        private IEnumerator DealDamageResponse(DealDamageAction dealDamage)
        {
            //TODO - Awkward two actions.  Should be someway to do both together.
            var co1 = GameController.IncreaseDamage(dealDamage, 2, cardSource: GetCardSource());
            var co2 = GameController.MakeDamageIrreducible(dealDamage, GetCardSource());

            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(co1);
                yield return base.GameController.StartCoroutine(co2);
            }
            else
            {
                base.GameController.ExhaustCoroutine(co1);
                base.GameController.ExhaustCoroutine(co2);
            }
        }
    }
}
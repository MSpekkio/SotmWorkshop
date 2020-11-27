using NUnit.Framework;
using System;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Handelabra.Sentinels.UnitTest;


namespace SotmWorkshop.Moonwolf
{
    [TestFixture()]
    public abstract class Base : BaseTest
    {
        protected HeroTurnTakerController moonwolf { get { return FindHero("Moonwolf"); } }

        protected TokenPool pullofthemoon => FindTokenPool("PullOfTheMoon", "PullOfTheMoon");
        protected IEnumerable<Card> AssertCardsHaveKeyword(string keyword, params string[] identifers)
        {
            return identifers.Select(ids =>
            {
                var card = GetCard(ids);
                AssertCardHasKeyword(card, keyword, false);
                return card;
            }).ToArray();
        }


    }
}

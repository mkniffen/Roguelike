using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using SlashIt;

namespace Tests.SlashIt
{
    public class AvailableMobileTests
    {
        [Fact]
        public void AvailableMobilesIsNotEmpty()
        {
            var availableMobiles = AvailableMobiles.Instance;
            Assert.NotEmpty(availableMobiles.All); 
        }

        [Fact]
        public void GetForIdReturnsAppropriateMobile()
        {
            int mobileId = 1;
            int mobileId2 = 2;

            var availableMobiles = AvailableMobiles.Instance;
            availableMobiles.All = new List<Mobile>
            {
                { new Mobile {MobileId = mobileId, DisplayCharacter = "@", Description =  "This guy is a newb!!", HitMessage = "The player ", Name = "Player", HitPoints = 30, TransitionTable = null, CurrentTransition = null, CurrentState = null }},
                { new Mobile {MobileId = mobileId2, DisplayCharacter = "r", Description = "A simple rat that wants to EAT you!", HitMessage = "The rat bites you!!!", Name = "Rat" , HitPoints = 10, TransitionTable = new RatTransitionTable(), CurrentTransition = (int)Transition.Attack, CurrentState = new AttackStateRat() }},
            };

            var item = availableMobiles.GetMobileById(mobileId);
            Assert.Equal("Player", item.Name);
            Assert.NotEqual("D", item.Name);
        }
    }
}

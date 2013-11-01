using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    public sealed class AvailableMobiles
    {
        static readonly AvailableMobiles instance = new AvailableMobiles();

        public List<Mobile> All = new List<Mobile>();

        AvailableMobiles()
        {
            XDocument doc = XDocument.Load(@".\Config\Mobiles.xml");

            foreach (XElement mobile in doc.Descendants("Mobiles").Descendants("Mobile"))
            {
                var mobileToAdd = new Mobile();

                mobileToAdd.Name = mobile.Element("Name").Value;
                mobileToAdd.DisplayCharacter = mobile.Element("DisplayCharacter").Value;
                mobileToAdd.HitPoints = Int32.Parse(mobile.Element("HitPoints").Value);
                mobileToAdd.Description = mobile.Element("Description").Value;
                mobileToAdd.HitMessage = mobile.Element("HitMessage").Value;
                mobileToAdd.TransitionTable = Constants.StateTransitionDictionary[mobile.Element("TransitionTable").Value];
                mobileToAdd.CurrentTransition = Constants.TransitionDictionary[mobile.Element("CurrentTransition").Value];
                mobileToAdd.MobileId = Int32.Parse(mobile.Element("MobileId").Value);
                mobileToAdd.CurrentState = Constants.StateDictionary[mobile.Element("CurrentState").Value];

                this.All.Add(mobileToAdd);
            }

            var ids = this.All.GroupBy(a => a.MobileId).Where(g => g.Count() > 1);

            if (ids.Count() > 0)
            {
                throw new Exception("Found non-unique mobile ids: " + string.Join(",", ids.SelectMany(g => g).Select(g => g.MobileId)));
            }
        }

        public static AvailableMobiles Instance
        {
            get { return instance; }
        }

        public Mobile GetMobileById(int id)
        {
            return this.All.Where(m => m.MobileId == id).Single<Mobile>();
        }
    }
}

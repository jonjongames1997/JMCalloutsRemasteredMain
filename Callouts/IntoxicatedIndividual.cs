using CalloutInterfaceAPI;

namespace JMCalloutsRemastered.Callouts
{
    [CalloutInterface("Intoxicated Individual", CalloutProbability.Medium, "An individual causing a scene possibly drunk or high", "Code 2", "LSPD")]


    public class IntoxicatedIndividual : Callout
    {

        private static Ped Suspect;
        private static Blip SuspectBlip;
        private static Vector3 Spawnnpoint;
        private static int counter;
        private static string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new ()
            {
                new(94.63f, -217.37f, 54.49f), // Shopping Center in Vinewood //
                new(-1682.72f,-296.65f, 51.81f), // Vinewood Cemetery
                new(-1392.72f, -607.95f, 30.32f), // Bahama Mamas. Requires either OpenInteriors or Enable All Interiors
                new(-47.78f, -1097.19f, 26.42f), // Simeon's Dealership
                new(128.20f, -1285.29f, 29.28f), // Vanilla Unicorn
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
            };
            Spawnnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(Spawnnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "A business owner reported an individual being drunk on business property.");
            CalloutMessage = "Suspect refused to leave property. Owner said that suspect is possibly be drunk or under the influence of narcotics. Approach with caustion.";
            CalloutPosition = Spawnnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[JM Callouts Remastered Log]: Intoxicated Individual callout accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Intoxicated Individual", "~b~Dispatch:~w~ Suspect located. Respond ~r~Code 2.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            Suspect = new Ped(Spawnnpoint);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.CadetBlue;
            SuspectBlip.IsRouteEnabled = true;

            if (Suspect.IsMale)
                malefemale = "sir";
            else
                malefemale = "ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (Suspect) Suspect.Delete();
            if (SuspectBlip) SuspectBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (MainPlayer.DistanceTo(Suspect) <= 10f)
            {

                Game.DisplayHelp("Press ~y~E ~w~to talk to Suspect. ~y~Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if (counter == 1)
                    {
                        Suspect.Face(MainPlayer);
                        Game.DisplaySubtitle("~b~Player~w~: Good Afternoon " + malefemale + ", How are you today?");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I'm fine, Officer. What's the problem?");
                    }
                    if (counter == 3)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: We've gotten reports from this business behind you that you were intoxicated. Did you have anything to drink today?");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I'm not **hiccup* drunk. I'm fine.");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: Let me give you a sobriety test to make sure you're not under the influence of alcohol or drugs.");
                    }
                    if (counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I DO NOT CONSENT TO THIS TYPE OF INTERROGATION!");
                    }
                    if (counter == 7)
                    {
                        Game.DisplaySubtitle("Conversation has ended!");
                        Suspect.Tasks.ReactAndFlee(Suspect);
                    }
                }
            }
            if (MainPlayer.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();
        }

        public override void End()
        {
            base.End();

            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }

            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Intoxicated Individual", "~b~You:~w~ Dispatch, we are ~g~Code 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.LogTrivial("JM Callouts Remastered - Intoxicated Individual is Code 4!");
        }
    }
}

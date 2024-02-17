using CalloutInterfaceAPI;

namespace JMCalloutsRemastered.Callouts
{
    [CalloutInterface("Refuse To Pay", CalloutProbability.Medium, "An Individual refusing to pay", "Code 2", "LSPD")]

    public class RefuseToPay : Callout
    {

        // General Variables //
        private static Ped Suspect;
        private static Blip SuspectBlip;
        private static Vector3 Spawnpoint;
        private static int counter;
        private static string malefemale;


        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(-53.57f, -1757.29f, 29.44f), // LTD on Grove Street
                new(166.99f, -1554.36f, 29.26f), // Ron Station on MacDonald Street/Davis Ave
                new(288.13f, -1267.06f, 29.44f), // Gas Station near Vanilla Unicorn on Capital Blvd
                new(2677.34f, 3281.31f, 55.24f), // Gas Station on Senora Freeway/Route 13
                new(2001.63f, 3779.19f, 32.18f), // Gas station on Alhambra Dr next to the 24/7 in Sandy Shores
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
            Spawnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "An Individual is refusing to pay for their gas. The individual is being little aggressive. Approach with caution");
            CalloutMessage = "Individual Refusing to pay";
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[JM Callouts Remastered Log]: Refuse To Pay callout Accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Refuse To Pay", "~b~Dispatch: The suspect has been spotted with a firearm! Respond ~r~Code 3");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            Suspect = new Ped(Spawnpoint);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.HotPink;
            SuspectBlip.IsRouteEnabled = true;

            if (Suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

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

                Game.DisplayHelp("Press ~y~E~w~ to interact with ~r~suspect~w~.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if (counter == 1)
                    {
                        Suspect.Face(MainPlayer);
                        Game.DisplaySubtitle("Player: Hello there " + malefemale + ", can you come talk to me for a minute?");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ About fucking time you showed up, officer!");
                    }
                    if (counter == 3)
                    {
                        Game.DisplayNotification("Tip: Try to calm the suspect down before it gets out of control.");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("Player: " + malefemale + ", I need you to calm down and tell me what happened.");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I was coming to get gas and the pump had no gas in it. I came in here to talk to the cashier about it and said that I'm lying.");
                    }
                    if (counter == 6)
                    {
                        Game.DisplaySubtitle("Player: Go on.");
                    }
                    if (counter == 7)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I told them, 'I'm not lying, go check it for yourself'. Then they called the cops and now I'm talking to you.");
                    }
                    if (counter == 8)
                    {
                        Game.DisplaySubtitle("Player: Alright, I will review the CCTV footage and see what happened. Can you have a sit for me on the ground until I have more info?");
                    }
                    if (counter == 9)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Yeah, fine. I'm in a hurry to record my tv show called ~y~'THE DOWNFALL OF LAZLOW'~w~.");
                    }
                    if (counter == 10)
                    {
                        Game.DisplayHelp("Go up to the business and review the CCTV footage then click 'E' to continue the investigation.");
                    }
                    if (counter == 11)
                    {
                        Game.DisplayNotification("You reviewed the CCTV footage. Suspect pumped gas in their car. Refused to pay for the gas.");
                    }
                    if (counter == 12)
                    {
                        Game.DisplaySubtitle("Player: Alright " + malefemale + ", I reviewed the footage and your story DIDN'T match. The footage never lies.");
                    }
                    if (counter == 13)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ Fuck this shit, I'm outta here!");
                    }
                    if (counter == 14)
                    {
                        Game.DisplaySubtitle("Conversation ended!");
                        Suspect.Tasks.FightAgainst(MainPlayer);
                        Suspect.Inventory.GiveNewWeapon("WEAPON_AUTOSHOTGUN", 500, true);
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
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "Refuse To Pay", "~b~You~w~: We are ~g~Code-4!~w~ Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
            Game.LogTrivial("JM Callouts Remastered - Refuse to pay is Code 4!");
        }
    }
}

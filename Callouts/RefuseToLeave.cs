using CalloutInterfaceAPI;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Refuse To Leave", CalloutProbability.Medium, "An individual refuses to leave property", "Code 2", "LSPD")]

    public class RefuseToLeave : Callout
    {

        // General Variables //
        private static readonly string[] wepList = new string[] { "WEAPON_PISTOL", "WEAPON_BAT", "WEAPON_KNIFE", "WEPAON_HAMMER", "WEAPON_MACHETE", "WEAPON_CROWBAR", "WEAPON_CARBINERIFLE" };
        private static Ped Suspect;
        private static Blip SuspectBlip;
        private static Vector3 Spawnpoint;
        private static int counter;
        private static string malefemale;


        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(-1222.80f, -907.12f, 12.33f), // Rob's Liquors near the Nightclub
                new(-1193.68f, -768.45f, 17.32f), // Suburban near Vespucci PD HQ
                new(-330.96f, 6081.46f, 31.45f), // Ammunation Near Paleto PD
                new(-113.23f, 6469.90f, 31.63f), // Paleto Bank
                new(-57.16f, 6522.26f, 31.49f), // Willie's Grocery Store
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
            CalloutInterfaceAPI.Functions.SendMessage(this, "Business employee told the individual to leave the property but refuses to. Employee suspects the individual to be under the influence.");
            CalloutMessage = "Individual refusing to leave property by business owner/employee.";
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[JM Callouts Remastered Log]: Refuse To Leave callout accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Refuse To Leave", "~b~Dispatch: ~w~Suspect has been spotted. Respond ~r~Code 2");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            Suspect = new Ped(Spawnpoint);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;


            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.BlueViolet;
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

                Game.DisplayHelp("Press ~y~E~w~ to interact with ~r~suspect~w~. ~y~Approach with caution~w~.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if (counter == 1)
                    {
                        Suspect.Face(MainPlayer);
                        Game.DisplaySubtitle("Player: Hello there " + malefemale + ", Can I talk to you for a second?");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ What now donut pigs?");
                    }
                    if (counter == 3)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: Can you tell me what's going on?");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ That bitch over there told me I can't come in here.");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: Is there a reason why they can't let you come in here?");
                    }
                    if (counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ I was outside the door asking people for money. They called the cops and they told me that I was trespassed from the property.");
                    }
                    if (counter == 7)
                    {
                        Game.DisplayNotification("~y~Tip~w~: ~o~If the suspect was trespassed from the property before, that's an arrestable offense.");
                    }
                    if (counter == 8)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: Ok. Well, you know you can be arrested and/or cited for trespassing, right?");
                    }
                    if (counter == 9)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ WHAT?! Are you f***ing with me?");
                    }
                    if (counter == 10)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: No, I'm not. Don't try anything stupid, you'll make things worse on yourself.");
                    }
                    if (counter == 11)
                    {
                        Game.DisplaySubtitle("~r~Suspect:~w~ F**k you and f**k them! I'm outta here, playa!");
                    }
                    if (counter == 12)
                    {
                        Game.DisplaySubtitle("Conversation ended!");
                        Suspect.Tasks.FightAgainst(MainPlayer);
                        Suspect.Inventory.GiveNewWeapon(wepList[new Random().Next((int)wepList.Length)], 500, true);
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

            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Refuse To Leave", "~b~You:~w~ Dispatch, we are ~g~Code 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.LogTrivial("JM Callouts Remastered - Refuse to leave is Code 4!");
        }

    }
}

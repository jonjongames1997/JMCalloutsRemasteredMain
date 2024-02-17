using CalloutInterfaceAPI;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("9-1-1 Hang Up", CalloutProbability.High, "An individual hung up on 911.", "Code 2", "LSPD")]

    public class _911HangUp : Callout
    {

        // General Variables //
        private static Ped Suspect;
        private static Blip SuspectBlip;
        private static Vector3 Spawnpoint;
        private static string malefemale;
        private static int counter;


        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(1082.087f, -346.2961f, 67.1872f), // Mirror Park near Horny's //
                new(-1294.99f, -1316.46f, 4.69f), // OCRP Postal 305/Vespucci Beach //
                new(-1281.43f, -1139.24f, 6.47f), // Bean Machine in Vespucci Beach
                new(-1335.80f, -929.51f, 11.75f), // Motel Near Rob's Liquors
                new(-232.05f, -2055.64f, 27.62f), // Maze Bank Arena Parking 
                new(139.11f, -1635.78f, 29.30f), // Near Ron Station on Davis Ave
                new(130.16F, -1737.21f, 30.11f), // Train Station next to Davis Mall
                new(-1324.74f, -397.55f, 35.83f), // Ammunation in Morningwood 
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
            CalloutInterfaceAPI.Functions.SendMessage(this, "A civilian called 9-1-1 then immediately hung up. Deal with this, Officer.");
            CalloutMessage = "A citizen called 911 then hung up on dispatch"; // Brief description of callout //
            CalloutPosition = Spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[JM Callouts Remastered Log]: 9-1-1 Hang Up callout accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~ 9-1-1 Hang Up", "~b~Dispatch: Suspect has been spotted. Respond ~r~Code 2~w~.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            Suspect = new Ped(Spawnpoint);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();
            Suspect.Tasks.PutHandsUp(-1, MainPlayer);
            SuspectBlip.Color = System.Drawing.Color.BlueViolet;
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

                Game.DisplayHelp("Press ~y~E~w~ to speak with the ~r~suspect~w~.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if (counter == 1)
                    {
                        Suspect.Face(MainPlayer);
                        Game.DisplaySubtitle("~b~Player~w~: Excuse me " + malefemale + ", Can I speak to you for a moment?");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Sure, Officer. What seems to be the problem?");
                    }
                    if (counter == 3)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: We received a call from your cell phone ping. Did you call 9-1-1?");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Oh, shit. I think Siri misheard what I've said. Oh, my lord, I do apologize about thi... Am I getting arrested?");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: Ok, let me see some identification from you and we'll go from there. Do you have any warrants that I should know about?");
                    }
                    if (counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Sure, here's my ID and no, officer, no warrants. I never been arrested before.");
                    }
                    if (counter == 7)
                    {
                        Game.DisplaySubtitle("~b~Player~w~: Ok, let me run your information real quick and we'll go from there.");
                    }
                    if (counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Snitch! I'm gonna give you the ass whooping of your life, Officer, that your parents couldn't give you as a child.");
                        Suspect.Tasks.FightAgainst(MainPlayer);
                        Suspect.Inventory.GiveNewWeapon("WEAPON_GOLFCLUB", 500, true);
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

            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~9-1-1 Hang Up", "~b~You: ~w~Dispatch, we are ~g~Code 4! Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.LogTrivial("JM Callouts Remastered - 911 Hang Up is Code 4!");
        }

    }
}

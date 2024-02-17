using CalloutInterfaceAPI;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Public Disturbance", CalloutProbability.Medium, "A individual causing a scene in public", "Code 3", "LSPD")]

    public class PublicDisturbance : Callout
    {

        // General Variables //
        private static Ped suspect;
        private static Blip SuspectBlip;
        private static Vector3 spawnPoint;
        private static int counter;
        private static string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(-174.17f, -1427.77f, 31.25f), // Across from the auto shop in strawberry
                new(1693.54f, 4822.75f, 42.06f), // Clothing Shop in Grape Seed
                new(1991.82f, 3048.46f, 47.22f), // Yellow Jack
                new(1197.32f, 2695.93f, 37.91f), // Clothing Shop on Route 68
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
            spawnPoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "A citizen's reporting a public disturbance.");
            CalloutMessage = "A citizen's reporting a person threatening a victim's life with a deadly weapon.";
            CalloutPosition = spawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[JM Callouts Remastered Log]: Public Disturbance callout accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Public Disturbance", "~b~Dispatch:~w~ Suspect has been spotted!. Respond ~r~Code 2.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            suspect = new Ped(spawnPoint);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            SuspectBlip = suspect.AttachBlip();
            SuspectBlip.Color = System.Drawing.Color.BurlyWood;
            SuspectBlip.IsRouteEnabled = true;

            if (suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect) suspect.Delete();
            if (SuspectBlip) SuspectBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (MainPlayer.DistanceTo(suspect) <= 10f)
            {

                Game.DisplayHelp("Press ~y~E~w~ to talk to suspect. ~y~Approach with caution~w~.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if (counter == 1)
                    {
                        suspect.Face(MainPlayer);
                        Game.DisplaySubtitle("~b~You (Officier)~w~: Excuse me, " + malefemale + ", Can you come talk to me for a second?");
                    }
                    if (counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: What do you want now, you donut eaters?");
                    }
                    if (counter == 3)
                    {
                        Game.DisplaySubtitle("~b~You (Officer):~w~ Can you explain to me on what's to be the problem?");
                    }
                    if (counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: I have a bipolar disorder which I can't control and it makes me say offensive things.");
                    }
                    if (counter == 5)
                    {
                        Game.DisplaySubtitle("~b~You (Officer)~w~: Do you take any type of medication for your disorder?");
                    }
                    if (counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Yes, I have forget about it. I do apologize for y'all to be called out here.");
                    }
                    if (counter == 7)
                    {
                        Game.DisplaySubtitle("~b~You (Officer)~w~: Why did threat someone's life for when they didn't do anything to you?");
                    }
                    if (counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: I didn't mean anything by it. I do apologize about it.");
                    }
                    if (counter == 9)
                    {
                        Game.DisplaySubtitle("~b~You (Officer)~w~: Next time, take your medicine when you are supposed to take it, " + malefemale + ".");
                    }
                    if (counter == 10)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: I understand that loud and clear, Officer.");
                    }
                    if (counter == 11)
                    {
                        Game.DisplaySubtitle("Conversation Ended!");
                        suspect.Tasks.Wander();
                    }
                }

                if (MainPlayer.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
            }
        }

        public override void End()
        {
            base.End();

            if (suspect.Exists())
            {
                suspect.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }

            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Public Disturbance", "~b~You:~w~ Dispatch, we are ~g~Code 4.~w~ Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.LogTrivial("JM Callouts Remastered - Public Disturbance is Code 4!");
        }
    }
}

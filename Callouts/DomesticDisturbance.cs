using CalloutInterfaceAPI;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Domestic Disturbance", CalloutProbability.Medium, "Reports of a domestic disturbance", "Code 2", "LSPD")]

    public class DomesticDisturbance : Callout
    {
        // General Variables //
        private static Ped victim;
        private static Ped suspect;
        private static Blip vicBlip;
        private static Blip susBlip;
        private static Vector3 spawnPoint;
        private static Vector3 suspectSpawnpoint;
        private static float suspectHeading;
        private static float heading;
        private static int counter;
        private static string malefemale;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = new(-9.02f, -1440.39f, 31.10f);
            heading = 204.81f;
            suspectSpawnpoint = new(-18.09f, -1432.39f, 31.10f);
            suspectHeading = 236.09f;
            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "A neighbor's reporting a loud argument next door.");
            CalloutMessage = "Reports of a domestic disturbance";
            CalloutPosition = spawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[JM Callouts Remastered Log]: Domestic Disturbance callout accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Domestic Disturbance", "~b~Dispatch: Suspect has been spotted. Respond ~r~Code 2~w~.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            victim = new Ped(spawnPoint, heading);
            victim.IsPersistent = true;
            victim.BlockPermanentEvents = true;

            suspect = new Ped(suspectSpawnpoint, suspectHeading);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            suspect.KeepTasks = true;
            susBlip = suspect.AttachBlip();
            susBlip.Color = System.Drawing.Color.Red;
            suspect.Tasks.StandStill(500);

            vicBlip = victim.AttachBlip();
            vicBlip.Color = System.Drawing.Color.DodgerBlue;
            vicBlip.IsRouteEnabled = true;

            if (victim.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect) suspect.Delete();
            if (vicBlip) vicBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            if(MainPlayer.DistanceTo(victim) <= 10f)
            {
                Game.DisplayHelp("Press ~y~E~w~ to interact with ~r~suspect~w~.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if(counter == 1)
                    {
                        victim.Face(MainPlayer);
                        Game.DisplaySubtitle("~b~You~w~: Los Santos Police Department. Hello, there " + malefemale + ". How are you? and what's seems to be the problem? I have gotten a call from your neighbor saying that you and another person were arguing about something.");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Hello, Officer. I am doing fine.... well, kinda. We are having an argument over headphones and earbuds. My buddy claims that over the head headphones is different from headphones. I told my buddy is stil headphones. I do apologize for you being called out.");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("~b~You~w~: Is that why I was being called out here over headphones and earbuds?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~b~You~w~: I'll solve this argument right now. Headphones is headphones. The over the ear headphones is considered headphones. Earbuds is earbuds. I swear y'all Gen Z's are being idiotic.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("~b~You~w~: But I will talk to your buddy and tell them what I said.");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: I'm not a Gen Z, officer, my buddy is. But ok officer.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("Conversation Ended. Talk to the ~r~Suspect~w~.");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Fuck you, motherfucker! I am not going back to prison and get my booty violated.");
                        suspect.Tasks.FightAgainst(MainPlayer);
                        suspect.Inventory.GiveNewWeapon("WEAPON_COMBATPISTOL", 500, true);
                        suspect.Armor = 500;
                    }
                }
            }

            if (MainPlayer.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();
        }

        public override void End()
        {
            if (victim.Exists())
            {
                victim.Dismiss();
            }
            if (vicBlip.Exists())
            {
                vicBlip.Delete();
            }

            if (suspect.Exists())
            {
                suspect.Dismiss();
            }

            if (susBlip.Exists())
            {
                susBlip.Delete();
            }

            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Domestic Disturbance", "~b~You:~w~ Dispatch, we are ~g~Code 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            base.End();

            Game.LogTrivial("[JM Callouts Remastered]: Domestic Disturbance is CODE 4!");
        }
    }
}

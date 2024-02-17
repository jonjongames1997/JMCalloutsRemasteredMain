using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System.Drawing;
using CalloutInterfaceAPI;
using System.Windows.Forms;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Unauthorized Access - Richard's Majestic", CalloutProbability.Medium, "Security reported an individual for having unauthroized access to Mr.Richards' movie studio", "Code 3", "LSPD")]


    public class UnauthroizedAccessMovieStudio : Callout
    {


        // General Variables //
        private Ped Suspect;
        private Blip suspectBlip;
        private Vector3 Spawnpoint;
        private float heading;
        private int counter;
        private string malefemale;


        public override bool OnBeforeCalloutDisplayed()
        {
            Spawnpoint = new Vector3(-1047.909f, -517.2541f, 36.03859f);
            heading = 156.4741f;
            ShowCalloutAreaBlipBeforeAccepting(Spawnpoint, 1000f);
            AddMaximumDistanceCheck(100f, Spawnpoint);
            CalloutMessage = "A security officer needs assistance";

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect = new Ped("CS_DRFRIEDLANDER", Spawnpoint, heading);
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            CalloutInterfaceAPI.Functions.SendMessage(this, "A security officer reported an individual for unauthroized access to the movie studio");

            suspectBlip = Suspect.AttachBlip();
            suspectBlip.Color = System.Drawing.Color.AliceBlue;
            suspectBlip.IsRouteEnabled = true;

            if (Suspect.IsMale)
                malefemale = "Sir";
            else
                malefemale = "Ma'am";

            counter = 0;

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if(Game.LocalPlayer.Character.DistanceTo(Suspect) <= 10f)
            {

                Game.DisplayHelp("Press ~y~E ~w~to talk to ~r~Suspect~w~. ~y~Approach with caution.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if(counter == 1)
                    {
                        Game.DisplaySubtitle("~y~Player: ~w~Excuse me, " + malefemale + ", Can you talk to me for a second?");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~Glad to see you, friend. Do you need any of my therapist sessions?");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("~y~Player: ~w~What the fuck are you talking about? NO!. Why are you on this property without proper authorization?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~That's shame my friend. I'm trying to promote my therapy sessions to these wonderful subjects about guidance, faith, and betrayal.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("~y~Player: ~w~First of all, I'm not your fucking friend, dipshit. Do you have anything on you that you have access to be on this property?");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect: ~w~Oh, no! He knows! Time to take you with me, friend.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("End of Conversation");
                        Suspect.Tasks.ReactAndFlee(Suspect);
                    }
                }
            }

            if(Suspect.IsCuffed || Suspect.IsDead || Game.LocalPlayer.Character.IsDead || !Suspect.Exists())
            {
                End();
            }
        }

        public override void End()
        {
            base.End();

            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (suspectBlip.Exists())
            {
                suspectBlip.Delete();
            }


            Game.LogTrivial("JM Callouts Remastered - Unauthorized Access Movie Studio is code 4!");
        }
    }
}

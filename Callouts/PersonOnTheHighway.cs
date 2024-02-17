using CalloutInterfaceAPI;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Person On The Highway", CalloutProbability.Medium, "A citizen's report of an individual on the freeway", "Code 2", "SAHP")]

    public class PersonOnTheHighway : Callout
    {
        private static readonly string[] wepList = new string[] { "WEAPON_KNIFE", "WEAPON_BAT", "WEAPON_DAGGER", "WEAPON_GOLFCLUB", "WEAPON_HAMMER", "WEAPON_HATCHET", "WEAPON_PISTOL", "WEAPON_COMBATPISTOL", "WEAPON_AUTOSHOTGUN" };
        private static readonly string[] pedList = new string[] { "a_m_m_afriamer_01", "ig_amandatownley", "ig_ashley", "g_f_y_ballas_01", "a_f_m_bodybuild_01", "a_f_y_eastsa_03", "ig_maryann", "ig_money", "s_m_y_baywatch_01", "a_f_y_beach_01", "a_f_m_bevhills_01", "a_f_m_fatbla_01" };
        private static Blip blip;
        private static Ped suspect;
        private static Vector3 spawnpoint;
        private static int counter;
        private static string malefemale;


        public override bool OnBeforeCalloutDisplayed()
        {
            List<Vector3> list = new()
            {
                new(2865.42f, 4259.82f, 50.08f), // Route 13 near Maude's House 
                new(1707.10f, 1413.60f, 85.92f), // Route 13 going into Blaine County
                new(2440.64f, 963.54f, 87.11f), // Near the wind farm on Route 15
                new(-2721.99f, 8.95f, 15.55f), // Route 1 going into Chumash
                new(1668.16f, -946.03f, 64.91f), // Polomino Freeway
                new(1311.71f, -2292.97f, 52.39f), // ElysianFields Freeway
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
            spawnpoint = LocationChooser.ChooseNearestLocation(list);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of an individual on the highway");
            CalloutMessage = "A citizen's reporting an individual on the highway. Respond Code 2.";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("JM Callouts Remastered [LOG]: Person on the highway callout accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~y~Person On The Highway", "~b~Dispatch~w~: The suspect has been spotted. Respond ~r~Code 2~w~.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            suspect = new Ped(pedList[new Random().Next((int)pedList.Length)], spawnpoint, 0f);
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            

            blip = suspect.AttachBlip();
            blip.Color = System.Drawing.Color.Gold;
            blip.IsRouteEnabled = true;

            suspect.Tasks.PutHandsUp(500, MainPlayer);
            suspect.KeepTasks = true;
            suspect.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);

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
            if (blip) blip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            base.Process();

            if(MainPlayer.DistanceTo(suspect) <= 10f)
            {

                Game.DisplayHelp("Press ~y~E~w~ to interact with the ~r~suspect~w~. Approach with ~y~CAUTION~w~.", false);

                if (Game.IsKeyDown(System.Windows.Forms.Keys.E))
                {
                    counter++;

                    if(counter == 1)
                    {
                        suspect.Face(MainPlayer);
                        Game.DisplaySubtitle("~b~Officer (You)~w~: Hey there, " + malefemale + ". What goin' on? Come talk to me real quick.");
                    }
                    if(counter == 2)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Hi, Officer, My Uber driver kicked me out his car for no reason.");
                    }
                    if(counter == 3)
                    {
                        Game.DisplaySubtitle("~b~Officer (You)~w~: Why would your driver do that? What was the reason?");
                    }
                    if(counter == 4)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: The driver is very picky on who they pick up. I find that as discrimination.");
                    }
                    if(counter == 5)
                    {
                        Game.DisplaySubtitle("~b~Officer (You)~w~: Ok, you didn't call a taxi? If you are being discriminated, you can contact Uber and file a complaint with them cause that's on the company. If the driver was threatening you or anything criminal, that's when we step in.");
                    }
                    if(counter == 6)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: No I didn't call a taxi. I prefer to walk. I know it's against the law to do that but nobody will stop and help me.");
                    }
                    if(counter == 7)
                    {
                        Game.DisplaySubtitle("~b~Officer (You)~w~: I am more than happy to call you a taxi. I don't want to put you in cuffs. Let me call you a taxi and get you home safe. I am worried for your safety,");
                    }
                    if(counter == 8)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Thank you, officer.");
                    }
                    if(counter == 9)
                    {
                        Game.DisplaySubtitle("~r~Suspect~w~: Death to Los Santos, Motherfucka!");
                        suspect.Tasks.FightAgainst(MainPlayer);
                        suspect.KeepTasks = true;
                        suspect.Inventory.GiveNewWeapon(wepList[new Random().Next((int)wepList.Length)], 500, true);
                        suspect.Armor = 500;
                    }
                }

                if (MainPlayer.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
            }
        }

        public override void End()
        {
            if (suspect.Exists())
            {
                suspect.Dismiss();
            }
            if (blip.Exists())
            {
                blip.Delete();
            }

            base.End();

            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Person On The Highway", "~b~You~w~: Dispatch, we are ~g~Code 4.~w~ Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.LogTrivial("JM Callouts Remastered [LOG]: Person On The Highway is code 4!");
        }
    }
}

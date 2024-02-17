using CalloutInterfaceAPI;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Reckless Driving", CalloutProbability.VeryHigh, "Reports of a reckless driver", "Code 3", "LSPD")]

    public class WrecklessDriving : Callout
    {
        private static readonly string[] vehicleList = new string[] { "CERBERUS3", "ISSI2", "AKUMA", "BIFTA", "CHINO2", "RROCKET", "SHOTARO", "HUSTLER", "" };
        private static Vehicle vehicle;
        private static Vector3 spawnpoint;
        private static Blip driverBlip;
        private static Ped driver;
        private static LHandle pursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(1000f));
            vehicle = new Vehicle(vehicleList[new Random().Next((int)vehicleList.Length)], spawnpoint);
            vehicle.IsPersistent = true;
            driver = vehicle.CreateRandomDriver();
            driver.BlockPermanentEvents = true;
            driver.IsPersistent = true;
            driver.Tasks.CruiseWithVehicle(vehicle.TopSpeed, VehicleDrivingFlags.Emergency);
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of a reckless driver");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 WE_HAVE_01 CRIME_SUSPECT_ON_THE_RUN_02 UNITS_RESPOND_CODE_03_01");
            CalloutMessage = "Reckless driving in the area";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[JM Callouts Remastered Log]: Reckless Driving callout accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Reckless Driving", "~b~Dispatch~w~: The suspect has been spotted! Respond ~r~Code 3~w~.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            driverBlip = new Blip(driver)
            {
                IsFriendly = false,
                Color = Color.Red,
                IsRouteEnabled = true,
            };

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (driver) driver.Delete();
            if (vehicle) vehicle.Delete();
            if (driverBlip) driverBlip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if(!vehicle || !driver)
            {
                End();
            }

            if(MainPlayer.DistanceTo(vehicle) < 30f && pursuit == null)
            {
                pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
                LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, driver);
                LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                if (driverBlip) driverBlip.Delete();
            }

            if(pursuit != null)
            {
                if (LSPD_First_Response.Mod.API.Functions.IsPursuitStillRunning(pursuit))
                {
                    Game.DisplaySubtitle("Go get ~r~'em~w~, Officer!");
                }
                else if (!IsEnding)
                {
                    End();
                }
            }

            if (MainPlayer.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();

            base.Process();
        }

        public override void End()
        {
            if (driver) driver.Dismiss();
            if (vehicle) vehicle.Dismiss();
            if (driverBlip) driverBlip.Delete();
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Reckless Driving", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
            base.End();

            Game.LogTrivial("[JM Callouts Remastered Log]: Reckless Driving is code 4!");
        }
    }
}

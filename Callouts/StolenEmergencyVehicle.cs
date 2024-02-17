using CalloutInterfaceAPI;
using System.Threading;

namespace JMCalloutsRemastered.Callouts
{

    [CalloutInterface("Stolen Emergency Vehicle", CalloutProbability.Medium, "Reports of a stolen emergency vehicle", "CODE 3", "LSPD")]

    public class StolenEmergencyVehicle : Callout
    {
        private static readonly string[] emergencyVehicles = new string[] { "POLICE", "POLICE2", "POLICE3", "SHERIFF", "SHERIFF2", "POLICE4", "FBI", "FBI2", "AMBULANCE", "FIRETRUK", "POLICEB", "PBUS", "PRANGER", "POLICET", "RIOT", "RIOT2", "LGUARD", "POLICEOLD1", "POLICEOLD2" };
        private static Vehicle emergencyVehicle;
        private static Ped suspect;
        private static Vector3 spawnpoint;
        private static Blip blip;
        private static LHandle pursuit;
        private static bool pursuitCreated = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 100f);
            CalloutInterfaceAPI.Functions.SendMessage(this, "Reports of a stolen emergency vehicle in the area");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_01 WE_HAVE_01 CRIME_SUSPECT_ON_THE_RUN_01 UNITS_RESPOND_CODE_03_02");
            CalloutMessage = "Multiple reports of a stolen emergency vehicle";
            CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("[JM Callouts Remastered Log]: Stolen Emergency Vehicle callout accepted!");
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Stolen Emergency Vehicle", "~b~Dispatch~w~: The suspect has been spotted! Respond ~r~Code 3~w~.");
            Game.DisplayHelp("Press ~y~END~w~ at anytime to end the callout", false);

            emergencyVehicle = new Vehicle(emergencyVehicles[new Random().Next((int)emergencyVehicles.Length)], spawnpoint);
            emergencyVehicle.IsSirenOn = true;

            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Dispatch", "Loading ~g~Information~w~ of the ~o~LSPD Database~w~...");
            LSPD_First_Response.Mod.API.Functions.DisplayVehicleRecord(emergencyVehicle, true);
            suspect = new Ped(spawnpoint);
            suspect.WarpIntoVehicle(emergencyVehicle, -1);
            suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
            suspect.BlockPermanentEvents = true;
            suspect.IsPersistent = true;

            blip = suspect.AttachBlip();

            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);
            pursuitCreated = true;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect) suspect.Delete();
            if (emergencyVehicle) emergencyVehicle.Delete();
            if (blip) blip.Delete();

            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            if (MainPlayer.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();

            base.Process();
        }

        public override void End()
        {
            if (blip) blip.Delete();
            if (emergencyVehicle) emergencyVehicle.Dismiss();
            if (suspect) suspect.Dismiss();
            Game.DisplayNotification("web_jonjongames", "web_jonjongames", "~w~JM Callouts Remastered", "~w~Stolen Emergency Vehicle", "~b~You~w~: Dispatch, we are ~g~CODE 4~w~. Show me back 10-8.");
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURHTER_UNITS_REQUIRED");
            base.End();

            Game.LogTrivial("[JM Callouts Remastered Log]: Stolen Police Vehicle is Code 4!");
        }

    }
}

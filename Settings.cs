using System.Windows.Forms;

namespace JMCalloutsRemastered
{
    internal static class Settings
    {
        internal static bool IllegalCampfireOnPublicBeach = true;
        internal static bool IllegalProstitution = true;
        internal static bool IntoxicatedIndividual = true;
        internal static bool _911HangUp = true;
        internal static bool PossibleProstitution = true;
        internal static bool PublicDisturbance = true;
        internal static bool RefuseToPay = true;
        internal static bool RefuseToLeave = true;
        internal static bool Soliciting = true;
        internal static bool TrespassingOnPrivateProperty = true;
        internal static bool TrespassingOnRailRoadProperty = true;
        internal static bool CodeKaren = true;
        internal static bool TrespassingOnConstructionProperty = true;
        internal static bool LostIndividual = true;
        internal static bool UnauthorizedAccessMovieStudio = true;
        internal static bool DeadBody = true;
        internal static bool SolicitingDelPerroPier = true;
        internal static bool WrecklessDriving = true;
        internal static bool StolenEmergencyVehicle = true;
        internal static bool TrespassingOnSchoolGrounds = true;
        internal static bool PersonOnTheHighway = true;
        internal static bool StolenConstructionEquipment = true;
        internal static bool DomesticDisturbance = true;
        internal static bool HelpMessages = true;
        internal static Keys EndCall = Keys.End;
        internal static Keys Dialog = Keys.E;
        internal static InitializationFile ini;
        internal static string inipath = "Plugins/LSPDFR/JMCalloutsRemastered.ini";

        internal static void LoadSettings()
        {
            Game.Console.Print("[LOG]: Loading config file from JM Callouts Remastered.");
            ini = new InitializationFile(inipath);
            ini.Create();
            Game.LogTrivial("Initializing Config for JMCalloutsRemastered....");
            Settings.CodeKaren = ini.ReadBoolean("Callouts", "CodeKaren", true);
            Settings.IllegalCampfireOnPublicBeach = ini.ReadBoolean("Callouts", "IllegalCampfireOnPublicBeach", true);
            Settings.IllegalProstitution = ini.ReadBoolean("Callouts", "IllegalProstitution", true);
            Settings.IntoxicatedIndividual = ini.ReadBoolean("Callouts", "IntoxicatedIndividual", true);
            Settings.PossibleProstitution = ini.ReadBoolean("Callouts", "PossibleProstitution", true);
            Settings.PublicDisturbance = ini.ReadBoolean("Callouts", "PublicDisturbance", true);
            Settings.RefuseToLeave = ini.ReadBoolean("Callouts", "RefuseToLeave", true);
            Settings.Soliciting = ini.ReadBoolean("Callouts", "Soliciting", true);
            Settings.TrespassingOnPrivateProperty = ini.ReadBoolean("Callouts", "TrespassingOnPrivateProperty", true);
            Settings.TrespassingOnRailRoadProperty = ini.ReadBoolean("Callouts", "TrespassingOnRailRoadProperty", true);
            Settings._911HangUp = ini.ReadBoolean("Callouts", "_911HangUp", true);
            Settings.TrespassingOnConstructionProperty = ini.ReadBoolean("Callouts", "TrespassingOnConstructionProperty", true);
            Settings.LostIndividual = ini.ReadBoolean("Callouts", "LostIndividual", true);
            Settings.UnauthorizedAccessMovieStudio = ini.ReadBoolean("Callouts", "UnauthorizedAccessMovieStudio", true);
            Settings.DeadBody = ini.ReadBoolean("Callouts", "Dead Body", true);
            Settings.SolicitingDelPerroPier = ini.ReadBoolean("Callouts", "SolicitingDelPerro", true);
            Settings.WrecklessDriving = ini.ReadBoolean("Callouts", "RecklessDriving", true);
            Settings.StolenEmergencyVehicle = ini.ReadBoolean("Callouts", "StolenEmergencyVehicle", true);
            Settings.TrespassingOnSchoolGrounds = ini.ReadBoolean("Callouts", "TrespassingOnSchoolGrounds", true);
            Settings.PersonOnTheHighway = ini.ReadBoolean("Callouts", "PersonOnTheHighway", true);
            Settings.StolenConstructionEquipment = ini.ReadBoolean("Callouts", "StolenConstructionEquipment", true);
            Settings.DomesticDisturbance = ini.ReadBoolean("Callouts", "DomesticDisturbance", true);
            Settings.HelpMessages = ini.ReadBoolean("HelpMessages", "Help Messages", true);
            EndCall = ini.ReadEnum("Keys", "EndCall", Keys.End);
            Dialog = ini.ReadEnum("Keys", "Dialog", Keys.E);
        }
        public static readonly string PluginVersion = "4.1.0";
    }
}

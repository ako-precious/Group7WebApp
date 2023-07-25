namespace Group7WebApp.Helpers
{
    public class Priviledges
    {
        public Dictionary<string, string> UserPreviledges = new Dictionary<string, string> {
            { "ViewContact","ViewContact" },
            { "EditContact","EditContact" },
            { "DeleteContact","DeleteContact" },
            { "CreateContact","CreateContact" }
        };
        public Dictionary<string, string> ManagerPreviledges = new Dictionary<string, string> {
            { "ViewContact","ViewContact" },
            { "EditContact","EditContact" },
            { "DeleteContact","DeleteContact" },
            { "CreateContact","CreateContact" }
        };
        public Dictionary<string, string> AdminPreviledges = new Dictionary<string, string> {
            { "ViewContact","ViewContact" },
            { "EditContact","EditContact" },
            { "DeleteContact","DeleteContact" },
            { "CreateContact","CreateContact" },
            { "ApproveContact","ApproveContact" }
        };
    }

    public class CustomClaimTypes
    {
        public const string Permission = "permission";
    }

    public static class PreviledgePacker
    {
        public static string PackPreviledgesIntoString(Dictionary<string, string> previledges)
        {
            return string.Join(';', previledges.Keys);
        }
    }
}

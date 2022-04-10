namespace CICD.BO
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Token { get; set; }

        public bool IsDefault { get; set; }
    }
}
using System.ComponentModel;

namespace CICD.Model
{
    [Description("[User]")]
    public class User
    {
        [Description("[Id]")]
        public int Id { get; set; }

        [Description("[Name]")]
        public string Name { get; set; }

        [Description("[Token]")]
        public string Token { get; set; }
    }
}

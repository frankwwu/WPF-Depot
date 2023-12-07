
namespace Kanban.Models
{
    public class User
    {
        public int Id { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", GivenName, Surname);
            }
        }

        public string Initial
        {
            get
            {
                if (!string.IsNullOrEmpty(GivenName) && !string.IsNullOrEmpty(Surname))
                {
                    return string.Format("{0}{1}", GivenName[0], Surname[0]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}

namespace webapi.Models
{
    public class User
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Adress { get; set; } = string.Empty;

        public DateTime RegistratedDate { get; set; } = DateTime.Now;

    }
}

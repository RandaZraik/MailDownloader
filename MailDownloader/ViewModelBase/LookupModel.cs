namespace MailDownloader
{
    /// <summary>
    /// Lookup base model
    /// </summary>
    public class LookupModel : BindableBase
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public LookupModel()
        {

        }

        public LookupModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

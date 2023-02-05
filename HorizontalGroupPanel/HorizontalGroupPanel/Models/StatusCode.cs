namespace HorizontalGroupPanel.Models
{
    public class StatusCode
    {
        public StatusCode(int code, string name)
        {
            Group = code / 100 * 100;
            Code = code;
            Name = name;
        }

        public int Group { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Code}: {Name}";
        }
    }
}

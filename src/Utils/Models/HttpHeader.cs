namespace Utils.Models
{
    public class HttpHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"HttpHeaders: [name={Name}, value={Value}";
        }
    }
}
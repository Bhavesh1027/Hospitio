namespace CSharpToTypeScript.Core.Models
{
    public class Context
    {
        public IEnumerable<string> GenericTypeParameters { get; set; } = new List<string>();

        public Context Clone() => (Context)MemberwiseClone();
    }
}
using System.ComponentModel.DataAnnotations;

namespace Transportation
{
    public abstract class Entity
    {
        [Key]
        public long ID { get; set; }

        protected string Stringify(string text)
        {
            if (text != null)
            {
                text = text.Replace("\\", "\\\\");
                text = text.Replace("\'", "\\\'");
            }
            return text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

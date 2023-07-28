using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
        internal class budget_fields
        {
            //nome
            [PrimaryKey, AutoIncrement]
            public int id {  get; set; }
            public int mese { get; set; }
            public string name { get; set; }
            public string data { get; set; }
            public string description { get; set; }
            //entrato o uscite
            public string type { get; set; }
            //cifra in €
            public string value { get; set; }
        }
    
}

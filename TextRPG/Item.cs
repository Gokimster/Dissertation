﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Item
    {
        public string description { get; set; }
        public string name { get; set; }
        
        public Item(string name)
        {
            this.name = name;
        }
        public Item(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
        
    }
}
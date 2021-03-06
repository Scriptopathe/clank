﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.ViewCreator
{
    /// <summary>
    /// Indique qu'un champ doit être exporté par ViewCreator vers le type clank donné.
    /// </summary>
    public class ExportAttribute : Attribute
    {
        public string AttrType { get; set; }
        public string Comment { get; set; }
        public ExportAttribute(string attrtype, string comment)
        {
            AttrType = attrtype;
            Comment = comment;
        }
    }

    public class AddFieldElem
    {
        public string AttrName { get; set; }
        public string AttrType { get; set; }
        public string Comment { get; set; }
        public AddFieldElem(string attrtype, string attrname, string comment)
        {
            AttrName = attrname;
            AttrType = attrtype;
            Comment = comment;
        }
    }
    /// <summary>
    /// Indique qu'un champ doit être exporté par ViewCreator vers le type clank donné.
    /// </summary>
    public class AddFieldAttribute : Attribute
    {
        public List<AddFieldElem> Fields {get;set; }
        public AddFieldAttribute(string attrtype, string attrname, string comment)
        {
            Fields = new List<AddFieldElem>();
            Fields.Add(new AddFieldElem(attrtype, attrname, comment));
        }

        public AddFieldAttribute(List<AddFieldElem> fields) { Fields = fields; }
    }
    public class AccessAttribute : Attribute
    {
        public string ObjectSource { get; set; }
        public string Comment { get; set; }
        public AccessAttribute(string objectSource, string comment)
        {
            ObjectSource = objectSource;
            Comment = comment;
        }
    }

    public class EnumAttribute : Attribute
    {
        public string Comment { get; set; }
        public EnumAttribute(string comment)
        {
            Comment = comment;
        }
    }
}

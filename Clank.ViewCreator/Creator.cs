﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
namespace Clank.ViewCreator
{
    /// <summary>
    /// Classe permettant de générer, à partir d'un type possédant des attributs 
    /// ExportAttribute sur ses propriétés / champs :
    /// 
    /// - Un fichier clank comprenant les champs donnés.
    /// - Une fonction FromView qui recrée l'objet original à partir de la view, et une fonction ToView qui crée la vue à partir
    ///   d'un objet original.
    /// 
    /// </summary>
    public class Creator
    {
        class Record
        {
            public string Type;
            public string Name;
            public string Comment;
            public Record(string type, string name, string comment)
            {
                Type = type;
                Name = name;
                Comment = comment;
            }
        }


        /// <summary>
        /// Crée une vue pour chaque type devant être exporté de l'assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CreateViews(Assembly assembly)
        {
            Dictionary<string, string> types = new Dictionary<string, string>();

            // TODO : générer les enums.
            foreach(Type type in assembly.GetTypes())
            {
                string view = CreateView(type);
                if(view.Contains(";")) // manière sale de voir s'il y a un champ.
                {
                    types.Add("_" + type.Name + "View.clank", view);
                }
            }

            
            return types;
        }

        /// <summary>
        /// Crée le fichier projet clank à partir des fichiers donnés.
        /// </summary>
        /// <param name="views"></param>
        /// <returns></returns>
        public static string CreateProjectFile(string name, string serverTargets, string clientTargets, Dictionary<string, string> files)
        {
            Clank.IDE.ProjectNode node = new IDE.ProjectNode();
            node.SavePath = "";
            foreach (var kvp in files)
                node.SourceFiles.Add(kvp.Key);
            node.Name = name;
            node.Settings = new IDE.ProjectSettings();
            node.Settings.ClientTargets = Core.Generation.GenerationTarget.TargetsFromString(clientTargets);
            node.Settings.ServerTarget = Core.Generation.GenerationTarget.FromString(serverTargets);

            StringWriter w = new StringWriter();
            XmlSerializer ser = new XmlSerializer(typeof(IDE.ProjectNode));
            ser.Serialize(w, node);
            w.Close();
            return w.ToString();
        }
        /// <summary>
        /// Crée le fichier main contenant les includes et les interfaces.
        /// </summary>
        public static string CreateMain(Assembly assembly, Dictionary<string, string> views)
        {
            StringBuilder b = new StringBuilder();
            List<string> macros = new List<string>();
            List<string> access = new List<string>();

            // Parcours tous les types pour en extraire les macros / access.
            foreach (Type type in assembly.GetTypes())
            {
                MethodInfo[] fields = type.GetMethods();
                foreach (MethodInfo info in fields)
                {
                    if (info.DeclaringType != type)
                        continue;
                    object[] attributes = info.GetCustomAttributes(typeof(AccessAttribute), false);
                    foreach(object att in attributes)
                    {
                        AccessAttribute attr = att as AccessAttribute;
                        if(attr != null)
                        {
                            macros.Add(CreateMacro("\t\t", info, attr));
                            access.Add(CreateAccess("\t\t", info, attr));
                        }
                    }
                }
            }

            b.AppendLine("main\r\n{");
            b.AppendLine("\tstate\r\n\t{");

            // Ecrit les includes
            foreach(var kvp in views)
            {
                b.AppendLine("\t\t#include " + kvp.Key);
            }

            b.AppendLine("\t}");
            // Macros
            b.AppendLine("\tmacro\r\n\t{");
            foreach(string macro in macros)
            {
                b.AppendLine(macro);
            }
            b.AppendLine();
            b.AppendLine("\t} # macro");

            // Access
            b.AppendLine("\taccess\r\n\t{");
            foreach (string a in access)
            {
                b.AppendLine(a);
            }
            b.AppendLine();
            b.AppendLine("\t} # access");

            b.AppendLine();
            b.AppendLine("} # main");
            return b.ToString();
        }



        /// <summary>
        /// Crée une fonction macro clank à partir du membre donné.
        /// </summary>
        static string CreateMacro(string padding, MethodInfo info, AccessAttribute attr)
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine(padding + "# " + attr.Comment);
            b.Append(padding + CreateTypeName(info.ReturnType) + " " + info.Name + "_macro");
            b.Append("(int clientId,");
            foreach(var arg in info.GetParameters())
            {
                b.Append(CreateTypeName(arg.ParameterType) + " " + arg.Name + ",");
            }
            b.Remove(b.Length - 1, 1); // supprime la virgule en trop
            b.Append(") { string cs = \"" + attr.ObjectSource + "." + info.Name + "($(clientId),");
            foreach (var arg in info.GetParameters())
            {
                b.Append("$(" + arg.Name + "),");
            }

            b.Remove(b.Length - 1, 1); // supprime la virgule en trop
            b.Append(")\"; }");

            return b.ToString();
        }
        /// <summary>
        /// Crée une fonction macro clank à partir du membre donné.
        /// </summary>
        static string CreateAccess(string padding, MethodInfo info, AccessAttribute attr)
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine(padding + "# " + attr.Comment);
            b.Append(padding + CreateTypeName(info.ReturnType) + " " + info.Name);
            b.Append("(int clientId,");
            foreach (var arg in info.GetParameters())
            {
                b.Append(CreateTypeName(arg.ParameterType) + " " + arg.Name + ",");
            }
            b.Remove(b.Length - 1, 1); // supprime la virgule en trop
            b.AppendLine(")");
            b.AppendLine(padding + "{ ");
            b.Append(padding + "\t" + info.Name + "_macro(clientId,");
            foreach (var arg in info.GetParameters())
            {
                b.Append(arg.Name + ",");
            }

            b.Remove(b.Length - 1, 1); // supprime la virgule en trop
            b.AppendLine(");");
            b.AppendLine(padding + "}");
            return b.ToString();
        }
        static string CreateTypeName(Type type)
        {
            StringBuilder b = new StringBuilder();
            b.Append(type.Name.Split('`')[0]);
            if (type.IsGenericType)
            {
                b.Append('<');
                foreach (Type t in type.GetGenericArguments())
                    b.Append(CreateTypeName(t) + ",");
                b.Remove(b.Length - 1, 1);
                b.Append('>');
            }
            return b.ToString();
        }


        /// <summary>
        /// Crée une vue pour le type donné.
        /// </summary>
        public static string CreateView(Type type) 
        {
            List<Record> records = new List<Record>();
            string typename = type.Name;

            MemberInfo[] fields = type.GetMembers();
            foreach (MemberInfo info in fields)
            {
                if (info.DeclaringType != type)
                    continue;
                object[] attributes = info.GetCustomAttributes(typeof(ExportAttribute), false);
                foreach(object att in attributes)
                {
                    ExportAttribute attr = att as ExportAttribute;
                    if(attr != null)
                    {
                        records.Add(new Record(attr.AttrType, info.Name, attr.Comment));
                    }
                }
            }


            StringBuilder b = new StringBuilder();
            b.AppendLine("# Généré automatiquement (Clank.ViewCreator)\r\n\r\n");
            b.AppendLine("state {");
            b.AppendLine("\tpublic serializable class " + typename + "View\r\n\t{\r\n");

            foreach(Record r in records)
            {
                b.AppendLine("\t\t#" + r.Comment);
                b.AppendLine("\t\tpublic " + r.Type + " " + r.Name + ";");
            }
            b.AppendLine("\t}");
            b.AppendLine("}");

            return b.ToString();
        }
    }
}
﻿using System.IO;
using System.Xml.Linq;

namespace TextRPG
{
    public static class PersistenceMgr
    {

        //----------------------------------------//
        //----------Methods-----------------------//
        //----------------------------------------//

        //creates a new document, initialize it as an xml adn save it 
        //with a given name
        public static void createXMLDoc(string name,string mainElemName)
        {
            XNamespace empNM = "urn:lst-emp:emp";

            XDocument xDoc = new XDocument(
                        new XDeclaration("1.0", "UTF-16", null),
                        new XElement(empNM + mainElemName));

            // Save to Disk
            xDoc.Save(name);
        }

        //initialize the fields and load the xml file or create it if it doesn't exist
        public static XElement initXML(string name, string mainElemName)
        {
            XElement xmlElem;
            try
            {
                xmlElem = XElement.Load(name);
            }
            catch (FileNotFoundException e)
            {
                createXMLDoc(name, mainElemName);
                xmlElem = XElement.Load(name);
            }
            return xmlElem;
        }

    }
}

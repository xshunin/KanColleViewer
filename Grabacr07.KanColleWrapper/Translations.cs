using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Grabacr07.KanColleWrapper.Internal;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleWrapper.Models.Raw;
using Livet;

namespace Grabacr07.KanColleWrapper
{
    public class Translations : NotificationObject
    {
        private XDocument ShipsXML;
        private XDocument ShipTypesXML;
        private XDocument EquipmentXML;
        private XDocument OperationsXML;
        private XDocument QuestsXML;

        public enum TransType { Ships, ShipTypes, Equipment, OperationMaps, OperationSortie, Quests, QuestDetail, QuestTitle };
        public bool EnableTranslations { get; set; }
        public bool EnableAddUntranslated { get; set; }

        private string CurrentCulture;

        internal Translations()
        {
            try
            {
                ShipsXML = XDocument.Load("Translations\\Ships.xml");
                ShipTypesXML = XDocument.Load("Translations\\ShipTypes.xml");
                EquipmentXML = XDocument.Load("Translations\\Equipment.xml");
                OperationsXML = XDocument.Load("Translations\\Operations.xml");
                QuestsXML = XDocument.Load("Translations\\Quests.xml");
            }
            catch { }
        }

        public void ChangeCulture(string Culture)
        {
            EnableTranslations = EnableTranslations && Culture != "ja-JP";
            CurrentCulture = Culture == "en-US" ? "" : Culture;

            try
            {
                ShipsXML = XDocument.Load("Translations\\" + CurrentCulture + "\\Ships.xml");
                ShipTypesXML = XDocument.Load("Translations\\" + CurrentCulture + "\\ShipTypes.xml");
                EquipmentXML = XDocument.Load("Translations\\" + CurrentCulture + "\\Equipment.xml");
                OperationsXML = XDocument.Load("Translations\\" + CurrentCulture + "\\Operations.xml");
                QuestsXML = XDocument.Load("Translations\\" + CurrentCulture + "\\Quests.xml");
            }
            catch { }
        }

        private IEnumerable<XElement> GetTranslationList(TransType Type)
        {
            switch(Type)
            {
                case TransType.Ships:
                    if (ShipsXML != null) 
                        return ShipsXML.Descendants("Ship");
                    break;
                case TransType.ShipTypes:
                    if (ShipTypesXML != null) 
                        return ShipTypesXML.Descendants("Type");
                    break;
                case TransType.Equipment:
                    if (EquipmentXML != null) 
                        return EquipmentXML.Descendants("Item");
                    break;
                case TransType.OperationMaps:
                    if (OperationsXML != null) 
                        return OperationsXML.Descendants("Map");
                    break;
                case TransType.OperationSortie:
                    if (OperationsXML != null) 
                        return OperationsXML.Descendants("Sortie");
                    break;
                case TransType.Quests:
                case TransType.QuestTitle:
                case TransType.QuestDetail:
                    if (QuestsXML != null) 
                        return QuestsXML.Descendants("Quest");
                    break;
            }

            return null;
        }

        public string GetTranslation(string JPString, TransType Type, Object RawData)
        {
            if (!EnableTranslations)
                return JPString;

            try
            {
                IEnumerable<XElement> TranslationList = GetTranslationList(Type);

                if (TranslationList == null)
                    return JPString;

                string JPChildElement = "JP-Name";
                string TRChildElement = "TR-Name";

                if (Type == TransType.QuestDetail)
                {
                    JPChildElement = "JP-Detail";
                    TRChildElement = "TR-Detail";
                }

                IEnumerable<XElement> FoundTranslation = TranslationList.Where(b => b.Element(JPChildElement).Value.Equals(JPString));

                foreach (XElement el in FoundTranslation)
                    return el.Element(TRChildElement).Value;
            }
            catch { }

            AddTranslation(RawData, Type);

            return JPString;
        }

        public void AddTranslation(Object RawData, TransType Type)
        {
            if (RawData == null || !EnableAddUntranslated)
                return;

            try
            {
                switch (Type)
                {
                    case TransType.Ships:
                        if (ShipsXML != null)
                        {
                            kcsapi_master_ship Data = RawData as kcsapi_master_ship;

                            if (Data == null)
                                return;

                            ShipsXML.Root.Add(new XElement("Ship",
                                new XElement("JP-Name", Data.api_name),
                                new XElement("TR-Name", Data.api_name)
                                ));
                            ShipsXML.Save("Translations\\" + CurrentCulture + "\\Ships.xml");
                        }
                        break;
                    case TransType.ShipTypes:
                        if (ShipTypesXML != null)
                        {
                            kcsapi_stype Data = RawData as kcsapi_stype;

                            if (Data == null)
                                return;

                            ShipTypesXML.Root.Add(new XElement("Type",
                                new XElement("JP-Name", Data.api_name),
                                new XElement("TR-Name", Data.api_name)
                                ));
                            ShipTypesXML.Save("Translations\\" + CurrentCulture + "\\ShipTypes.xml");
                        }
                        break;
                    case TransType.Equipment:
                        if (EquipmentXML != null)
                        {
                            kcsapi_master_slotitem Data = RawData as kcsapi_master_slotitem;

                            if (Data == null)
                                return;

                            EquipmentXML.Root.Add(new XElement("Item",
                                new XElement("JP-Name", Data.api_name),
                                new XElement("TR-Name", Data.api_name)
                                ));
                            EquipmentXML.Save("Translations\\" + CurrentCulture + "\\Equipment.xml");
                        }
                        break;
                    case TransType.OperationMaps:
                        if (OperationsXML != null)
                        {
                            kcsapi_battleresult Data = RawData as kcsapi_battleresult;

                            if (Data == null)
                                return;

                            OperationsXML.Root.Add(new XElement("Map",
                                new XElement("JP-Name", Data.api_quest_name),
                                new XElement("TR-Name", Data.api_quest_name)
                                ));
                            OperationsXML.Save("Translations\\" + CurrentCulture + "\\Operations.xml");
                        }
                        break;
                    case TransType.OperationSortie:
                        if (OperationsXML != null)
                        {
                            kcsapi_battleresult Data = RawData as kcsapi_battleresult;

                            if (Data == null)
                                return;

                            OperationsXML.Root.Add(new XElement("Sortie",
                                new XElement("JP-Name", Data.api_enemy_info.api_deck_name),
                                new XElement("TR-Name", Data.api_enemy_info.api_deck_name)
                                ));
                            OperationsXML.Save("Translations\\" + CurrentCulture + "\\Operations.xml");
                        }
                        break;
                    case TransType.Quests:
                    case TransType.QuestTitle:
                    case TransType.QuestDetail:
                        if (QuestsXML != null)
                        {
                            kcsapi_quest Data = RawData as kcsapi_quest;

                            if (Data == null)
                                return;

                            IEnumerable<XElement> FoundTranslationDetail = QuestsXML.Descendants("Quest").Where(b => b.Element("JP-Detail").Value.Equals(Data.api_detail));
                            IEnumerable<XElement> FoundTranslationTitle = QuestsXML.Descendants("Quest").Where(b => b.Element("JP-Name").Value.Equals(Data.api_title));

                            // Check the current list for any errors and fix them before writing a whole new element.
                            if (Type == TransType.QuestTitle && FoundTranslationDetail != null && FoundTranslationDetail.Any())
                            {
                                // The title is wrong, but the detail is right. Fix the title.
                                foreach (XElement el in FoundTranslationDetail)
                                    el.Element("JP-Name").Value = Data.api_title;

                            }
                            else if (Type == TransType.QuestDetail && FoundTranslationTitle != null && FoundTranslationTitle.Any())
                            {
                                // We found an existing detail, the title must be broken. Fix it.
                                foreach (XElement el in FoundTranslationTitle)
                                    el.Element("JP-Detail").Value = Data.api_detail;
                            }
                            else
                            {
                                // The quest doesn't exist at all. Add it.
                                QuestsXML.Root.Add(new XElement("Quest",
                                    new XElement("ID", Data.api_no),
                                    new XElement("JP-Name", Data.api_title),
                                    new XElement("TR-Name", Data.api_title),
                                    new XElement("JP-Detail", Data.api_detail),
                                    new XElement("TR-Detail", Data.api_detail)
                                    ));
                            }

                            QuestsXML.Save("Translations\\" + CurrentCulture + "\\Quests.xml");
                        }
                        break;
                }
            }
            catch { }
        }

    }
}

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

        public bool EnableAddUntranslated { get; set; }

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

        private IEnumerable<XElement> GetTranslationList(TransType Type)
        {
            IEnumerable<XElement> Translations = null;

            switch(Type)
            {
                case TransType.Ships:
                    if (ShipsXML != null) Translations = ShipsXML.Descendants("Ship");
                    break;
                case TransType.ShipTypes:
                    if (ShipTypesXML != null) Translations = ShipTypesXML.Descendants("Type");
                    break;
                case TransType.Equipment:
                    if (EquipmentXML != null) Translations = EquipmentXML.Descendants("Item");
                    break;
                case TransType.OperationMaps:
                    if (OperationsXML != null) Translations = OperationsXML.Descendants("Map");
                    break;
                case TransType.OperationSortie:
                    if (OperationsXML != null) Translations = OperationsXML.Descendants("Sortie");
                    break;
                case TransType.Quests:
                case TransType.QuestTitle:
                case TransType.QuestDetail:
                    if (QuestsXML != null) Translations = QuestsXML.Descendants("Quest");
                    break;
            }

            return Translations;
        }

        public string GetTranslation(string JPString, TransType Type)
        {
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
                    JPChildElement = "TR-Detail";
                }

                IEnumerable<XElement> FoundTranslation = TranslationList.Where(b => b.Element(JPChildElement).Value.Equals(JPString));

                foreach (XElement el in FoundTranslation)
                    return el.Element(TRChildElement).Value;
            }
            catch { }

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
                            ShipsXML.Save("Translations\\Ships.xml");
                        }
                        break;
                    case TransType.ShipTypes:
                        if (ShipTypesXML != null)
                        {
                            kcsapi_stype Data = RawData as kcsapi_stype;

                            if (Data == null)
                                return;

                            ShipsXML.Root.Add(new XElement("Type",
                                new XElement("JP-Name", Data.api_name),
                                new XElement("TR-Name", Data.api_name)
                                ));
                            ShipTypesXML.Save("Translations\\ShipTypes.xml");
                        }
                        break;
                    case TransType.Equipment:
                        if (EquipmentXML != null)
                        {
                            kcsapi_master_slotitem Data = RawData as kcsapi_master_slotitem;

                            if (Data == null)
                                return;

                            ShipsXML.Root.Add(new XElement("Item",
                                new XElement("JP-Name", Data.api_name),
                                new XElement("TR-Name", Data.api_name)
                                ));
                            EquipmentXML.Save("Translations\\Equipment.xml");
                        }
                        break;
                    case TransType.OperationMaps:
                        if (OperationsXML != null)
                        {
                            kcsapi_battleresult Data = RawData as kcsapi_battleresult;

                            if (Data == null)
                                return;

                            ShipsXML.Root.Add(new XElement("Map",
                                new XElement("JP-Name", Data.api_quest_name),
                                new XElement("TR-Name", Data.api_quest_name)
                                ));
                            OperationsXML.Save("Translations\\Operations.xml");
                        }
                        break;
                    case TransType.OperationSortie:
                        if (OperationsXML != null)
                        {
                            kcsapi_battleresult Data = RawData as kcsapi_battleresult;

                            if (Data == null)
                                return;

                            ShipsXML.Root.Add(new XElement("Sortie",
                                new XElement("JP-Name", Data.api_enemy_info.api_deck_name),
                                new XElement("TR-Name", Data.api_enemy_info.api_deck_name)
                                ));
                            OperationsXML.Save("Translations\\Operations.xml");
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

                            ShipsXML.Root.Add(new XElement("Quest",
                                new XElement("ID", Data.api_no),
                                new XElement("JP-Name", Data.api_title),
                                new XElement("TR-Name", Data.api_title),
                                new XElement("JP-Detail", Data.api_detail),
                                new XElement("TR-Detail", Data.api_detail)
                                ));
                            QuestsXML.Save("Translations\\Quests.xml");
                        }
                        break;
                }
            }
            catch { }
        }

        public void AddTranslation(XElement TranslationElement, TransType Type)
        {
            if (TranslationElement == null || !EnableAddUntranslated)
                return;

            try
            {
                switch (Type)
                {
                    case TransType.Ships:
                        if (ShipsXML != null)
                        {
                            ShipsXML.Root.Add(TranslationElement);
                            ShipsXML.Save("Translations\\Ships.xml");
                        }
                        break;
                    case TransType.ShipTypes:
                        if (ShipTypesXML != null)
                        {
                            ShipTypesXML.Root.Add(TranslationElement);
                            ShipTypesXML.Save("Translations\\ShipTypes.xml");
                        }
                        break;
                    case TransType.Equipment:
                        if (EquipmentXML != null)
                        {
                            EquipmentXML.Root.Add(TranslationElement);
                            EquipmentXML.Save("Translations\\Equipment.xml");
                        }
                        break;
                    case TransType.OperationMaps:
                    case TransType.OperationSortie:
                        if (OperationsXML != null)
                        {
                            OperationsXML.Root.Add(TranslationElement);
                            OperationsXML.Save("Translations\\Operations.xml");
                        }
                        break;
                    case TransType.Quests:
                    case TransType.QuestTitle:
                    case TransType.QuestDetail:
                        if (QuestsXML != null)
                        {
                            QuestsXML.Root.Add(TranslationElement);
                            QuestsXML.Save("Translations\\Quests.xml");
                        }
                        break;
                }
            }
            catch { }
        }

    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

namespace Base.IO
{

    /// <summary>
    /// Loads a piece of dialog from an xml. 
    /// Right now the path and the file tag are a constants in here. At some point
    /// this has to change.
    /// <remarks>
    /// Ok, this is the documents structure
    /// <root>                          -> document root
    ///   <dialogs>                     -> all the dialogs are beneath this node
    ///     <dial_group id="my_id">     -> this way we can group diferents dialogs (for ex: same Scene dialogs)
    ///       <dial id="other_id">      -> this is the dialog by itself
    ///         <sent>TEXT</sent>       -> sentences of the dialog
    ///       ...
    ///   <choices>                     -> all the choices are beneath this node
    ///     <choice_group id="ot_id">   -> we can group different choices, for ex: same scene
    ///       <choice id="other_id">    -> the choice 
    ///         <sent>TEXT</sent>       -> choices
    ///
    /// </remarks>
    /// </summary>
    public class LoadDialogFromXML : Node
    {

        private const string PATH_DEF = @"C:\Cosas\Programming\Godot\Phone_Horror\TestThings\";
        private const string FILE_DEF = "Test.xml";

        /// <summary>
        /// TAG used on the dialog branch
        /// </summary>
        private const string DIAL_TAG = "dialogs";

        /// <summary>
        /// TAG used on the dialog group 
        /// </summary>
        private const string DIAL_GROUP_TAG = "dial_group";

        /// <summary>
        /// TAG used on a simple dialog, the dialog itself
        /// </summary>
        private const string DIAL_SIMPLE_TAG = "dial";

        /// <summary>
        /// TAG used on the choices branch
        /// </summary>
        private static string CHOICE_TAG = "choices";

        /// <summary>
        /// Tag used on the choices group
        /// </summary>
        private const string CHOICE_GROUP_TAG = "choice_group";

        /// <summary>
        /// Tag uwsed on a simple choices, the choices itself
        /// </summary>
        private const string CHOICE_SIMPLE_TAG = "choice";

        /// <summary>
        /// The id tag
        /// </summary>
        private const string ID_TAG = "id";


        public static StringBuilder builder = new StringBuilder();

        public enum TextType : byte { DIALOG, CHOICE }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="dialId"></param>
        /// <param name="textType"></param>
        /// <returns></returns>
        public static string LoadXmlElement(in string groupId, in string dialId, in TextType textType)
        {
            using (XmlReader reader = XmlReader.Create(String.Concat(PATH_DEF, FILE_DEF)))
            {
                // we get the rood
                reader.MoveToElement();

                // we use this to get to dialogs/choices tag
                if (reader.ReadToDescendant(ElementTAGByType(textType)))
                {
                    // we are no dialogs/choices, so foreach xxx_group, find the 
                    // one wiht id = groupId
                    bool found = false;
                    string groupTAG = ProperGroupTAG(textType);
                    found = !reader.ReadToDescendant(groupTAG);

                    while (found == false)
                    {
                        if (reader.GetAttribute(ID_TAG) != groupId)
                        {
                            found = !reader.ReadToNextSibling(groupTAG);
                            MyConsole.WriteError("Not Found: " + reader.Name);
                        }
                        else
                        {
                            found = !reader.ReadToDescendant(FinalElementByType(textType));
                            while (found == false)
                            {
                                if (reader.GetAttribute("id") == dialId)
                                {
                                    ContentToBuilder(reader, reader.Name);
                                    found = true;
                                }
                                else
                                {
                                    found = !reader.ReadToNextSibling(FinalElementByType(textType));
                                }
                            }
                        }
                    }

                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns the element tag (dialogs, choiches) based on the typoe
        /// </summary>
        /// <param name="textType">type o the text</param>
        /// <returns>dialogs, choices, etc</returns>
        private static string ElementTAGByType(in TextType textType)
        {
            switch (textType)
            {
                case TextType.DIALOG:
                    return DIAL_TAG;
                case TextType.CHOICE:
                    return CHOICE_TAG;
                default:
                    return "";
            }
        }

        private static string FinalElementByType(in TextType textType)
        {
            switch (textType)
            {
                case TextType.DIALOG:
                    return DIAL_SIMPLE_TAG;
                case TextType.CHOICE:
                    return CHOICE_SIMPLE_TAG;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns the proper GroupTag based ton the textTypes
        /// </summary>
        /// <param name="textType"> the type of the text</param>
        /// <returns>XXX_group</returns>
        private static string ProperGroupTAG(in TextType textType)
        {
            switch (textType)
            {
                case TextType.DIALOG:
                    return DIAL_GROUP_TAG;
                case TextType.CHOICE:
                    return CHOICE_GROUP_TAG;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Populates the <see cref="builder"> with each line marked <sent>
        /// at last, wr return the object to string
        /// </summary>
        /// <param name="reader">the <see cref="XmlReader"> user to read</param>
        /// <param name="closeName">close tag of the element</param>
        private static void ContentToBuilder(in XmlReader reader, in string closeName)
        {
            builder.Clear();

            while (reader.Read() && reader.Name != closeName)
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    builder.AppendLine(reader.Value.ToString());                    
                }
            }
        }
    }
}

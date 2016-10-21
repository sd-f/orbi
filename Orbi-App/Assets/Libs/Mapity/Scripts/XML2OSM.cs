using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class XML2OSM
{
    // Actions
    public Action<string, string>           OnParseTagAction;
    public Action<uint>                     OnParseWayNodeRefAction;
    public Action<uint, float, float>       OnParseNodeAction;
    public Action<string,uint>              OnParseMemberAction;
    public Action<uint>                     OnParseWayAction;
    public Action<uint>                     OnParseRelationAction;
    public Action<float,float,float,float>  OnParseBoundAction;

    /// <summary>
    /// Parse a line of data
    /// </summary>
    /// <param name="strXML"> String of XML data</param>
    public void ParseLine(string strXML)
    {
        int strPosition = 0;

        if ( strPosition < strXML.Length )
        {
            // Parse Node
            ParseNode(strXML, ref strPosition);
        }
    }

    /// <summary>
    /// Parse a Node from the XML
    /// </summary>
    /// <param name="strXML">String of XML data</param>
    /// <param name="strPosition"> Ref to position in the string</param>
    void ParseNode(string strXML, ref int strPosition)
    {
        // Skip spaces until first char
        SkipSpaces(strXML, ref strPosition);

        ++strPosition;// Skip '<'

        // Skip spaces until first node name
        SkipSpaces(strXML, ref strPosition);
                
        // Tag Nodes
        if (IsNode(strXML, ref strPosition, "t"))
        {
            FindFirstSpaceAfterNode(strXML, ref strPosition);

            // k
            string strKey = (string)ParseAttributeValue(strXML, ref strPosition, "k");

            // v
            string strValue = (string)ParseAttributeValue(strXML, ref strPosition, "v");

            if (OnParseTagAction != null)
            {
                OnParseTagAction(strKey, strValue);
            }
        }
        // Way Node Refs
        else if (IsNode(strXML, ref strPosition, "nd"))
        {
            FindFirstSpaceAfterNode(strXML, ref strPosition);
            
            // ref
            uint uRef = (uint)ParseAttributeValue(strXML, ref strPosition, "r", ValueHint.Uint);

            if (OnParseWayNodeRefAction != null)
            {
                OnParseWayNodeRefAction( uRef );
            }
        }
        // Parse Nodes
        else if (IsNode(strXML, ref strPosition, "nod"))
        {
            FindFirstSpaceAfterNode(strXML, ref strPosition);            

            // id
            uint iId = (uint)ParseAttributeValue(strXML, ref strPosition, "i", ValueHint.Uint);

            // lat
            float fLat = (float)ParseAttributeValue(strXML, ref strPosition, "la", ValueHint.Float);

            // lon
            float fLon = (float)ParseAttributeValue(strXML, ref strPosition, "lo", ValueHint.Float);

            if (OnParseNodeAction != null)
            {
                OnParseNodeAction( iId, fLat, fLon );
            }
        }
        // Member Nodes
        else if (IsNode(strXML, ref strPosition, "mem"))
        {
            FindFirstSpaceAfterNode(strXML, ref strPosition);

            // type
            string strType = (string)ParseAttributeValue(strXML, ref strPosition, "t");

            // ref
            uint uRef = (uint)ParseAttributeValue(strXML, ref strPosition, "r", ValueHint.Uint);

            if (OnParseMemberAction != null)
            {
                OnParseMemberAction(strType, uRef);
            }
        }
        // Way Nodes
        else if (IsNode(strXML, ref strPosition, "w"))
        {
            FindFirstSpaceAfterNode(strXML, ref strPosition);
            
            // id
            uint uId = (uint)ParseAttributeValue(strXML, ref strPosition, "i", ValueHint.Uint);

            if (OnParseWayAction != null)
            {
                OnParseWayAction(uId);
            }
        }
        // Relation Nodes
        else if (IsNode(strXML, ref strPosition, "r"))
        {
            FindFirstSpaceAfterNode(strXML, ref strPosition);
            
            // id
            uint uId = (uint)ParseAttributeValue(strXML, ref strPosition, "i", ValueHint.Uint);

            if (OnParseRelationAction != null)
            {
                OnParseRelationAction(uId);
            }
        }
        // Bound Nodes
        else if( IsNode( strXML, ref strPosition, "b") )
        {
            FindFirstSpaceAfterNode(strXML, ref strPosition);

            // minlat
            float fMinlat = (float)ParseAttributeValue(strXML, ref strPosition, "m", ValueHint.Float);

            // minlon
            float fMinlon = (float)ParseAttributeValue(strXML, ref strPosition, "m", ValueHint.Float);

            // maxlat
            float fMaxlat = (float)ParseAttributeValue(strXML, ref strPosition, "m", ValueHint.Float);

            // maxlon
            float fMaxlon = (float)ParseAttributeValue(strXML, ref strPosition, "m", ValueHint.Float);

            if (OnParseBoundAction != null)
            {
                OnParseBoundAction(fMinlat, fMinlon, fMaxlat, fMaxlon);
            }
        }
    }

    /// <summary>
    /// Checks for a space
    /// </summary>
    /// <param name="character"></param>
    /// <returns>If a space was found</returns>
    bool IsSpace(char character)
    {
        return character == ' ' || character == '\t' || character == '\n' || character == '\r';
    }

    /// <summary>
    /// Increments strPosition to the next non space in the string
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    void SkipSpaces(string strXML, ref int strPosition)
    {
        while (strPosition < strXML.Length)
        {
            bool bFoundSpace = IsSpace(strXML[strPosition]);
            if (!bFoundSpace)
            {
                break;
            }

            ++strPosition;
        }
    }

    /// <summary>
    /// The type of the value we are looking for
    /// </summary>
    enum ValueHint
    {
       Uint,
       Float,
       String
    }

    /// <summary>
    /// Parses an attribute value by first checking if the node name hint matches, then pulls the value out based on the type hint
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    /// <param name="strNodeNameHint"></param>
    /// <param name="eValueHint"></param>
    /// <returns></returns>
    private object ParseAttributeValue(string strXML, ref int strPosition, string strNodeNameHint, ValueHint eValueHint = ValueHint.String)
    {
        object strValue;

        SkipSpaces(strXML, ref strPosition);

        bool bMatch = IsNode(strXML, ref strPosition, strNodeNameHint);

        if (bMatch)
        {
            SkipAttributeToValue(strXML, ref strPosition);

            char quote = strXML[strPosition]; // get " quote

            ++strPosition; // skip quote

            if (eValueHint == ValueHint.Uint)
            {
                strValue = GetValueAsUint(strXML, ref strPosition, quote, '\0', false);
            }
            else if (eValueHint == ValueHint.Float)
            {
                strValue = GetValueAsFloat(strXML, ref strPosition, quote, '\0', false);
            }
            else
            {
                strValue = GetValueAsString(strXML, ref strPosition, quote, '\0', false);
            }

            ++strPosition; // skip quote
        }
        else
        {
            //Skip node and value
            SkipAttributeAndValue(strXML, ref strPosition);

            // Try again
            strValue = ParseAttributeValue( strXML, ref strPosition, strNodeNameHint, eValueHint );
        }

        return strValue;
    }

    /// <summary>
    /// Skips an unused attribute and value in the xml data
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    private void SkipAttributeAndValue(string strXML, ref int strPosition)
    {
        while (strXML[strPosition] != '=' && strXML[strPosition] != '\0')
        {
            ++strPosition;
        }

        ++strPosition; // skip '='

        SkipSpaces(strXML, ref strPosition);

        char quote = strXML[strPosition]; // get " quote

        ++strPosition; // skip quote

        while (strXML[strPosition] != quote && strXML[strPosition] != '\0')
        {
            ++strPosition;
        }

        ++strPosition; // skip quote
    }

    /// <summary>
    /// Skips the attribute until it finds its value
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    private void SkipAttributeToValue(string strXML, ref int strPosition)
    {
        while (strXML[strPosition] != '=' && strXML[strPosition] != '\0')
        {
            ++strPosition;
        }

        ++strPosition; // skip '='

        SkipSpaces(strXML, ref strPosition);
    }

    /// <summary>
    /// Finds the first space after a node
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    private void FindFirstSpaceAfterNode(string strXML, ref int strPosition)
    {
        strPosition += 2; // Move forward 2 chars, each node is at least 2 chars long

        while (!IsSpace(strXML[strPosition]))
        {
            ++strPosition;
        }
    }

    /// <summary>
    /// Gets Value As String
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    /// <param name="endChar"></param>
    /// <param name="altEndChar"></param>
    /// <param name="stopOnSpace"></param>
    /// <returns></returns>
    string GetValueAsString(string strXML, ref int strPosition, char endChar, char altEndChar, bool stopOnSpace)
    {
        int startPos = strPosition;

        FindEnd(strXML, ref strPosition, endChar, altEndChar, stopOnSpace);

        return strXML.Substring(startPos, strPosition - startPos);
    }

    /// <summary>
    /// Gets Value As Uint
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    /// <param name="endChar"></param>
    /// <param name="altEndChar"></param>
    /// <param name="stopOnSpace"></param>
    /// <returns></returns>
    uint GetValueAsUint(string strXML, ref int strPosition, char endChar, char altEndChar, bool stopOnSpace)
    {
        int startPos = strPosition;

        FindEnd(strXML, ref strPosition, endChar, altEndChar, stopOnSpace);

        return ToUint( strXML, startPos, strPosition - startPos);
    }

    /// <summary>
    /// Gets Value As Float
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    /// <param name="endChar"></param>
    /// <param name="altEndChar"></param>
    /// <param name="stopOnSpace"></param>
    /// <returns></returns>
    float GetValueAsFloat(string strXML, ref int strPosition, char endChar, char altEndChar, bool stopOnSpace)
    {
        int startPos = strPosition;

        FindEnd(strXML, ref strPosition, endChar, altEndChar, stopOnSpace);

        return ToFloat(strXML, startPos, strPosition - startPos);
    }

    /// <summary>
    /// Finds the end position
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    /// <param name="endChar"></param>
    /// <param name="altEndChar"></param>
    /// <param name="stopOnSpace"></param>
    /// <returns></returns>
    private int FindEnd(string strXML, ref int strPosition, char endChar, char altEndChar, bool stopOnSpace)
    {
        while ((!stopOnSpace || !IsSpace(strXML[strPosition])) && strXML[strPosition] != endChar && strXML[strPosition] != altEndChar)
        {
            ++strPosition;
        }
        return strPosition;
    }

    /// <summary>
    /// Checks if the node anmes matches, earlying out as soon as possible
    /// </summary>
    /// <param name="strXML"></param>
    /// <param name="strPosition"></param>
    /// <param name="strNode"></param>
    /// <returns></returns>
    bool IsNode( string strXML, ref int strPosition, string strNode )
    {
        int iStartPos = strPosition;

        int strNodeLength = strNode.Length;

        bool bMatch = strNodeLength > 0 ? true : false;

        int strNodeIndex = 0;

        while( strNodeIndex < strNodeLength )
        {
            if (strXML[iStartPos + strNodeIndex] != strNode[strNodeIndex])
            {
                bMatch = false;

                break;
            }

            ++strNodeIndex;
        }

        return bMatch;
    }

    /// <summary>
    /// Converts a string to a uint
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="startPos"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    uint ToUint(string strValue, int startPos, int length)
    {
        uint uintValue = 0;

        int strPosition = 0;

        for (; strPosition < length; ++strPosition)
        {
            uintValue = (uint)( uintValue * 10 + (strValue[startPos + strPosition] - '0') );
        }

        return uintValue;
    }

    /// <summary>
    /// Converts a string to a float
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="startPos"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    float ToFloat(string strValue, int startPos, int length)
    {
        float floatValue = 0;
        int decimalPointPosition = 0;

        bool negative = false;

        int strPosition = startPos;

        if (strValue[strPosition] == '-')
        {
            negative = true;
            ++strPosition;
        }

        for (; strPosition - startPos < length; ++strPosition)
        {
            if (strValue[strPosition] == '.')
            {
                decimalPointPosition = strPosition - startPos;
                continue;
            }

            floatValue = floatValue * 10 + (strValue[strPosition] - '0');
        }

        if (negative)
        {
            floatValue *= -1.0f;
        }

        if( decimalPointPosition > 0 )
        {
            int offset = ( length - decimalPointPosition ) - 1; // Take one for the decimal place itself

            for (int i = 0; i < offset; ++i)
            {
                // Shift decimal place
                floatValue *= 0.1f;
            }
        }

        return floatValue;
    }
}

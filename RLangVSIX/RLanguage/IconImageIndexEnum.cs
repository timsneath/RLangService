using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLanguage
{
    public enum IconImageIndex
    {
        // access types
        AccessPublic = 0,
        AccessInternal = 1,
        AccessFriend = 2,
        AccessProtected = 3,
        AccessPrivate = 4,
        AccessShortcut = 5,

        Base = 6,
        // Each of the following icon type has 6 versions,
        //corresponding to the access types
        Class = Base + 0,
        Constant = Base + 1,
        Delegate = Base + 2,
        Enumeration = Base + 3,
        EnumMember = Base + 4,
        Event = Base + 5,
        Exception = Base + 6,
        Field = Base + 7,
        Interface = Base + 8,
        Macro = Base + 9,
        Map = Base + 10,
        MapItem = Base + 11,
        Method = Base + 12,
        OverloadedMethod = Base + 13,
        Module = Base + 14,
        Namespace = Base + 15,
        Operator = Base + 16,
        Property = Base + 17,
        Struct = Base + 18,
        Template = Base + 19,
        Typedef = Base + 20,
        Type = Base + 21,
        Union = Base + 22,
        Variable = Base + 23,
        ValueType = Base + 24,
        Intrinsic = Base + 25,
        JavaMethod = Base + 26,
        JavaField = Base + 27,
        JavaClass = Base + 28,
        JavaNamespace = Base + 29,
        JavaInterface = Base + 30,
        // Miscellaneous icons with one icon for each type.
        Error = 187,
        GreyedClass = 188,
        GreyedPrivateMethod = 189,
        GreyedProtectedMethod = 190,
        GreyedPublicMethod = 191,
        BrowseResourceFile = 192,
        Reference = 193,
        Library = 194,
        VBProject = 195,
        VBWebProject = 196,
        CSProject = 197,
        CSWebProject = 198,
        VB6Project = 199,
        CPlusProject = 200,
        Form = 201,
        OpenFolder = 202,
        ClosedFolder = 203,
        Arrow = 204,
        CSClass = 205,
        Snippet = 206,
        Keyword = 207,
        Info = 208,
        CallBrowserCall = 209,
        CallBrowserCallRecursive = 210,
        XMLEditor = 211,
        VJProject = 212,
        VJClass = 213,
        ForwardedType = 214,
        CallsTo = 215,
        CallsFrom = 216,
        Warning = 217,
    }

}

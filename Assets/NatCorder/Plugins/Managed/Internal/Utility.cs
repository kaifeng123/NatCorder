/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

//#define DOC_GEN // Internal. Do not use

namespace NatSuite.Recorders.Internal {

    using UnityEngine;
    using System;
    using System.IO;
    #if DOC_GEN
    using System.Linq;
    using Calligraphy;
    #endif

    public static class Utility {

        private static string directory;

        public static string GetPath (string extension) {
            if (directory == null) {
                var editor = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor|| Application.platform == RuntimePlatform.WindowsPlayer;
                directory = editor ? Directory.GetCurrentDirectory() : Application.persistentDataPath;
            }
            var timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            var name = $"recording_{timestamp}{extension}";
            var path = Path.Combine(directory, name);
            return path;
        }
    }

    public sealed class DocAttribute : 
    #if DOC_GEN
    CADescriptionAttribute {
        public DocAttribute (string descriptionKey) : base(Docs.docs[descriptionKey]) {}
        public DocAttribute (string summaryKey, string descriptionKey) : base(Docs.docs[descriptionKey], Docs.docs[summaryKey]) {}
    #else
    Attribute {
        public DocAttribute (string descriptionKey) {}
        public DocAttribute (string summaryKey, string descriptionKey) {}
    #endif
    }

    public sealed class CodeAttribute :
    #if DOC_GEN
    CACodeExampleAttribute {
        public CodeAttribute (string key) : base(Docs.examples[key]) {}
    #else
    Attribute {
        public CodeAttribute (string key) {}
    #endif
    }

    public sealed class RefAttribute :
    #if DOC_GEN
    CASeeAlsoAttribute {
        public RefAttribute (params string[] keys) : base(keys.Select(k => Docs.references[k]).ToArray()) {}
    #else
    Attribute {
        public RefAttribute (params string[] keys) {}
    #endif
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5485
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LythumOSL.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LythumOSL.Core.Resources.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Object &apos;{0}&apos; is not valid!.
        /// </summary>
        internal static string ObjectIsNotValid1 {
            get {
                return ResourceManager.GetString("ObjectIsNotValid1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to String &apos;{0}&apos; is not valid!.
        /// </summary>
        internal static string StringIsNotValid1 {
            get {
                return ResourceManager.GetString("StringIsNotValid1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to String &apos;{0}&apos; must be convertable to numeric!.
        /// </summary>
        internal static string StringMustBeNumeric1 {
            get {
                return ResourceManager.GetString("StringMustBeNumeric1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to String &apos;{0}&apos; is wrong length, must be exactly {1} lenght!.
        /// </summary>
        internal static string StringWrongLenRequire2 {
            get {
                return ResourceManager.GetString("StringWrongLenRequire2", resourceCulture);
            }
        }
    }
}
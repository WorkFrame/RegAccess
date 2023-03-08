using System.Text.RegularExpressions;
using NetEti.Globals;
using Microsoft.Win32;

namespace NetEti.ApplicationEnvironment
{
    /// <summary>
    /// Aufzählung der Registry-Rootkeys.
    /// </summary>
    public enum RegistryRoot
    {
        /// <summary>Registry-Root.</summary>
        HkeyClassesRoot,
        /// <summary>Registry-Root.</summary>
        HkeyCurrentUser,
        /// <summary>Registry-Root.</summary>
        HkeyLocalMachine,
        /// <summary>Registry-Root.</summary>
        HkeyUsers,
        /// <summary>Registry-Root.</summary>
        HkeyCurrentConfig
    };

    /// <summary>
    /// Liest Werte aus der Registry ein.
    /// </summary>
    /// <remarks>
    /// File: RegAccess.cs<br></br>
    /// Autor: Erik Nagel, NetEti<br></br>
    ///<br></br>
    /// 08.03.2012 Erik Nagel: erstellt<br></br>
    /// 15.10.2017 Erik Nagel: RegistryBasePath eingeführt.<br></br>
    /// </remarks>
    public class RegAccess : IGetStringValue
    {
        #region public members

        #region IGetStringValue Members

        /// <summary>
        /// Liefert den Wert eines einzelnen Parameters (Regedit rechts).
        /// Nur bei String-Parametern anwendbar!
        /// </summary>
        /// <param name="key">
        /// Pfad zum Parameter, dessen Wert (Regedit rechte Seite) gesucht werden
        /// soll, ausgehend von aktuell eingestellten RegistryRoot und RegistryBasePath.
        /// </param>
        /// <param name="defaultValue">Wird zurückgeliefert, wenn kein passender Eintrag gefunden wurde.</param>
        /// <returns>String-Wert des Keys (Registry rechts)</returns>
        public string? GetStringValue(string key, string? defaultValue)
        {
            string? rtn = null;
            string keyString = Regex.Replace(key, @"\\[^\\]*$", "");
            if (keyString.Equals(key))
            {
                keyString = "";
            }
            if (!String.IsNullOrEmpty(this.RegistryBasePath))
            {
                keyString = Path.Combine(this.RegistryBasePath, keyString);
            }
            string valueName = Regex.Replace(key, @".*\\", "");
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("RegAccess: Registry-Zugriffe sind nur für Windows implementiert.");
            }
            RegistryKey? rk
                = this._rootKeys[this.CurrentRegistryRoot].OpenSubKey(keyString);
            if (rk != null)
            {
                foreach (string sKey in rk.GetValueNames())
                {
                    if (sKey == valueName)
                    {
                        try
                        {
                            rtn = (string?)rk.GetValue(sKey);
                            break;
                        }
                        catch (InvalidCastException /* ex */)
                        {
                        }
                    }
                }
                rk.Close();
            }
            if ((String.IsNullOrEmpty(rtn) || (rtn.Trim().Length == 0)))
            {
                rtn = defaultValue;
            }
            return (rtn);
        }

        /// <summary>
        /// Liefert ein Array der Werte eines einzelnen Parameters (Regedit rechts).
        /// Nur bei REG_MULTI_SZ-Parametern anwendbar!
        /// </summary>
        /// <param name="key">
        /// Pfad zum Parameter, dessen Wert (Regedit rechte Seite) gesucht werden
        /// soll, ausgehend von aktuell eingestellten RegistryRoot und RegistryBasePath.
        /// </param>
        /// <param name="defaultValues">Werden zurückgeliefert, wenn keine passenden Einträge gefunden wurden.</param>
        /// <returns>String-Werte (REG_MULTI_SZ)) des Keys (Registry rechts)</returns>
        public string?[]? GetStringValues(string key, string?[]? defaultValues)
        {
            string?[]? rtn = null;
            string keyString = Regex.Replace(key, @"\\[^\\]*$", "");
            if (keyString.Equals(key))
            {
                keyString = "";
            }
            if (!String.IsNullOrEmpty(this.RegistryBasePath))
            {
                keyString = Path.Combine(this.RegistryBasePath, keyString);
            }
            string valueName = Regex.Replace(key, @".*\\", "");
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("RegAccess: Registry-Zugriffe sind nur für Windows implementiert.");
            }
            RegistryKey? rk
                = this._rootKeys[this.CurrentRegistryRoot].OpenSubKey(keyString);
            if (rk != null)
            {
                foreach (string sKey in rk.GetValueNames())
                {
                    if (sKey == valueName)
                    {
                        try
                        {
                            rtn = (string?[]?)rk.GetValue(sKey);
                            break;
                        }
                        catch (InvalidCastException /* ex */)
                        {
                        }
                    }
                }
                rk.Close();
            }
            if (rtn == null)
            {
                rtn = defaultValues;
            }
            return rtn;
        }

        /// <summary>
        /// Liefert einen beschreibenden Namen dieses StringValueGetters,
        /// z.B. Name plus ggf. Quellpfad.
        /// </summary>
        public string Description { get; set; }

        #endregion IGetStringValue Members

        /// <summary>
        /// Die aktuell eingestellte RegistryRoot
        /// Default: RegistryRoot.HkeyLocalMachine
        /// </summary>
        public RegistryRoot CurrentRegistryRoot { get; set; }

        /// <summary>
        /// Basis-Pfad, in dem in der Registry nach einer Einstellung gesucht wird.
        /// Enthält der Pfad eine der RegistryRoots, z.B. "HKEY_CURRENT_USER", wird
        /// die intern eingestellte RegistryRoot ebenfalls umgestellt.
        /// Default für die intern eingestellte RegistryRoot ist "HKEY_LOCAL_MACHINE".
        /// Default: ""
        /// </summary>
        public string RegistryBasePath
        {
            get
            {
                return this._registryBasePath;
            }
            set
            {
                this.SetRegistryBasePath(value);
            }
        }

        /// <summary>
        /// Parametrisierter Konstruktur, erwartet eine RegistryRoot.
        /// </summary>
        /// <param name="initRegRoot">Die vorerst zu verwendende RegistryRoot</param>
        public RegAccess(RegistryRoot initRegRoot)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("RegAccess: Registry-Zugriffe sind nur für Windows implementiert.");
            }
            this._rootKeys = new Dictionary<RegistryRoot, RegistryKey>();
            this._rootKeyStrings = new Dictionary<string, RegistryRoot>();
            this._rootKeys[RegistryRoot.HkeyClassesRoot] = Registry.ClassesRoot;
            this._rootKeyStrings["HKEY_CLASSES_ROOT"] = RegistryRoot.HkeyClassesRoot;
            this._rootKeys[RegistryRoot.HkeyCurrentUser] = Registry.CurrentUser;
            this._rootKeyStrings["HKEY_CURRENT_USER"] = RegistryRoot.HkeyCurrentUser;
            this._rootKeys[RegistryRoot.HkeyLocalMachine] = Registry.LocalMachine;
            this._rootKeyStrings["HKEY_LOCAL_MACHINE"] = RegistryRoot.HkeyLocalMachine;
            this._rootKeys[RegistryRoot.HkeyUsers] = Registry.Users;
            this._rootKeyStrings["HKEY_USERS"] = RegistryRoot.HkeyUsers;
            this._rootKeys[RegistryRoot.HkeyCurrentConfig] = Registry.CurrentConfig;
            this._rootKeyStrings["HKEY_CURRENT_CONFIG"] = RegistryRoot.HkeyCurrentConfig;
            this.CurrentRegistryRoot = initRegRoot;
            this.Description = "Registry: " + this.CurrentRegistryRoot.ToString();
            this._registryBasePath = "";
            this.RegistryBasePath = "";
        }

        /// <summary>
        /// Parameterloser Konstruktor, setzt die RegistryRoot auf Default = 
        /// RegistryRoot.HkeyLocalMachine und ruft damit den parametrisierten Konstruktor.
        /// </summary>
        public RegAccess()
          : this(RegistryRoot.HkeyLocalMachine) { }

        /// <summary>
        /// Setzt den Registry-Zugriffskey für alle nachfolgende Zugriffe auf
        /// den übergebenen Basis-Pfad, wenn sich der übergebene registryBasePath
        /// fehlerfrei in ein entsprechendes Equivalent aus Registy-Keys umwandeln lässt.
        /// </summary>
        /// <param name="registryBasePath">Pfad zum Registry-Key, unterhalb dessen zukünftige Zugriffe erfolgen sollen.</param>
        /// <exception cref="ArgumentException">Wird ausgelöst, wenn der übergebene registryBasePath nicht in einen RegistryKey konvertierbar ist.</exception>
        public void SetRegistryBasePath(string registryBasePath)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("RegAccess: Registry-Zugriffe sind nur für Windows implementiert.");
            }
            registryBasePath = registryBasePath.TrimEnd(new char[] { '\\', '/' });
            RegistryRoot accessRegistryRoot = this.CurrentRegistryRoot;
            string rootKey = registryBasePath.Split(new char[] { '/', '\\' })[0];
            if (this._rootKeyStrings.ContainsKey(rootKey))
            {
                accessRegistryRoot = this._rootKeyStrings[rootKey];
                registryBasePath = registryBasePath.Replace(rootKey, "").TrimStart(new char[] { '\\', '/' });
            }
            RegistryKey? rk = this._rootKeys[accessRegistryRoot].OpenSubKey(registryBasePath);
            if (rk != null)
            {
                this._registryBasePath = registryBasePath;
                this.CurrentRegistryRoot = accessRegistryRoot;
            }
        }

        /// <summary>
        /// Liefert die Unterschlüssel von RegistryRoot + dem übergebenen keyString
        /// (Regedit: linke Seite).
        /// </summary>
        /// <param name="keyString">
        /// Pfad zum Schlüssel, dessen Subkeys gesucht werden sollen,
        /// ausgehend von der aktuell eingestellten RegistryRoot.
        /// </param>
        /// <returns>String[] mit den Schlüsselnamen (kann leer sein)</returns>
        public String[] GetSubKeyNames(string keyString)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("RegAccess: Registry-Zugriffe sind nur für Windows implementiert.");
            }
            string[] rtn = { };
            if (!String.IsNullOrEmpty(this.RegistryBasePath))
            {
                keyString = Path.Combine(this.RegistryBasePath, keyString);
            }
            RegistryKey? rk
                = this._rootKeys[this.CurrentRegistryRoot].OpenSubKey(keyString);
            if (rk != null)
            {
                rtn = rk.GetSubKeyNames();
                rk.Close();
            }
            return (rtn);
        }

        /// <summary>
        /// Liefert zu einem Schlüssel in der Registry links die Parameternamen von
        /// der rechten Seite der Registry (Regedit).
        /// </summary>
        /// <param name="keyString">
        /// Pfad zum Schlüssel, dessen Parameter-Namen (Regedit rechts) gesucht
        /// werden sollen, ausgehend von der aktuell eingestellten RegistryRoot.
        /// </param>
        /// <returns>String[] mit den Parameternamen (kann leer sein)</returns>
        public String[] GetSubValueNames(string keyString)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("RegAccess: Registry-Zugriffe sind nur für Windows implementiert.");
            }
            string[] rtn = { };
            if (!String.IsNullOrEmpty(this.RegistryBasePath))
            {
                keyString = Path.Combine(this.RegistryBasePath, keyString);
            }
            RegistryKey? rk
                = this._rootKeys[this.CurrentRegistryRoot].OpenSubKey(keyString);
            if (rk != null)
            {
                rtn = rk.GetValueNames();
                rk.Close();
            }
            return (rtn);
        }

        /*
        public void SetStringRegistryValue(string key, string stringValue)
        {
          RegistryKey rkSoftware;
          RegistryKey rkCompany;
          RegistryKey rkApplication;
          rkSoftware = Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY, true);
          rkCompany = rkSoftware.CreateSubKey(COMPANY_NAME);
          if (rkCompany != null)
          {
            rkApplication = rkCompany.CreateSubKey(APPLICATION_NAME);
            if (rkApplication != null)
            {
              rkApplication.SetValue(key, stringValue);
            }
          }
        }
        */

        #endregion public members

        #region private members

        private Dictionary<RegistryRoot, RegistryKey> _rootKeys;
        private string _registryBasePath;
        private Dictionary<string, RegistryRoot> _rootKeyStrings;

        #endregion private members

    }
}
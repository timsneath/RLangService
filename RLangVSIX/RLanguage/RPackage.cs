using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Microsoft.VisualStudio.Package;

namespace RLanguagePackage
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    //[PackageRegistration(UseManagedResourcesOnly = true)]
    //[InstalledProductRegistration("#1110", "#1112", "1.0", IconResourceID = 1400)] // Info on this package for Help/About
    [Guid(RPackageGuids.PackageGuidString)]
    //[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideService(typeof(RLanguageService), ServiceName = "R Language Service")]
    [ProvideLanguageExtension(typeof(RLanguageService), ".R")]
    [ProvideLanguageService(typeof(RLanguageService), "R", 1106, CodeSense = false, EnableCommenting = true, RequestStockColors = true, EnableAsyncCompletion = false)]
    //[ProvideLanguageCodeExpansion(typeof(RLanguageService), "R Language Service", 1106, "rlanguage", @"%InstallRoot%\R\SnippetsIndex.xml")]
    public class RPackage : Package, IOleComponent
    {
        private uint m_componentID;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            var serviceContainer = this as IServiceContainer;
            var langService = new RLanguageService();
            langService.SetSite(this);
            serviceContainer.AddService(typeof(RLanguageService), langService, true);

            IOleComponentManager mgr = GetService(typeof(SOleComponentManager)) as IOleComponentManager;
            if (m_componentID == 0 && mgr != null)
            {
                OLECRINFO[] crinfo = new OLECRINFO[1];
                crinfo[0].cbSize            = (uint)Marshal.SizeOf(typeof(OLECRINFO));
                crinfo[0].grfcrf            = (uint)_OLECRF.olecrfNeedIdleTime |
                                              (uint)_OLECRF.olecrfNeedPeriodicIdleTime;
                crinfo[0].grfcadvf          = (uint)_OLECADVF.olecadvfModal |
                                              (uint)_OLECADVF.olecadvfRedrawOff |
                                              (uint)_OLECADVF.olecadvfWarningsOff;
                crinfo[0].uIdleTimeInterval = 1000;
                int hr = mgr.FRegisterComponent(this, crinfo, out m_componentID);

                Debug.WriteLineIf(hr != 0, "hr != 0");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (m_componentID != 0)
            {
                IOleComponentManager mgr = GetService(typeof(SOleComponentManager)) as IOleComponentManager;

                if (mgr != null)
                {
                    int hr = mgr.FRevokeComponent(m_componentID);
                }
                m_componentID = 0;
            }
            base.Dispose(disposing);
        }

        #region IOleComponent Members
        int IOleComponent.FDoIdle(uint grfidlef)
        {
            bool bPeriodic = (grfidlef & (uint)_OLEIDLEF.oleidlefPeriodic) != 0;

            var service = GetService(typeof(RLanguageService)) as LanguageService;

            if (service != null)
            {
                service.OnIdle(bPeriodic);
            }

            return 0;
        }

        int IOleComponent.FContinueMessageLoop(uint uReason, IntPtr pvLoopData, MSG[] pMsgPeeked)
        {
            return 1;
        }

        int IOleComponent.FPreTranslateMessage(MSG[] pMsg)
        {
            return 0;
        }

        int IOleComponent.FQueryTerminate(int fPromptUser)
        {
            return 1;
        }

        int IOleComponent.FReserved1(uint dwReserved, uint message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        IntPtr IOleComponent.HwndGetWindow(uint dwWhich, uint dwReserved)
        {
            return IntPtr.Zero;
        }

        void IOleComponent.OnActivationChange(IOleComponent pic, int fSameComponent, OLECRINFO[] pcrinfo, int fHostIsActivating, OLECHOSTINFO[] pchostinfo, uint dwReserved)
        {
        }

        void IOleComponent.OnAppActivate(int fActive, uint dwOtherThreadID)
        {
        }

        void IOleComponent.OnEnterState(uint uStateID, int fEnter)
        {
        }

        void IOleComponent.OnLoseActivation()
        {
        }

        void IOleComponent.Terminate()
        {
        }

        #endregion
    }
}

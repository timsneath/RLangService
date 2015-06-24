using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace RLanguage
{
    public class RLanguageService : LanguageService
    {

        private LanguagePreferences m_preferences;
        private RLanguageScanner m_scanner;

        public override string Name
        {
            get
            {
                return "R Language";
            }
        }

        public override string GetFormatFilterList()
        {
            throw new NotImplementedException();
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            if (m_preferences == null)
            {
                m_preferences = new LanguagePreferences(this.Site,
                                                        typeof(RLanguageService).GUID,
                                                        this.Name);
                m_preferences.Init();
            }

            return m_preferences;
        }


        /// <summary>
        /// This method returns an instance of an IScanner object that implements a line-oriented parser or scanner used for obtaining tokens and their types and triggers. This scanner is used in the Colorizer class for colorization although the scanner can also be used for getting token types and triggers as a prelude to a more complex parsing operation. You must supply the class that implements the IScanner interface and you must implement all the methods on the IScanner interface.
        /// </summary>
        /// <param name="buffer">Text buffer</param>
        /// <returns></returns>
        public override IScanner GetScanner(IVsTextLines buffer)
        {
            if (m_scanner == null)
            {
                m_scanner = new RLanguageScanner(buffer);
            }

            return m_scanner;
        }

        /// <summary>
        /// Parses the source file based on a number of different reasons. This method is given a ParseRequest object that describes what is expected from a particular parsing operation. The ParseSource method invokes a more complex parser that determines token functionality and scope. The ParseSource method is used in support for IntelliSense operations as well as brace matching. Even if you do not support such advanced operations, you still must return a valid AuthoringScope object and that requires you to create a class that implements the AuthoringScope interface and implement all methods on that interface. You can return null values from all methods but the AuthoringScope object itself must not be a null value.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public override AuthoringScope ParseSource(ParseRequest req)
        {
            return new RAuthoringScope();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace RLanguagePackage
{
    class RLanguageSource : Source
    {
        public RLanguageSource(LanguageService service, IVsTextLines textLines, Colorizer colorizer) : base(service, textLines, colorizer)
        {
        }

        public override CommentInfo GetCommentFormat()
        { 
            var commentInfo = new CommentInfo();
            commentInfo.LineStart = "#";
            commentInfo.UseLineComments = true;

            return commentInfo;
        }
    }
}
using md_dbdocs.app.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Models.DocsModels
{
    public class DocsGeneralInfoModel
    {
        public int SectionId { get; set; }                          // section to which it belongs
        public int MinorId { get; set; }                            // list position
        public string MajorId { get { return GetMajorId(); } }      // string format "1.0.0.0"
        public int HeaderLevel { get; set; }                        // header level (count of #)
        public string Title { get; set; }                           // section title
        public string Header { get; set; }                          // section md header
        public string MdLink { get; set; }                          // markdown style internal link
        public string FilePath { get; set; }                        // path to md file with description

        public RelayCommand BrowseCommand { get; set; }

        private string GetMajorId() => $"{ SectionId }.0.0.{ MinorId }";
    }
}

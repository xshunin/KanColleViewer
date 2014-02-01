using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.KanColleWrapper.Models.Raw
{
    // ReSharper disable InconsistentNaming
    public class kcsapi_ranking
    {
        public int api_no { get; set; }
        public int api_member_id { get; set; }
        public int api_level { get; set; }
        public int api_rank { get; set; }
        public string api_nickname { get; set; }
        public int api_experience { get; set; }
        public string api_comment { get; set; }
        public int api_rate { get; set; }
        public int api_flag { get; set; }
        public string api_nickname_id { get; set; }
        public string api_comment_id { get; set; }
    }
    // ReSharper restore InconsistentNaming
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KabuUriKai.Common;

namespace KabuUriKai
{
    public class KabuData : CommonData
    {
        /// <summary>
        /// 市場
        /// </summary>
        public string Shijyou { get; set; }
        /// <summary>
        /// 業種
        /// </summary>
        public string Gyoushu { get; set; }
        /// <summary>
        /// 始値
        /// </summary>
        public int? Hajimene { get; set; }
        /// <summary>
        /// 高値
        /// </summary>
        public int? Takane { get; set; }
        /// <summary>
        /// 安値
        /// </summary>
        public int? Yasune { get; set; }
        /// <summary>
        /// 出来高
        /// </summary>
        public int? Dekidaka { get; set; }
        /// <summary>
        /// 日付
        /// </summary>
        public DateTime? Hiduke { get; set; }
    }
}

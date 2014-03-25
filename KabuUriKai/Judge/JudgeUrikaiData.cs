using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KabuUriKai.Common;

namespace KabuUriKai.Judge
{
    public class JudgeUrikaiData : CommonData
    {
        /// <summary>
        /// 売買判定値(true:買、false:売)
        /// </summary>
        public bool? Urikai { get; set; }
        /// <summary>
        /// 平均値計算
        /// </summary>
        public double? Avg { get; set; }

        public JudgeUrikaiData( bool? Urikai = null, double? Avg = null ) : base()
        {
            this.Urikai = Urikai;
            this.Avg = Avg;
        }

        public JudgeUrikaiData(string Code, string Meigara = null, double? Owarine = null)
            : base(Code,Meigara,Owarine)
        {
            this.Urikai = null;
            this.Avg = null;
        }

        public JudgeUrikaiData(string Code, string Meigara, double? Owarine = null,
                                    bool? Urikai = null, double? Avg = null) : base( Code, Meigara, Owarine )
        {
            this.Urikai = Urikai;
            this.Avg = Avg;
        }

    }
}

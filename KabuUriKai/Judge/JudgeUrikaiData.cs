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
        public string Urikai { get; set; }
        /// <summary>
        /// 平均値計算
        /// </summary>
        public double Avg { get; set; }

        public JudgeUrikaiData( string Urikai = null, double Avg = 0.0 ) : base()
        {
            this.Urikai = Urikai;
            this.Avg = Avg;
        }

        public JudgeUrikaiData(string Code, string Meigara = null, string Owarine = null)
            : base(Code,Meigara,Owarine)
        {
            this.Urikai = null;
            this.Avg = 0.0;
        }

        public JudgeUrikaiData(string Code, string Meigara, string Owarine = null,
                                    string Urikai = null, double Avg = 0.0) : base( Code, Meigara, Owarine )
        {
            this.Urikai = Urikai;
            this.Avg = Avg;
        }

    }
}

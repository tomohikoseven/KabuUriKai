
namespace KabuUriKai.Common
{
    public class CommonData : URIData
    {
        /// <summary>
        /// 銘柄コード
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 銘柄名
        /// </summary>
        public string Meigara { get; set; }
        /// <summary>
        /// 終値
        /// </summary>
        public string Owarine { get; set; }

        public CommonData( string Code = null, string Meigara = null, string Owarine = null )
        {
            this.Code = Code;
            this.Meigara = Meigara;
            this.Owarine = Owarine;
        }

    }
}

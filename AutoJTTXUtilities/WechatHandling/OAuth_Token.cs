using System.Net;
using System.Text;

namespace AutoJTTXUtilities.WechatHandling
{
    public class OAuth_Token
    {
        public string _access_token;

        public string _expires_in;

        public string _refresh_token;

        public string _openid;

        public string _scope;

        public string access_token
        {
            get
            {
                return this._access_token;
            }
            set
            {
                this._access_token = value;
            }
        }

        public string expires_in
        {
            get
            {
                return this._expires_in;
            }
            set
            {
                this._expires_in = value;
            }
        }

        public string refresh_token
        {
            get
            {
                return this._refresh_token;
            }
            set
            {
                this._refresh_token = value;
            }
        }

        public string openid
        {
            get
            {
                return this._openid;
            }
            set
            {
                this._openid = value;
            }
        }

        public string scope
        {
            get
            {
                return this._scope;
            }
            set
            {
                this._scope = value;
            }
        }

        public string GetJson(string url)
        {
            string returnText = new WebClient
            {
                Credentials = CredentialCache.DefaultCredentials,
                Encoding = Encoding.UTF8
            }.DownloadString(url);
            returnText.Contains("errcode");
            return returnText;
        }

        public OAuth_Token Get_token(string Code)
        {
            string appid = "wx0e61e107650f560f";
            string appsecret = "97689e8d760ac4e4c482b3aff2c33bd9";
            string Str = this.GetJson(string.Concat(new string[] { "https://api.weixin.qq.com/sns/oauth2/access_token?appid=", appid, "&secret=", appsecret, "&code=", Code, "&grant_type=authorization_code" }));
            return JsonHelper.ParseFromJson<OAuth_Token>(Str);
        }

        public OAuthUser Get_UserInfo(string access_token, string openid)
        {
            string Str = this.GetJson(string.Concat(new string[] { "https://api.weixin.qq.com/sns/userinfo?access_token=", access_token, "&openid=", openid, "&lang=zh_CN" }));
            return JsonHelper.ParseFromJson<OAuthUser>(Str);
        }

    }
}
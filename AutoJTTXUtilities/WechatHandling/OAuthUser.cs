namespace AutoJTTXUtilities.WechatHandling
{
    public class OAuthUser
    {
        private string _openID;

        private string _searchText;

        private string _nickname;

        private string _sex;

        private string _province;

        private string _city;

        private string _country;

        private string _headimgUrl;

        private string _privilege;

        private string _unionid;

        public string openid
        {
            get
            {
                return this._openID;
            }
            set
            {
                this._openID = value;
            }
        }

        public string SearchText
        {
            get
            {
                return this._searchText;
            }
            set
            {
                this._searchText = value;
            }
        }

        public string nickname
        {
            get
            {
                return this._nickname;
            }
            set
            {
                this._nickname = value;
            }
        }

        public string sex
        {
            get
            {
                return this._sex;
            }
            set
            {
                this._sex = value;
            }
        }

        public string province
        {
            get
            {
                return this._province;
            }
            set
            {
                this._province = value;
            }
        }

        public string city
        {
            get
            {
                return this._city;
            }
            set
            {
                this._city = value;
            }
        }

        public string country
        {
            get
            {
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }

        public string headimgurl
        {
            get
            {
                return this._headimgUrl;
            }
            set
            {
                this._headimgUrl = value;
            }
        }

        public string privilege
        {
            get
            {
                return this._privilege;
            }
            set
            {
                this._privilege = value;
            }
        }

        public string unionid
        {
            get
            {
                return this._unionid;
            }
            set
            {
                this._unionid = value;
            }
        }
    }
}
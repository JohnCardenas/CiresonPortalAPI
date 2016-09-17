using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Newtonsoft.Json;
using System.Dynamic;

namespace CiresonPortalAPI.ConfigurationItems
{
    public class User : ConfigurationItem
    {
        #region Fields
        bool   _isPartialUser = false;
        string _emailAddress = null;
        #endregion // Fields

        #region Read-Only Properties
        /// <summary>
        /// Returns the user's business phone number. Read-only.
        /// </summary>
        public string BusinessPhone
        {
            get { return this.GetPrimitiveValue<string>("BusinessPhone"); }
        }

        /// <summary>
        /// Returns the user's second business phone number. Read-only.
        /// </summary>
        public string BusinessPhone2
        {
            get { return this.GetPrimitiveValue<string>("BusinessPhone2"); }
        }

        /// <summary>
        /// Returns the user's city. Read-only.
        /// </summary>
        public string City
        {
            get { return this.GetPrimitiveValue<string>("City"); }
        }

        /// <summary>
        /// Returns the user's company. Read-only.
        /// </summary>
        public string Company
        {
            get { return this.GetPrimitiveValue<string>("Company"); }
        }

        /// <summary>
        /// Returns the user's country. Read-only.
        /// </summary>
        public string Country
        {
            get { return this.GetPrimitiveValue<string>("Country"); }
        }

        /// <summary>
        /// Returns the user's department. Read-only.
        /// </summary>
        public string Department
        {
            get { return this.GetPrimitiveValue<string>("Department"); }
        }

        /// <summary>
        /// Returns the user's distinguished name. Read-only.
        /// </summary>
        public string DistinguishedName
        {
            get { return this.GetPrimitiveValue<string>("DistinguishedName"); }
        }

        /// <summary>
        /// Returns the user's domain. Read-only.
        /// </summary>
        public string Domain
        {
            get { return this.GetPrimitiveValue<string>("Domain"); }
        }

        /// <summary>
        /// Returns the user's employee id. Read-only.
        /// </summary>
        public string EmployeeId
        {
            get { return this.GetPrimitiveValue<string>("EmployeeId"); }
        }

        /// <summary>
        /// Returns the user's email address. Read-only.
        /// </summary>
        public string Email
        {
            get
            {
                if (_emailAddress == null)
                {
                    _emailAddress = string.Empty;

                    // Find the user's email address
                    if (((IDictionary<string, object>)this.CurrentObject).ContainsKey("Preference"))
                    {
                        if (this.CurrentObject.Preference is List<object>)
                        {
                            foreach (var prefObj in this.CurrentObject.Preference)
                            {
                                if (prefObj.ChannelName == "SMTP")
                                {
                                    _emailAddress = prefObj.TargetAddress;
                                    break;
                                }
                            }
                        }
                    }
                }

                return _emailAddress;
            }
        }

        /// <summary>
        /// Returns the user's fax number. Read-only.
        /// </summary>
        public string Fax
        {
            get { return this.GetPrimitiveValue<string>("Fax"); }
        }
        
        /// <summary>
        /// Returns the user's first name. Read-only.
        /// </summary>
        public string FirstName
        {
            get { return this.GetPrimitiveValue<string>("FirstName"); }
        }

        /// <summary>
        /// Returns the fully qualified domain name the user belongs to. Read-only.
        /// </summary>
        public string FQDN
        {
            get { return this.GetPrimitiveValue<string>("FQDN"); }
        }

        /// <summary>
        /// Returns the user's home phone number. Read-only.
        /// </summary>
        public string HomePhone
        {
            get { return this.GetPrimitiveValue<string>("HomePhone"); }
        }

        /// <summary>
        /// Returns the user's second home phone number. Read-only.
        /// </summary>
        public string HomePhone2
        {
            get { return this.GetPrimitiveValue<string>("HomePhone2"); }
        }

        /// <summary>
        /// Returns the user's initials. Read-only.
        /// </summary>
        public string Initials
        {
            get { return this.GetPrimitiveValue<string>("Initials"); }
        }

        /// <summary>
        /// Returns true if there is only a partial subset of user data available.
        /// </summary>
        public bool IsPartialUser
        {
            get { return _isPartialUser; }
            private set { _isPartialUser = value; }
        }

        /// <summary>
        /// Returns the user's last name. Read-only.
        /// </summary>
        public string LastName
        {
            get { return this.GetPrimitiveValue<string>("LastName"); }
        }

        /// <summary>
        /// Returns the user's mobile phone number. Read-only.
        /// </summary>
        public string Mobile
        {
            get { return this.GetPrimitiveValue<string>("Mobile"); }
        }

        /// <summary>
        /// Returns the user's office. Read-only.
        /// </summary>
        public string Office
        {
            get { return this.GetPrimitiveValue<string>("Office"); }
        }

        /// <summary>
        /// Returns the organizational unit of the user. Read-only.
        /// </summary>
        public string OrganizationalUnit
        {
            get { return this.GetPrimitiveValue<string>("OrganizationalUnit"); }
        }

        /// <summary>
        /// Returns the user's pager number. Read-only.
        /// </summary>
        public string Pager
        {
            get { return this.GetPrimitiveValue<string>("Pager"); }
        }

        /// <summary>
        /// Returns the user's state. Read-only.
        /// </summary>
        public string State
        {
            get { return this.GetPrimitiveValue<string>("State"); }
        }

        /// <summary>
        /// Returns the user's street address. Read-only.
        /// </summary>
        public string StreetAddress
        {
            get { return this.GetPrimitiveValue<string>("StreetAddress"); }
        }

        /// <summary>
        /// Returns the title of this user object. Read-only.
        /// </summary>
        public string Title
        {
            get { return this.GetPrimitiveValue<string>("Title"); }
        }

        /// <summary>
        /// Returns the userPrincipalName of the user. Read-only.
        /// </summary>
        public string UPN
        {
            get { return this.GetPrimitiveValue<string>("UPN"); }
        }

        /// <summary>
        /// Returns the sAMAccountName of the user. Read-only.
        /// </summary>
        public string UserName
        {
            get { return this.GetPrimitiveValue<string>("UserName"); }
        }

        /// <summary>
        /// Returns the user's zip code. Read-only.
        /// </summary>
        public string Zip
        {
            get { return this.GetPrimitiveValue<string>("Zip"); }
        }

        #endregion Read-Only Properties

        #region Constructors
        public User (PartialUser p) : base(null, true, true)
        {
            this.CurrentObject = new ExpandoObject();
            this.CurrentObject.BaseId = p.Id;
            this.CurrentObject.DisplayName = p.Name;
            this.IsPartialUser = true;
            this.ReadOnly = true;
        }

        internal User(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal User() : base(null, true, true) { }
        #endregion // Constructors

        #region Relationship Properties
        /// <summary>
        /// Returns this user's Manager. Read only.
        /// </summary>
        public User Manager
        {
            get { return this.GetRelatedObject<User>("Manager"); }
        }
        #endregion // Relationship Properties

        #region General Methods
        /// <summary>
        /// Refreshes this User from the portal. This will reset any changes made to the object.
        /// This method must be called before accessing properties of children in relationship collections in order to populate all properties.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public override async Task<bool> Refresh(AuthorizationToken authToken)
        {
            return await this.RefreshType<User>(authToken);
        }

        /// <summary>
        /// Retrieves all user properties for a partial user object.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public async Task<bool> FetchFullAttributes(AuthorizationToken authToken)
        {
            if (this.IsPartialUser)
            {
                // Fetch a full user object and replace the partial data model with the full one
                User fullUser = await UserController.GetUserById(authToken, this.BaseId);
                this.CurrentObject = fullUser.CurrentObject;
                this.IsPartialUser = false;
                this.ReadOnly = true;
                return true;
            }
            else
                return false;
        }
        #endregion // General Methods
    }
}

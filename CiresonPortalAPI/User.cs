﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Newtonsoft.Json;
using System.Dynamic;

namespace CiresonPortalAPI
{
    public class User
    {
        internal dynamic _oUserObj;

        #region Read-Only Properties

        /// <summary>
        /// Returns the user's business phone number. Read-only.
        /// </summary>
        public string BusinessPhone { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "BusinessPhone"); } }

        /// <summary>
        /// Returns the user's second business phone number. Read-only.
        /// </summary>
        public string BusinessPhone2 { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "BusinessPhone2"); } }

        /// <summary>
        /// Returns the user's city. Read-only.
        /// </summary>
        public string City { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "City"); } }

        /// <summary>
        /// Returns the user's company. Read-only.
        /// </summary>
        public string Company { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Company"); } }

        /// <summary>
        /// Returns the user's country. Read-only.
        /// </summary>
        public string Country { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Country"); } }

        /// <summary>
        /// Returns the user's department. Read-only.
        /// </summary>
        public string Department { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Department"); } }

        /// <summary>
        /// Returns the user's display name. Read-only.
        /// </summary>
        public string DisplayName { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "DisplayName"); } }

        /// <summary>
        /// Returns the user's distinguished name. Read-only.
        /// </summary>
        public string DistinguishedName { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "DistinguishedName"); } }

        /// <summary>
        /// Returns the user's domain. Read-only.
        /// </summary>
        public string Domain { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Domain"); } }

        /// <summary>
        /// Returns the user's employee id. Read-only.
        /// </summary>
        public string EmployeeId { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "EmployeeId"); } }

        /// <summary>
        /// Returns the user's fax number. Read-only.
        /// </summary>
        public string Fax { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Fax"); } }

        /// <summary>
        /// Returns the user's first name. Read-only.
        /// </summary>
        public string FirstName { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "FirstName"); } }

        /// <summary>
        /// Returns the fully qualified domain name the user belongs to. Read-only.
        /// </summary>
        public string FQDN { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "FQDN"); } }

        /// <summary>
        /// Returns the user's home phone number. Read-only.
        /// </summary>
        public string HomePhone { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "HomePhone"); } }

        /// <summary>
        /// Returns the user's second home phone number. Read-only.
        /// </summary>
        public string HomePhone2 { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "HomePhone2"); } }

        /// <summary>
        /// Returns the user's ID. Read-only.
        /// </summary>
        public Guid Id { get { return DynamicObjectHelpers.GetProperty<Guid>(_oUserObj, "BaseId"); } }

        /// <summary>
        /// Returns the user's initials. Read-only.
        /// </summary>
        public string Initials { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Initials"); } }

        /// <summary>
        /// Returns the user's last name. Read-only.
        /// </summary>
        public string LastName { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "LastName"); } }

        /// <summary>
        /// Returns the user's mobile phone number. Read-only.
        /// </summary>
        public string Mobile { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Mobile"); } }

        /// <summary>
        /// Returns the user's office. Read-only.
        /// </summary>
        public string Office { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Office"); } }

        /// <summary>
        /// Returns the organizational unit of the user. Read-only.
        /// </summary>
        public string OrganizationalUnit { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "OrganizationalUnit"); } }

        /// <summary>
        /// Returns the user's pager number. Read-only.
        /// </summary>
        public string Pager { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Pager"); } }

        /// <summary>
        /// Returns the user's state. Read-only.
        /// </summary>
        public string State { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "State"); } }

        /// <summary>
        /// Returns the user's street address. Read-only.
        /// </summary>
        public string StreetAddress { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "StreetAddress"); } }

        /// <summary>
        /// Returns the title of this user object. Read-only.
        /// </summary>
        public string Title { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Title"); } }

        /// <summary>
        /// Returns the userPrincipalName of the user. Read-only.
        /// </summary>
        public string UPN { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "UPN"); } }

        /// <summary>
        /// Returns the sAMAccountName of the user. Read-only.
        /// </summary>
        public string UserName { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "UserName"); } }

        /// <summary>
        /// Returns the user's zip code. Read-only.
        /// </summary>
        public string Zip { get { return DynamicObjectHelpers.GetProperty<string>(_oUserObj, "Zip"); } }

        #endregion Read-Only Properties

        public User(PartialUser user)
        {
            _oUserObj = new ExpandoObject();
            _oUserObj.BaseId = user.Id;
            _oUserObj.DisplayName = user.Name;
        }

        internal User(dynamic obj)
        {
            _oUserObj = obj;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}

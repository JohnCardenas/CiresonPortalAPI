using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CiresonPortalAPI.ConfigurationItems
{
    public class Location : ConfigurationItem
    {
        #region Read-Write Properties

        /// <summary>
        /// Gets or sets Address Line 1 of the Location.
        /// </summary>
        public string AddressLine1
        {
            get { return this.GetPrimitiveValue<string>("LocationAddress1"); }
            set { this.SetPrimitiveValue<string>("LocationAddress1", value, "AddressLine1"); }
        }

        /// <summary>
        /// Gets or sets Address Line 2 of the Location.
        /// </summary>
        public string AddressLine2
        {
            get { return this.GetPrimitiveValue<string>("LocationAddress2"); }
            set { this.SetPrimitiveValue<string>("LocationAddress2", value, "AddressLine2"); }
        }

        /// <summary>
        /// Gets or sets the City of the Location.
        /// </summary>
        public string City
        {
            get { return this.GetPrimitiveValue<string>("LocationCity"); }
            set { this.SetPrimitiveValue<string>("LocationCity", value, "City"); }
        }

        /// <summary>
        /// Gets or sets the State of the Location.
        /// </summary>
        public string State
        {
            get { return this.GetPrimitiveValue<string>("LocationState"); }
            set { this.SetPrimitiveValue<string>("LocationState", value, "State"); }
        }

        /// <summary>
        /// Gets or sets the postal (zip) code of the Location.
        /// </summary>
        public string PostalCode
        {
            get { return this.GetPrimitiveValue<string>("LocationPostCode"); }
            set { this.SetPrimitiveValue<string>("LocationPostCode", value, "PostalCode"); }
        }

        /// <summary>
        /// Gets or sets the country of the Location.
        /// </summary>
        public string Country
        {
            get { return this.GetPrimitiveValue<string>("LocationCountry"); }
            set { this.SetPrimitiveValue<string>("LocationCountry", value, "Country"); }
        }

        /// <summary>
        /// Gets or sets the contact name of the Location.
        /// </summary>
        public string ContactName
        {
            get { return this.GetPrimitiveValue<string>("LocationContact"); }
            set { this.SetPrimitiveValue<string>("LocationContact", value, "ContactName"); }
        }

        /// <summary>
        /// Gets or sets the contact email of the Location.
        /// </summary>
        public string ContactEmail
        {
            get { return this.GetPrimitiveValue<string>("LocationEmail"); }
            set { this.SetPrimitiveValue<string>("LocationEmail", value, "ContactEmail"); }
        }

        /// <summary>
        /// Gets or sets the contact phone number of the Location.
        /// </summary>
        public string ContactPhone
        {
            get { return this.GetPrimitiveValue<string>("LocationPhone"); }
            set { this.SetPrimitiveValue<string>("LocationPhone", value, "ContactPhone"); }

        }

        /// <summary>
        /// Gets or sets the contact fax number of the Location.
        /// </summary>
        public string ContactFax
        {
            get { return this.GetPrimitiveValue<string>("LocationFax"); }
            set { this.SetPrimitiveValue<string>("LocationFax", value, "ContactFax"); }
        }
        #endregion Read-Write Properties

        #region Relationship Properties

        /// <summary>
        /// Returns a list of all child Locations. Read only.
        /// </summary>
        public RelatedObjectList<Location> Children
        {
            get
            {
                return new RelatedObjectList<Location>(this, "Source_LocationContainsLocation");
            }
        }

        /// <summary>
        /// Gets or sets the parent Location.
        /// </summary>
        public Location Parent
        {
            get { return this.GetRelatedObject<Location>("Target_LocationContainsLocation"); }
            set { this.SetRelatedObject("Target_LocationContainsLocation", value, "Parent"); }
        }
        #endregion Relationship Properties

        #region Constructors
        internal Location(ExpandoObject obj, bool existingObject = false, bool readOnly = true) : base(obj, existingObject, readOnly) { }
        internal Location() : base() { }
        #endregion // Constructors

        #region Public Methods
        /// <summary>
        /// Refreshes this Location from the portal. This will reset any changes made to the object.
        /// This method must be called before accessing properties of children in relationship collections in order to populate all properties.
        /// </summary>
        /// <param name="authToken">AuthorizationToken to use</param>
        /// <returns></returns>
        public override async Task<bool> Refresh(AuthorizationToken authToken)
        {
            return await this.RefreshType<Location>(authToken);
        }
        #endregion // Public Methods
    }
}

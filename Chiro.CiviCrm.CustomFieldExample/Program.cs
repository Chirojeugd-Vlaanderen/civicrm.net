/*
   Copyright 2015 Chirojeugd-Vlaanderen vzw

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Wcf.CustomFieldExample.Properties;
using Newtonsoft.Json.Linq;

namespace Chiro.CiviCrm.Wcf.CustomFieldExample
{
    /// <summary>
    /// This program illustrates how you can address your custom fields in CiviCRM.
    /// 
    /// If you are looking for a more general example, please see the project
    /// Chiro.CiviCrm.Wcf.Example.
    /// </summary>
    class Program
    {
        private static readonly string SiteKey = Settings.Default.SiteKey;
        private static readonly string ApiKey = Settings.Default.ApiKey;

        static void Main(string[] args)
        {
            Example1();
            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }

        /// <summary>
        /// Retrieve a relationship with a multiselect custom field.
        /// 
        /// This does not work, see
        /// http://forum.civicrm.org/index.php/topic,35535.0.html
        /// </summary>
        public static void Example1()
        {
            using (var channelFactory = new ChannelFactory<ICiviCrmApi>("*"))
            {
                using (var channel = channelFactory.CreateChannel())
                {
                    Console.WriteLine("Please check the 'access all custom data' permission of your API user.");
                    // Retrieving custom fields is rather convoluted, involving a cast to JObject.
                    // I am not sure how to improve this.
                    var result =
                        channel.GetSingle(ApiKey, SiteKey, CiviEntity.Relationship, new IdRequest(2885)) as JObject;
                    if (result == null)
                    {
                        Console.WriteLine("Relationship not found.");
                        return;
                    }

                    var relationship = result.ToObject<CustomRelatioship>();
                    Console.WriteLine("Contact ID a: {0}", relationship.ContactIdA);
                    Console.WriteLine("Contact ID b: {0}", relationship.ContactIdB);
                    if (relationship.Functies != null)
                    {
                        // The API returns multi-select custom fileds as dictionary's {{value1, value1}, {value2, value2},...}
                        // Maybe you should just write a wrapper if you need this.
                        Console.WriteLine("Functions: {0}", String.Join(",", relationship.Functies.Select(f => f.Value)));
                    }
                }
            }
        }

        /// <summary>
        /// Saves and retrieves contact with a custom field.
        /// </summary>
        public static void Example2()
        {
            var customContact = new CustomContact
            {
                ContactType = ContactType.Individual,
                Gender = Gender.Male,
                FirstName = "Will",
                LastName = "Tura",
                GapId = 797204
            };

            using (var channelFactory = new ChannelFactory<ICiviCrmApi>("*"))
            {
                using (var channel = channelFactory.CreateChannel())
                {
                    var result1 = channel.ContactSave(ApiKey, SiteKey, customContact);
                    Debug.Assert(result1.Id.HasValue);

                    // Custom fields don't come by default for contacts. So we have to specify ReturnFields.
                    var result2 =
                        channel.GetSingle(ApiKey, SiteKey, CiviEntity.Contact,
                            new IdRequest {Id = result1.Id.Value, ReturnFields = "custom_10,first_name,last_name"}) as
                            JObject;
                    Debug.Assert(result2 != null);
                    var contact = result2.ToObject<CustomContact>();

                    Console.WriteLine("{0} {1} ({2})", contact.FirstName, contact.LastName, contact.GapId);
                }
            }
        }
    }
}

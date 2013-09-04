/*
   Copyright 2013 Chirojeugd-Vlaanderen vzw

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
using System.ServiceModel;

namespace Chiro.Cdf.ServiceHelper
{
    /// <summary>
    /// This static class provides a method CallService, which calls WCF services.
    /// This way you don't have to create a proxy yourself; you only have to
    /// define the service in your configuration file.
    /// Maybe there are better ways to achieve this.
    /// </summary>
    public static class ServiceHelper
    {
        public static TResult CallService<TContract, TResult>(Func<TContract, TResult> method) where TContract: IDisposable
        {
            var factory = new ChannelFactory<TContract>(String.Empty); // endpoints have no names
            using (var proxy = factory.CreateChannel())
            {
                return method(proxy);
            }
        }

        public static void CallService<TContract>(Action<TContract> method) where TContract : IDisposable
        {
            var factory = new ChannelFactory<TContract>(String.Empty); // endpoints have no names
            using (var proxy = factory.CreateChannel())
            {
                method(proxy);
            }
        }
    }
}

/* 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library2525D
{

    /// <summary>
    /// Container for the attributes of a *single* layer/icon of a 2525D Military Symbol 
    /// </summary>
    class MilitarySymbolLayer
    {

        public string ReferenceId
        {
            get
            {
                return referenceId;
            }
            set
            {
                referenceId = value;
            }
        }
        protected string referenceId = string.Empty;

        public string GraphicLayer
        {
            get
            {
                return graphicLayer;
            }
        }
        protected string graphicLayer = string.Empty;

        public List<string> Tags
        {
            get
            {
                tags.Clear();

                if (!string.IsNullOrEmpty(this.referenceId))
                    tags.Add(this.referenceId);

                if (!string.IsNullOrEmpty(this.graphicLayer))
                    tags.Add(this.graphicLayer);

                return tags;
            }
        }
        protected List<string> tags = new List<string>();

    }

}

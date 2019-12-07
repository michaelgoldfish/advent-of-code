using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class OrbitObject
    {
        public readonly string name;        // Name of the orbiting object
        public OrbitObject orbiting;        // Object being orbited by this object
        public List<OrbitObject> orbitedBy; // List of objects that are orbiting this one

        public OrbitObject(string name)
        {
            this.name = name;
            orbiting = null;
            orbitedBy = new List<OrbitObject>();
        }

        public OrbitObject(string name, OrbitObject orbiting)
        {
            this.name = name;
            this.orbiting = orbiting;
            orbitedBy = new List<OrbitObject>();
        }

        public OrbitObject(string name, OrbitObject orbiting, List<OrbitObject> orbitedBy)
        {
            this.name = name;
            this.orbiting = orbiting;
            this.orbitedBy = orbitedBy;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Powerup
{
    private String name;
    private Vector2 powerupLocation;

    public Powerup(String powerupName, Vector2 location)
    {
        name = powerupName;
        powerupLocation = location;
    }

    public String getName()
    {
        return name;
    }

    public Vector2 getLocation()
    {
        return powerupLocation;
    }
    public void setLocation(Vector2 newLoc)
    {
        powerupLocation = newLoc;
    }
}
